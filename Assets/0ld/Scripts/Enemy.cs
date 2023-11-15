using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Old_Labyrinth
{

    public class Enemy : MonoBehaviour
    {
        [SerializeField] [Tooltip("Velocity per second")] float gravity = 20f, normalForce = 1f;

        [SerializeField] float moveSpeed = 7f, nodeOffset = 1f, pauseDuration = 3f, detectionRange = 5f;
        [SerializeField] Transform[] node;
        Vector3[] nodePosition;

        int n;

        bool inRange;

        float gravVelocity, pauseTimer;

        Vector3 moveDirection;

        CharacterController enemy;

        Transform player;

        void Start()
        {
            enemy = GetComponent<CharacterController>();
            player = GameObject.Find("Player").transform;
            // Remember child node's position in worldspace
            nodePosition = new Vector3[node.Length];
            for (int i = 0; i < node.Length; i++) { nodePosition.SetValue(node[i].position, i); }
        }

        void Update()
        {
            CheckForPlayer();
            UpdatePatrol();
            UpdateMovement();
            UpdateGravity();
        }

        // Patrol between x amount of nodes
        void UpdatePatrol()
        {
            if (!inRange)
            {
                if (transform.position.x < node[n].position.x - nodeOffset || transform.position.x > node[n].position.x + nodeOffset ||
                                transform.position.z < node[n].position.z - nodeOffset || transform.position.z > node[n].position.z + nodeOffset)
                { moveDirection = new Vector3(node[n].position.x - transform.position.x, moveDirection.y, node[n].position.z - transform.position.z).normalized; }
                else if (transform.position.x > node[n].position.x - nodeOffset &&
                         transform.position.x < node[n].position.x + nodeOffset)
                { moveDirection = Vector3.zero; if (n < node.Length - 1) n++; else n = 0; pauseTimer = pauseDuration; }

                // Maintain child node's position in worldspace
                for (int i = 0; i < node.Length; i++) { node[i].position = new Vector3(nodePosition[i].x, nodePosition[i].y, nodePosition[i].z); }
            }
        }

        void UpdateMovement()
        {
            if (pauseTimer <= 0) { enemy.Move(moveSpeed * Time.deltaTime * moveDirection); } else { pauseTimer -= Time.deltaTime; }
        }

        void CheckForPlayer()
        {
            inRange = Vector3.Distance(player.position, transform.position) < detectionRange;
        }

        // The character moves downward (negative up) at velocity (gravVelocity) per second (deltaTime)
        void UpdateGravity()
        {
            enemy.Move(gravVelocity * Time.deltaTime * -transform.up);  // Move() must be called before isGrounded              <--|| required for proper   ||
            if (enemy.isGrounded) { gravVelocity = normalForce; }       // A static normal force must be applied when grounded  <--|| ground detection      ||
            else { gravVelocity += gravity * Time.deltaTime; } // increase gravVelocity by meters (gravity) per second (deltaTime)
        }

        // If player seen run towards player
        // If exits patrol range returns to patrol
        // If enters attack range attacks player
    }
}