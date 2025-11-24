using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
        private IState currentState;

        public void ChangeState(IState newState)
        {
            currentState?.OnExit();
            currentState = newState;
            currentState.OnEnter();
        }

        public void Update()
        {
            currentState?.OnExecute();
        }


}
