using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orphanage.UI;

namespace Orphanage
{
    namespace AI
    {
        public class HuntingNunEnemy : AEnemy
        {
            #region Variablen
            public bool Trigger { get; set; }

            [Header ("Settings")]
            [SerializeField]
            private float m_secondsUntilCredits = 0.5f;
            #endregion

            /// <summary>
            /// Initialisiert den HuntingNunEnemy-Typen
            /// </summary>
            protected override void Initialize()
            {
                Trigger = false;
                m_animator.SetBool("Hunting", true);;
                
                m_stateMachine = new StateMachine();

                m_hunt = new HuntState(this, m_stateMachine);
                m_deactivated = new DeactivatedState(this, m_stateMachine);

                m_stateMachine.Initialize(m_deactivated);
            }

            private void Footstep()
            {
                //Platzhalter für Nics Sound...
            }

            #region OnTrigger
            /// <summary>
            /// Bei Kollision mit Spieler, wird die Coroutine  von StartCredits aufgerufen. Damit starten die Credits
            /// </summary>
            /// <param name="other"></param>
            private void OnTriggerEnter(Collider other)
            {
                if (other.gameObject.CompareTag("Player"))
                {
                    m_animator.SetBool("Attack", true);
                    m_agent.destination = this.gameObject.transform.position;
                    Trigger = false;
                    StartCoroutine(StartCredits.Instance.Wait(m_secondsUntilCredits));
                }
            }
            #endregion
        }
    }
}
