using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orphanage
{
    namespace Lightning
    {
        public class FlickeringLight : MonoBehaviour
        {
            #region Variablen
            [Header ("Settings")]
            [SerializeField]
            private float m_min;
            [SerializeField]
            private float m_max;

            private Light m_light;
            #endregion

            // Start is called before the first frame update
            void Start()
            {
                m_light = GetComponent<Light>();

                StartCoroutine(Flashing());
            }

            #region Enumerator
            /// <summary>
            /// Schaltet in vorgegebenen/zufälligen Abständen Licht an und aus
            /// </summary>
            /// <returns></returns>
            private IEnumerator Flashing()
            {
                while (true)
                {
                    yield return new WaitForSeconds(Random.Range(m_min, m_max));
                    m_light.enabled = !m_light.enabled;
                }
            }
            #endregion
        }
    }
}
