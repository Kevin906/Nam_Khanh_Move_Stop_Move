using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : GameUnit
{
    [SerializeField] private Transform Skin;
    public Animator Anim;
    private string CurrentAnim;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void OnInit()
	{
		throw new System.NotImplementedException();
	}

	public override void OnDespawn()
	{
		throw new System.NotImplementedException();
	}
}
