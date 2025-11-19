using UnityEngine;

[CreateAssetMenu(fileName = "Equipment", menuName = "ScriptableObjets/Equipment")]
public class Equipment : ScriptableObject
{

    [SerializeField] float maxDurability;
    [SerializeField] float currentDurability;
    [SerializeField] float movementSpeed;
    [SerializeField] float armor;




}