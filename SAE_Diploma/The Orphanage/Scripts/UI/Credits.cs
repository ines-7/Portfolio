using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Orphanage
{
    namespace UI
    {
        public class Credits : MonoBehaviour
        {
            #region Variablen
            [Header ("Refrences")]
            [SerializeField]
            private Animator m_animator;
            #endregion

            // Update is called once per frame
            void Update()
            {
                //solbald man den State "Finish" im Animator erreicht, werden die Credits angezeigt
                if (m_animator.GetCurrentAnimatorStateInfo(0).IsName("Finish"))
                {
                    PlayerPrefs.DeleteKey(EPlayerPrefs.SAVEPOINT.ToString());
                    SceneManager.LoadScene(0, LoadSceneMode.Single);
                }
                
            }
        }
    }
}
