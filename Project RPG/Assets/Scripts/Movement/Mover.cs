using RPG.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace RPG.Movement
{
    public class Mover : MonoBehaviour, IAction
    {
        private NavMeshAgent navMeshAgent;
        private Animator animator;
        private Health health;
        private int i = 0;
        private bool isDead = false;

        void Start()
        {
            navMeshAgent = GetComponent<NavMeshAgent>();
            animator = GetComponent<Animator>();
            health = GetComponent<Health>();
        }

        void Update()
        {
            if (!isDead)
            {
                if (health.IsDead())
                {
                    isDead = true;
                    navMeshAgent.enabled = false;
                }
                else
                {
                    UpdateAnimator();
                }
            }
        }

        private void UpdateAnimator()
        {
            // We use transform.InverseTransformDirection to get the local Velocity of the player so
            // we can use its z value to change the move speed (blend value)
            animator.SetFloat("moveSpeed", 
                transform.InverseTransformDirection(navMeshAgent.velocity).z);
        }

        public void StartMoveAction(Vector3 destination)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            MoveTo(destination);
        }

        public void MoveTo(Vector3 destination)
        {
            navMeshAgent.destination = destination;
            navMeshAgent.isStopped = false;
        }

        public void Cancel()
        {
            navMeshAgent.isStopped = true;
        }
    }
}
