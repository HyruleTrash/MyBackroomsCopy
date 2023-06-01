using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public int CollectedAlmondWater;
    public int ToBeCollectedAlmondWater;

    float _Time;
    bool IsTiming = false;
    float MaxTime = 2;

    private void Update()
    {
        if (IsTiming == true)
        {
            _Time += Time.deltaTime;
            if (_Time >= MaxTime)
            {
                IsTiming = false;
                _Time = 0;
            }
        }
    }

    public void AddAlmondWaterCollect()
    {
        if (!IsTiming)
        {
            CollectedAlmondWater++;
            IsTiming = true;
        }
    }
}
