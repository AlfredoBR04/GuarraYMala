using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Units))]
[RequireComponent(typeof(Shooting))]
public class EnemyAI : MonoBehaviour
{
    private Units unit;
    private Shooting shooting;
    [SerializeField] private float visionRange = 30f;
    [SerializeField] private float attackRange;
    public float weaponRange;
    private bool isActing = false;
    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        attackRange = weaponRange;
    }

    private void Awake()
    {
        unit = GetComponent<Units>();
        shooting = GetComponent<Shooting>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        if (unit.isFriendly) return;

        if (TurnManager.Instance.isPlayerTurn)
        {
            return;
        }

        if (!isActing)
        {
            StartCoroutine(DoenemyTurn());
        }
    }

    IEnumerator DoenemyTurn()
    {
        isActing = true;

        Units target = FindClosestPlayerUnit();

        if (target == null)
        {
            Debug.Log(unit.characterName + " no encuentra objetivos validos");
            unit.FinishAction();
            isActing = false;
            yield break;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        // Intentar atacar inmediatamente
        if (distanceToTarget <= attackRange && hasLineOfSight(target))
        {
            yield return AttackTarget(target);
            unit.FinishAction();
        }
        else
        {
            // Moverse hacia el objetivo
            yield return MoveTowardTarget(target.transform.position);

            // Intentar atacar otra vez
            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget <= attackRange && hasLineOfSight(target))
            {
                yield return AttackTarget(target);
            }
        }

        // terminar el turno
        unit.FinishAction();
        isActing = false;
    }


    private IEnumerator MoveTowardTarget(Vector3 targetPosition)
    {
        Debug.Log(unit.characterName + " se mueve buscando a su objetivo:");

        
        agent.isStopped = false;
        agent.destination = targetPosition;

        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            animator.SetFloat("forwardMovement", agent.velocity.magnitude);
            yield return null;
        }

        agent.isStopped = true;
        animator.SetFloat("forwardMovement", 0f);
        unit.FinishMovement(); // para de moverse
    }


    private IEnumerator AttackTarget(Units target)
    {
        Debug.Log(unit.characterName + " ataca a " + target.characterName);

        Vector3 lookDir = target.transform.position - transform.position;
        lookDir.y = 0f;
        if (lookDir != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDir);
        }

        shooting.Shoot(target.transform.position, attackRange);
        yield return new WaitForSeconds(0.2f);

        unit.FinishAttack();  // ataca
    }


    private bool hasLineOfSight(Units target)
    {
        return shooting.IsOnLoS(target.transform.position, weaponRange);
    }


    private Units FindClosestPlayerUnit()
    {


        Units closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Units playerUnit in TurnManager.Instance.playerUnits)
        {
            float dist = Vector3.Distance(transform.position, playerUnit.transform.position);
            if (dist < closestDist && dist <= visionRange)
            {
                closestDist = dist;
                closest = playerUnit;
            }
        }

        return closest;

    }
}