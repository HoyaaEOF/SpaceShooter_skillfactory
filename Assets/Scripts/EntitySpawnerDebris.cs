using UnityEngine;

namespace SpaceShooter
{
    public class EntitySpawnerDebris : MonoBehaviour
    {
        [SerializeField] private Destructible[] m_DebrisPrefabs;
        [SerializeField] private int m_NumDebris;
        [SerializeField] private CircleArea m_Area;
        [SerializeField] private float m_RandomSpeed;

        private void Start()
        {
            for (int i = 0; i < m_NumDebris; i++)
            {
                SpawnDebris();
            }
        }

        private void SpawnDebris()
        {
            int index = Random.Range(0, m_DebrisPrefabs.Length);

            GameObject debris = Instantiate(m_DebrisPrefabs[index].gameObject);

            debris.transform.position = m_Area.GetRandomInsideZone();

            debris.GetComponent<Destructible>().EventOnDeath.AddListener(OnDebrisDie);

            Rigidbody2D rb = debris.GetComponent<Rigidbody2D>(); 

            if (rb != null && m_RandomSpeed > 0)
            {
                rb.velocity = (Vector2)UnityEngine.Random.insideUnitCircle * m_RandomSpeed;
            }
        }

        private void OnDebrisDie()
        {
            SpawnDebris();
            //SpawnFragment();
        }

        /*[SerializeField] private Destructible m_FragmentPrefab;
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
        }*/
    }
}