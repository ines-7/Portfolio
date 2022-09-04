using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;

namespace Orphanage
{
    namespace Lightning
    {
        public class DimmingLight : MonoBehaviour
        {
            #region Variablen
            [SerializeField]
            private float m_timeUntilChange = 20.0f;

            [SerializeField]
            private float m_changeSpeed = 5.0f;

            [SerializeField]
            private float m_minIntensity = 250.0f;
            [SerializeField]
            private float m_maxIntensity = 1000.0f;

            private float m_speed;
            private HDAdditionalLightData m_light;
            #endregion

            private void Awake()
            {
                m_light = GetComponent<HDAdditionalLightData>();
            }

            // Start is called before the first frame update
            void Start()
            {
                m_light.intensity = 250.0f;
            }

            /// <summary>
            /// lässt die Licht Helligkeit steigen und sinken über Zeit
            /// </summary>
            void Update()
            {
                if (m_light.intensity <= m_minIntensity)
                    m_speed = m_changeSpeed;
                else if (m_light.intensity >= m_maxIntensity)
                    m_speed = -m_changeSpeed;

                m_light.intensity = m_light.intensity + m_speed * Time.deltaTime;
            }
        }
    }
}
