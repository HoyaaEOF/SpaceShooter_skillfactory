using UnityEngine;

namespace SpaceShooter
{
    public class Player : SingletonBase<Player>
    {
        [SerializeField] private int m_NumLives;
        [SerializeField] private SpaceShip m_Ship;
        [SerializeField] private GameObject m_PlayerShipPrefab;

        [SerializeField] private CameraController m_CameraController;
        [SerializeField] private MovementController m_MovementController;

        public SpaceShip ActiveShip => m_Ship;

        protected override void Awake()
        {
            base.Awake();

            if (m_Ship != null) Destroy(m_Ship.gameObject);
        }

        private void Start()
        {
            Respawn();
            m_Ship.EventOnDeath.AddListener(OnShipDeath);
        }

        private void OnShipDeath()
        {
            m_NumLives--;

            if (m_NumLives > 0) Invoke("Respawn", 50.0f * Time.deltaTime);

            else LevelSequenceController.Instance.FinishCurrentLevel(false);
        }

        private void Respawn()
        {
            if (LevelSequenceController.PlayerShip != null)
            {
                var newPlayerShip = Instantiate(LevelSequenceController.PlayerShip);

                m_Ship = newPlayerShip.GetComponent<SpaceShip>();

                m_CameraController.SetTarget(m_Ship.transform);
                m_MovementController.SetTargetShip(m_Ship);

                m_Ship.EventOnDeath.AddListener(OnShipDeath);
            }
        }

        #region Score

        public int score { get; private set; }

        public int numKills { get; private set; }

        public void AddKill()
        {
            numKills++;
        }

        public void AddScore(int num)
        {
            score += num;
        }

        #endregion
    }
}