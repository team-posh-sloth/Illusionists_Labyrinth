using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            interactable.Detect();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            if (Input.GetKey(KeyCode.E))
            {
                interactable.Interact();
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent(out Interactable interactable))
        {
            interactable.Undetect();
        }
    }
}
