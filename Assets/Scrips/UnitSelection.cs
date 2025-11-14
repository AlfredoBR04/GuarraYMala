using UnityEngine;

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
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100f))
            {
                Units units = hit.collider.GetComponent<Units>();
                if (units != null && units.isFriendly && !units.hasActed)
                {
                    SelectUnit(units);
                    Debug.Log("soy una unidad" + units.name + " seleccionada");
                }
                else
                {
                    DeselectUnit();
                    Debug.Log("no soy una unidad");
                }
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