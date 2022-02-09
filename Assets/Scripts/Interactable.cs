using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Interactable : MonoBehaviour
{
    [SerializeField] GameObject dialogueBubble;

    [SerializeField] [Multiline(6)] [TextArea(1, 6)] string text;

    void Start()
    {
        dialogueBubble.GetComponentInChildren<TMP_Text>().text = text;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            Detect();
        }
    }

    void OnTriggerStay(Collider other)
    {
        dialogueBubble.transform.LookAt(new Vector3(Camera.main.transform.position.x, dialogueBubble.transform.position.y, Camera.main.transform.position.z));
        if (other.name == "Player")
        {
            if (Input.GetKey(KeyCode.E))
            {
                Interact();
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            Undetect();
        }
    }

    public void Detect()
    {
        dialogueBubble.GetComponent<Animator>().Play("popup");
    }

    public void Undetect()
    {
        dialogueBubble.GetComponent<Animator>().Play("popdown");
    }

    public void Interact()
    {
    }

}
