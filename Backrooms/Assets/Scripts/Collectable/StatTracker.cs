using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StatTracker : MonoBehaviour
{
    public float CollectedAlmondWater = 20;
    public float ToBeCollectedAlmondWater = 20;

    float _Time;
    bool IsTiming = false;
    float MaxTime = 2;

    public GameObject RedFilter;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        CollectedAlmondWater = ToBeCollectedAlmondWater;
    }

    private void Update()
    {
        
        CollectedAlmondWater -= 0.1f * Time.deltaTime;
        RedFilter.GetComponent<UnityEngine.Rendering.PostProcessing.PostProcessVolume>().weight = Mathf.Abs(CollectedAlmondWater / ToBeCollectedAlmondWater - 1);
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
            AppHelper.Quit();
        }
    }

    public void AddAlmondWaterCollect(int amount)
    {
        if (!IsTiming)
        {
            CollectedAlmondWater += amount;
            if (CollectedAlmondWater >= ToBeCollectedAlmondWater)
            {
                CollectedAlmondWater = ToBeCollectedAlmondWater;
            }
            IsTiming = true;
        }
    }
}
