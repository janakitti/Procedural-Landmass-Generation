using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public enum NormalizeMode {Local, Global};

    /// <summary>
    /// Generate a noise map by sampling points from Perlin noise
    /// </summary>
    /// <param name="mapWidth">Width of noise map</param>
    /// <param name="mapHeight">Height of noise map</param>
    /// <param name="seed">Seed for octave offset randomization</param>
    /// <param name="scale">Zoom scale of map</param>
    /// <param name="octaves">Controls number of layers of detail</param>
    /// <param name="persistence">Controls decay in amplitude over octaves</param>
    /// <param name="lacunarity">Controls frequenct/level of detail in an octave</param>
    /// <param name="offset"></param>
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset, NormalizeMode normalizeMode)
    {

        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        // Want each ocatve to be sampled from radically different locations on the Perlin noise
        Vector2[] octaveOffsets = new Vector2[octaves];

        float maxPossibleHeight = 0;
        float amplitude = 1;
        float frequency = 1;

        // Generate random sample location offset + the user-controlled "pan offset" for the octaves
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) - offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);

            // For Global normalize mode
            maxPossibleHeight += amplitude;
            amplitude *= persistance;
        }

        // To prevent division by 0, scale should be positive
        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        // These will keep track of the highest and lowest height values on the map for normalization
        float maxLocalNoiseHeight = float.MinValue;
        float minLocalNoiseHeight = float.MaxValue;

        // Used to make noise scale grow about the centre of the map
        float halfHeight = mapHeight / 2f;
        float halfWidth = mapWidth / 2f;
        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                // Initial values for a given point
                amplitude = 1;
                frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    // Increased frequency results in points being sampled further apart and therefore more rapid change in heights
                    // Add octaveOffsets to make sure that each octave is sampling from random locations in the Perlin noise
                    float sampleX = (x - halfWidth + octaveOffsets[i].x) / scale * frequency;
                    float sampleY = (y - halfHeight + octaveOffsets[i].y) / scale * frequency;

                    // Sample Perlin noise
                    // * 2 - 1 to give range of -1 to 1, and allow some octaves to potentially decrease noiseHeight
                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // Will give us a range -1 to 1
                    noiseHeight += perlinValue * amplitude;

                    // Update amplitude and frequency for successive octaves
                    amplitude *= persistance; // Amplitude decreases each octave (0 < persistance < 1)
                    frequency *= lacunarity; // Frequency increases each octave (lacunarity > 1)
                }

                // Max and min height updates
                if (noiseHeight > maxLocalNoiseHeight)
                {
                    maxLocalNoiseHeight = noiseHeight;
                } else if (noiseHeight < minLocalNoiseHeight)
                {
                    minLocalNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize the noise map back to the range 0 to 1
        for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                // Returns value from 0 to 1 
                // i.e. if noiseMap[x,y] == minNoiseHeight, InverseLerp returns 0
                //      if noiseMap[x,y] == maxNoiseHeight, InverseLerp returns 1
                if (normalizeMode == NormalizeMode.Local)
                {
                    noiseMap[x, y] = Mathf.InverseLerp(minLocalNoiseHeight, maxLocalNoiseHeight, noiseMap[x, y]);
                } else
                {
                    // Since out actual values in the noiseMap are rarely ever going to come close to the maxPossibleHeight
                    // we just divide maxPossibleHeight by some arbitrary value (1.75) just so that it's a bit more reasonable
                    float normalizedHeight = (noiseMap[x, y] + 1) / (2f * maxPossibleHeight / 1.75f);
                    noiseMap[x, y] = Mathf.Clamp(normalizedHeight, 0, int.MaxValue);
                }
                
            }
        }
        return noiseMap;
    }
}
