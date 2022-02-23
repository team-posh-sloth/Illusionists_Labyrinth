using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeEnemy : MonoBehaviour
{
    public GameObject enemySet;
    [SerializeField] GameObject tokenPrefab;

    public bool isReal;

    [SerializeField] float gravity = 20f, normalForce = 1f, moveSpeed = 7f, destinationOffset = 10f, homeOffset = 1f, homeRange = 20f;

    [SerializeField] int hitPointMax = 3;
    public int hitPoints;

    float gravVelocity;

    bool followPlayer, returnHome, attackPlayer;

    Vector3 moveDirection;
    public Vector3 homePosition;

    CharacterController enemy;

    Animator anim;

    Transform player;

    void Start()
    {
        enemy = GetComponent<CharacterController>();
        moveDirection = new Vector3();
        homePosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
        anim = GetComponent<Animator>();
        hitPoints = hitPointMax;
    }

    void Update()
    {
        UpdateGravity();
        UpdateMovement();
        FollowPlayer();
        AttackPlayer();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            followPlayer = true;
            player = other.gameObject.transform;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.name == "Player")
        {
            followPlayer = false;
            returnHome = true;
        }
    }

    public void DropToken()
    {
        Instantiate(tokenPrefab, new Vector3(transform.position.x, transform.position.y + 1, transform.position.z + 1), tokenPrefab.transform.rotation);
    }

    void FollowPlayer()
    {
        if (Vector3.Distance(transform.localPosition, homePosition) > homeRange)
        {
            followPlayer = false;
            returnHome = true;
        }
        if (followPlayer)
        {
            enemy.transform.LookAt(new Vector3(player.position.x, 0f, player.position.z));
            if (Vector3.Distance(transform.localPosition, player.localPosition) > destinationOffset)
            {
                anim.SetBool("Walking", true);
                moveDirection = transform.forward;
            }
            else { moveDirection = new Vector3(); anim.SetBool("Walking", false); attackPlayer = true; }
        }
        else
        {
            if (returnHome)
            {
                enemy.transform.LookAt(new Vector3(homePosition.x, 0f, homePosition.z));
                if (Vector3.Distance(transform.localPosition, homePosition) > homeOffset)
                {
                    anim.SetBool("Walking", true);
                    moveDirection = transform.forward;
                }
                else { moveDirection = new Vector3(); enemy.transform.LookAt(new Vector3(player.position.x, 0f, player.position.z)); returnHome = false; anim.SetBool("Walking", false); }
            }
        }
    }

    void AttackPlayer()
    {
        if (attackPlayer)
        {
            anim.Play("Kick");
            attackPlayer = false;
        }
    }

    void UpdateGravity()
    {
        enemy.Move(gravVelocity * Time.deltaTime * -transform.up);  // Move() must be called before isGrounded              <--|| required for proper   ||
        if (enemy.isGrounded) { gravVelocity = normalForce; }       // A static normal force must be applied when grounded  <--|| ground detection      ||
        else { gravVelocity += gravity * Time.deltaTime; } // increase gravVelocity by meters (gravity) per second (deltaTime)
    }


    void UpdateMovement()
    {
        enemy.Move(moveSpeed * Time.deltaTime * moveDirection);
    }

    public void ShuffleReal() // Redetermines which of the enemy set is real
    {
        int randomEnemy = Random.Range(0, enemySet.GetComponentsInChildren<MazeEnemy>().Length);

        for (int n = 0; n < enemySet.GetComponentsInChildren<MazeEnemy>().Length; n++)
        {
            print("Should this one be real? Random Enemy is: " + randomEnemy);
            if (n == randomEnemy)
            {
                print("Yes");
                enemySet.GetComponentsInChildren<MazeEnemy>()[n].isReal = true;
            }
            else
            {
                print("No");
                enemySet.GetComponentsInChildren<MazeEnemy>()[n].isReal = false;
            }
        }

    }

}
