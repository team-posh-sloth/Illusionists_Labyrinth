using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Old_Labyrinth
{
    public class FalseWall : MonoBehaviour
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                if (player.trueSight)
                {
                    // Instantly reduces truesight timer
                    player.trueSightTimer = 0.5f;
                }
            }
        }
    }
}