using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth
{
    public class Player : MonoBehaviour
    {
        private void Start()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.Escape))
            {
                Application.Quit();
            }
        }

        enum MoveState
        {
            idle, walk, run, reverse, strafeL, strafeR
        }

        enum ActionState
        {
            attack
        }

    }

}
