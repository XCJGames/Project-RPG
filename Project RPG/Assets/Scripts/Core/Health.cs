using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class Health : MonoBehaviour
    {
        [SerializeField] float currentHealth = 100f;
        [SerializeField] float maxHealth = 100f;

        Animator animator;
        bool isDead;

        private void Start()
        {
            isDead = false;
            animator = GetComponent<Animator>();
        }

        public void TakeDamage(float damage)
        {
            if(currentHealth > 0)
            {
                currentHealth = Mathf.Clamp(currentHealth - damage, 0, maxHealth);
                if (currentHealth == 0)
                {
                    Die();
                }
            }
        }

        private void Die()
        {
            isDead = true;
            animator.SetTrigger("die");
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        public bool IsDead()
        {
            return isDead;
        }

        public float GetCurrentHealth()
        {
            return currentHealth;
        }
    }
}
