using RPG.Combat;
using RPG.Core;
using RPG.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        Health health;
        private void Start()
        {
            health = GetComponent<Health>();
        }
        void Update()
        {
            if (!health.IsDead())
            {
                if (InteractWithCombat()) return;
                if (InteractWithMovement()) return;
            }
        }

        private bool InteractWithCombat()
        {
            RaycastHit[] hits = Physics.RaycastAll(GetMouseRay());
            foreach(RaycastHit hit in hits)
            {
                hit.transform.gameObject.TryGetComponent(out CombatTarget ct);
                if (ct != null && !ct.GetComponent<Health>().IsDead())
                {
                    if (Input.GetMouseButton(1))
                    {
                        GetComponent<Fighter>().Attack(ct.gameObject);
                    }
                    return true;
                }
            }
            return false;
        }

        private bool InteractWithMovement()
        {
            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (hasHit)
            {
                // GetMouseButtonDown() returns true only the frame the button is pressed
                // GetMouseButton() returns true every frame the button is pressed
                if (Input.GetMouseButton(1))
                {
                    GetComponent<Mover>().StartMoveAction(hit.point);
                }
                return true;
            }
            return false;
        }

        private static Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
