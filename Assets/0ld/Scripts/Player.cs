using System.Collections;
using UnityEngine;

namespace Old_Labyrinth
{

    public class Player : MonoBehaviour
    {
        public AudioClip walkingSound;
        public AudioClip runningSound;
        public AudioClip damagedSound;
        public AudioClip weaponSound;
        public AudioClip shieldSound;
        public AudioClip wallAppearingSound;
        public AudioClip poofSound;


        public bool trueSight;
        [SerializeField] GameObject dispellEffect;
        [SerializeField] float trueSightTime;
        [SerializeField][Tooltip("Velocity per second")] float gravity = 20f, normalForce = 1f;
        [SerializeField][Tooltip("Meters per second")] float moveSpeed;
        [SerializeField][Range(0, 360)][Tooltip("Max input degrees per second")] float rotSpeed;
        [SerializeField][Range(0, 5)][Tooltip("Max input zoom percentage per second")] float zoomSpeed;
        [SerializeField] int hitPointMax = 3;

        float xRot, yRot, yPos, zPos, gravVelocity, zoomPercentage, runMultiplier;
        public float trueSightTimer;

        int hitPoints;

        Transform cam;

        [SerializeField] GameObject trueSightLens;

        CharacterController character;
        Vector3 startPosition;

        public Animator anim;
        public AudioSource audio;

        void Start()
        {
            Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to Game Window (Esc. key frees it in editor)
            character = GetComponent<CharacterController>(); // References the Character Controller
            cam = Camera.main.transform; // References the main camera transform
            anim = GetComponent<Animator>();
            hitPoints = hitPointMax;
            startPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            trueSightTimer = trueSightTime;
            audio = gameObject.GetComponent<AudioSource>();
        }

        void Update()
        {
            UpdateRotation(); // Moving the mouse on the X axis rotates the player on the Y axis
            UpdateZoom(); // Moving the mouse on the Y axis controls camera zoom on the player
            UpdateXZMovement(); // Planar input is added to player movement
            UpdateGravity(); // Gravity is added to player movement

            UpdateActions();
            UpdateTrueSight();
            UpdateAudio();
        }

        void UpdateAudio()
        {
            if (anim.GetCurrentAnimatorStateInfo(0).IsName("walking"))
            {
                audio.Stop();
            }
        }
        void UpdateTrueSight()
        {
            if (trueSight)
            {
                if (trueSightTimer == trueSightTime) // activate once as countdown begins
                {
                    trueSightLens.SetActive(false);
                    foreach (GameObject falsePath in GameObject.FindGameObjectsWithTag("False Path"))
                    {
                        falsePath.GetComponent<MeshRenderer>().enabled = true;
                    }
                    foreach (GameObject falseWall in GameObject.FindGameObjectsWithTag("False Wall"))
                    {
                        falseWall.GetComponent<MeshRenderer>().enabled = false;
                    }
                }

                trueSightTimer -= Time.deltaTime; //Count down during true sight
            }
            if (trueSightTimer <= 0 && trueSight) //Return things to normal once the time is up
            {
                trueSightLens.SetActive(true);
                foreach (GameObject falsePath in GameObject.FindGameObjectsWithTag("False Path"))
                {
                    falsePath.GetComponent<MeshRenderer>().enabled = false;
                }
                foreach (GameObject falseWall in GameObject.FindGameObjectsWithTag("False Wall"))
                {
                    if (falseWall.name != "TrueSightStop")
                    {
                        falseWall.GetComponent<MeshRenderer>().enabled = true;
                    }
                }

                trueSight = false; // truesight time is up so trueSight is off
                trueSightTimer = trueSightTime; // reset timer for next time
            }
        }

        // Mouse input on the X axis increases the object's rotation on its Y axis (rotSpeed is the maximum rotation in degrees per second)
        void UpdateRotation()
        {
            if (Input.GetAxis("Mouse X") > 0.1f || Input.GetAxis("Mouse X") < -0.1f)
            {
                yRot += Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed; transform.eulerAngles = new Vector3(0, yRot);
            }
        }

        // Mouse input on the Y axis interpolates both the position and angle of the camera behind the player (zoomSpeed is zoom percentage per second)
        void UpdateZoom()
        {
            zoomPercentage += Input.GetAxis("Mouse Y") * Time.deltaTime * zoomSpeed; zoomPercentage = Mathf.Clamp01(zoomPercentage); // zoomPercentage is clamped between 0 and 1 (1 is 100%)
            yPos = Mathf.Lerp(4, 2, zoomPercentage); zPos = Mathf.Lerp(-7.5f, -3, zoomPercentage); xRot = Mathf.Lerp(30, 15, zoomPercentage); // yPos, xPos and xRot interpolate at the rate of zoomPercentage)
            cam.localPosition = new Vector3(0, yPos, zPos); cam.localEulerAngles = new Vector3(xRot, 0); // localPosition must be used to keep values relative to player
        }

