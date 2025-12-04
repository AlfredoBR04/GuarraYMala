using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    [Header("Movement Control")]
    [SerializeField] public Transform destinationDummie;
    private NavMeshAgent agent;
    private Animator animator;
    private Units unit;

    private bool hasMovedOnce = false; //controla si ya se movió una vez

    // AÑADIDO PARA LA PREVISUALIZACIÓN
    [Header("Path Preview")]
    public LineRenderer lineRenderer;
    private NavMeshPath previewPath;

    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        unit = GetComponent<Units>();
        agent.updatePosition = false;

        // AÑADIDO PARA LA PREVISUALIZACIÓN
        previewPath = new NavMeshPath();
        if (lineRenderer != null)
            lineRenderer.positionCount = 0;
    }

    void Update()
    {
        //CLICK PARA MOVER SOLO UNA VEZ
        if (Input.GetMouseButtonDown(1) && !hasMovedOnce)
        {
            HandleClick();
            hasMovedOnce = true; // ya no podrá moverse más
        }

        //ANIMACION 
        animator.SetFloat("forwardMovement", agent.velocity.magnitude);

        if (!unit.hasMoved && agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending && agent.hasPath)
        {
            unit.FinishMovement();
        }

        if (agent.remainingDistance <= agent.stoppingDistance && !agent.pathPending)
        {
            animator.SetFloat("forwardMovement", 0f);
        }
        else
        {
            animator.SetFloat("forwardMovement", agent.velocity.magnitude);
        }

        // BORRAR LA LÍNEA CUANDO LLEGA
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (lineRenderer != null)
                lineRenderer.positionCount = 0;
        }
    }

    public void HandleClick()
    {
        Debug.Log("Click detected, handling movement...");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow, 1f);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            Debug.DrawLine(ray.origin, hit.point, Color.green, 2f);
            destinationDummie.position = hit.point;
            agent.SetDestination(destinationDummie.position);

            //PREVISUALIZAR LA RUTA
            if (lineRenderer != null)
            {
                if (agent.CalculatePath(destinationDummie.position, previewPath))
                {
                    Vector3[] elevatedCorners = previewPath.corners;

                    // SUBIR LIGERAMENTE LA LÍNEA PARA QUE SE VEA
                    for (int i = 0; i < elevatedCorners.Length; i++)
                        elevatedCorners[i].y += 0.05f;

                    lineRenderer.positionCount = elevatedCorners.Length;
                    lineRenderer.SetPositions(elevatedCorners);
                }
            }
        }
    }

    private void OnAnimatorMove()
    {
        Vector3 position = animator.rootPosition;
        position.y = agent.nextPosition.y;
        transform.position = position;
        agent.nextPosition = transform.position;
    }
}