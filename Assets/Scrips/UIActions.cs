using UnityEngine;

public class UIActions : MonoBehaviour
{
    // Llamar desde el botón: comprueba la unidad seleccionada y habilita su movimiento por clic
    public void EnableMovementForSelected()
    {
        if (UnitSelection.Instance == null)
        {
            Debug.LogWarning("UIActions: UnitSelection.Instance no existe.");
            return;
        }

        var unit = UnitSelection.Instance.selectedUnit;
        if (unit == null)
        {
            Debug.LogWarning("UIActions: No hay ninguna unidad seleccionada.");
            return;
        }

        // Usa el método público de Units para habilitar movimiento (encapsulación)
        unit.EnableMovement();
        Debug.Log("UIActions: EnableMovement llamado para " + unit.name);
    }
}
