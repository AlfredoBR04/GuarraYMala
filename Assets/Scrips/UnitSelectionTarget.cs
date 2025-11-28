using UnityEngine;

public class UnitSelectionTargget : MonoBehaviour
{
    [SerializeField] GameObject[] elementsUIToTarget;

    void Update()
    {
        if (UnitSelection.Instance == null)
            return;

        // Guardamos la referencia a la unidad seleccionada.
        var selectedUnit = UnitSelection.Instance.selectedUnit;

        // Desactivar todos los elementos por defecto.
        DesactivarAllUI();

        // Si no hay unidad seleccionada, salimos.
        if (selectedUnit == null)
            return;

        // Según el nombre de la unidad, activamos el UI correspondiente.
        switch (selectedUnit.name)
        {
            case "Ellen":
                if (elementsUIToTarget.Length > 0)
                    elementsUIToTarget[0].SetActive(true);
                break;

            case "Grenadier":
                if (elementsUIToTarget.Length > 1)
                    elementsUIToTarget[1].SetActive(true);
                break;

            default:
                break;
        }
    }

    void DesactivarAllUI()
    {
        for (int i = 0; i < elementsUIToTarget.Length; i++)
        {
            elementsUIToTarget[i].SetActive(false);
        }
    }
}
