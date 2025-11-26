using UnityEngine;
using System.Collections;
using System;
using UnityEngine.AI;

[RequireComponent(typeof(Units))]
[RequireComponent(typeof(Shooting))]

public class EnemyAI : MonoBehaviour
{
    private Units units;
    private Shooting shooting;
    [SerializeField] float visionRange = 10f;
    private float attackRange;
    NavMeshAgent agent;

    void Awake()
    {
        units = GetComponent<Units>();
        shooting = GetComponent<Shooting>();
    }

    void Update()
    {
        if (units.isFriendly)
        {
            return;
        }
        if (TurnManager.Instance.isPlayerTurn)
        {
            //unit.hasActed = true;
            return;
        }
        if (!units.hasActed)
        {
            // Lógica simple de IA: buscar la unidad enemiga más cercana y disparar si está en línea de visión
            StartCoroutine(DoEnemyTurn());
        }
    }


    private IEnumerator DoEnemyTurn()
    {
        Units target = FindClosestPlayerUnit();
        if (target == null)
        {
            Debug.Log(units.characterName + ": No player units found, skipping turn.");
            units.FinishAction();
            yield break;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        if (distanceToTarget <= attackRange && hasLineOfSight(target))
        {
            yield return AttackTarget(target);
        }
        else //Muevo al personaje para que este en la linea de vision
        {
            yield return MoveTowardTarget(target.transform.position);

            //Vuelvo a intentar disparar al personaje

            distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

            if (distanceToTarget <= attackRange && hasLineOfSight(target))
            {
                yield return AttackTarget(target);
            }
            else
                units.FinishAction();
        }



    }

    private IEnumerable MoveTowardTarget(Vector3 position)
    {
        Debug.Log(unit.characterName + "Se mueve buscando a su objetivo");


        agent.destination = targetPosition;
        yield return new WaitForSeconds(5);
        units.FinishMove();
    }


    private bool hasLineOfSight(Units target)
    {
        return shooting.isOnLoS(target.transform.position, attackRange);
    }


    private IEnumerator AttackTarget(Units target)
    {
        Debug.Log(units.characterName + " is attacking " + target.characterName);

        Vector3 lookDirection = target.transform.position - transform.position;
        lookDirection.y = 0;
        if (lookDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(lookDirection);
        }

        shooting.Shoot(target.transform.position, attackRange);
        units.hasActed = true;

        yield return new WaitForSeconds(1f); // Espera 1 segundo para simular el tiempo de ataque

        if (units.hasMoved)
        {
            units.FinishAttack();
            units.FinishAction

        }
        else
        {
            units.FinishAttack();
        }
    }



    private Units FindClosestPlayerUnit()
    {
        Units closest = null;
        float closestDistance = Mathf.Infinity;
        foreach (Units playerUnit in TurnManager.Instance.playerUnits)
        {
            float distance = Vector3.Distance(transform.position, playerUnit.transform.position);
            if (distance < closestDistance && distance <= visionRange)
            {
                closestDistance = distance;
                closest = playerUnit;
            }
        }
        return closest;
    }
}