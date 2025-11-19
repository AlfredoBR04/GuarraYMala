using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Main")]
    [SerializeField] string name;
    protected int level;
    [Header("Character Stats")]
    float currentLife;
    [SerializeField] float maxLife;
    [SerializeField] float baseAttackDamage;
    [SerializeField] protected float armorValue;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        name = gameObject.name;
        currentLife = maxLife;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void TakeDamage(float damage)
    {
        float finalDamage = damage - armorValue;
        currentLife -= finalDamage;
        IsAlive();
    }

    void IsAlive()
    {
        if (currentLife <= 0)
        {
            Debug.Log(name + " has died");
            Destroy(gameObject);
        }
    }
}