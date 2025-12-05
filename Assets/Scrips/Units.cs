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

    PlayerCharacter playerChar;
    public WeaponSelectionUI weaponSelectionUI;

    private void Awake()
    {
        clickToMove = GetComponent<ClickToMove>();
        shooting = GetComponent<Shooting>();

        clickToMove.enabled = false;
        shooting.enabled = false;

        if (actionPanel != null)
            actionPanel.SetActive(false);

         if (playerChar != null && playerChar.targetSelectionPanel != null)
            playerChar.targetSelectionPanel.SetActive(false);
    }

    public void Move()
    {
        Debug.Log(characterName + " - Move() llamado. hasActed=" + hasActed + ", hasMoved=" + hasMoved);
        
        if (hasActed || hasMoved)
        {
            Debug.LogWarning(characterName + " - No puede moverse. hasActed=" + hasActed + ", hasMoved=" + hasMoved);
            return;
        }

        if (isFriendly) 
        {
            // Asegurarse de que el movimiento esté habilitado
            if (clickToMove != null)
            {
                clickToMove.EnableMoveMode();
                clickToMove.destinationDummie.position = transform.position;
                clickToMove.enabled = true;
                Debug.Log(characterName + " - ClickToMove habilitado y listo para moverse");
            }
            else
            {
                Debug.LogError(characterName + " - clickToMove es null!");
            }

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
            
            if (playerChar != null)
            {
                // Mostrar panel de selección de armas con botones
                if (weaponSelectionUI != null)
                {
                    weaponSelectionUI.ShowWeaponSelection(this);
                    playerChar.ShowWeaponSelectionPanel();
                }
                else
                {
                    Debug.LogWarning("WeaponSelectionUI no está asignado. Asígnalo en el Inspector.");
                }
            }
        }
        
    }

    public void ExecuteAttackWithWeapon()
    {
        if (hasActed || hasAttacked)
            return;

        if (isFriendly)
        {
            PlayerCharacter playerChar = GetComponent<PlayerCharacter>();
            float damageDealt = 10f;
            float penetration = 0f;
            float attackRange = 5f;  // Rango por defecto si no tiene arma
            string weaponUsed = "puño";
            
            if (playerChar != null)
            {
                Weapon equippedWeapon = playerChar.GetEquippedWeapon();
                if (equippedWeapon != null)
                {
                    damageDealt = equippedWeapon.GetWeaponDamage();
                    penetration = equippedWeapon.GetWeaponPenetration();
                    weaponUsed = equippedWeapon.GetWeaponName();
                    attackRange = equippedWeapon.GetWeaponRange();
                }
            }
            
            if (playerChar != null && playerChar.targetSelectionPanel != null)
            {
                // ➕ Abrir panel de selección de enemigos
                playerChar.targetSelectionPanel.SetActive(true);
            }

            // Buscar enemigos en rango
            Units targetInRange = FindEnemyInRange(attackRange);
            
            if (targetInRange != null)
            {
                Debug.Log(characterName + " ataca con " + weaponUsed + " causando " + damageDealt + " de daño a " + targetInRange.characterName);
                
                // Aplicar daño al enemigo (incluyendo penetración del arma)
                Character targetCharacter = targetInRange.GetComponent<Character>();
                if (targetCharacter != null)
                {
                    targetCharacter.TakeDamage(damageDealt, penetration);
                    
                    // Verificar si el enemigo murió
                    if (!targetCharacter.IsAlive())
                    {
                        StartCoroutine(MostrarAccion(endTurn, targetInRange.characterName + " ha muerto"));
                        targetInRange.enabled = false;  // Deshabilitar el Unit muerto
                        targetInRange.gameObject.SetActive(false);  // Ocultar el enemigo muerto
                    }
                    else
                    {
                        StartCoroutine(MostrarAccion(endTurn, characterName + " ataca a " + targetInRange.characterName + " (-" + damageDealt + ")"));
                    }
                }
            }
            else
            {
                Debug.Log(characterName + " intenta atacar con " + weaponUsed + " pero no hay enemigos en rango (" + attackRange + "m)");
                StartCoroutine(MostrarAccion(endTurn, "Sin enemigos en rango"));
            }
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
        
        Debug.Log(characterName + " - StartTurnForThisUnit: hasActed=" + hasActed + ", hasMoved=" + hasMoved + ", hasAttacked=" + hasAttacked);
        
        // Resetear el sistema de movimiento para permitir movimiento en el nuevo turno
        if (clickToMove != null)
        {
            clickToMove.EnableMoveMode();
            
        }
        
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