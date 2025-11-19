using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{


    bool isOnLoS;

    public bool isOnS(Vector3 enemyPosition)
    {
        bool isLosM;
  if(Physics.Raycast(Transform.position, enemyTransfor.position, out hit, 1000f))
            isLos = true;
        return isLoS;
    }
} 