using UnityEngine;

namespace Old_Labyrinth
{
    public class Interactable : MonoBehaviour
    {
        bool inRange;

        void Update() { if (Input.GetKeyDown(KeyCode.E) && inRange) { Interact(); } } // Interact is called when pressing "E" in trigger range (GetKeyDown works most reliably in Update)

        void OnTriggerEnter(Collider other) { if (other.name == "Player") { Detect(); inRange = true; } } // Detect is called when player enters trigger range

        void OnTriggerStay(Collider other) { if (other.name == "Player") { Watch(); } } // Watch is called when player stays in trigger range

        void OnTriggerExit(Collider other) { if (other.name == "Player") { Undetect(); inRange = false; } } // Undetect is called when player exits trigger range

        protected virtual void Detect() { } // Virtual function to be customized by children (i.e. Dialogue)

        protected virtual void Watch() { } // Virtual function to be customized by children

        protected virtual void Interact() { } // Virtual function to be customized by children

        protected virtual void Undetect() { } // Virtual function to be customized by children

    }
}