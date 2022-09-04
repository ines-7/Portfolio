using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Orphanage
{
    namespace AI
    {
        public abstract class AEnemy : MonoBehaviour
        {
            #region Variablen
            public AState m_idle;
            public AState m_search;
            public AState m_attack;
            public AState m_hunt;
            public AState m_deactivated;

            public NavMeshAgent m_agent;
            protected StateMachine m_stateMachine;
            public Animator m_animator;
            #endregion

            // Start is called before the first frame update
            void Start()
            {
                Initialize();
            }

            /// <summary>
            /// Ruft die Update-Methode der StateMachine auf
            /// </summary>
            void Update()
            {
                m_stateMachine.Update();
            }

            /// <summary>
            /// Wird in den jeweiligen Kindklassen bestimmt. Methode wird dann in der Start() ausgeführt.
            /// </summary>
            protected abstract void Initialize();
        }


    }
}
