using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu()]
public class TerrainData : UpdatableData
{
    public float uniformScale = 2f;

    public bool useFlatShading;
    public bool useFalloff;

    public float meshHeightMultiplier;
    // Used to specify the degree to which different height values should be affected by the meshHeightMultiplier
    public AnimationCurve meshHeightCurve;
}
