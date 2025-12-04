using UnityEngine;

public class AttackRangeVisualizer : MonoBehaviour
{
    private Units unit;
    private PlayerCharacter playerChar;
    private float attackRange = 5f;  // Rango por defecto
    private LineRenderer lineRenderer;
    private bool isSelected = false;

    private void OnEnable()
    {
        unit = GetComponent<Units>();
        playerChar = GetComponent<PlayerCharacter>();
        
        // Crear LineRenderer si no existe
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
            lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
            lineRenderer.startColor = Color.blue;
            lineRenderer.endColor = Color.blue;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.positionCount = 0;
        }
        
        UpdateAttackRange();
    }

    public void UpdateAttackRange()
    {
        if (playerChar != null)
        {
            Weapon equippedWeapon = playerChar.GetEquippedWeapon();
            if (equippedWeapon != null)
            {
                attackRange = equippedWeapon.GetWeaponRange();
            }
            else
            {
                attackRange = 5f;  // Rango por defecto si no tiene arma
            }
        }
    }

    private void Update()
    {
        // Solo mostrar el rango si la unidad está seleccionada
        isSelected = UnitSelection.Instance != null && UnitSelection.Instance.GetSelectedUnit() == unit;
        
        if (isSelected)
        {
            UpdateAttackRange();
            
            // Verificar si hay enemigos dentro del rango
            bool hasEnemyInRange = HasEnemyInRange();
            Color rangeColor = hasEnemyInRange ? Color.red : Color.blue;
            
            DrawCircleWithLineRenderer(transform.position, attackRange, 30, rangeColor);
        }
        else
        {
            // No mostrar el rango si no está seleccionado
            lineRenderer.positionCount = 0;
        }
    }

    // Verifica si hay algún enemigo dentro del rango
    private bool HasEnemyInRange()
    {
        if (TurnManager.Instance == null || unit == null)
            return false;

        foreach (Units enemy in TurnManager.Instance.enemyUnits)
        {
            if (enemy != null)
            {
                float distanceToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
                if (distanceToEnemy <= attackRange)
                {
                    return true;
                }
            }
        }
        
        return false;
    }

    // Dibuja un círculo usando LineRenderer con color dinámico
    private void DrawCircleWithLineRenderer(Vector3 center, float radius, int segments, Color color)
    {
        Vector3[] positions = new Vector3[segments + 1];
        
        for (int i = 0; i <= segments; i++)
        {
            float angle = (i / (float)segments) * 360f * Mathf.Deg2Rad;
            positions[i] = center + new Vector3(Mathf.Cos(angle) * radius, 0.1f, Mathf.Sin(angle) * radius);
        }

        lineRenderer.positionCount = positions.Length;
        lineRenderer.SetPositions(positions);
        lineRenderer.startColor = color;
        lineRenderer.endColor = color;
    }

    // Dibuja el rango de ataque en el Scene view (solo en editor)
    public void OnDrawGizmos()
    {
        // Dibujar círculo de rango de ataque
        Gizmos.color = new Color(1f, 0.5f, 0f, 0.3f);  // Naranja semi-transparente
        DrawCircle(transform.position, attackRange, 20);
    }

    // Dibuja un círculo en el gizmo
    public void DrawCircle(Vector3 center, float radius, int segments)
    {
        float angle = 0f;
        float angleStep = 360f / segments;
        Vector3 lastPoint = center + new Vector3(radius, 0, 0);

        for (int i = 0; i <= segments; i++)
        {
            float radians = angle * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(Mathf.Cos(radians) * radius, 0, Mathf.Sin(radians) * radius);
            Gizmos.DrawLine(lastPoint, newPoint);
            lastPoint = newPoint;
            angle += angleStep;
        }
    }
}
