using UnityEngine;

public class Character : MonoBehaviour
{
    [Header("Character Main")]
    [SerializeField] string name;
    protected int level;
    [Header("Character Stats")]
    [SerializeField] protected float currentLife;
    [SerializeField] protected float maxLife;
    [SerializeField] float baseAttackDamage;
    private bool isDead = false;

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

    public void TakeDamage(float damage)
    {
        if (isDead) return;  // No recibir daño si ya está muerto
        
        float armorReduction = GetTotalArmor();
        float finalDamage = damage - armorReduction;
        if (finalDamage < 0) finalDamage = 0;
        currentLife -= finalDamage;
        Debug.Log(name + " recibe " + finalDamage + " de daño. Vida actual: " + currentLife);
        
        // Solo morir cuando la vida llega exactamente a 0 o menos
        if (currentLife <= 0)
        {
            isDead = true;
            Debug.Log(name + " ha muerto");
        }
    }

    public void TakeDamage(float weaponDamage, float penetration)
    {
        if (isDead) return;  // No recibir daño si ya está muerto
        
        float armorReduction = GetTotalArmor();
        Debug.Log(name + " armadura total: " + armorReduction);
        
        // La penetración ignora parte de la armadura
        armorReduction = Mathf.Max(0, armorReduction - penetration);
        
        // Daño = daño del arma - armadura (después de penetración)
        float finalDamage = weaponDamage - armorReduction;
        if (finalDamage < 0) finalDamage = 0;
        currentLife -= finalDamage;
        Debug.Log(name + " recibe " + finalDamage + " de daño (Arma: " + weaponDamage + " - (Armadura: " + GetTotalArmor() + " - Penetración: " + penetration + ")). Vida actual: " + currentLife);
        
        // Solo morir cuando la vida llega exactamente a 0 o menos
        if (currentLife <= 0)
        {
            isDead = true;
            Debug.Log(name + " ha muerto");
        }
    }

    // Método virtual que las clases hijas pueden sobrescribir
    public virtual float GetTotalArmor()
    {
        return 0f;
    }

    public bool IsAlive()
    {
        return !isDead;
    }

    public float GetCurrentLife()
    {
        return currentLife;
    }

    public float GetMaxLife()
    {
        return maxLife;
    }
}