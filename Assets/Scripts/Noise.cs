using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise
{
    public static float[,] GenerateNoiseMap(int mapWidth, int mapHeight, int seed, float scale, int octaves, float persistance, float lacunarity, Vector2 offset)
    {
        float[,] noiseMap = new float[mapWidth, mapHeight];

        System.Random prng = new System.Random(seed);
        Vector2[] octaveOffsets = new Vector2[octaves];

        // Generate random sample location offset + the user-controlled "pan offset"
        for (int i = 0; i < octaves; i++)
        {
            float offsetX = prng.Next(-100000, 100000) + offset.x;
            float offsetY = prng.Next(-100000, 100000) + offset.y;
            octaveOffsets[i] = new Vector2(offsetX, offsetY);
        }

        if (scale <= 0)
        {
            scale = 0.0001f;
        }

        // These will keep track of the highest and lowest height values on the map
        float maxNoiseHeight = float.MinValue;
        float minNoiseHeight = float.MaxValue;

        // Used to make noise scale grow about the centre of the map
        float halfHeight = mapHeight / 2f;
        float halfWidth = mapWidth / 2f;
        
        for (int y = 0; y < mapHeight; y++)
        {
            for (int x = 0; x < mapWidth; x++)
            {
                float amplitude = 1;
                float frequency = 1;
                float noiseHeight = 0;

                for (int i = 0; i < octaves; i++)
                {
                    float sampleX = (x - halfWidth) / scale * frequency + octaveOffsets[i].x;
                    float sampleY = (y - halfHeight) / scale * frequency + octaveOffsets[i].y;

                    float perlinValue = Mathf.PerlinNoise(sampleX, sampleY) * 2 - 1; // Will give us a range -1 to 1
                    noiseHeight += perlinValue * amplitude;

                    // Update amplitude and frequency for successive octaves
                    amplitude *= persistance; // Amplitude decreases each octave (0 < persistance < 1)
                    frequency *= lacunarity; // Frequency increases each octave (lacunarity > 1)
                }

                // Max and min height updates
                if (noiseHeight > maxNoiseHeight)
                {
                    maxNoiseHeight = noiseHeight;
                } else if (noiseHeight < minNoiseHeight)
                {
                    minNoiseHeight = noiseHeight;
                }
                noiseMap[x, y] = noiseHeight;
            }
        }

        // Normalize the noise map to the range 0 to 1
        for (int y = 0; y < mapHeight; y++)
        {
            for(int x = 0; x < mapWidth; x++)
            {
                // Returns value from 0 to 1 
                // i.e. if noiseMap[x,y] == minNoiseHeight, InverseLerp returns 0
                //      if noiseMap[x,y] == maxNoiseHeight, InverseLerp returns 1
                noiseMap[x, y] = Mathf.InverseLerp(minNoiseHeight, maxNoiseHeight, noiseMap[x, y]);
            }
        }
        return noiseMap;
    }
}
