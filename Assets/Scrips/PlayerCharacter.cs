using UnityEngine;
using System.Collections.Generic;
using UnityEngine.TextCore.Text;

public class PlayerCharacter : Character
{
    float experience;
    Weapon equippedWeapon;
    Equipment equippedEquipment;
    [SerializeField]List<Equipment> equipmentList = new List<Equipment>();
    [SerializeField]List<Weapon> weaponList = new List<Weapon>();

    public GameObject targetSelectionPanel;
    public GameObject weaponSelectionPanel;


    void Start()
    {
        if (weaponList.Count > 0)
            equippedWeapon = weaponList[0];
            
        if (equipmentList.Count > 0)
            equippedEquipment = equipmentList[0];

        if (targetSelectionPanel != null)
            targetSelectionPanel.SetActive(false);
            
        if (weaponSelectionPanel != null)
            weaponSelectionPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void Earnexperience( float expGain)
    {
        experience += expGain;
    }

    void LevelUp()
    {
        level++;
    }

    public Weapon GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    public void EquipWeapon(int weaponIndex)
    {
        if (weaponIndex >= 0 && weaponIndex < weaponList.Count)
        {
            equippedWeapon = weaponList[weaponIndex];
            Debug.Log(gameObject.name + " equipó " + equippedWeapon.GetWeaponName());
            
            if (weaponSelectionPanel != null)
                weaponSelectionPanel.SetActive(false);
        }
    }

    public List<Weapon> GetWeaponList()
    {
        return weaponList;
    }

    public void ShowWeaponSelectionPanel()
    {
        if (weaponSelectionPanel != null)
            weaponSelectionPanel.SetActive(true);
    }

    public void HideWeaponSelectionPanel()
    {
        if (weaponSelectionPanel != null)
            weaponSelectionPanel.SetActive(false);
    }

    public override float GetTotalArmor()
    {
        // Asegurarse de que el equipamiento esté inicializado
        if (equippedEquipment == null && equipmentList.Count > 0)
        {
            equippedEquipment = equipmentList[0];
        }
        
        float totalArmor = 0f;
        
        // Sumar armadura de todo el equipamiento equipado
        if (equippedEquipment != null)
        {
            totalArmor = equippedEquipment.GetArmor();
            Debug.Log(gameObject.name + " - Armadura del equipamiento " + equippedEquipment.name + ": " + totalArmor);
        }
        
        
        return totalArmor;
    }
}