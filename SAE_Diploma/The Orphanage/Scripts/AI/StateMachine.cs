using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Orphanage
{
    namespace AI
    {
        public class StateMachine
        {
            private AState m_currentState;

            /// <summary>
            /// Setzt den startState und ruft deren Start-Methode auf
            /// </summary>
            /// <param name="startState"></param>
            public void Initialize(AState startState)
            {
                m_currentState = startState;
                m_currentState.Start();
            }

            /// <summary>
            /// Ruft die Update-Methode des derzeitigen States auf
            /// </summary>
            public void Update()
            {
                m_currentState.Update();
            }

            /// <summary>
            /// Ändert den derzeitigen State und ruft die Start- und Update-Methode des States auf
            /// </summary>
            /// <param name="newState"></param>
            public void MakeTransition(AState newState)
            {
                m_currentState = newState;
                m_currentState.Start();
                m_currentState.Update();
            }
        }
    }
}
