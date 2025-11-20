using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{


    public void Shoot(Vector3 enemyPosition, float weaponRange)
    {
        if (IsOnLoS(enemyPosition, weaponRange));
        {
            Debug.Log("Enemigo en linea de tiro")
        }
        else
        {
            Debug.Log("Enemigo no esta en linea de tiro");
        }
    }

    public bool IsOnLoS(Vector3 enemyPosition, float weaponRange)
    {
        RaycastHit hit;

        if (Physics.Raycast(Transform.position, enemyPosition, out hit, weaponRange))
        {
            Character character =hit.collider.GetComponent<Character>();

            if (character != null)
            {
                return true;
            }
        }
        return false;
    }   
} 