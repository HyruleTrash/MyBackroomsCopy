using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkInfo : MonoBehaviour
{
    public string BiomeName = "default";
    public Material[] materials;
    public bool DebugBiomeColours = false;

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
