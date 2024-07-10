using UnityEngine;

public class CubeManager : MonoBehaviour
{
    public GameObject[] cubePrefabs;
    public Transform[] spawnPoints; 

    private void Start()
    {
        for (int i = 0; i < spawnPoints.Length; i++)
        {
            SpawnCube(i);
        }
    }

    private void SpawnCube(int index)
    {
        if (index >= 0 && index < cubePrefabs.Length && index < spawnPoints.Length)
        {
            GameObject cube = Instantiate(cubePrefabs[index], spawnPoints[index].position, spawnPoints[index].rotation);
            Cube cubeScript = cube.GetComponent<Cube>();
            cubeScript.spawnPoint = spawnPoints[index];
        }
        else
        {
            Debug.LogError("вышел за границу массива, индекс: " + index);
        }
    }
}
