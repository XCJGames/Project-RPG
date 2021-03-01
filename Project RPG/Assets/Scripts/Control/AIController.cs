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
        [SerializeField] float dwellingTime = 2f;
        [SerializeField] PatrolPath patrolPath;
        [SerializeField] float waypointTolerance = 1f;
        [SerializeField] float positionOffset = 10f;

        GameObject player = null;
        Fighter fighter;
        Health health;
        Mover mover;

        bool isDead = false;
        Vector3 guardPosition;
        float timeSinceLastChase = Mathf.Infinity;
        float timeSinceWaypointReached = 0;
        int currentWaypoint = 0;
        float[] rotateIntervals = { 0.33f, 0.66f };
        int currentRotate = 0;

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
                        PatrolBehaviour();
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

        private void PatrolBehaviour()
        {
            Vector3 nextPosition = guardPosition;

            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    if (timeSinceWaypointReached > dwellingTime)
                    {
                        CycleWaypoint();
                        timeSinceWaypointReached = 0;
                        currentRotate = 0;
                    }
                    timeSinceWaypointReached += Time.deltaTime;
                    RotateRandomly();
                }
                nextPosition = GetCurrentWaypoint();
            }

            mover.StartMoveAction(nextPosition);
        }

        private void RotateRandomly()
        {
            for (int i = 0; i < rotateIntervals.Length && currentRotate < rotateIntervals.Length; i++)
            {
                if (i == currentRotate &&
                    timeSinceWaypointReached >= dwellingTime * rotateIntervals[i])
                {
                    currentRotate++;
                    Vector3 randomPosition = GetRandomPosition();
                    Debug.Log("randomPosition: " + randomPosition);
                    transform.LookAt(randomPosition);
                    return;
                }
                else if (i > currentRotate) return;
            }
        }

        private Vector3 GetRandomPosition()
        {
            float x = UnityEngine.Random.Range(transform.position.x - positionOffset,
                transform.position.x + positionOffset);
            float z = UnityEngine.Random.Range(transform.position.z - positionOffset,
                transform.position.z + positionOffset);
            return new Vector3(x, transform.position.y, z);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint <= waypointTolerance;
        }

        private void CycleWaypoint()
        {
            currentWaypoint = patrolPath.GetNextIndex(currentWaypoint);
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypoint);
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
