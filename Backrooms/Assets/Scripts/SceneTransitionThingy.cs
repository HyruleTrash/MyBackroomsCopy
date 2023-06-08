using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneTransitionThingy : MonoBehaviour
{
    public Material White;
    public List<Material> originalMaterials = new List<Material>();
    public bool isTransitionOrNot = false;
    bool isWhite = false;
    float _Time = 0;
    float timeBetweenShift = 0;
    public float MaxtimeBetweenShifts = 0.02f;
    public float MintimeBetweenShifts = 0.01f;

    void Start()
    {
        if (Random.Range(0, (2400 / 5)) < 1)
        {
            for (int i = 0; i < gameObject.GetComponent<MeshRenderer>().materials.Length; i++)
            {
                originalMaterials.Add(gameObject.GetComponent<MeshRenderer>().materials[i]);
            }
            isTransitionOrNot = true;
            Debug.Log(transform.parent.GetComponent<ChunkInfo>().Id);
        }
    }

    void Update()
    {
        if (isTransitionOrNot == true && transform.GetComponent<MeshRenderer>().isVisible)
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
                isWhite = !isWhite;

                Material[] toChange = gameObject.GetComponent<MeshRenderer>().materials;
                if (isWhite)
                {
                    gameObject.GetComponent<MeshRenderer>().material = originalMaterials[0];
                }
                else
                {
                    gameObject.GetComponent<MeshRenderer>().material = White;
                }
            }
        }
    }
}
