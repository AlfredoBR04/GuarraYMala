using UnityEngine;

public class Units : MonoBehaviour
{
    [SerializeField] string characterName;

    public bool hasActed = true;
    public bool hasAttacked = false;
    bool hasMoved = false;
    public bool isPlayerUnit;
    ClickToMove clickToMove;

    

     public bool isFriendly
    {
        get { return isPlayerUnit; }
    }

    private void Awake()
{
    clickToMove = GetComponent<ClickToMove>();
    if (clickToMove != null)
    {
        clickToMove.enabled = false;
    }
    else
    {
        Debug.LogWarning(gameObject.name + " no tiene ClickToMove asignado.");
    }
}

    public void Move()
    {

        Debug.Log ("le doy al boton");
        if (hasActed || hasMoved)
            return;

        if (isPlayerUnit)
        {
            clickToMove.enabled = true;
            Debug.Log(characterName + " is moving.");
        }
        else
        {
            Debug.Log(characterName + " • an enemy unit is moving.");
        }

        
    }

    public void Attack()
    {
        if (hasActed || hasAttacked)
            return;

        Debug.Log(characterName + " is attacking.");

        FinishAttack();
    }

    public void PassTurn()
    {
        if (hasActed)
            return;

        Debug.Log(characterName + " is passing the turn.");

        FinishAction();
    }

    

    public void StartTurnForThisUnit()
    {
        hasActed = false;
        hasAttacked = false;   // corregido
        hasMoved = false;
    }



    public void FinishMove()
    {
        clickToMove.enabled = false;
        hasMoved = true;
        Debug.Log(characterName + " has finished moving.");
    }

    public void FinishAttack()
    {
        hasAttacked = true;
    }

    public void FinishAction()
    {
        hasActed = true;
        TurnManager.Instance.CheckEndTurn(); // ahora funciona
    }


}
