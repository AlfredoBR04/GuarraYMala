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
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
