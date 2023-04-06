using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnMap : MonoBehaviour
{
    [SerializeField]
    private GameObject mapPrefab;

    public void SpawnMaper()
    {
        Instantiate(mapPrefab);
    }
}
