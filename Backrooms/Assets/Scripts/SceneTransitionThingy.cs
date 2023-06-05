using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionThingy : MonoBehaviour
{
    public Material White;
    public Material[] originalMaterials;
    bool isTransitionOrNot = false;
    bool isWhite = false;
    float _Time = 0;
    float timeBetweenShift = 0;
    public float MaxtimeBetweenShifts = 2;
    public float MintimeBetweenShifts = 1;

    void Start()
    {
        if (Random.Range(0, 92) < 1)
        {
            originalMaterials = gameObject.GetComponent<MeshRenderer>().materials;
            isTransitionOrNot = true;
            Debug.Log(transform.parent.gameObject.GetComponent<ChunkInfo>().Id);
        }
    }

    void Update()
    {
        if (isTransitionOrNot == true)
        {
            if (timeBetweenShift == 0)
            {
                timeBetweenShift = Random.Range(MintimeBetweenShifts, MaxtimeBetweenShifts);
            }

            _Time += Time.deltaTime;

            if (_Time > timeBetweenShift)
            {
                _Time = 0;
                timeBetweenShift = 0;
                if (isWhite)
                {
                    gameObject.GetComponent<MeshRenderer>().materials = originalMaterials;
                }
                else
                {
                    for (int i = 0; i < gameObject.GetComponent<MeshRenderer>().materials.Length; i++)
                    {
                        gameObject.GetComponent<MeshRenderer>().materials[i] = White;
                    }
                }
            }
        }
    }
}
