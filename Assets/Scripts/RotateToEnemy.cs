using UnityEngine;

public class RotateToEnemy : MonoBehaviour
{
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private float m_Radius;
    private float m_ShortestDistance;
    private Collider2D m_ClosestCollider;

    private void Start()
    {
        Collider2D[] m_Enemies = Physics2D.OverlapCircleAll(transform.position, m_Radius);
        m_ShortestDistance = Mathf.Infinity;

        for (int i = 0; i < m_Enemies.Length; i++)
        {
            if (m_Enemies[i].transform.root.CompareTag("Player") == true) continue;

            float dist = (transform.position - m_Enemies[i].transform.position).sqrMagnitude;

            if (dist < m_ShortestDistance)
            {
                m_ShortestDistance = dist;
                m_ClosestCollider = m_Enemies[i];
            }
        }
    }

    private void Update()
    {
        if (m_ClosestCollider != null)
        {
            Vector3 target = m_ClosestCollider.transform.position - transform.position;

            transform.up = Vector3.Slerp(transform.up, target, Time.deltaTime * m_RotationSpeed);
        }
    }
}
