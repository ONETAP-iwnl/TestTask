using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cube : MonoBehaviour
{
    [HideInInspector]
    public Transform spawnPoint;

    private void Start()
    {
        spawnPoint = transform.parent;
    }
}
