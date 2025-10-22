using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

public class Level : MonoBehaviour
{
    public NavMeshData navMeshData;
    public Transform SpawnPoint;
    public Transform startPoint;
    public int botAmount;

    public void OnInit()
    {

    }
}