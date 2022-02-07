using System.Collections; using System.Collections.Generic; using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed, rotSpeed, gravity = 9.8f;

    float rotation, gravVelocity;

    CharacterController character;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; // Locks the cursor to Game Window (Esc. key frees it in editor)
        character = GetComponent<CharacterController>(); // References the Character Controller
    }

    void Update()
    {
        UpdateRotation(); // Moving the mouse rotates the player left and right
        UpdateXZ(); // Planar input is added to player movement
        UpdateGravity(); // Gravity is added to player movement
    }

    // Mouse input on the X axis increases the object's rotation on its Y axis (rotSpeed is the maximum rotation in degrees per frame)
    void UpdateRotation() { rotation += Input.GetAxis("Mouse X") * rotSpeed; transform.eulerAngles = new Vector3(0, rotation); }
    // Planar input (i.e. WASD) creates a normalized vector which is then multiplied to the magnitude of units (moveSpeed) per second (Time.deltaTime) of character movement
    void UpdateXZ() { character.Move(moveSpeed * Time.deltaTime * (transform.forward * Input.GetAxisRaw("Vertical") + transform.right * Input.GetAxisRaw("Horizontal")).normalized); }
    void UpdateGravity() { if (character.isGrounded) { gravVelocity = 0; } else { gravVelocity += gravity * Time.deltaTime; } character.Move(-gravVelocity * Time.deltaTime * transform.up); }

}