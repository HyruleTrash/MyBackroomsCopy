using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInfo : MonoBehaviour
{
    public string BiomeName = "default";
    public int DistanceFromOriginPoint;
    public Material[] materials;
    public bool DebugBiomeColours = false;
    public int Id = -1;

    private void Start()
    {
        if (DebugBiomeColours)
        {
            foreach (Transform child in this.transform)
            {
                if (BiomeName == "default")
                {
                    child.GetComponent<MeshRenderer>().material = materials[0];
                }
                else
                {
                    child.GetComponent<MeshRenderer>().material = materials[1];
                }
            }
        }
    }
}
