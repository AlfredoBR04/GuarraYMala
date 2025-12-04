using UnityEngine;
using UnityEngine.EventSystems;

public class UnitSelection : MonoBehaviour
{
    public static UnitSelection Instance;
    public Units selectedUnit;

    private void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (!TurnManager.Instance.isPlayerTurn)
            return;

        // ⬅ Usamos el lock del TURNMANAGER
        if (TurnManager.Instance.selectionLocked)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            if (EventSystem.current.IsPointerOverGameObject())
                return;

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Units unit = hit.collider.GetComponent<Units>();

                if (unit != null &&
                    unit.isFriendly &&
                    TurnManager.Instance.playerUnits.Contains(unit) &&
                    !unit.hasActed)
                {
                    SelectUnit(unit);
                }
            }
        }
    }

    private void SelectUnit(Units unit)
    {
        if (selectedUnit != null)
            selectedUnit.EnableActionButtons(false);

        selectedUnit = unit;

        // ⬅ Bloquear selección desde el TurnManager
        TurnManager.Instance.selectionLocked = true;
        TurnManager.Instance.selectedUnit = unit;

        foreach (Units u in TurnManager.Instance.playerUnits)
        {
            if (u == unit)
                u.EnableActionButtons(true);
            else
                u.EnableActionButtons(false);
        }

        Debug.Log("Unidad seleccionada: " + unit.characterName);
    }

    public void PassTurn()
    {
        if (selectedUnit != null)
        {
            selectedUnit.hasActed = true;
            selectedUnit.EnableActionButtons(false);
        }

        selectedUnit = null;

        // ⬅ Liberar selección desde el TurnManager
        TurnManager.Instance.selectionLocked = false;
        TurnManager.Instance.selectedUnit = null;

        foreach (Units u in TurnManager.Instance.playerUnits)
            u.EnableActionButtons(false);

        TurnManager.Instance.CheckEndTurn();
    }

    public Units GetSelectedUnit()
    {
        return selectedUnit;
    }
}