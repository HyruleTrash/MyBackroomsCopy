using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.SceneManagement;

public class StatTracker : MonoBehaviour
{
    public float CollectedAlmondWater = 20;
    public float ToBeCollectedAlmondWater = 20;

    public string[] Levels;
    public int CurrentLevel = 0;
    public string CurrentSceneName = "Level0";
    public bool AllowedToCountUpALevel = true;

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
            SceneManager.LoadScene("Menu");
            Destroy(gameObject);
        }

        if (AllowedToCountUpALevel == false && CurrentSceneName != SceneManager.GetActiveScene().name)
        {
            AllowedToCountUpALevel = true;
        }

        CurrentSceneName = SceneManager.GetActiveScene().name;
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

    public void NextLevel()
    {
        if (AllowedToCountUpALevel == true)
        {
            CurrentLevel++;
            AllowedToCountUpALevel = false;
            if (Levels.Length > CurrentLevel && CurrentLevel != 0)
            {
                Debug.Log(SceneManager.GetActiveScene().name);
                SceneManager.LoadScene(Levels[CurrentLevel]);
            }
            else
            {
                AppHelper.Quit();
            }
        }
    }
}
