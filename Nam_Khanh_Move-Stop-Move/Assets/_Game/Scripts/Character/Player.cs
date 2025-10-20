using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character
{
    [SerializeField] private float speed = 5;

    void Update()
    {
		if (GameManager.Instance.IsState(GameState.Gameplay))
        {
			if (Input.GetMouseButton(0))
			{
				Vector3 nectPoint = JoystickControl.direct * speed * Time.deltaTime + TF.position;
			}
		}
    }
}
