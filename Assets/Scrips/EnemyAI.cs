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
    private float attackRange;
    private bool isActing = false;
    NavMeshAgent agent;
    Animator animator;

    void Start()
    {
        UpdateAttackRange();
    }

    private void Awake()
    {
        unit = GetComponent<Units>();
        shooting = GetComponent<Shooting>();
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
    }

    private void UpdateAttackRange()
    {
        // Obtener el rango del arma equipada
        EnemyCharacter enemyChar = GetComponent<EnemyCharacter>();
        if (enemyChar != null)
        {
            Weapon equippedWeapon = enemyChar.GetEquippedWeapon();
            if (equippedWeapon != null)
            {
                attackRange = equippedWeapon.GetWeaponRange();
            }
            else
            {
                attackRange = 5f; // Rango por defecto
            }
        }
        else
        {
            attackRange = 5f;
        }
    }

    void Update()
    {
        if (unit.isFriendly) return;

        // No actuar si el enemigo está muerto
        Character character = GetComponent<Character>();
        if (character != null && !character.IsAlive())
        {
            if (agent != null)
                agent.enabled = false;
            return;
        }

        if (TurnManager.Instance.isPlayerTurn)
        {
            return;
        }

        // No actuar si ya actuó en este turno
        if (unit.hasActed)
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

        // Si está muerto, pasar turno inmediatamente
        Character character = GetComponent<Character>();
        if (character != null && !character.IsAlive())
        {
            Debug.Log(unit.characterName + " está muerto, pasa turno");
            gameObject.SetActive(false);  // Ocultar el enemigo muerto
            unit.FinishAction();
            isActing = false;
            yield break;
        }

        Units target = FindClosestPlayerUnit();

        if (target == null)
        {
            Debug.Log(unit.characterName + " no encuentra objetivos validos");
            unit.FinishAction();
            isActing = false;
            yield break;
        }

        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

        // Verificar si hay un jugador en rango de ataque inmediatamente
        if (distanceToTarget <= attackRange && hasLineOfSight(target))
        {
            // Detenerse y atacar sin moverse
            agent.isStopped = true;
            yield return AttackTarget(target);
            unit.FinishAction();
            isActing = false;
            yield break;  // Terminar turno después de atacar
        }

        // Si no hay nadie en rango, moverse hacia el objetivo más cercano
        yield return MoveTowardTarget(target.transform.position);

        // Verificar nuevamente después de moverse
        distanceToTarget = Vector3.Distance(transform.position, target.transform.position);
        target = FindClosestPlayerUnit();  // Buscar de nuevo por si cambió

        if (target != null && distanceToTarget <= attackRange && hasLineOfSight(target))
        {
            // Detenerse antes de atacar
            agent.isStopped = true;
            yield return AttackTarget(target);
            // Terminar inmediatamente después del ataque
            unit.FinishAction();
            isActing = false;
            yield break;
        }

        // Si no pudo atacar después de moverse, terminar el turno
        agent.isStopped = true;
        unit.FinishAction();
        isActing = false;
    }


    private IEnumerator MoveTowardTarget(Vector3 targetPosition)
    {
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

        // Obtener el arma del enemigo
        EnemyCharacter enemyChar = GetComponent<EnemyCharacter>();
        float damageDealt = 10f;
        float penetration = 0f;
        string weaponUsed = "puño";
        
        if (enemyChar != null)
        {
            Weapon equippedWeapon = enemyChar.GetEquippedWeapon();
            if (equippedWeapon != null)
            {
                damageDealt = equippedWeapon.GetWeaponDamage();
                penetration = equippedWeapon.GetWeaponPenetration();
                weaponUsed = equippedWeapon.GetWeaponName();
            }
        }

        shooting.Shoot(target.transform.position, attackRange);
        
        // Aplicar daño al jugador
        Character targetCharacter = target.GetComponent<Character>();
        if (targetCharacter != null)
        {
            targetCharacter.TakeDamage(damageDealt, penetration);
            
            // Verificar si el jugador murió
            if (!targetCharacter.IsAlive())
            {
                Debug.Log(target.characterName + " ha muerto por ataque de " + unit.characterName);
                target.enabled = false;
                target.gameObject.SetActive(false);
            }
            else
            {
                Debug.Log(unit.characterName + " causa " + damageDealt + " de daño a " + target.characterName + " con " + weaponUsed);
            }
        }
        
        yield return new WaitForSeconds(0.2f);

        unit.FinishAttack();  // ataca
    }


    private bool hasLineOfSight(Units target)
    {
        return shooting.IsOnLoS(target.transform.position, attackRange);
    }


    private Units FindClosestPlayerUnit()
    {
        Units closest = null;
        float closestDist = Mathf.Infinity;

        foreach (Units playerUnit in TurnManager.Instance.playerUnits)
        {
            if (playerUnit == null) continue;
            
            // Ignorar unidades muertas
            Character playerCharacter = playerUnit.GetComponent<Character>();
            if (playerCharacter != null && !playerCharacter.IsAlive())
                continue;

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