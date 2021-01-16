using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] List<Transform> targets;

    private NavMeshAgent navMeshAgent;
    private Animator animator;
    private int i = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        //FixedMove();
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
        UpdateAnimator();
    }

    private void UpdateAnimator()
    {
        // We use transform.InverseTransformDirection to get the local Velocity of the player so
        // we can use its z value to change the move speed (blend value)
        animator.SetFloat("moveSpeed", 
            transform.InverseTransformDirection(navMeshAgent.velocity).z);
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        bool hasHit = Physics.Raycast(ray, out hit);
        if (hasHit)
        {
            navMeshAgent.destination = hit.point;
        }
    }

    private void FixedMove()
    {
        if(i < targets.Count)
        {
            navMeshAgent.destination = targets[i].position;
            if (transform.position.x == targets[i].position.x && transform.position.z == targets[i].position.z) i++;
        }
    }
}
