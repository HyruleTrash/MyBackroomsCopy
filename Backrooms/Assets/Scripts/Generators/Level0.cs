using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Level0 : MonoBehaviour
{
    public class NewChunk
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public string Biome;
        public int Id = -1;
        public int DistanceFromOrigin;

        public NewChunk(Vector3 pos, Quaternion rot, string name, int Newid)
        {
            Biome = name;
            Rotation = rot;
            Position = pos;
            Id = Newid;
        }
    }

    public GameObject Player;

    public GameObject[] ChunkRoofTiles;
    public Chunk[] Chunks;
    public Biome[] Biomes;

    public GameObject[] Walls;

    public GameObject tempVisualizer;

    public Vector3 Distance;
    public int RenderDistance = 40;

    public int sizeSpawnChunks = 24;
    public int sizeRenderDistance = 24;

    public int HeighestId = 0;
    public Vector3 LatestCurrentChunkPos;
    public Vector3 CurrentChunkPos;

    List<NewChunk> SpawnChunks = new List<NewChunk>();
    List<NewChunk> AllChunks = new List<NewChunk>();
    List<NewChunk> NewChunks = new List<NewChunk>();

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
    public bool DoesChunksContainBiome(string BiomeName, List<NewChunk> List)
    {
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].Biome == BiomeName)
            {
                return true;
            }
        }
        return false;
    }
    public List<NewChunk> GetAllOfBiomeType(string BiomeName, List<NewChunk> Chunks)
    {
        List<NewChunk> FoundChunks = new List<NewChunk>();
        for (int i = 0; i < Chunks.Count; i++)
        {
            if (Chunks[i].Biome == BiomeName)
            {
                FoundChunks.Add(Chunks[i]);
            }
        }
        return FoundChunks;
    }
    public int GetChunkListIDWithSamePosition(Vector3 pos, List<NewChunk> List)
    {
        int FoundID = -1;
        for (int i = 0; i < List.Count; i++)
        {
            if (List[i].Position == pos)
            {
                FoundID = i;
                break;
            }
        }
        return FoundID;
    }
    public Vector3 NearestChunkPosition(Vector3 GivenPos)
    {
        Vector3 NearestChunk = new Vector3((int)Math.Round(GivenPos.x / Distance.x, 0), (int)Math.Round(GivenPos.y / Distance.y, 0), (int)Math.Round(GivenPos.z / Distance.z, 0));
        return NearestChunk;
    }

    public void GrowBiomeSpawn(NewChunk chunk)
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
            NewChunk FoundChunk = new NewChunk(Vector3.zero, new Quaternion(0, 0, 0, 0), "temp", -1);
            int tempFoundint = -1;
            // Go through all directions + a random one extra
            if (i != NearestChunks.Length)
            {
                tempFoundint = GetChunkListIDWithSamePosition(chunk.Position + NearestChunks[i], SpawnChunks);
                if (tempFoundint < SpawnChunks.Count && tempFoundint != -1)
                {
                    FoundChunk = SpawnChunks[tempFoundint];
                }
                else
                {
                    /* Debug.Log("No Chunk Found on position: " + (chunk.Position + NearestChunks[i]));
                    And so make it exist */
                    HeighestId++;
                    int Dir = Random.Range(-1, 2);
                    FoundChunk = new NewChunk((chunk.Position + NearestChunks[i]), Quaternion.Euler(0, 90f * Dir, 0), "temp", HeighestId);
                    SpawnChunks.Add(FoundChunk);
                }
            }
            else
            {
                int RandomDir = Random.Range(0, NearestChunks.Length);
                tempFoundint = GetChunkListIDWithSamePosition(chunk.Position + NearestChunks[RandomDir] * 2, SpawnChunks);
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
                    SpawnChunks[GetChunkListIDWithSamePosition(FoundChunk.Position, SpawnChunks)] = FoundChunk;
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
                        SpawnChunks[GetChunkListIDWithSamePosition(FoundChunk.Position, SpawnChunks)] = FoundChunk;
                        if (FoundChunk.DistanceFromOrigin - 1 > 0)
                        {
                            NextChunks.Add(FoundChunk);
                        }
                    } else if (chunk.DistanceFromOrigin - 1 > FoundChunk.DistanceFromOrigin)
                    {
                        FoundChunk.Biome = chunk.Biome;
                        FoundChunk.DistanceFromOrigin = chunk.DistanceFromOrigin - 1;
                        SpawnChunks[GetChunkListIDWithSamePosition(FoundChunk.Position, SpawnChunks)] = FoundChunk;
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
            GrowBiomeSpawn(NextChunks[i]);
        }
        int index = CurrentBiomeGrowings.IndexOf(false);
        if (index != -1)
        {
            CurrentBiomeGrowings.RemoveAt(index);
            CurrentBiomeGrowings.Insert(index, true);
        }
    }

    public void GrowBiomeNewChunks(NewChunk chunk)
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
            NewChunk FoundChunk = new NewChunk(Vector3.zero, new Quaternion(0, 0, 0, 0), "temp", -1);
            int tempFoundint = -1;
            // Go through all directions + a random one extra
            if (i != NearestChunks.Length)
            {
                tempFoundint = GetChunkListIDWithSamePosition(chunk.Position + NearestChunks[i], AllChunks);
                if (tempFoundint < AllChunks.Count && tempFoundint != -1)
                {
                    FoundChunk = AllChunks[tempFoundint];
                }
                else
                {
                    /* Debug.Log("No Chunk Found on position: " + (chunk.Position + NearestChunks[i]));
                    And so make it exist */
                    HeighestId++;
                    int Dir = Random.Range(-1, 2);
                    FoundChunk = new NewChunk((chunk.Position + NearestChunks[i]), Quaternion.Euler(0, 90f * Dir, 0), "temp", HeighestId);
                    AllChunks.Add(FoundChunk);
                }
            }
            else
            {
                int RandomDir = Random.Range(0, NearestChunks.Length);
                tempFoundint = GetChunkListIDWithSamePosition(chunk.Position + NearestChunks[RandomDir] * 2, AllChunks);
                if (tempFoundint < AllChunks.Count && tempFoundint != -1)
                {
                    FoundChunk = AllChunks[tempFoundint];
                }/*
                else
                {
                    Debug.Log("No Chunk Found on position, that was chosen with random Dir: " + (chunk.Position + NearestChunks[RandomDir] * 2));
                }*/
            }

            // if growable, then grow
            if (tempFoundint < AllChunks.Count && tempFoundint != -1)
            {
                if (FoundChunk.Biome == "temp")
                {
                    FoundChunk.Biome = chunk.Biome;
                    FoundChunk.DistanceFromOrigin = chunk.DistanceFromOrigin - 1;
                    AllChunks[GetChunkListIDWithSamePosition(FoundChunk.Position, AllChunks)] = FoundChunk;
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
                        AllChunks[GetChunkListIDWithSamePosition(FoundChunk.Position, AllChunks)] = FoundChunk;
                        if (FoundChunk.DistanceFromOrigin - 1 > 0)
                        {
                            NextChunks.Add(FoundChunk);
                        }
                    }
                    else if (chunk.DistanceFromOrigin - 1 > FoundChunk.DistanceFromOrigin)
                    {
                        FoundChunk.Biome = chunk.Biome;
                        FoundChunk.DistanceFromOrigin = chunk.DistanceFromOrigin - 1;
                        AllChunks[GetChunkListIDWithSamePosition(FoundChunk.Position, AllChunks)] = FoundChunk;
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
            GrowBiomeNewChunks(NextChunks[i]);
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
        for (int i = 0 - (sizeSpawnChunks / 2); i < (sizeSpawnChunks / 2); i++)
        {
            for (int j = 0 - (sizeSpawnChunks / 2); j < (sizeSpawnChunks / 2); j++)
            {
                HeighestId++;
                int Dir = Random.Range(-1, 2);
                SpawnChunks.Add(new NewChunk(new Vector3(i * Distance.x, 0, j * Distance.z), Quaternion.Euler(0, 90f * Dir, 0), "temp", HeighestId));
            }
        }
        // Create Biome Origin Points
        List<NewChunk> AllOfTempBiomeType = GetAllOfBiomeType("temp", SpawnChunks);
        for (int x = 0; x < sizeSpawnChunks * 0.2; x++)
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
                SpawnChunks[GetChunkListIDWithSamePosition(Origin.Position, SpawnChunks)] = Origin;

                // Grow Biome
                GrowBiomeSpawn(Origin);

                //Again If needed
                if (DoesChunksContainBiome("temp", SpawnChunks) == true)
                {
                    AllOfTempBiomeType = GetAllOfBiomeType("temp", SpawnChunks);
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
            GameObject Chunk = Instantiate(RandomChunk.ChunkObject, SpawnChunks[i].Position, SpawnChunks[i].Rotation, this.gameObject.transform);
            Chunk.GetComponent<ChunkInfo>().BiomeName = SpawnChunks[i].Biome;
            Chunk.GetComponent<ChunkInfo>().DistanceFromOriginPoint = SpawnChunks[i].DistanceFromOrigin;
            Chunk.GetComponent<ChunkInfo>().Id = SpawnChunks[i].Id;
            GameObject RoofTile = Instantiate(ChunkRoofTiles[0], ChunkRoofTiles[0].transform.position + Chunk.transform.position, ChunkRoofTiles[0].transform.rotation, Chunk.transform);
        }
        AllChunks = SpawnChunks;
        CurrentChunkPos = NearestChunkPosition(Player.transform.position);
        LatestCurrentChunkPos = CurrentChunkPos;

        // loop trough all chunks and check if there is nothing next to them
        for (int i = 0; i < AllChunks.Count; i++)
        {
            Vector3[] NearestChunks = {
            new Vector3(Distance.x, 0, 0),
            new Vector3(-Distance.x, 0, 0),
            new Vector3(0, 0, Distance.z),
            new Vector3(0, 0, -Distance.z)
            };
            Vector3[] LookDirections = {
            new Vector3(0, 180, 0),
            new Vector3(0, 0, 0),
            new Vector3(0, 90, 0),
            new Vector3(0, -90, 0),
            };

            for (int j = 0; j < NearestChunks.Length; j++)
            {
                int chunkID = GetChunkListIDWithSamePosition(AllChunks[i].Position + NearestChunks[j], AllChunks);
                if (chunkID == -1)
                {
                    GameObject wall = Instantiate(Walls[Random.Range(0, Walls.Length)], AllChunks[i].Position + (NearestChunks[j] / 2), Quaternion.Euler(LookDirections[j]), this.gameObject.transform);
                }
            }
        }
    }

    private void Update()
    {
        CurrentChunkPos = NearestChunkPosition(Player.transform.position);
    }
}
