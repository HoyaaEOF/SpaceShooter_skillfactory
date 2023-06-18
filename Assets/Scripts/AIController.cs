using UnityEngine;

namespace SpaceShooter
{
    [RequireComponent(typeof(SpaceShip))]
    public class AIController : MonoBehaviour
    {
        public enum AIBehaviour
        {
            Null,
            Patrol
        }

        [SerializeField] private AIBehaviour m_AIBehaviour;

        [SerializeField] private AIPointPatrol m_PatrolPoint;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationLinear;

        [Range(0.0f, 1.0f)]
        [SerializeField] private float m_NavigationAngular;

        [SerializeField] private float m_RandomSelectMovePointTime;

        [SerializeField] private float m_FindNewTargetTime;

        [SerializeField] private float m_ShootDelay;

        [SerializeField] private float m_EvadeRayLength;

        [SerializeField] private float m_PredictionMultiply;

        [SerializeField] private Transform[] m_PatrolSpots;

        private SpaceShip m_SpaceShip;

        private Vector3 m_MovePosition;

        private Destructible m_SelectedTarget;

        private Timer m_RandomizeDirectionTimer;
        private Timer m_FireTimer;
        private Timer m_FindNewTargetTimer;

        private int m_PatrolSpotNum;

        private Rigidbody2D rb;

        private void Start()
        {
            m_SpaceShip = GetComponent<SpaceShip>();

            InitTimers();

            m_PatrolSpotNum = 0;
        }

        private void Update()
        {
            UpdateTimers();

            UpdateAI();
        }

        private void UpdateAI()
        {
            if (m_AIBehaviour == AIBehaviour.Null)
            {

            }

            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                UpdateBehaviourPatrol();
            }
        }

        public void UpdateBehaviourPatrol()
        {
            ActionFindNewMovePosition();
            ActionFindNewAttackTarget();
            ActionEvadeCollision();
            ActionControlShip();
        }

        private void ActionFindNewMovePosition()
        {
            if (m_AIBehaviour == AIBehaviour.Patrol)
            {
                if (m_SelectedTarget != null)
                {
                    m_MovePosition = ActionMakeLead();
                }

                else
                {
                    if (m_PatrolSpots.Length != 0)
                    {
                        if (m_PatrolSpotNum == m_PatrolSpots.Length) m_PatrolSpotNum = 0;

                        m_MovePosition = m_PatrolSpots[m_PatrolSpotNum].transform.position;

                        if ((m_PatrolSpots[m_PatrolSpotNum].transform.position - m_SpaceShip.transform.position).sqrMagnitude < 1.0f * 1.0f) m_PatrolSpotNum++;
                    }

                    else
                    {
                        if (m_PatrolPoint != null)
                        {
                            bool isInsidePatrolZone = (m_PatrolPoint.transform.position - transform.position).sqrMagnitude < m_PatrolPoint.Radius * m_PatrolPoint.Radius;

                            if (isInsidePatrolZone == true)
                            {
                                if (m_RandomizeDirectionTimer.IsFinished == true)
                                {
                                    Vector2 newPoint = UnityEngine.Random.onUnitSphere * m_PatrolPoint.Radius + m_PatrolPoint.transform.position;

                                    m_MovePosition = newPoint;

                                    m_RandomizeDirectionTimer.Start(m_RandomSelectMovePointTime);
                                }
                            }

                            else
                            {
                                m_MovePosition = m_PatrolPoint.transform.position;
                            }
                        }
                    }
                }
            }
        }

        private void ActionEvadeCollision()
        {
            if (Physics2D.Raycast(transform.position, transform.up, m_EvadeRayLength) == true)
            {
                m_MovePosition = transform.position + transform.right * 100.0f;
            }
        }

        private void ActionControlShip()
        {
            m_SpaceShip.ThrustControl = m_NavigationLinear;

            m_SpaceShip.TorqueControl = ComputeAliginTorqueNormalized(m_MovePosition, m_SpaceShip.transform) * m_NavigationAngular;
        }

        private const float MAX_ANGLE = 45.0f;

        private static float ComputeAliginTorqueNormalized(Vector3 targetPosition, Transform ship)
        {
            Vector2 localTargetPosition = ship.InverseTransformPoint(targetPosition);

            float angle = Vector3.SignedAngle(localTargetPosition, Vector3.up, Vector3.forward);

            angle = Mathf.Clamp(angle, -MAX_ANGLE, MAX_ANGLE) / MAX_ANGLE;

            return -angle;
        }

        private void ActionFindNewAttackTarget()
        {
            if (m_FindNewTargetTimer.IsFinished == true)
            {
                m_SelectedTarget = FindNearestDestructibleTarget();

                m_FindNewTargetTimer.Start(m_FindNewTargetTime);

                if (m_SelectedTarget != null)
                {
                    ActionFire();
                    ActionMakeLead();
                }

            }
        }

        private void ActionFire()
        {
            if (m_SelectedTarget != null)
            {
                if (m_FireTimer.IsFinished == true)
                {
                    m_SpaceShip.Fire(TurretMode.Primary);

                    m_FireTimer.Start(m_ShootDelay);
                }
            }
        }

        private Destructible FindNearestDestructibleTarget()
        {
            float maxDist = 10f;

            Destructible potentialTarget = null;

            foreach (var v in Destructible.AllDestructibles)
            {
                if (v.GetComponent<SpaceShip>() == m_SpaceShip) continue;

                if (v.TeamId == Destructible.TeamIdNeutral) continue;

                if (v.TeamId == m_SpaceShip.TeamId) continue;

                float dist = Vector2.Distance(m_SpaceShip.transform.position, v.transform.position);

                if (dist < maxDist)
                {
                    maxDist = dist;

                    potentialTarget = v;
                }
            }

            return potentialTarget;
        }

        private Vector3 ActionMakeLead()
        {
            if (m_SelectedTarget == null) 
            {
                m_SelectedTarget = FindNearestDestructibleTarget();
            }

            rb = m_SelectedTarget.GetComponentInParent<Rigidbody2D>();

            Vector3 newTarget = m_SelectedTarget.transform.position + (Vector3) rb.velocity * m_PredictionMultiply;

            return newTarget;
        }

        private void SetPatrolBehaviour(AIPointPatrol point)
        {
            m_AIBehaviour = AIBehaviour.Patrol;

            m_PatrolPoint = point;
        }

        #region Timers

        private void InitTimers()
        {
            m_RandomizeDirectionTimer = new Timer(m_RandomSelectMovePointTime);

            m_FireTimer = new Timer(m_ShootDelay);

            m_FindNewTargetTimer = new Timer(m_FindNewTargetTime);
        }

        private void UpdateTimers()
        {
            m_RandomizeDirectionTimer.RemoveTime(Time.deltaTime);

            m_FireTimer.RemoveTime(Time.deltaTime);

            m_FindNewTargetTimer.RemoveTime(Time.deltaTime);
        }

        #endregion
    }
}