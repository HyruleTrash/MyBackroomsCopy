using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : MonoBehaviour
{
    public Chunk[] Chunks;
    public Biome[] Biomes;

    public Vector3 Distance;

    public int size = 24;

    void Start()
    {
        int BiomeID = Random.Range(0, Biomes.Length);
        int CurrentAmountOfChunks = 0;
        for (int i = 0 - (size / 2); i < ( size / 2 ); i++)
        {
            for (int j = 0 - (size / 2); j < (size / 2); j++)
            {
                CurrentAmountOfChunks++;


                int RandValue = Random.Range(0, 100);
                int previousPercentages = 0;
                int ID = 0;
                bool ChunkFound = false;
                for (int x = 0; x < Chunks.Length; x++)
                {
                    // get Correct Biome
                    for (int y = 0; y < Chunks[x].Chances.Length; y++)
                    {
                        if (Chunks[x].Chances[y].BiomeName == Biomes[BiomeID].BiomeName && !ChunkFound)
                        {
                            previousPercentages += Chunks[x].Chances[y].Chance;
                            if (RandValue <= previousPercentages)
                            {
                                ID = x;
                                ChunkFound = true;
                                break;
                            }
                        }
                    }
                    
                }
                //ID = Random.Range(0, Chunks.Length);
                int Dir = Random.Range(-1, 2);
                GameObject Chunk = Instantiate(Chunks[ID].ChunkObject, new Vector3(i * Distance.x, 0, j * Distance.z), Quaternion.Euler(0,90f*Dir, 0));
                Chunk.GetComponent<ChunkInfo>().BiomeName = Biomes[BiomeID].BiomeName;


                if (Biomes[BiomeID].MaxAmountChunks < CurrentAmountOfChunks)
                {
                    BiomeID = Random.Range(0, Biomes.Length);
                    CurrentAmountOfChunks = 0;
                }
            }
        }
    }

    
}
