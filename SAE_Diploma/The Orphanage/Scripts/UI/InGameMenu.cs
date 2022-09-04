using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using Orphanage.SceneManagement;
using Orphanage.Player;

namespace Orphanage
{
    namespace UI
    {
        public class InGameMenu : MonoBehaviour
        {
            #region Variablen
            [Header ("Refrences")]
            [SerializeField]
            private GameObject m_optionsPanel;
            [SerializeField]
            private SceneRequestCollection m_sceneRequest;
            #endregion

            #region Returner
            /// <summary>
            /// Wechselt die Szene zurück zum Spiel
            /// </summary>
            public void ReturnToGame()
            {
                PlayerController.Instance.MenuActive = false;
                m_sceneRequest.Add(SceneRequest.CreateRequest(1, ELoadingScene.UNLOAD, ESceneFX.VIGNETTEFX));
            }

            /// <summary>
            /// Wechselt die Szene zurück ins Hauptmenü
            /// </summary>
            public void ReturnToMainMenu()
            {
                PlayerController.Instance.MenuActive = false;
                SceneManager.LoadScene(0, LoadSceneMode.Single);
            }
            #endregion

            /// <summary>
            /// Lässt die Optionen erscheinen
            /// </summary>
            public void Options()
            {
                EventSystem.current.SetSelectedGameObject(null);

                if (m_optionsPanel.activeSelf)
                {
                    m_optionsPanel.SetActive(false);
                }
                else
                    m_optionsPanel.SetActive(true);
            }

            /// <summary>
            /// Verlassen des Spiels
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