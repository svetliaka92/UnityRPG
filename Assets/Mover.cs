using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Mover : MonoBehaviour
{
    [SerializeField] private Transform target;

    private Animator animator;
    private NavMeshAgent agent;

    private int forwardSpeedId = Animator.StringToHash("ForwardSpeed");
    
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }
    
    void Update()
    {
        if (Input.GetMouseButton(0))
            MoveToCursor();

        UpdateAnimator();
    }

    private void MoveToCursor()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;

        bool hasHit = Physics.Raycast(ray, out hitInfo, 100f);
        if (hasHit)
            agent.SetDestination(hitInfo.point);
    }

    private void UpdateAnimator()
    {
        Vector3 velocity = agent.velocity;
        Vector3 localVelocity = transform.InverseTransformDirection(velocity);

        float speed = localVelocity.z;

        animator.SetFloat(forwardSpeedId, speed);
    }
}
