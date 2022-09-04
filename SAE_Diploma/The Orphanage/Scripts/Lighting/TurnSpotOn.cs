using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orphanage
{
    namespace Lightning
    {
        public class TurnSpotOn : MonoBehaviour
        {
            private Light m_light;

            private void Start()
            {
                m_light = GetComponentInParent<Light>();
            }

            #region OnTrigger
            /// <summary>
            /// Bei Kollision mit Spieler, wird das Licht eingeschaltet
            /// </summary>
            /// <param name="other"></param>
            private void OnTriggerEnter(Collider other)
            {
                if(other.gameObject.CompareTag("Player"))
                {
                    m_light.enabled = true;
                }
            }
            #endregion
        }
    }
}
