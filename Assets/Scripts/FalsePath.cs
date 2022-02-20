using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FalsePath : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        print("We have made contact");
        if (collision.gameObject.name == "Player")
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        print("We have made contact with controllercollider");
    }

}
