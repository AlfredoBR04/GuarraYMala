using System;
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent (typeof(Unit))]
[RequireComponent (typeof(Shooting))]

public class EnemyAI : MonoBehaviour
{

    private Units unit;
    private Shooting shooting;
    [SerializeField]private float visionRange = 30f;
    private float attackRange;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    private void Awake()
    {
        unit = GetComponent<Units>();
        shooting = GetComponent<Shooting>();

    }

    // Update is called once per frame
    void Update()
    {
        //compruebo que la IA sea Enemiga y no una unidad aliada
        if (unit.isFriendly) return; 

        //Si es el turno del jugador, entonces no podemos acutar
        if(TurnManager.Instance.isPlayerTurn)
        {
           
           return;
        }
        // si es el turno del enemigo y esta unidad no ha ejecutado su logica.
        if (!unit.hasActed)
        {
            StartCoroutine(DoenemyTurn());
        }
    }
    IEnumerator DoenemyTurn()
    {
        //Encuentra el persinaje aliado mas cercano para atacarle
        Unit target = FindClosetPlayerUnit();

        //Si no tenemos enemigos, saltamos turno
        if(target == null)
        {
            Debug.Log(unit.characterName + "No encuentra objetivo validos, salta el turno");
            unit.FinishAction();
            yield break;
        }

        //Si esta en linea de vision, le ataco.
        float distanceToTarget = Vector3.Distance(transform.position, target.transform.position);

       if (distanceToTarget <= attackRange && hasLineOfSight(target))
        {
            yield return AttackTarget(target);
        }
    }

    private IEnumerator AttackTarget(Unit target)
    {
        throw new NotImplementedException();
    }

    private bool hasLineOfSight(Unit target)
    {
        throw new NotImplementedException();
    }

    //funcion que calcula y devuelve cual es la unidad aliada
    //mas cercana para atacar dentro de un rango de vision
    private Unit FindClosetPlayerUnit()
    {
        Unit closest = null;
        float closestDist = Mathf.Infinity;

        foreach(Unit playerUnit in TurnManager.Instance.playerUnits)
        {
            float dist = Vector3.Distance(transform.position, playerUnit.transform.position);
            
            if(dist < closestDist && dist <= visionRange)
            {
                closestDist = dist;
                closest = playerUnit;
            }
        }

        return closest;
    }
}
