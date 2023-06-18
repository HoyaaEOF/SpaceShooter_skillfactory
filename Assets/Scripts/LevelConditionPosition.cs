using UnityEngine;

namespace SpaceShooter
{
    public class LevelConditionPosition : MonoBehaviour
    {
        private SpaceShip m_Ship;

        [SerializeField] private int m_Score;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            m_Ship = collision.transform.root.GetComponent<SpaceShip>();
            if (m_Ship != null && m_Ship == Player.Instance.ActiveShip)
            {
                Player.Instance.AddScore(m_Score);
                LevelSequenceController.Instance.FinishCurrentLevel(true);
            }
        }
    }
}