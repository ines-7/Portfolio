using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Orphanage
{
    namespace UI
    {
        public class StartCredits : MonoBehaviour
        {
            public static StartCredits Instance { get; private set; }

            #region Variablen
            [SerializeField]
            private float m_seconds = 1.0f;
            #endregion

            private void Awake()
            {
                if(Instance != null)
                {
                    Destroy(this.gameObject);
                    return;
                }
                Instance = this;
            }

            private void OnDestroy()
            {
                if(Instance == this)
                {
                    Instance = null;
                }
            }

            #region OnTrigger
            /// <summary>
            /// wenn man mit dem Spieler kollidiert, startet die Coroutine, damit die Credits starten
            /// </summary>
            /// <param name="other"></param>
            private void OnTriggerEnter(Collider other)
            {
                if(other.gameObject.CompareTag("Player"))
                {
                    StartCoroutine(Wait(m_seconds));
                }
            }
            #endregion

            #region Coroutine
            /// <summary>
            /// Wartet für eine bestimmte Zeit, bevor die Credits starten
            /// </summary>
            /// <param name="_seconds"></param>
            /// <returns></returns>
            public IEnumerator Wait(float _seconds)
            {
                yield return new WaitForSeconds(_seconds);
                SceneManager.LoadScene(3, LoadSceneMode.Single);
            }
            #endregion
        }
    }
}
