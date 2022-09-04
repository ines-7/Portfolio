using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using TMPro;
using Orphanage.Utility;

namespace Orphanage
{
    namespace UI
    {
        public class OptionsPanel : MonoBehaviour
        {
            public static OptionsPanel Instance { get; set; }

            #region Variablen
            #region Sound Settings
            [Header ("Sound Settings")]
            [SerializeField]
            private GameObject m_soundPanel;
            [SerializeField]
            private GameObject m_muteCheckmark;

            [SerializeField]
            private VolumeSlider m_master;
            [SerializeField]
            private VolumeSlider m_music;
            [SerializeField]
            private VolumeSlider m_sfx;

            [SerializeField]
            private Button m_soundReset;
            #endregion

            #region Graphic Setting
            [Header ("Graphic Settings")]
            [SerializeField]
            private GameObject m_graphicPanel;
            [SerializeField]
            private GameObject m_fullscreenCheckmark;

            [SerializeField]
            private Button m_graphicReset;

            [SerializeField]
            private Brightness m_brightness;

            [SerializeField]
            private TMP_Dropdown m_quality;
            [SerializeField]
            private TMP_Dropdown m_resolution;
            #endregion

            #region Control Settings
            [Header ("Control Settings")]
            [SerializeField]
            private GameObject m_controlPanel;

            [SerializeField]
            private Button m_controlReset;
            #endregion
            #endregion

            private void Awake()
            {
                if (Instance != null)
                {
                    Destroy(this.gameObject);
                    return;
                }
                Instance = this;
            }

            private void OnDestroy()
            {
                if (Instance == this)
                {
                    Instance = null;
                }
            }

            // Start is called before the first frame update
            void Start()
            {
                InitQualityOptions();
                InitResolutionOptions();
            }

            #region Panels
            /// <summary>
            /// Öffnet die Sound-Optionen
            /// </summary>
            public void OpenSoundOptions()
            {
                CloseOpenPanels();
                if (!m_soundPanel.activeSelf)
                    m_soundPanel.SetActive(true);
            }

            /// <summary>
            /// Öffnet die Grafik-Optionen
            /// </summary>
            public void OpenGraphicOptions()
            {
                CloseOpenPanels();
                if (!m_graphicPanel.activeSelf)
                    m_graphicPanel.SetActive(true);
            }
            
            /// <summary>
            /// Öffnet die Control-Optionen
            /// </summary>
            public void OpenControlOptions()
            {
                CloseOpenPanels();
                if (!m_controlPanel.activeSelf)
                    m_controlPanel.SetActive(true);
            }

            /// <summary>
            /// Schließt alle offenen Panels im OptionsPanel
            /// </summary>
            internal void CloseOpenPanels()
            {
                if (m_controlPanel.activeSelf)
                    m_controlPanel.SetActive(false);
                if (m_graphicPanel.activeSelf)
                    m_graphicPanel.SetActive(false);
                if (m_soundPanel.activeSelf)
                    m_soundPanel.SetActive(false);
            }
            #endregion

            #region Sound
            /// <summary>
            /// Muted das Spiel
            /// </summary>
            public void Mute()
            {
                if (m_muteCheckmark.activeSelf)
                {
                    m_muteCheckmark.SetActive(false);
                    PlayerPrefs.SetInt(EPlayerPrefs.MUTE.ToString(), 0);
                }
                else
                {
                    m_muteCheckmark.SetActive(true);
                    PlayerPrefs.SetInt(EPlayerPrefs.MUTE.ToString(), 1);
                }

                PlayerPrefs.Save();
                EventSystem.current.SetSelectedGameObject(null);

                m_master.Mute();
                m_music.Mute();
                m_sfx.Mute();

                GetSettings.Instance.Changes = true;
            }

            /// <summary>
            /// Setzt die Sound Settings auf den Default-Wert zurück
            /// </summary>
            public void SoundDefault()
            {
                float soundDefault = (float)EDefaultValues.SOUND - 80.0f;

                m_master.SetVolume(soundDefault);
                m_music.SetVolume(soundDefault);
                m_sfx.SetVolume(soundDefault);

                m_soundReset.interactable = false;
                GetSettings.Instance.Changes = true;
            }
            #endregion

            #region Resets
            /// <summary>
            /// Aktiviert den Reset-Button der Sound-Settings
            /// </summary>
            public void ActivateSoundReset()
            {
                m_soundReset.interactable = true;
            }

            /// <summary>
            /// Aktiviert den Reset-Button der Control-Settings
            /// </summary>
            public void ActivateControlReset() 
            {
                m_controlReset.interactable = true;
            }

            /// <summary>
            /// Aktiviert oder Deaktiviert den Reset-Button der Grafik
            /// </summary>
            /// <param name="_active"></param>
            public void ActivGraphicReset(bool _active)
            {
                m_graphicReset.interactable = _active;
            }

