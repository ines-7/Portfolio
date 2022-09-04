using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Orphanage.Player;

namespace Orphanage
{
    namespace AI
    {
        public class KITrigger : MonoBehaviour
        {
            #region Variablen
            [Header ("Refrences")]
            [SerializeField]
            private HuntingNunEnemy m_nun;

            [SerializeField] HeartbeatRequestCollection m_heartbeatRequest;
            

            [Header ("Settings")]
            [SerializeField] 
            private bool m_activate;
            #endregion

            #region OnTrigger
            /// <summary>
            /// Bei Kollision mit Spieler, wird der Gegner(HuntingNunEnemy) aktiviert
            /// </summary>
            /// <param name="other"></param>
            private void OnTriggerEnter(Collider other)
            {
                if(other.gameObject.CompareTag("Player"))
                {
                    m_nun.m_animator.SetBool("Activate", m_activate);
                    m_nun.Trigger = m_activate;

                    Destroy(this.gameObject);
                    m_heartbeatRequest.Add(HeartbeatRequest.CreateRequest(180));
                }
            }
            #endregion
        }
    }
}