        // XZ axis input (i.e. WASD) creates a vector on the XZ plane (forward and right) with a magnitude of units (moveSpeed) per second (deltaTime)
        void UpdateXZMovement()
        {
            //if (anim.GetCurrentAnimatorStateInfo(0).IsName("attack") || anim.GetCurrentAnimatorStateInfo(0).IsName("block"))
            //{
            //    runMultiplier = 0;
            //}
            //else
            //{
            if (Input.GetKey(KeyCode.W)) { anim.SetBool("Is walking", true); } else { anim.SetBool("Is walking", false); }
            if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W))
            {
                anim.SetBool("Is running", true);
                runMultiplier = 2; // Player moves twice as fast holding shift
            }
            else
            {
                anim.SetBool("Is running", false);
                runMultiplier = 1;
            }
            //}
            character.Move(moveSpeed * runMultiplier * Time.deltaTime * (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized);
        }

        // The character moves downward (negative up) at velocity (gravVelocity) per second (deltaTime)
        void UpdateGravity()
        {
            character.Move(gravVelocity * Time.deltaTime * -transform.up);  // Move() must be called before isGrounded              <--|| required for proper   ||
            if (character.isGrounded) { gravVelocity = normalForce; }       // A static normal force must be applied when grounded  <--|| ground detection      ||
            else { gravVelocity += gravity * Time.deltaTime; } // increase gravVelocity by meters (gravity) per second (deltaTime)
        }

        void UpdateActions()
        {
            if (Input.GetMouseButtonDown(0))
            {
                //if (!anim.GetCurrentAnimatorStateInfo(0).IsName("attack"))
                //{
                //}
                anim.Play("attack");
                if (audio.clip != weaponSound)
                {
                    audio.clip = weaponSound;
                }
                if (!audio.isPlaying)
                {
                    audio.volume = 0.5f;
                    audio.pitch = 1.2f;
                    audio.PlayDelayed(0.35f);
                }
            }
            if (Input.GetMouseButton(1))
            {
                anim.Play("block");
            }
            else if (anim.GetCurrentAnimatorStateInfo(0).IsName("block"))
            {
                anim.Play("idle");
            }
        }

        private void OnControllerColliderHit(ControllerColliderHit hit)
        {
            if (hit.collider.tag == "Illusionist")
            {
                GameObject particleEffect = Instantiate(dispellEffect, new Vector3(hit.transform.localPosition.x, hit.transform.localPosition.y, hit.transform.localPosition.z), hit.transform.localRotation);
                particleEffect.GetComponent<ParticleSystem>().Play();
                hit.gameObject.SetActive(false);
                audio.volume = 5;
                audio.pitch = 1;
                audio.clip = poofSound;
                audio.Play();
            }
            if (hit.collider.tag == "False Path")
            {
                StartCoroutine(FadeIn(hit.gameObject.GetComponent<MeshRenderer>()));
            }
            if (hit.collider.tag == "True Sight Token")
            {
                audio.volume = 5f;
                audio.pitch = 1;
                audio.clip = poofSound;
                audio.Play();
                Destroy(hit.gameObject);
                trueSight = true;
            }
        }
        IEnumerator FadeIn(MeshRenderer mesh)
        {
            if (!mesh.enabled)
            {
                audio.volume = 2f;
                audio.clip = wallAppearingSound;
                audio.Play();
                mesh.enabled = true;

                // Set mesh to transparent (see Unity Documentation "Changing the Rendering Mode using a Script")
                mesh.material.SetOverrideTag("RenderType", "Transparent");
                mesh.material.SetFloat("_SrcBlend", (float)UnityEngine.Rendering.BlendMode.One);
                mesh.material.SetFloat("_DstBlend", (float)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                mesh.material.SetFloat("_ZWrite", 0.0f);
                mesh.material.DisableKeyword("_ALPHATEST_ON");
                mesh.material.DisableKeyword("_ALPHABLEND_ON");
                mesh.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                mesh.material.renderQueue = (int)UnityEngine.Rendering.RenderQueue.Transparent;
            }
            else yield break;
            for (float i = 0; i <= 1; i += Time.deltaTime)
            {
                mesh.material.color = new Color(mesh.material.color.r, mesh.material.color.g, mesh.material.color.b, i);
                yield return null;
            }
        }

        public void takeDamage()
        {
            hitPoints -= 1;
            if (hitPoints <= 0)
            {
                character.enabled = false;
                gameObject.transform.localPosition = new Vector3(startPosition.x, startPosition.y, startPosition.z);
                character.enabled = true;
                hitPoints = hitPointMax;
            }
        }
    }
}
