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
    GameObject targetSelection;
    public bool isPlayerUnit;

    public TMP_Text endTurn;

    private void Awake()
    {
        clickToMove = GetComponent<ClickToMove>();
        shooting = GetComponent<Shooting>();

        clickToMove.enabled = false;
        shooting.enabled = false;
    }

    public void Move()
    {
        if (hasActed || hasMoved)
            return;
        
        if (isPlayerUnit) 
        {
            clickToMove.EnableMoveMode();
            clickToMove.destinationDummie.position = transform.position;
            clickToMove.enabled = true;
            Debug.Log(characterName + " Usa Mover");
            
        }
        
        
    }

    public void Attack()
    {
        if (hasActed || hasAttacked)
        {
            return;
        }

        if (isFriendly)
        {
            shooting.enabled = true;
            targetSelection.SetActive(true);
        }
        else
        {
            Debug.Log("Ataca pero en malvado");
        }
        StartCoroutine (MostrarAccion(endTurn, characterName + " usa la acción de atacar"));

        Debug.Log(characterName + " usa la accion atacar");
        FinishAttack();
    }

    public void PassTurn()
    {
        if (hasActed)
        {
            return;
        }
        StartCoroutine (MostrarAccion(endTurn, characterName + " finaliza el turno"));

        Debug.Log(characterName + " pasa su turno");
        FinishAction();
    }

    IEnumerator MostrarAccion (TMP_Text textoUI, string mensaje)
    {
        textoUI.text = mensaje;
        textoUI.gameObject.SetActive (true);

        yield return new WaitForSeconds (3f);

        textoUI.gameObject.SetActive(false);

    }


    public void StartTurnForThisUnit()
    {
        hasActed = false;
        hasAttacked = false;
        hasMoved = false;
    }

    public void FinishMovement ()
    {
        clickToMove.enabled = false;
        hasMoved = true;
    }

    public void FinishAttack ()
    {
        hasAttacked = true;
    }
    public void FinishAction()
    {
        hasActed = true;
        TurnManager.Instance.CheckEndTurn();
    }

    public void EnableMovement()
{
    clickToMove.enabled = true;
}
}