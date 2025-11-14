using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance; 
    public bool isPlayerTurn = true;
    public List<Units> enemyUnits = new List<Units>(); 
    public List<Units> playerUnits = new List<Units>(); 

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        StartPlayerTurn();
    }

    private void StartPlayerTurn()
    {
        isPlayerTurn = true;
        ResetUnits(playerUnits);
        Debug.Log("Player's Turn Started");
    }

    private void StartEnemyTurn()
    {
        isPlayerTurn = false;
        ResetUnits(enemyUnits);
        Debug.Log("Enemy's Turn Started");
    }

    private void ResetUnits(List<Units> units)
    {
        foreach (Units unit in units)
            unit.hasActed = false;
    }

    private bool AllUnitsActed(List<Units> units)
    {
        foreach (Units u in units)
            if (!u.hasActed)
                return false;

        return true;
    }

    public void CheckEndTurn()
    {
        if (isPlayerTurn)
        {
            if (AllUnitsActed(playerUnits))
                StartEnemyTurn();
        }
        else
        {
            if (AllUnitsActed(enemyUnits))
                StartPlayerTurn();
        }
    }
}
