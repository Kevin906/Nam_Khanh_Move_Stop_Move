using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerDie : IState
{
        private Player player;

        public StatePlayerDie(Player p)
        {
            player = p;
        }

        public void OnEnter()
        {
            player.ChangeAnim("dead");
        }

        public void OnExecute() { }
        public void OnExit() { }
}