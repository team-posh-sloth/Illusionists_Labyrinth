using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float rotSpeed;

    float rotX;
    float rotY;

    CharacterController player;

    void Start()
    {
        // Locks the cursor to Game Window (Esc. key frees it in editor)
        Cursor.lockState = CursorLockMode.Locked;

        player = GetComponent<CharacterController>();
    }

    void Update()
    {
        MouseRotation(); // Moving the mouse rotates the player left and right
        ForwardInput(); // 
    }

    void MouseRotation()
    {
        if (Input.GetAxisRaw("Mouse X") != 0)
        {
            rotY += Input.GetAxis("Mouse X") * rotSpeed;
            transform.eulerAngles = new Vector3(0, rotY);
        }
    }
    void ForwardInput() { if (Input.GetAxisRaw("Vertical") == 1) { player.Move(transform.forward * Time.deltaTime * moveSpeed); } }
}
