using UnityEngine;

namespace SpaceShooter
{

    public class Teleporter : MonoBehaviour
    {
        [SerializeField] private Teleporter target;

        public bool isReceive;
        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (isReceive == true) return;

            SpaceShip m_Ship = collision.GetComponentInParent<SpaceShip>();

            if (m_Ship != null)
            {
                target.isReceive = true;
                m_Ship.transform.position = target.transform.position;
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            SpaceShip m_Ship = collision.GetComponentInParent<SpaceShip>();

            if (m_Ship != null)
            {
                isReceive = false;

            }
        }
    }
}
