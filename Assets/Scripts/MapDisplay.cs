using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/**
 * Applies noise map to a texture to be applied to a plane
 */
public class MapDisplay : MonoBehaviour
{
    public Renderer textureRenderer;
    public MeshFilter meshFilter;
    public MeshRenderer meshRenderer;

    public void DrawTexture(Texture2D texture)
    {
        textureRenderer.sharedMaterial.mainTexture = texture;
        // Set the size of the plane to the same size as the map
        textureRenderer.transform.localScale = new Vector3(texture.width, 1, texture.height);
    }

    public void DrawMesh(MeshData meshData, Texture2D texture)
    {
        meshFilter.sharedMesh = meshData.CreateMesh();
        // Use sharedMaterial so that the texture updates right in the editor, without the need to run
        meshRenderer.sharedMaterial.mainTexture = texture;
    }
}
