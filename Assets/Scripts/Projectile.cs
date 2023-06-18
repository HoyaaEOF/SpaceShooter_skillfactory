using UnityEngine;

namespace SpaceShooter
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float m_Velocity;

        [SerializeField] private float m_LifeTime;

        [SerializeField] private int m_Damage;

        [SerializeField] private ImpactEffect m_ImpactEffectPrefab;

        private float m_Timer;

        private bool IsActive = true;

        private void Update()
        {
            float stepLenght = Time.deltaTime * m_Velocity;

            Vector2 step = transform.up * stepLenght;

            RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.up, stepLenght);

            if (hit)
            {
                Destructible destructible = hit.collider.transform.root.GetComponent<Destructible>();

                if (destructible != null && destructible != m_Parent)
                {
                    destructible.ApplyDamage(m_Damage);

                    /*if (m_Parent == Player.Instance.ActiveShip)
                    {
                        Player.Instance.AddScore(destructible.ScoreValue);
                    }*/

                    if (IsActive)
                    {
                        SingletonBase<Player>.Instance.AddScore(destructible.ScoreValue);
                        IsActive = false;
                    }
                }


                OnProjectileLifeTimeEnd(hit.collider, hit.point);
            }

            m_Timer += Time.deltaTime;

            if (m_Timer > m_LifeTime) Destroy(gameObject);

            transform.position += new Vector3(step.x, step.y, 0);
        }

        private void OnProjectileLifeTimeEnd(Collider2D col, Vector2 pos)
        {
            Destroy(gameObject);
        }

        private Destructible m_Parent;

        public void SetParentShooter(Destructible parent)
        {
            m_Parent = parent;
        }
    }
}