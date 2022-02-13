using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed, rotSpeed, gravity = 9.8f, normalForce = 1f;

    float rotation, gravVelocity;
    
    CharacterController character;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to Game Window (Esc. key frees it in editor)
        character = GetComponent<CharacterController>(); // References the Character Controller
    }

    void Update()
    {
        UpdateRotation(); // Moving the mouse rotates the player on the Y axis
        UpdateXZMovement(); // Planar input is added to player movement
        UpdateGravity(); // Gravity is added to player movement
    }

    // Mouse input on the X axis increases the object's rotation on its Y axis (rotSpeed is the maximum rotation in degrees per frame)
    void UpdateRotation() { rotation += Input.GetAxis("Mouse X") * rotSpeed; transform.eulerAngles = new Vector3(0, rotation); }

    // XZ axis input (i.e. WASD) creates a vector on the XZ plane (forward and right) with a magnitude of units (moveSpeed) per second (deltaTime)
    void UpdateXZMovement()  { 
        character.Move(moveSpeed * Time.deltaTime * (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized);
    }

    // The character moves downward (negative up) at velocity (gravVelocity) per second (deltaTime)
    void UpdateGravity() {
        character.Move(gravVelocity * Time.deltaTime * -transform.up);  // Move() must be called before isGrounded              <--|| required for proper   ||
        if (character.isGrounded) { gravVelocity = normalForce; }       // A static normal force must be applied when grounded  <--|| ground detection      ||
        else { gravVelocity += gravity * Time.deltaTime; } // increase gravVelocity by meters (gravity) per second (deltaTime)
    }

}
