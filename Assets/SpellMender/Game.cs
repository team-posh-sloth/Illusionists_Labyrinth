using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth
{
    public static class Game
    {
        public static void LockCursor()
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Confined;
        }

        public static void Quit()
        {
            Application.Quit();
        }
    }
}
