using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;
    [SerializeField] float gravity = 9.8f;

    float rotX;
    float rotY;
    float gravValue;

    CharacterController player;
    Vector3 moveVector = new Vector3();

    //DEBUG
    float timeElapsed = 0f;
    bool keepCount = true;

    void Start()
    {
        // Locks the cursor to Game Window (Esc. key frees it in editor)
        Cursor.lockState = CursorLockMode.Locked;

        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        MouseRotation(); // Moving the mouse rotates the player left and right
        MoveForward(); // Forward input is added to Movement Vector
        ObeyGravity(); // Gravity is added to Movement Vector
        ApplyMovement(); // Movement Vector is applied to Character Controller

        print("Gravity: " + ((500 - transform.position.y) / (timeElapsed * timeElapsed)));
        timeElapsed += Time.deltaTime;
    }
    void FixedUpdate()
    {
    }

    void MouseRotation() { if (Input.GetAxisRaw("Mouse X") != 0) { rotY += Input.GetAxis("Mouse X") * rotSpeed; transform.eulerAngles = new Vector3(0, rotY); } }
    void MoveForward() { if (Input.GetAxisRaw("Vertical") == 1) { player.Move(transform.forward * Time.deltaTime * moveSpeed); } }
    void ObeyGravity() { if (player.isGrounded) { moveVector.y = 0; } gravValue = gravity * Time.deltaTime; moveVector.y += -gravValue * Time.deltaTime; }
    void ApplyMovement() { player.Move(moveVector); }
}
