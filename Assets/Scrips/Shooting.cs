using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{


    public void Shoot()
    {
        isOnLoS();
    }

    bool isOnLoS;

    public bool isOnS(Vector3 enemyPosition, float weaponRange)
    {
        RaycastHit hit;

        if (Physics.Raycast(Transform.position, enemyTransfor.position, out hit, weaponRange))
        {
            Character character =hit.collider.GetComponent<Character>();

            if (unit != null)
            {
                return
            }
        }
    }
} 