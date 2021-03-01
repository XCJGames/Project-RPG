using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Combat
{
    public class Fighter : MonoBehaviour, IAction
    {
        [SerializeField] float weaponRange = 2f;
        [SerializeField] float timeBetweenAttacks = 1f;
        [SerializeField] float damage = 5f;

        float timeSinceLastAttack = Mathf.Infinity;

        Transform target;
        Animator animator;

        private void Start()
        {
            animator = GetComponent<Animator>();
        }

        void Update()
        {
            timeSinceLastAttack += Time.deltaTime;
            if (target == null) return;
            if (!TargetIsInRange())
            {
                GetComponent<Mover>().MoveTo(target.position);
            }
            else
            {
                GetComponent<Mover>().Cancel();
                AttackBehaviour();
            }
        }

        private void AttackBehaviour()
        {
            if(CanAttack())
            {
                transform.LookAt(target);
                if (timeSinceLastAttack > timeBetweenAttacks)
                {
                    animator.ResetTrigger("stopAttack");
                    animator.SetTrigger("attack");
                    timeSinceLastAttack = 0;
                    // This will trigger Hit Animation Event
                }
            }
        }

        private bool CanAttack()
        {
            return target != null && TargetIsAlive();
        }

        private void DoDamageToTarget()
        {
            if(target != null)
            {
                target.GetComponent<Health>().TakeDamage(damage);
            }
        }

        private bool TargetIsInRange()
        {
            return Vector3.Distance(transform.position, target.position) <= weaponRange;
        }

        private bool TargetIsAlive()
        {
            return !target.GetComponent<Health>().IsDead();
        }

        public void Attack(GameObject ct)
        {
            if(!ct.GetComponent<Health>().IsDead())
            {
                GetComponent<ActionScheduler>().StartAction(this);
                target = ct.transform;
            }
        }

        public void Cancel()
        {
            target = null;
            animator.ResetTrigger("attack");
            animator.SetTrigger("stopAttack");
        }

        /// <summary>
        /// Animation Event
        /// </summary>
        public void Hit()
        {
            DoDamageToTarget();
        }
    }
}
