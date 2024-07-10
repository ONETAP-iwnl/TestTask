using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    public int cubeID;
    [HideInInspector]
    public Transform spawnPoint;
    public bool IsDropped = false;

    private void Start()
    {
        spawnPoint = transform.parent;
    }
}
