using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeFiller : MonoBehaviour
{
    public GameObject[] cubePrefabs;
    public GameObject[] targetPositions; 

    void Start()
    {
        FillRandomCubes();
    }

    void FillRandomCubes()
    {
        foreach (GameObject targetPosition in targetPositions)
        {
            GameObject randomCubePrefab = cubePrefabs[Random.Range(0, cubePrefabs.Length)];
            GameObject cube = Instantiate(randomCubePrefab, targetPosition.transform.position, Quaternion.identity, targetPosition.transform);
            cube.GetComponent<Cube>().cubeID = Random.Range(0, cubePrefabs.Length);
        }
    }
}
