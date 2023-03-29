using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level0 : MonoBehaviour
{
    public class NewChunk
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public string Biome;
        public int DistanceFromOrigin;

        public NewChunk(Vector3 pos, Quaternion rot, string name)
        {
            Biome = name;
            Rotation = rot;
            Position = pos;
        }
    }

    public GameObject[] ChunkRoofTiles;
    public Chunk[] Chunks;
    public Biome[] Biomes;

    public GameObject tempVisualizer;

    public Vector3 Distance;

    public int size = 24;

    List<NewChunk> SpawnChunks = new List<NewChunk>();

    List<bool> CurrentBiomeGrowings = new List<bool>();


    public Biome GetRandomBiome(Biome[] biomes)
    {
        // Calculate the total chance of all biomes
        int totalChance = 0;
        foreach (Biome biome in biomes)
        {
            totalChance += biome.ChanceOfBiome;
        }

        // Generate a random number between 0 and the total chance
        int randomNum = Random.Range(0, totalChance);

        // Iterate over each biome and subtract its chance of being chosen
        // from the random number until the random number becomes negative.
        // The biome whose chance of being chosen caused the random number to
        // become negative is the biome that is chosen.
        foreach (Biome biome in biomes)
        {
            randomNum -= biome.ChanceOfBiome;
            if (randomNum < 0)
            {
                return biome;
            }
        }

        // If no biome was chosen, return null
        return null;
    }
    public Chunk GetRandomChunk(Chunk[] chunks, string biomeName)
    {
        if (biomeName == "temp")
        {
            Chunk empty = new Chunk();
            empty.ChunkObject = tempVisualizer;
            return empty;
        }
        else
        {
            List<Chunk> matchingChunks = new List<Chunk>();
            int totalWeight = 0;

            // Find all chunks that match the given biome name
            foreach (Chunk chunk in chunks)
            {
                foreach (BiomeChance chance in chunk.Chances)
                {
                    if (chance.BiomeName == biomeName)
                    {
                        matchingChunks.Add(chunk);

                        // Add the chance to the total weight, but check for overflow
                        checked
                        {
                            totalWeight += chance.Chance;
                        }

                        break; // Stop checking chances for this chunk
                    }
                }
            }

            // Generate a random number between 0 and the total weight
            int randomNumber = Random.Range(0, totalWeight);

            // Iterate through the matching chunks and subtract their chance from the random number until it becomes less than or equal to 0
            foreach (Chunk chunk in matchingChunks)
            {
                foreach (BiomeChance chance in chunk.Chances)
                {
                    if (chance.BiomeName == biomeName)
                    {
                        randomNumber -= chance.Chance;
                        if (randomNumber <= 0 && chance.Chance != 0)
                        {
                            if (chunk.ChunkObject.gameObject.name.Contains("(1)") && biomeName == "Pillars")
                            {
                                Debug.Log(chance.Chance);
                            }
                            return chunk;
                        }
                    }
                }
            }

            // This should never happen, but throw an exception if no chunk is found
            throw new System.Exception("No chunk found for biome " + biomeName + ".");
        }
    }
    public int GetChanceByBiomeName(string BiomeName)
    {
        for (int i = 0; i < Biomes.Length; i++)
        {
            if (Biomes[i].BiomeName == BiomeName)
            {
                return Biomes[i].ChanceOfBiome;
            }
        }
        return 0;
    }
    public bool DoesSpawnChunksContainBiome(string BiomeName)
    {
        for (int i = 0; i < SpawnChunks.Count; i++)
        {
            if (SpawnChunks[i].Biome == BiomeName)
            {
                return true;
            }
        }
        return false;
    }
    public List<NewChunk> GetAllOfBiomeType(string BiomeName)
    {
        List<NewChunk> FoundChunks = new List<NewChunk>();
        for (int i = 0; i < SpawnChunks.Count; i++)
        {
            if (SpawnChunks[i].Biome == BiomeName)
            {
                FoundChunks.Add(SpawnChunks[i]);
            }
        }
        return FoundChunks;
    }
    public int GetChunkIDWithSamePosition(Vector3 pos)
    {
        int FoundID = -1;
        for (int i = 0; i < SpawnChunks.Count; i++)
        {
            if (SpawnChunks[i].Position == pos)
            {
                FoundID = i;
                break;
            }
        }
        return FoundID;
    }

    public void GrowBiome(NewChunk chunk)
    {
        CurrentBiomeGrowings.Add(false);


        Vector3[] NearestChunks = {
            new Vector3(Distance.x, 0, 0),
            new Vector3(-Distance.x, 0, 0),
            new Vector3(0, 0, Distance.z),
            new Vector3(0, 0, -Distance.z),
            new Vector3(Distance.x, 0, -Distance.z),
            new Vector3(-Distance.x, 0, -Distance.z),
            new Vector3(-Distance.x, 0, Distance.z),
            new Vector3(Distance.x, 0, Distance.z),
        };
        List<NewChunk> NextChunks = new List<NewChunk>();
        for (int i = 0; i < NearestChunks.Length + 1; i++)
        {
            NewChunk FoundChunk = new NewChunk(Vector3.zero, new Quaternion(0, 0, 0, 0), "temp");
            int tempFoundint = -1;
            // Go through all directions + a random one extra
            if (i != NearestChunks.Length)
            {
                tempFoundint = GetChunkIDWithSamePosition(chunk.Position + NearestChunks[i]);
                if (tempFoundint < SpawnChunks.Count && tempFoundint != -1)
                {
                    FoundChunk = SpawnChunks[tempFoundint];
                }
                else
                {
                    /* Debug.Log("No Chunk Found on position: " + (chunk.Position + NearestChunks[i]));
                    And so make it exist */
                    int Dir = Random.Range(-1, 2);
                    FoundChunk = new NewChunk((chunk.Position + NearestChunks[i]), Quaternion.Euler(0, 90f * Dir, 0), "temp");
                    SpawnChunks.Add(FoundChunk);
                }
            }
            else
            {
                int RandomDir = Random.Range(0, NearestChunks.Length);
                tempFoundint = GetChunkIDWithSamePosition(chunk.Position + NearestChunks[RandomDir] * 2);
                if (tempFoundint < SpawnChunks.Count && tempFoundint != -1)
                {
                    FoundChunk = SpawnChunks[tempFoundint];
                }/*
                else
                {
                    Debug.Log("No Chunk Found on position, that was chosen with random Dir: " + (chunk.Position + NearestChunks[RandomDir] * 2));
                }*/
            }

            // if growable, then grow
            if (tempFoundint < SpawnChunks.Count && tempFoundint != -1)
            {
                if (FoundChunk.Biome == "temp")
                {
                    FoundChunk.Biome = chunk.Biome;
                    FoundChunk.DistanceFromOrigin = chunk.DistanceFromOrigin - 1;
                    SpawnChunks[GetChunkIDWithSamePosition(FoundChunk.Position)] = FoundChunk;
                    if (FoundChunk.DistanceFromOrigin - 1 > 0)
                    {
                        NextChunks.Add(FoundChunk);
                    }
                }
                else
                {
                    if (GetChanceByBiomeName(chunk.Biome) < GetChanceByBiomeName(FoundChunk.Biome))
                    {
                        FoundChunk.Biome = chunk.Biome;
                        FoundChunk.DistanceFromOrigin = chunk.DistanceFromOrigin - 1;
                        SpawnChunks[GetChunkIDWithSamePosition(FoundChunk.Position)] = FoundChunk;
                        if (FoundChunk.DistanceFromOrigin - 1 > 0)
                        {
                            NextChunks.Add(FoundChunk);
                        }
                    } else if (chunk.DistanceFromOrigin - 1 > FoundChunk.DistanceFromOrigin)
                    {
                        FoundChunk.Biome = chunk.Biome;
                        FoundChunk.DistanceFromOrigin = chunk.DistanceFromOrigin - 1;
                        SpawnChunks[GetChunkIDWithSamePosition(FoundChunk.Position)] = FoundChunk;
                        if (FoundChunk.DistanceFromOrigin - 1 > 0)
                        {
                            NextChunks.Add(FoundChunk);
                        }
                    }
                }

            }
        }

        // remove from current info of growing
        for (int i = 0; i < NextChunks.Count; i++)
        {
            GrowBiome(NextChunks[i]);
        }
        int index = CurrentBiomeGrowings.IndexOf(false);
        if (index != -1)
        {
            CurrentBiomeGrowings.RemoveAt(index);
            CurrentBiomeGrowings.Insert(index, true);
        }
    }

    void Start()
    {
        // Make spawn Chunks
        for (int i = 0 - (size / 2); i < (size / 2); i++)
        {
            for (int j = 0 - (size / 2); j < (size / 2); j++)
            {
                int Dir = Random.Range(-1, 2);
                SpawnChunks.Add(new NewChunk(new Vector3(i * Distance.x, 0, j * Distance.z), Quaternion.Euler(0, 90f * Dir, 0), "temp"));
            }
        }

        // Create Biome Origin Points
        List<NewChunk> AllOfTempBiomeType = GetAllOfBiomeType("temp");
        for (int x = 0; x < size * 0.2; x++)
        {
            if (CurrentBiomeGrowings.IndexOf(false) == -1)
            {
                // Pick Random Origin
                int OriginID = Random.Range(0, AllOfTempBiomeType.Count);
                NewChunk Origin = AllOfTempBiomeType[OriginID];
                AllOfTempBiomeType.Remove(Origin);

                // Get RandomBiome
                Biome GeneratedBiome = GetRandomBiome(Biomes);

                // Set Biome
                Origin.Biome = GeneratedBiome.BiomeName;
                Origin.DistanceFromOrigin = Random.Range(GeneratedBiome.MaxAmountChunks[0], GeneratedBiome.MaxAmountChunks[1]);
                SpawnChunks[GetChunkIDWithSamePosition(Origin.Position)] = Origin;

                // Grow Biome
                GrowBiome(Origin);

                //Again If needed
                if (DoesSpawnChunksContainBiome("temp") == true)
                {
                    AllOfTempBiomeType = GetAllOfBiomeType("temp");
                }
                else
                {
                    break;
                }
            }
        }

        // Generate Chunks
        for (int i = 0; i < SpawnChunks.Count; i++)
        {
            if (SpawnChunks[i].Biome == "temp")
            {
                SpawnChunks[i].Biome = "default";
            }
            Chunk RandomChunk = GetRandomChunk(Chunks, SpawnChunks[i].Biome);
            GameObject Chunk = Instantiate(RandomChunk.ChunkObject, SpawnChunks[i].Position, SpawnChunks[i].Rotation);
            Chunk.GetComponent<ChunkInfo>().BiomeName = SpawnChunks[i].Biome;
            Chunk.GetComponent<ChunkInfo>().DistanceFromOriginPoint = SpawnChunks[i].DistanceFromOrigin;
            GameObject RoofTile = Instantiate(ChunkRoofTiles[0], ChunkRoofTiles[0].transform.position + Chunk.transform.position, ChunkRoofTiles[0].transform.rotation, Chunk.transform);
        }
    }
}
