using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace SpaceShooter
{
    /// <summary>
    /// Уничтожаемый объект. То, что может иметь хитпоинты
    /// </summary>
    public class Destructible : Entity
    {
        #region Properties
        [SerializeField] private GameObject m_ExplosionEffect;
        [SerializeField] private GameObject m_VisualModel;
        /// <summary>
        /// Объект игнорирует повреждения
        /// </summary>
        [SerializeField]
        private bool m_Indestructible;
        public bool IsIndestructible => m_Indestructible;

        /// <summary>
        /// Стартовое кол-во хитпоинтов
        /// </summary>
        [SerializeField]
        private int m_HitPoints;

        /// <summary>
        /// Текущее кол-во хитпоинтов
        /// </summary>
        private int m_CurrentHitPoints;
        public int HitPoints => m_HitPoints;

        #endregion

        #region Unity Events

        protected virtual void Start()
        {
            m_CurrentHitPoints = m_HitPoints;
        }

        #endregion

        #region Public API

        /// <summary>
        /// Применение дамага к объекту
        /// </summary>
        /// <param name="damage"> Урон, наносимый объекту </param>
        public void ApplyDamage(int damage)
        {
            if (m_Indestructible) return;

            m_CurrentHitPoints -= damage;

            if (m_CurrentHitPoints <= 0) OnDeath();
        }
        #endregion
        /// <summary>
        /// Переопределяемое событие уничтожения объекта, когда хп ниже нуля
        /// </summary>
        protected virtual void OnDeath()
        {
            m_VisualModel.SetActive(false);
            m_ExplosionEffect.SetActive(true);
            m_ExplosionEffect.GetComponent<ParticleSystem>().Play();
            Destroy(gameObject,50.0f*Time.deltaTime);
            m_EventOnDeath?.Invoke();
        }

        private static HashSet<Destructible> m_AllDestructibles;

        public static IReadOnlyCollection<Destructible> AllDestructibles => m_AllDestructibles;

        protected virtual void OnEnable()
        {
            if (m_AllDestructibles == null) m_AllDestructibles = new HashSet<Destructible>();

            m_AllDestructibles.Add(this);
        }

        protected virtual void OnDestroy()
        {
            m_AllDestructibles.Remove(this);
        }

        public const int TeamIdNeutral = 0;

        [SerializeField] private int m_TeamId;
        public int TeamId => m_TeamId;

        [SerializeField] private UnityEvent m_EventOnDeath;
        public UnityEvent EventOnDeath => m_EventOnDeath;

        protected void Indestrictable()
        {
            m_Indestructible = true;
        }

        #region Score

        [SerializeField] private int m_ScoreValue;
        public int ScoreValue => m_ScoreValue;

        public void OnKill()
        {
            Player.Instance.AddKill();
        }

        #endregion
    }
}