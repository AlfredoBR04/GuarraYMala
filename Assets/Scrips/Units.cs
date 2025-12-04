using UnityEngine;
using TMPro;
using System.Collections;

public class Units : MonoBehaviour
{
    [SerializeField] public string characterName;

    public bool hasActed = true;
    bool hasAttacked = false;
    public bool hasMoved = false;
    [SerializeField] public bool isFriendly;

    ClickToMove clickToMove;
    Shooting shooting;

    public TMP_Text endTurn;

    public GameObject actionPanel;

    private void Awake()
    {
        clickToMove = GetComponent<ClickToMove>();
        shooting = GetComponent<Shooting>();

        clickToMove.enabled = false;
        shooting.enabled = false;

        if (actionPanel != null)
            actionPanel.SetActive(false);
    }

    public void Move()
    {
        if (hasActed || hasMoved)
            return;

        if (isFriendly) 
        {
            clickToMove.destinationDummie.position = transform.position;
            clickToMove.enabled = true;

            Debug.Log(characterName + " Usa Mover");
        }
    }

    public void Attack()
    {
        if (hasActed || hasAttacked)
            return;

        if (isFriendly)
        {
            PlayerCharacter playerChar = GetComponent<PlayerCharacter>();
            float damageDealt = 10f;
            float attackRange = 5f;  // Rango por defecto si no tiene arma
            string weaponUsed = "puño";
            
            if (playerChar != null)
            {
                Weapon equippedWeapon = playerChar.GetEquippedWeapon();
                if (equippedWeapon != null)
                {
                    damageDealt = equippedWeapon.GetWeaponDamage();
                    weaponUsed = equippedWeapon.GetWeaponName();
                    attackRange = equippedWeapon.GetWeaponRange();
                }
            }

            // Buscar enemigos en rango
            Units targetInRange = FindEnemyInRange(attackRange);
            
            if (targetInRange != null)
            {
                Debug.Log(characterName + " ataca con " + weaponUsed + " causando " + damageDealt + " de daño a " + targetInRange.characterName);
                StartCoroutine(MostrarAccion(endTurn, characterName + " ataca a " + targetInRange.characterName));
                // Aquí puedes aplicar daño a targetInRange
            }
            else
            {
                Debug.Log(characterName + " intenta atacar con " + weaponUsed + " pero no hay enemigos en rango (" + attackRange + "m)");
                StartCoroutine(MostrarAccion(endTurn, "Sin enemigos en rango"));
            }
        }
        else
        {
            Debug.Log("Ataca pero en malvado");
        }

        FinishAttack();
    }

    private Units FindEnemyInRange(float range)
    {
        Units closestEnemy = null;
        float closestDistance = Mathf.Infinity;

        foreach (Units enemy in TurnManager.Instance.enemyUnits)
        {
            if (enemy != null)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);
                if (distance <= range && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestEnemy = enemy;
                }
            }
        }

        return closestEnemy;
    }

    public void PassTurn()
    {
        if (hasActed)
            return;

        StartCoroutine(MostrarAccion(endTurn, characterName + " finaliza el turno"));
        Debug.Log(characterName + " pasa su turno");

        FinishAction();
    }

    IEnumerator MostrarAccion(TMP_Text textoUI, string mensaje)
    {
        if (textoUI == null)
            yield break;
        
        textoUI.text = mensaje;
        textoUI.gameObject.SetActive(true);

        yield return new WaitForSeconds(3f);

        textoUI.gameObject.SetActive(false);
    }

    public void StartTurnForThisUnit()
    {
        hasActed = false;
        hasAttacked = false;
        hasMoved = false;
    }

    public void FinishMovement()
    {
        clickToMove.enabled = false;
        hasMoved = true;
    }

    public void FinishAttack()
    {
        hasAttacked = true;
    }

    public void FinishAction()
    {
        hasActed = true;

        EnableActionButtons(false);
        TurnManager.Instance.UnitFinishedTurn(this);
    }

  
    public void EnableMovement()
    {
        clickToMove.enabled = true;
    }

    public void EnableActionButtons(bool state)
    {
        if (actionPanel != null)
            actionPanel.SetActive(state);
    }
}