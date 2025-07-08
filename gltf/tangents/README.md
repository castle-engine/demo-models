# glTF models (converted to X3D) with tangent vectors

This directory contains 3 models downloaded from [Sketchfab](https://castle-engine.io/sketchfab) and available on various _Creative Commons_ license variants (consult the files `license.txt` or `license.md` in subdirectories for details).

The original download from Sketchfab contained glTF files with explicit (provided in file) _tangent_ vectors.

We converted them to X3D, using the latest [Castle Model Viewer 5.3.0](https://castle-engine.io/castle-model-viewer) that supports `Tangent` node recently added to the X3D 4.1 spec. About `Tangent` node in X3D see:

- https://github.com/michaliskambi/x3d-tests/wiki/Tangent-node-in-X3D

- https://www.web3d.org/specifications/X3Dv4Draft/ISO-IEC19775-1v4.1-CD//Part01/components/rendering.html#Tangent

The X3D versions, and screenshots (from [Castle Model Viewer](https://castle-engine.io/castle-model-viewer)) are also in subdirectories below.

Note that the glTF and X3D versions should look identical in [Castle Model Viewer](https://castle-engine.io/castle-model-viewer) _but only if you disable "View->Blending" for glTF models (this workarounds Sketchfab bug, see below)_.

Note: The conversion glTF -> X3D was done using _modified_ version of [Castle Model Viewer](https://castle-engine.io/castle-model-viewer), but for reasons unrelated to the `Tangent` node:

- We change `alphaMode` conversion to make the materials _opaque_. This workarounds a problem specific to [Sketchfab, see our docs here](https://castle-engine.io/sketchfab). It's unrelated to X3D, glTF. Sketchfab generates these glTF models and just sets wrong alpha mode (they look bad in other glTF viewers as well).

    So glTF models (downloaded from Sketchfab) look wrong out of the box, you need to disable "View->Blending". The X3D models have been already corrected by us, so no need to worry about it.

- We removed animation from the conversion process, because the engine `master` branch has a naive conversion of glTF skinned animation -> X3D that results in huge X3D files. We have a proper solution implemented for this (see [skinned animation on GPU, new Skin node](https://castle-engine.io/skin)) but it's not yet merged to engine `master`, as of 2025-07-08, we're finalizing both implementation and docs to make it really good.

    Since the animation was not the point of this demo (`Tangent` node was), we just removed it from `scene.x3d` files in subdirectories here.

## See also Khronos tests

- https://github.com/KhronosGroup/glTF-Sample-Assets/tree/main/Models/NormalTangentMirrorTest : tests using tangent vectors information from glTF. You can convert it to X3D and thus have an equivalent test of `Tangent` node. Though, admittedly, it's hard to interpret results without [EnvironmentLight](https://castle-engine.io/roadmap#_environment_lighting) support, which is assumes by the screenshots in the testcase.

- https://github.com/KhronosGroup/glTF-Sample-Assets/tree/main/Models/NormalTangentTest : tests auto-generating tangent vectors by the engine. (Loosely related to this. This is what happens if model, glTF or X3D, does not provide tangent vectors.)
