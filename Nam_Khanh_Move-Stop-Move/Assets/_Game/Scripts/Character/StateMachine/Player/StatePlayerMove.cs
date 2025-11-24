using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatePlayerMove : IState
{
        private Player player;

        public StatePlayerMove(Player p)
        {
            player = p;
        }

        public void OnEnter()
        {
            player.ChangeAnim("run");
        }

        public void OnExecute()
        {
        if (player.currentTarget != null)
        {
            player.stateMachine.ChangeState(player.attackState);
            return;
        }

        Vector3 dir = JoystickControl.direct;

        if (dir == Vector3.zero)
        {
            player.stateMachine.ChangeState(player.idleState);
            return;
        }

        Vector3 next = player.TF.position + dir * player.Speed * Time.deltaTime;
        player.TF.position = player.CheckGround(next);
        player.model.forward = dir;
    }

        public void OnExit() { }

}
