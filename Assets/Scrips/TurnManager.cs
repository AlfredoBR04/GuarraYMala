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

        Debug.Log("Turno del jugador iniciado");
    }

    private void StartEnemyTurn()
    {
        isPlayerTurn = false;
        ResetUnits(enemyUnits);

        // liberar selección al pasar a enemigo
        selectedUnit = null;
        selectionLocked = false;

        Debug.Log("Turno enemigo iniciado");
    }

    private void ResetUnits(List<Units> units)
    {
        foreach (Units unit in units)
        {
            if (unit != null)
            {
                // Solo resetear unidades vivas
                Character character = unit.GetComponent<Character>();
                if (character != null && character.IsAlive())
                {
                    unit.StartTurnForThisUnit(); // Resetea hasActed, hasMoved y hasAttacked
                }
                else
                {
                    unit.hasActed = true; // Unidades muertas se marcan como que ya actuaron
                }
            }
        }
    }

    private bool AllUnitsActed(List<Units> units)
    {
        foreach (Units u in units)
        {
            if (u != null)
            {
                // Solo contar unidades vivas
                Character character = u.GetComponent<Character>();
                if (character != null && character.IsAlive() && !u.hasActed)
                    return false;
            }
        }

        return true;
    }

    private bool AllUnitsDead(List<Units> units)
    {
        foreach (Units u in units)
        {
            if (u != null)
            {
                Character character = u.GetComponent<Character>();
                if (character != null && character.IsAlive())
                    return false;
            }
        }
        return true;
    }

    public void CheckEndTurn()
    {
        // Verificar victoria o derrota
        if (AllUnitsDead(playerUnits))
        {
            Debug.Log("¡DERROTA! Todos los aliados han muerto");
            // Aquí puedes añadir lógica de Game Over
            return;
        }

        if (AllUnitsDead(enemyUnits))
        {
            Debug.Log("¡VICTORIA! Todos los enemigos han sido eliminados");
            // Aquí puedes añadir lógica de Victoria
            return;
        }

        // Continuar alternando turnos
        if (isPlayerTurn)
        {
            if (AllUnitsActed(playerUnits))
            {
                Debug.Log("Todos los aliados han actuado. Turno enemigo.");
                StartEnemyTurn();
            }
        }
        else
        {
            if (AllUnitsActed(enemyUnits))
            {
                Debug.Log("Todos los enemigos han actuado. Turno jugador.");
                StartPlayerTurn();
            }
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