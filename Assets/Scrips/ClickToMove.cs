using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ClickToMove : MonoBehaviour
{
    [Header("Movement Control")]
    Vector3 destination;
    [SerializeField] public Transform destinationDummie;
    private NavMeshAgent agent;
    Animator animator;
    bool isSelectingDestination = false;
    Units unit;
    void OnEnable()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        unit = GetComponent<Units>();

        agent.updatePosition = false;
        //agent.destination = transform.position;   //  para evitar errores al activarse
    }

     public void EnableMoveMode()
    {
        isSelectingDestination = true;
    }

    void Update()
    {
        // --- CLICK PARA MOVER ---
        if (Input.GetMouseButtonDown(1)) // botón derecho del mouse
        {
            HandleClick();
            isSelectingDestination = false;
        }

        // --- ANIMACION ---
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
        // --- TERMINAR MOVIMIENTO AL LLEGAR ---
        /*if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                // Llama a FinishMove automáticamente
                Units unit = GetComponent<Units>();
                if (!unit.hasMoved)   // solo si no lo ha hecho aún
                {
                    unit.FinishMovement();
                }

                // Desactivar el ClickToMove para no seguir moviendo
                this.enabled = false;
            }
        }*/
    }

    public void HandleClick()
    {
        Debug.Log("Click detected, handling movement...");
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Visualizar el raycast que sale de la cámara
        Debug.DrawRay(ray.origin, ray.direction * 100f, Color.yellow, 1f);

        if (Physics.Raycast(ray, out hit, 100f))
        {
            // Dibuja la línea hasta el punto de impacto en Scene view
            Debug.DrawLine(ray.origin, hit.point, Color.green, 2f);

            destinationDummie.position = hit.point;
            agent.SetDestination(destinationDummie.position);
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