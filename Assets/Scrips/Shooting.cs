using Unity.VisualScripting;
using UnityEngine;

public class Shooting : MonoBehaviour
{
    // Dibuja el raycast en la Scene view solo cuando se realiza un disparo.
    // Llama a Shoot(...) desde otros scripts para disparar y ver la línea.
    public void Shoot(Vector3 enemyPosition, float weaponRange)
    {
        Vector3 origin = transform.position;
        Vector3 direction = enemyPosition - origin;

        // Visualización: dibuja la línea del raycast en la Scene (verde si golpea, rojo si no)
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

        // Mantener la lógica de comprobación separada si la necesitas en otro sitio
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
