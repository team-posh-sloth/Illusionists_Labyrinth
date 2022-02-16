using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] [Tooltip("Velocity per second")] float gravity = 20f, normalForce = 1f;
    [SerializeField] [Tooltip("Meters per second")] float moveSpeed;
    [SerializeField] [Range(0, 360)] [Tooltip("Max input degrees per second")] float rotSpeed;
    [SerializeField] [Range(0, 5)] [Tooltip("Max input zoom percentage per second")] float zoomSpeed;

    float xRot, yRot, yPos, zPos, gravVelocity, zoomPercentage, runMultiplier;

    Transform cam;
    
    CharacterController character;

    Animator anim;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to Game Window (Esc. key frees it in editor)
        character = GetComponent<CharacterController>(); // References the Character Controller
        cam = Camera.main.transform; // References the main camera transform
        anim = GetComponent<Animator>();
    }

    void Update()
    {
        UpdateRotation(); // Moving the mouse on the X axis rotates the player on the Y axis
        UpdateZoom(); // Moving the mouse on the Y axis controls camera zoom on the player
        UpdateXZMovement(); // Planar input is added to player movement
        UpdateGravity(); // Gravity is added to player movement

        UpdateActions();
    }

    // Mouse input on the X axis increases the object's rotation on its Y axis (rotSpeed is the maximum rotation in degrees per second)
    void UpdateRotation() { yRot += Input.GetAxis("Mouse X") * Time.deltaTime * rotSpeed; transform.eulerAngles = new Vector3(0, yRot); }

    // Mouse input on the Y axis interpolates both the position and angle of the camera behind the player (zoomSpeed is zoom percentage per second)
    void UpdateZoom() 
    {
        zoomPercentage += Input.GetAxis("Mouse Y") * Time.deltaTime * zoomSpeed; zoomPercentage = Mathf.Clamp01(zoomPercentage); // zoomPercentage is clamped between 0 and 1 (1 is 100%)
        yPos = Mathf.Lerp(4, 2, zoomPercentage); zPos = Mathf.Lerp(-7.5f, -3, zoomPercentage); xRot = Mathf.Lerp(30, 15, zoomPercentage); // yPos, xPos and xRot interpolate at the rate of zoomPercentage)
        cam.localPosition = new Vector3(0, yPos, zPos); cam.localEulerAngles = new Vector3(xRot, 0); // localPosition must be used to keep values relative to player
    }

    // XZ axis input (i.e. WASD) creates a vector on the XZ plane (forward and right) with a magnitude of units (moveSpeed) per second (deltaTime)
    void UpdateXZMovement()  {
        if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.W)) runMultiplier = 2; else runMultiplier = 1; // Player moves twice as fast holding shift
        character.Move(moveSpeed * runMultiplier * Time.deltaTime * (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized);
    }

    // The character moves downward (negative up) at velocity (gravVelocity) per second (deltaTime)
    void UpdateGravity() {
        character.Move(gravVelocity * Time.deltaTime * -transform.up);  // Move() must be called before isGrounded              <--|| required for proper   ||
        if (character.isGrounded) { gravVelocity = normalForce; }       // A static normal force must be applied when grounded  <--|| ground detection      ||
        else { gravVelocity += gravity * Time.deltaTime; } // increase gravVelocity by meters (gravity) per second (deltaTime)
    }

    void UpdateActions()
    {
        if (Input.GetMouseButtonDown(0))
        {
            anim.Play("attack");
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

}
