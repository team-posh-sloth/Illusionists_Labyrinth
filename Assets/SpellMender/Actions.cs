using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

namespace Labyrinth
{
    public static class Actions
    {
        // Keys
        private static readonly KeyCode quit = KeyCode.Escape;

        // Axes
        private static readonly string
            leftRight = "Horizontal",
            upDown = "Vertical";

        // States
        public static Vector3 inputDirection;

        public static void Read()
        {
            Quit(Input.GetKeyDown(quit));

            InputDirection(Input.GetAxis(leftRight), Input.GetAxis(upDown));
        }

        public static void InputDirection(float horizontal, float vertical)
        {
            if (inputDirection.x == horizontal && inputDirection.z == vertical) return; // Exit this function if there's NO DIFFERENCE in input direction between frames
            if (inputDirection.x != horizontal) inputDirection.x = horizontal;          // Update HORIZONTAL input direction if it's different from last frame
            if (inputDirection.z != vertical) inputDirection.z = vertical;              // Update VERTICAL input direction if it's different from last frame

            Player.moveDirection = inputDirection.normalized;                           // Update Player move direction with normalized input direction
        }

        public static void Quit(bool keyDown)
        {
            if (keyDown) Game.Quit();
        }
    }
}
