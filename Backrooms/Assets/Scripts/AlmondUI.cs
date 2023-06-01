using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AlmondUI : MonoBehaviour
{
    public GameObject player;
    public GameObject StatTracker;
    public TextMeshPro Text;

    int CollectedAlmondWater = 0;
    int ToBeCollectedAlmondWater = 0;

    Vector3 Offset;

    private void Start()
    {
        player = GameObject.Find("Player");
        StatTracker = GameObject.Find("StatTracker");
        Offset = gameObject.GetComponent<RectTransform>().position;
    }

    private void Update()
    {
        CollectedAlmondWater = StatTracker.GetComponent<StatTracker>().CollectedAlmondWater;
        ToBeCollectedAlmondWater = StatTracker.GetComponent<StatTracker>().ToBeCollectedAlmondWater;

        Text.text = CollectedAlmondWater + " / " + ToBeCollectedAlmondWater;

        gameObject.GetComponent<RectTransform>().position = new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z) + Offset;
    }
}
