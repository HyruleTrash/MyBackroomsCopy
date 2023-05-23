using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlmondwaterSpin : MonoBehaviour
{
    public int Speed = 20;
    float time = 0;

    void Update()
    {
        time += Time.deltaTime * Speed;

        transform.eulerAngles = new Vector3(0, time, 0);
    }
}
