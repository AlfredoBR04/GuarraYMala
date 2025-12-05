using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponSelectionUI : MonoBehaviour
{
    public GameObject weaponButtonPrefab;
    public Transform weaponButtonContainer;
    private Units currentUnit;

    public void ShowWeaponSelection(Units unit)
    {
        Debug.Log("ShowWeaponSelection llamado para " + unit.characterName);
        currentUnit = unit;
        
        if (weaponButtonPrefab == null)
        {
            Debug.LogError("weaponButtonPrefab no está asignado en el Inspector!");
            return;
        }
        
        if (weaponButtonContainer == null)
        {
            Debug.LogError("weaponButtonContainer no está asignado en el Inspector!");
            return;
        }
        
        // Limpiar botones anteriores
        foreach (Transform child in weaponButtonContainer)
        {
            Destroy(child.gameObject);
        }

        PlayerCharacter playerChar = unit.GetComponent<PlayerCharacter>();
        if (playerChar != null)
        {
            var weaponList = playerChar.GetWeaponList();
            Debug.Log("Número de armas en la lista: " + weaponList.Count);
            
            for (int i = 0; i < weaponList.Count; i++)
            {
                int weaponIndex = i;
                Weapon weapon = weaponList[i];
                
                if (weapon == null)
                {
                    Debug.LogWarning("Arma en índice " + i + " es null");
                    continue;
                }
                
                GameObject buttonObj = Instantiate(weaponButtonPrefab, weaponButtonContainer);
                Button button = buttonObj.GetComponent<Button>();
                TMP_Text buttonText = buttonObj.GetComponentInChildren<TMP_Text>();
                
                if (buttonText != null)
                {
                    buttonText.text = weapon.GetWeaponName() + "\nDaño: " + weapon.GetWeaponDamage() + " | Rango: " + weapon.GetWeaponRange();
                }
                else
                {
                    Debug.LogWarning("El prefab de botón no tiene un TMP_Text hijo");
                }
                
                if (button != null)
                {
                    button.onClick.AddListener(() => OnWeaponSelected(weaponIndex));
                }
                
                Debug.Log("Botón creado para arma: " + weapon.GetWeaponName());
            }
        }
        else
        {
            Debug.LogError("El unit no tiene componente PlayerCharacter!");
        }
    }

    private void OnWeaponSelected(int weaponIndex)
    {
        Debug.Log("Arma seleccionada: índice " + weaponIndex);
        if (currentUnit != null)
        {
            PlayerCharacter playerChar = currentUnit.GetComponent<PlayerCharacter>();
            if (playerChar != null)
            {
                playerChar.EquipWeapon(weaponIndex);
                currentUnit.ExecuteAttackWithWeapon();
            }
        }
    }
}
