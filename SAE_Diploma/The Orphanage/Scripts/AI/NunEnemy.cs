using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orphanage.UI;

namespace Orphanage
{
    namespace AI
    { 
        public class NunEnemy : AEnemy
        {
            #region Variablen
            public List<Transform> MoveOptions { get => m_transform; }
            public float SeeAngle { get => m_seeAngle; }
            public float TimeBetweenChange { get => m_timeBetweenChange; }

            [Header ("Refrences")]
            [SerializeField]
            public GameObject m_player;
            [SerializeField]
            private List<Transform> m_transform;

            [Header("Settings")]
            [SerializeField]
            private float m_seeAngle = 10.0f;
            [SerializeField]
            private float m_waitSeconds = 1.5f;
            [SerializeField]
            private float m_timeBetweenChange = 20.0f;
            #endregion

            /// <summary>
            /// Initialisiert den NunEnemy-Typen
            /// </summary>
            protected override void Initialize()
            {
                m_stateMachine = new StateMachine();

                m_idle = new IdleState(this, m_stateMachine);
                m_search = new SearchState(this, m_stateMachine);

                m_stateMachine.Initialize(m_idle);
            }

            private void Footstep()
            {
                //Platzhalter für Nics Sound...
            }

            /// <summary>
            /// Bei einer Kollision mit dem Spieler wird die Attack Animation ausgeführt und die StartCredits Coroutine gestartet
            /// </summary>
            /// <param name="collision"></param>
            private void OnCollisionEnter(Collision collision)
            {
                if(collision.gameObject.CompareTag("Player"))
                {
                    m_animator.SetBool("Attack", true);
                    m_agent.destination = this.gameObject.transform.position;
                    StartCoroutine(StartCredits.Instance.Wait(m_waitSeconds));
                }
            }
        }
    }
}
