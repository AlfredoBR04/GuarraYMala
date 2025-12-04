using UnityEngine;
using System.Collections.Generic;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance; 
    public bool isPlayerTurn = true;
    public List<Units> enemyUnits = new List<Units>(); 
    public List<Units> playerUnits = new List<Units>(); 

    // --- AÑADIDO (mínimo necesario) ---
    public Units selectedUnit = null;
    public bool selectionLocked = false;
    // -----------------------------------

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

        // liberar selección al empezar turno
        selectedUnit = null;
        selectionLocked = false;

        Debug.Log("Player's Turn Started");
    }

    private void StartEnemyTurn()
    {
        isPlayerTurn = false;
        ResetUnits(enemyUnits);

        // liberar selección al pasar a enemigo
        selectedUnit = null;
        selectionLocked = false;

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

    // --- AÑADIDO (selección mínima) ---
    public bool TrySelectUnit(Units unit)
    {
        if (!isPlayerTurn) return false;     // no en turno enemigo
        if (selectionLocked) return false;   // ya hay unidad seleccionada
        if (unit.hasActed) return false;     // ya actuó

        selectedUnit = unit;
        selectionLocked = true;
        return true;
    }

    // --- AÑADIDO (liberar selección al terminar turno) ---
    public void UnitFinishedTurn(Units unit)
    {
        unit.hasActed = true;

        // liberar selección
        selectedUnit = null;
        selectionLocked = false;

        CheckEndTurn();
    }
}