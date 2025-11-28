using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    
    public void Shoot(Vector3 enemyPosition, float weaponRange)
    {
        Vector3 origin = transform.position;
        Vector3 direction = enemyPosition - origin;

       
        RaycastHit hitInfo;
        bool hit = Physics.Raycast(origin, direction.normalized, out hitInfo, weaponRange);

        if (hit)
        {
            Debug.DrawLine(origin, hitInfo.point, Color.green, 1f);
            Debug.Log("Enemigo en linea de tiro");
        }
        else
        {
            Debug.DrawRay(origin, direction.normalized * weaponRange, Color.red, 1f);
            Debug.Log("Enemigo no esta en linea de tiro");
        }

    }

    // Comprueba si hay línea de tiro hacia enemyPosition dentro de weaponRange
    public bool IsOnLoS(Vector3 enemyPosition, float weaponRange)
    {
        Vector3 origin = transform.position;
        Vector3 direction = enemyPosition - origin;

        RaycastHit hit;
        if (Physics.Raycast(origin, direction.normalized, out hit, weaponRange))
        {
            Character character = hit.collider.GetComponent<Character>();
            return character != null;
        }
        return false;
    }
}
