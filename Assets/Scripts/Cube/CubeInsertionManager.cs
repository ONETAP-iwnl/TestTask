using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeInsertionManager : MonoBehaviour
{
    public GameObject[] targetZones;
    public GameObject[] zone1TargetZones;

    private List<GameObject> insertedCubes = new();

    public void InsertCube(GameObject cube, int zoneIndex)
    {
        GameObject zone = targetZones[zoneIndex];
        cube.transform.position = zone.transform.position;
        cube.transform.rotation = zone.transform.rotation;
        cube.transform.SetParent(zone.transform);
        insertedCubes.Add(cube);
    }

    public bool IsZoneOccupied(int zoneIndex)
    {
        GameObject zone = targetZones[zoneIndex];
        return zone.transform.childCount > 0;
    }

    public bool CheckCorrectPlacement()
    {
        bool allCorrect = true;

        for (int i = 0; i < targetZones.Length; i++)
        {
            GameObject zone = targetZones[i];
            if (zone.transform.childCount == 0)
            {
                allCorrect = false;
                Debug.Log("куб не вставлен в целевую зону " + i);
            }
            else
            {
                GameObject actualCube = zone.transform.GetChild(0).gameObject;
                Cube actualCubeComponent = actualCube.GetComponent<Cube>();

                GameObject correctCubeInZone1 = zone1TargetZones[i].transform.GetChild(0).gameObject;
                Cube correctCubeComponent = correctCubeInZone1.GetComponent<Cube>();

                if (actualCubeComponent.cubeID != correctCubeComponent.cubeID)
                {
                    allCorrect = false;
                    Debug.Log("Неправильный куб в таргет-зоне " + i);
                }
            }
        }

        return allCorrect;
    }
}
