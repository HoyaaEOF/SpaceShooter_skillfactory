using UnityEngine;

namespace SpaceShooter
{
    public class RadiusBomb : MonoBehaviour
    {
        [SerializeField] private float m_Radius;
        [SerializeField] private int m_Damage;
        [SerializeField] private GameObject m_ExplosionEffect;

        private void OnDestroy()
        {
            Collider2D[] m_Enemies = Physics2D.OverlapCircleAll(transform.position, m_Radius);

            for (int i = 0; i < m_Enemies.Length; i++)
            {
                if (m_Enemies[i].transform.root.CompareTag("Player") == true) continue;

                Destructible destr = m_Enemies[i].transform.root.GetComponent<Destructible>();

                if (destr != null)
                {
                    m_Enemies[i].transform.root.GetComponent<Destructible>().ApplyDamage(m_Damage);
                } 
            }
            
        }

    }
}