using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using Orphanage.Audio;
using TMPro;
using Orphanage.Utility;

namespace Orphanage
{
    namespace UI
    {
        public class VolumeSlider : MonoBehaviour
        {
            #region Variablen
            [Header ("Refrences")]
            [SerializeField]
            private OptionsPanel m_optionsPanel;

            [SerializeField]
            private EMixerTypes m_volumeType;
            [SerializeField]
            private AudioMixer m_audioMixer;
            [SerializeField]
            private TextMeshProUGUI m_volumeText;

            private Slider m_slider;
            #endregion

            private void Awake()
            {
                m_slider = GetComponentInChildren<Slider>();
            }

            // Start is called before the first frame update
            void Start()
            {
                string name = GetVolumeName();
                float volume = PlayerPrefs.GetFloat(name, 0.0f);

                m_audioMixer.SetFloat(name, volume);

                SetVolumeText(volume);

                if (m_audioMixer.GetFloat(name, out volume))
                {
                    m_slider.value = volume;
                }
            }

            #region Setter
            /// <summary>
            /// Setzt die Lautstärke anhand eines floats/durch einen Slider
            /// </summary>
            /// <param name="_volume"></param>
            public void SetVolume(float _volume)
            {
                string name = GetVolumeName();

                m_audioMixer.SetFloat(name, _volume);
                PlayerPrefs.SetFloat(name, _volume);

                PlayerPrefs.Save();

                SetVolumeText(_volume);

                m_slider.value = _volume;
                m_optionsPanel.ActivateSoundReset();
                GetSettings.Instance.Changes = true;
            }

            /// <summary>
            /// Die Lautstärke wird neben dem Slider angegeben und hier gesetzt
            /// </summary>
            /// <param name="_volume"></param>
            private void SetVolumeText(float _volume)
            {
                float volume = _volume + 80.0f;

                m_volumeText.text = volume.ToString();
            }

            /// <summary>
            /// Mutet den jeweilgen VolumeType
            /// </summary>
            public void Mute()
            {
                string name = GetVolumeName();
                float _volume = -80.0f;

                if (PlayerPrefs.GetInt(EPlayerPrefs.MUTE.ToString()) == 0)
                {
                    _volume = PlayerPrefs.GetFloat(name);
                }

                m_audioMixer.SetFloat(name, _volume);
                SetVolumeText(_volume);
            }
            #endregion

            #region Strings
            /// <summary>
            /// Holt den jeweiligen Namen der AudioMixer Group
            /// </summary>
            /// <returns></returns>
            private string GetVolumeName()
            {
                string name = string.Empty;

                switch (m_volumeType)
                {
                    case EMixerTypes.MASTER:
                        {
                            name = "MasterVolume";
                            break;
                        }
                    case EMixerTypes.MUSIC:
                        {
                            name = "MusicVolume";
                            break;
                        }
                    case EMixerTypes.SFX:
                        {
                            name = "EffectVolume";
                            break;
                        }
                }

                return name;
            }
            #endregion
        }
    }
}
