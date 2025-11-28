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
    
    void Start()
    {
        
    }

    void Update()
    {
        if (!TurnManager.Instance.isPlayerTurn)
        {
            return;
        }

        if (Input.GetMouseButtonDown(0))
        {
            // Ignorar clics sobre UI
            if (EventSystem.current.IsPointerOverGameObject())
            {
                return;
            }

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Units units = hit.collider.GetComponent<Units>();

                // Verificar que sea una unidad aliada en TurnManager.playerUnits
                bool isPlayerUnit = false;
                if (units != null && TurnManager.Instance != null && TurnManager.Instance.playerUnits != null)
                {
                    isPlayerUnit = TurnManager.Instance.playerUnits.Contains(units) && units.isFriendly;
                }

                if (isPlayerUnit && !units.hasActed)
                {
                    SelectUnit(units);
                    Debug.Log("soy una unidad" + units.name + " seleccionada");
                }
                else if (units != null)
                {
                    // Solo deseleccionar si clicas sobre otra unidad (no seleccionable)
                    DeselectUnit();
                    Debug.Log("unidad no seleccionable");
                }
                // Si no es unidad, mantiene la selección anterior
            }
        }
    }

    private void SelectUnit(Units units)
    {
        selectedUnit = units;
        // Implement unit selection logic here
    }
    
    public void DeselectUnit()
    {
        if(selectedUnit != null)
        {
            selectedUnit = null;
        }
    }
}