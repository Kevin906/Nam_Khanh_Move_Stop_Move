using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VisionRange : MonoBehaviour
{
    public System.Action<Transform> OnTargetEnter;
    public System.Action<Transform> OnTargetExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IDamageAble>(out var target))
            OnTargetEnter?.Invoke(other.transform);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.TryGetComponent<IDamageAble>(out var target))
            OnTargetExit?.Invoke(other.transform);
    }
}

