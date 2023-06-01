using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmondSpawner : MonoBehaviour
{
    public GameObject AlmondCollectable;
    public GameObject StatTracker;

    void Start()
    {
        StatTracker = GameObject.Find("StatTracker");
        if (Random.Range(0, 92) < 1)
        {
            StatTracker.GetComponent<StatTracker>().ToBeCollectedAlmondWater++;
            Instantiate(AlmondCollectable, transform);
        }
    }
}
