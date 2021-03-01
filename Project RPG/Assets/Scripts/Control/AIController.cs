using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float chaseDistance = 5f;
        [SerializeField] float suspicionTime = 3f;

        GameObject player = null;
        Fighter fighter;
        Health health;
        Mover mover;

        bool isDead = false;
        Vector3 guardPosition;
        float timeSinceLastChase = Mathf.Infinity;

        private void Start()
        {
            player = GameObject.FindWithTag("Player");
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
            mover = GetComponent<Mover>();

            guardPosition = transform.position;
        }
        private void Update()
        {
            if (!health.IsDead())
            {
                if (player != null && !isDead)
                {
                    if (DistanceToPlayer() <= chaseDistance && !PlayerIsDead())
                    {
                        CombatBehaviour();
                    }
                    else if (timeSinceLastChase < suspicionTime)
                    {
                        SuspicionBehaviour();
                    }
                    else
                    {
                        GuardBehaviour();
                    }
                }    
            }
            timeSinceLastChase += Time.deltaTime;
        }

        private void SuspicionBehaviour()
        {
            //fighter.Cancel();
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private bool PlayerIsDead()
        {
            return player.GetComponent<Health>().IsDead();
        }

        private void GuardBehaviour()
        {
            mover.StartMoveAction(guardPosition);
        }

        private void CombatBehaviour()
        {
            timeSinceLastChase = 0;
            fighter.Attack(player);
        }

        private float DistanceToPlayer()
        {
            return Vector3.Distance(transform.position, player.transform.position);
        }

        /// <summary>
        /// Called by Unity
        /// </summary>
        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, chaseDistance);
        }
    }
}
