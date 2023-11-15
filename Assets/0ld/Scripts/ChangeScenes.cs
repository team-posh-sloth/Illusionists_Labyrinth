using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Old_Labyrinth
{
    public class ChangeScenes : MonoBehaviour
    {
        void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out Player player))
            {
                SceneManager.LoadScene("World");
            }
        }
    }
}