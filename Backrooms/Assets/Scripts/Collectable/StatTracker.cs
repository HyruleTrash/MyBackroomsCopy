using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatTracker : MonoBehaviour
{
    public float CollectedAlmondWater = 20;
    public float ToBeCollectedAlmondWater = 20;

    float _Time;
    bool IsTiming = false;
    float MaxTime = 2;

    private void Start()
    {
        CollectedAlmondWater = ToBeCollectedAlmondWater;
    }

    private void Update()
    {
        CollectedAlmondWater -= 0.1f * Time.deltaTime;
        if (IsTiming == true)
        {
            _Time += Time.deltaTime;
            if (_Time >= MaxTime)
            {
                IsTiming = false;
                _Time = 0;
            }
        }
        if (CollectedAlmondWater <= 0)
        {
            //AppHelper.Quit();
        }
    }

    public void AddAlmondWaterCollect()
    {
        if (!IsTiming)
        {
            CollectedAlmondWater++;
            if (CollectedAlmondWater >= ToBeCollectedAlmondWater)
            {
                CollectedAlmondWater = ToBeCollectedAlmondWater;
            }
            IsTiming = true;
        }
    }
}
