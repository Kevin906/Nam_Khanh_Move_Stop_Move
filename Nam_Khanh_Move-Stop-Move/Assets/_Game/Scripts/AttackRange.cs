using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackRange : MonoBehaviour
{
    [SerializeField] private Transform rangeCircle;
    [SerializeField] private float attackRange = 3f;

    void Start()
    {
        UpdateRangeCircle();
        ShowRange(true);
    }

    public void UpdateRangeCircle()
    {
        if (rangeCircle != null)
        {
            float scale = attackRange / 5f;
            rangeCircle.localScale = new Vector3(10, 10, scale);
        }
    }

    public void ShowRange(bool show)
    {
        if (rangeCircle != null)
            rangeCircle.gameObject.SetActive(show);
    }
}
