using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Labyrinth
{
    public class Player : MonoBehaviour
    {

        enum MoveState
        {
            idle, walk, run, reverse, strafeL, strafeR
        }

        enum ActionState
        {
            attack
        }

    }

}
