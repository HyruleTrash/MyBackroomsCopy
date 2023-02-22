using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : MonoBehaviour
{
    public Chunk[] Chunks;

    public Vector3 Distance;

    int size = 24;

    void Start()
    {
        for (int i = 0 - (size / 2); i < ( size / 2 ); i++)
        {
            for (int j = 0 - (size / 2); j < (size / 2); j++)
            {
                int ID = Random.Range(0, Chunks.Length);
                int Dir = Random.Range(-1, 2);
                Instantiate(Chunks[ID].ChunkObject, new Vector3(i * Distance.x, 0, j * Distance.z), Quaternion.Euler(0,90f*Dir, 0));
            }
        }
    }

    
}
