using System.Runtime.CompilerServices;
using NUnit.Framework.Constraints;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UIElements;

public class ClickToMove : MonoBehaviour
{
    [Header("Movement Control")]
    Vector3 destination;
    Rigidbody rb;
    [SerializeField] Transform destinationDummie;
    private NavMeshAgent agent;
    Animator animator;

    void OnEnable()
    {
        rb = GetComponent<Rigidbody>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        agent.updatePosition = false;
        agent.destination = destinationDummie.position;
    }

    void Update()
    {
        // Input System Antiguo
        if (Input.GetMouseButtonDown(1)) // botón derecho del mouse
        {
            HandleClick();
            Units unit = GetComponent<Units>();
            unit.FinishMove();
        }

        animator.SetFloat("forwardMovement", agent.velocity.magnitude);
    }

    private void HandleClick()
    {
        RaycastHit hit;

        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            destinationDummie.position = hit.point;
            agent.destination = destinationDummie.position;

            hit.collider.GetComponent<Units>();
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 position = animator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;

        
    }
}
