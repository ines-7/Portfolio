using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using Orphanage.Utility;


namespace Orphanage
{
    namespace UI
    {
        public class Brightness : MonoBehaviour
        {
            #region Variablen
            [Header ("Refrences")]
            [SerializeField]
            private Volume m_volumeComponent;
            [SerializeField]
            private Image m_image;
            [SerializeField]
            private Slider m_slider;

            private ColorAdjustments m_colorAdjustments = null;
            #endregion

            private void Awake()
            {
                if (m_colorAdjustments == null)
                    m_volumeComponent.profile.TryGet<ColorAdjustments>(out m_colorAdjustments);
            }

            /// <summary>
            /// setzt den Value des Sliders auf den aktuellen Helligkeitswert
            /// </summary>
            private void Start()
            {
                m_slider.value = PlayerPrefs.GetFloat(EDefaultValues.BRIGHTNESS.ToString(), (float)EDefaultValues.BRIGHTNESS);
            }

            #region Setter
            /// <summary>
            /// Setzt die Helligkeit der Szene fest. Speichert Helligkeit ebenfalls in einem PlayerPref
            /// </summary>
            /// <param name="_value"></param>
            public void SetBrightness(float _value)
            {
                if (m_colorAdjustments == null)
                    m_volumeComponent.profile.TryGet<ColorAdjustments>(out m_colorAdjustments);
                
                m_colorAdjustments.postExposure.value = _value;

                SetPicLight();

                GetSettings.Instance.Changes = true;

                PlayerPrefs.SetFloat(EDefaultValues.BRIGHTNESS.ToString(), _value);
                PlayerPrefs.Save();

                OptionsPanel.Instance.ActivGraphicReset(true);
            }

            /// <summary>
            /// Setzt vom Bild die Helligkeit/Transparenz zum Helligkeits vergleich fest
            /// </summary>
            private void SetPicLight()
            {
                Color color = new Color(m_image.color.r, m_image.color.g,
                                        m_image.color.b, m_image.color.a);

                color.a = Value();
                m_image.color = color;
            }
            #endregion

            #region Reset
            /// <summary>
            /// Setzt Helligkeit und damit verbunden Bild und Slider zurück zum default
            /// </summary>
            public void ResetBrightness()
            {
                SetBrightness((float)EDefaultValues.BRIGHTNESS);
                m_slider.value = (float)EDefaultValues.BRIGHTNESS;
                OptionsPanel.Instance.ActivGraphicReset(false);
            }
            #endregion

            #region floats
            /// <summary>
            /// Gibt den Wert für die Helligkeit/Transparenz des Bildes zurück
            /// </summary>
            /// <returns>float Helligkeit/Transparenz für das Bild</returns>
            private float Value()
            {
                float tmp = (-m_slider.minValue) + m_slider.maxValue;

                float hlp = m_slider.value + (-m_slider.minValue);

                float bri = (hlp / tmp);

                return bri;
            }
            #endregion
        }
    }
}
