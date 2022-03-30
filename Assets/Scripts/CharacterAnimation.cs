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
    public float maxWalkVelocity = 1.0f;
    public float maxRunVelocity = 2.0f;

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

        //set current max velocity
        float currentMaxVelocity = runPressed ? maxRunVelocity : maxWalkVelocity;

        //button presses
        if (forwardPressed && velocityX < currentMaxVelocity)
        {
            velocityZ += Time.deltaTime * acceleration;
        }

        if (rightPressed && velocityZ > -currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * acceleration;
        }

        if (leftPressed && velocityZ < currentMaxVelocity)
        {
            velocityX += Time.deltaTime * acceleration;
        }

        //decelerations
        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ -= Time.deltaTime * deceleration;
        }

        if (!forwardPressed && velocityZ > 0.0f)
        {
            velocityZ = 0.0f;
        }

        if (!rightPressed && velocityX < 0.0f)
        {
            velocityX += Time.deltaTime * deceleration;
        }
       
        if (!leftPressed && velocityX > 0.0f)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
        //if no buttons pressed and velocity isn't zero set to zero
        if (!leftPressed && !rightPressed && velocityX != 0.0f && (velocityX > -0.05f && velocityX < 0.05f))
        {
            velocityX = 0.0f;
        }

        if (forwardPressed && runPressed && velocityX > currentMaxVelocity)
        {
            velocityX = currentMaxVelocity;
        }
        else if (forwardPressed && velocityX > currentMaxVelocity)
        {
            velocityX -= Time.deltaTime * deceleration;
        }
        else if (forwardPressed && velocityX < currentMaxVelocity && velocityX > (currentMaxVelocity - 1.0f))
        {
            velocityX = currentMaxVelocity;
        }
        //set anim floats
        animator.SetFloat("velocityZ", velocityZ);
        animator.SetFloat("velocityX", velocityX);



    }

}
