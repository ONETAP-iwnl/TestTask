using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject[] cubePrefabs; // Префабы различных кубов
    public Transform[] spawnPoints; // Точки респауна для кубов
    public float respawnDelay = 2.0f; // Задержка перед респауном куба

    private Dictionary<Transform, GameObject> activeCubes = new Dictionary<Transform, GameObject>();

    private void Start()
    {
        if (cubePrefabs.Length != spawnPoints.Length)
        {
            Debug.LogError("Mismatch in cubePrefabs and spawnPoints arrays! Make sure they have the same number of elements.");
            return;
        }

        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnCube(i);
        }
    }

    public void CubePickedUp(Transform spawnPoint)
    {
        StartCoroutine(RespawnCube(spawnPoint));
    }

    private IEnumerator RespawnCube(Transform spawnPoint)
    {
        yield return new WaitForSeconds(respawnDelay);

        // Находим индекс точки респауна в массиве spawnPoints
        int index = System.Array.IndexOf(spawnPoints, spawnPoint);
        if (index != -1)
        {
            SpawnCube(index);
        }
        else
        {
            Debug.LogError("Spawn point not found in the spawnPoints array!");
        }
    }

    private void SpawnCube(int index)
    {
        if (index >= 0 && index < cubePrefabs.Length && index < spawnPoints.Length)
        {
            GameObject cube = Instantiate(cubePrefabs[index], spawnPoints[index].position, spawnPoints[index].rotation);
            Cube cubeScript = cube.GetComponent<Cube>();
            cubeScript.spawnPoint = spawnPoints[index];
            activeCubes[spawnPoints[index]] = cube;
        }
        else
        {
            Debug.LogError("Index out of bounds when spawning cube! Index: " + index);
        }
    }
}
