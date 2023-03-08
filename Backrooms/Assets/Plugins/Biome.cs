using UnityEngine.Audio;
using UnityEngine;
[System.Serializable]
public class Biome
{
    public string BiomeName;
    public int ChanceOfBiome;
    public int[] MaxAmountChunks = new int[2];
}
