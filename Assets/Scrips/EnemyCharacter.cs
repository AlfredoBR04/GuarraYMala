using UnityEngine;

public class EnemyCharacter : Character
{
    [SerializeField] Weapon equippedWeapon;
    [SerializeField] Equipment equippedEquipment;

    public Weapon GetEquippedWeapon()
    {
        return equippedWeapon;
    }

    public override float GetTotalArmor()
    {
        float totalArmor = 0f;
        
        if (equippedEquipment != null)
        {
            totalArmor = equippedEquipment.GetArmor();
        }
        
        return totalArmor;
    }
}