            /// <summary>
            /// Setzt die Graphic Settings auf den Default-Wert zurück
            /// </summary>
            public void GraphicReset()
            {
                if (!Screen.fullScreen)
                    SetFullscreen();
                if (QualitySettings.GetQualityLevel() != (int)EDefaultValues.QUALITY)
                {
                    QualitySettings.SetQualityLevel((int)EDefaultValues.QUALITY);
                    m_quality.value = QualitySettings.GetQualityLevel();
                }
                m_brightness.ResetBrightness();

                GetSettings.Instance.Changes = true;
            }

            /// <summary>
            /// Setzt die Control Settings auf den Default-Wert zurück
            /// </summary>
            public void ControlReset()
            {
                ActionsSkript[] actions = m_controlPanel.GetComponentsInChildren<ActionsSkript>();

                foreach (ActionsSkript action in actions)
                {
                    action.Reset();
                }

                m_controlReset.interactable = false;
                GetSettings.Instance.Changes = true;
            }
            #endregion

            #region Graphic
            #region Quality
            /// <summary>
            /// fügt dem Dropdown alle möglichen Qualities hinzu
            /// </summary>
            private void InitQualityOptions()
            {
                for(int i = 0; i < QualitySettings.names.Length; i++)
                {
                    TMP_Dropdown.OptionData tmp = new TMP_Dropdown.OptionData();
                    tmp.text = QualitySettings.names[i];
                    m_quality.options.Add(tmp);
                }

                QualitySettings.SetQualityLevel(PlayerPrefs.GetInt(EDefaultValues.QUALITY.ToString(), (int)EDefaultValues.QUALITY));
                m_quality.value = QualitySettings.GetQualityLevel();
            }

            /// <summary>
            /// Setzt die Qualität anhand des Values des Dropdowns/ints
            /// </summary>
            /// <param name="_value"></param>
            public void SetQuality(int _value)
            {
                TMP_Dropdown.OptionData tmp = m_quality.options[0];

                QualitySettings.SetQualityLevel(_value);
                m_quality.value = QualitySettings.GetQualityLevel();
                m_graphicReset.interactable = true;

                PlayerPrefs.SetInt(EDefaultValues.QUALITY.ToString(), _value);
                PlayerPrefs.Save();

                GetSettings.Instance.Changes = true;
            }
            #endregion

            #region Resolution
            /// <summary>
            /// Fügt alle möglichen Resolutions hinzu und setzt das Dropdown auf den derzeitigen Wert
            /// </summary>
            private void InitResolutionOptions()
            {
                Resolution[] resolutions = Screen.resolutions;
                for (int i = 0; i < Screen.resolutions.Length; i++)
                {
                    TMP_Dropdown.OptionData tmp = new TMP_Dropdown.OptionData();
                    tmp.text = $"{resolutions[i].width}x{resolutions[i].height}";
                    m_resolution.options.Add(tmp);

                    if(resolutions[i].width == Screen.currentResolution.width
                        && resolutions[i].height == Screen.currentResolution.height)
                    {
                        m_resolution.value = i;
                    }
                }
            }

            /// <summary>
            /// Setzt die Resolution, welche im Dropdown ausgewählt wurde
            /// </summary>
            /// <param name="_value"></param>
            public void SetResolution(int _value)
            {
                Resolution resolution = new Resolution();
                
                string resoText = string.Empty;
                int resoInt = 0;

                for(int i = 0; i < m_resolution.options[_value].text.Length; i++)
                {
                    if (m_resolution.options[_value].text[i] == 'x')
                        break;

                    resoText = resoText + m_resolution.options[_value].text[i];
                }
                resoInt = int.Parse(resoText);

                resolution.width = resoInt;

                resoText = string.Empty;

                for(int i = m_resolution.options[_value].text.Length-1; i > 0; i--)
                {
                    if (m_resolution.options[_value].text[i] == 'x')
                        break;

                    resoText = m_resolution.options[_value].text[i] + resoText;
                }
                resoInt = int.Parse(resoText);

                resolution.height = resoInt;

                Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
            }
            #endregion

            #region Fullscreen
            /// <summary>
            /// Setzt den Fullscreen. Wenn On, dann Off. Wenn Off, dann On
            /// </summary>
            public void SetFullscreen()
            {
                Screen.fullScreen = !Screen.fullScreen;

                if(m_fullscreenCheckmark.activeInHierarchy)
                {
                    m_fullscreenCheckmark.SetActive(false);
                }
                else
                {
                    m_fullscreenCheckmark.SetActive(true);
                }

                EventSystem.current.SetSelectedGameObject(null);
            }
            #endregion
            #endregion
        }
    }
}
