using UnityEngine;

namespace SpaceShooter
{
    public class FragmentsSpawn : MonoBehaviour
    {
        [SerializeField] private Destructible m_FragmentPrefab;
        [SerializeField] private int m_FragmentAmount;

        public void SpawnFragment()
        {
            for (int i = 0; i < m_FragmentAmount; i++)
            {
                GameObject Asteroid = Instantiate(m_FragmentPrefab.gameObject);
                Asteroid.transform.position = transform.position;

                Rigidbody2D rb = Asteroid.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    rb.velocity = Random.insideUnitCircle.normalized;
                    rb.rotation = Random.Range(0, 360);
                }
            }
        }
    }
}