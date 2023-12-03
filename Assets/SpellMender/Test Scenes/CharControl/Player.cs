using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth
{
    public class Player : MonoBehaviour
    {
        public float speed;
        public static Vector3 moveDirection;

        private void Start()
        {
            Game.LockCursor();
        }

        private void Update()
        {
            Actions.Read();
            Move();
        }

        private void Move()
        {
            //To-Do: Move based on camera direction
            //To-Do: Start the camera further away from the player
            //To-Do: Set deadzone for camera movement (don't make it move so easily)
            transform.position += moveDirection * speed * Time.deltaTime;
            transform.LookAt(moveDirection + transform.position);
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
