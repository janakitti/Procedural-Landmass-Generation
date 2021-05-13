# Procedural Landmass Generation

This is an infinite 3D terrain generator created using Unity. I learned how to create this project from Sebastian Lague's wonderful YouTube series on [Procedural Landmass Generation](https://www.youtube.com/playlist?list=PLFt_AvWsXl0eBW2EiBtl_sxmDtSgZBxB3). The generator uses Perlin noise to generate a height map that gets applied to vertices on a mesh.

### Features

* Adjustable parameters for:
  * `octaves`: each level of Perlin noise, layered on top of each other for finer and finer details
  * `lacunarity`: controls the frequency of each octave
  * `persistence`: controls the amplitude of each octave
* Adjustable settings for terrain colours
* Varying levels of detail for different chunks based on their distance from the player
* Falloff map mode for creating island-style terrain
* Flatshading for a more low-poly style

![demo1](https://github.com/janakitti/Procedural-Landmass-Generation/blob/main/demo_assets/demo1.gif)

### Next Steps

* Create custom shaders 
* Improve the FPS controller
* Create in-game controls for adjusting the terrain parameters and colours