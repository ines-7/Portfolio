using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using Orphanage.SceneManagement;
using Orphanage.Audio;

namespace Orphanage
{
    namespace UI
    {
        public class MainMenu : MonoBehaviour
        {
            #region Variablen
            [Header("Refrences")]
            [SerializeField]
            private Button m_LoadButton;
            [SerializeField]
            private OptionsPanel m_optionsPanel;
            [SerializeField]
            private SceneRequestCollection m_sceneRequest;
            [SerializeField]
            private BGMRequestCollection m_bgmRequests;

            #endregion

            /// <summary>
            /// Wenn für SavePoint ein Wert vorliegt und dieser nicht 0 ist, wird der Lade-Button aktiviert
            /// </summary>
            private void Awake()
            {
                m_bgmRequests.Add(BGMRequest.CreateRequest(EMusicTypes.MAIN));

                if (PlayerPrefs.HasKey(EPlayerPrefs.SAVEPOINT.ToString()))
                {
                    if (PlayerPrefs.GetInt(EPlayerPrefs.SAVEPOINT.ToString()) != 0)
                        m_LoadButton.interactable = true;
                }
            }

            #region Game
            /// <summary>
            /// Startet ein neues Spiel
            /// </summary>
            public void StartGame()
            {

                PlayerPrefs.SetInt(EPlayerPrefs.SAVEPOINT.ToString(), 0);
                PlayerPrefs.Save();

                m_bgmRequests.Add(BGMRequest.CreateRequest(EMusicTypes.DRONE));
                m_sceneRequest.Add(SceneRequest.CreateRequest(2, ELoadingScene.LOAD, ESceneFX.SIMPLEFADE, LoadSceneMode.Single));
            }

            /// <summary>
            /// Lädt das bereits vorhandene Spiel
            /// </summary>
            public void LoadGame()
            {
                SceneManager.LoadScene(2, LoadSceneMode.Single);
            }
            #endregion

            /// <summary>
            /// Öffnet das Optionen-Menü
            /// </summary>
            public void Options()
            {
                EventSystem.current.SetSelectedGameObject(null);

                if (m_optionsPanel.gameObject.activeSelf)
                {
                    m_optionsPanel.CloseOpenPanels();
                    m_optionsPanel.gameObject.SetActive(false);
                }
                else
                    m_optionsPanel.gameObject.SetActive(true);
            }

            /// <summary>
            /// Verlässt das Spiel
            /// </summary>
            public void QuitGame()
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            }
        }

    }
}

