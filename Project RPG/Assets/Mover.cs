using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] List<Transform> targets;

    private NavMeshAgent navMeshAgent;
    private int i = 0;

    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        //FixedMove();
        if (Input.GetMouseButtonDown(0))
        {
            MoveToCursor();
        }
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
