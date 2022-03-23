using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CharacterAnimation : MonoBehaviour
{
    Animator animator;
    float velocityZ = 0.0f;
    float velocityX = 0.0f;
    public float acceleration = 2.0f;
    public float deceleration = 2.0f;
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {
        bool forwardPressed = Input.GetKey("w");
        bool leftPressed = Input.GetKey("a");
        bool rightPressed = Input.GetKey("d");
        bool backPressed = Input.GetKey("s");
        bool runPressed = Input.GetKey("left shift");


        if (forwardPressed && velocityX < 1.0f && !runPressed)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (rightPressed && velocityZ > -1.0f && !runPressed)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityZ < 1.0f && !runPressed)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ = 0.0f;
        }

        animator.SetFloat("velocityZ", velocityZ);
        animator.SetFloat("velocityX", velocityX);



    }

}
