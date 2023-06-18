using UnityEngine;

namespace SpaceShooter
{

    public class PowerupMovement : Powerup
    {
        [SerializeField] private MovementController m_MovementController;
        [SerializeField] private float m_Value;
        [SerializeField] private float m_lifeTime;
        protected override void OnPickedUp(SpaceShip ship)
        {
            m_MovementController.AddAcceleration(m_Value, m_lifeTime);
        }
    }
}