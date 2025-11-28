using UnityEngine;

[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObjets/Weapon")]
public class Weapon : ScriptableObject
{

    [SerializeField] float weaponDamage;
    [SerializeField] float weaponPenetration;
    [SerializeField] string weaponName;
    [SerializeField] int magazine;
    [SerializeField] int magazineSize;
    [SerializeField] float weaponRange;

    public float GetWeaponDamage() => weaponDamage;
    public string GetWeaponName() => weaponName;
    public float GetWeaponRange() => weaponRange;
    public float GetWeaponPenetration() => weaponPenetration;
}
