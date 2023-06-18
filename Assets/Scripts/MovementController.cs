using UnityEngine;

namespace SpaceShooter
{
    public class MovementController : MonoBehaviour
    {
        public enum ControlMode
        {
            Keyboard,
            Mobile
        }

        [SerializeField] private SpaceShip m_TargetShip;
        public void SetTargetShip(SpaceShip ship) => m_TargetShip = ship;

        [SerializeField] private VirtualJoystick m_MobileJoystick;

        [SerializeField] private ControlMode m_ControlMode;

        [SerializeField] private PointerClickHold m_MobileFirePrimary;
        [SerializeField] private PointerClickHold m_MobileFireSecondary;

        private float a = 1.0f;

        private float m_Timer;
        private float m_lifeTimePowerup;

        private bool IsAcceleration = false;

        private void Start()
        {
            if (m_ControlMode == ControlMode.Keyboard)
            {
                m_MobileJoystick.gameObject.SetActive(false);
                m_MobileFirePrimary.gameObject.SetActive(false);
                m_MobileFireSecondary.gameObject.SetActive(false);
            }
            else
            {
                m_MobileJoystick.gameObject.SetActive(true);
                m_MobileFirePrimary.gameObject.SetActive(true);
                m_MobileFireSecondary.gameObject.SetActive(true);
            }
        }

        private void Update()
        {
            if (m_TargetShip == null) return;

            if (IsAcceleration == true)
            {
                m_Timer += Time.deltaTime;

                if (m_Timer > m_lifeTimePowerup)
                {
                    IsAcceleration = false;
                    a = 1.0f;
                }

            }

            if (m_ControlMode == ControlMode.Keyboard) ControlKeyboard();
            if (m_ControlMode == ControlMode.Mobile) ControlMobile();

            

        }

        private void ControlMobile()
        {
            var dir = m_MobileJoystick.Value;
            dir.y *= a;
            m_TargetShip.ThrustControl = dir.y;
            m_TargetShip.TorqueControl = -dir.x;
            if (m_MobileFirePrimary.IsHold == true) m_TargetShip.Fire(TurretMode.Primary);
            if (m_MobileFireSecondary.IsHold == true) m_TargetShip.Fire(TurretMode.Secondary);
        }

        private void ControlKeyboard()
        {
            float Thrust = 0;
            float Torque = 0;

            if (Input.GetKey(KeyCode.UpArrow)) Thrust = 1.0f * a; 
            if (Input.GetKey(KeyCode.DownArrow)) Thrust = -1.0f * a;
            if (Input.GetKey(KeyCode.LeftArrow)) Torque = 1.0f;
            if (Input.GetKey(KeyCode.RightArrow)) Torque = -1.0f;
            if (Input.GetKey(KeyCode.Space)) m_TargetShip.Fire(TurretMode.Primary);
            if (Input.GetKey(KeyCode.X)) m_TargetShip.Fire(TurretMode.Secondary);

            m_TargetShip.ThrustControl = Thrust;
            m_TargetShip.TorqueControl = Torque;
        }

        public void AddAcceleration(float value, float lifeTime)
        {
            IsAcceleration = true;
            a = value;
            m_lifeTimePowerup = lifeTime;
        }
    }
}