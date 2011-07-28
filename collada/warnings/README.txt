Note for Collada 1.3.1 versions of humanoid:

  In short, they are simply faulty files, and as far as I can see it's clearly
  a bug in Blender Collada 1.3.1 exporter.

  The 1.3.1 exporter is generally buggy, e.g. <accessor> count is wrong,
  but this can be detected and fixed by Kambi VRML game engine.
  For humanoid, there are transformation problems
  that simply prevent any program to correctly reread these Collada files:

  - baked-matrix version has awfully broken matrices.

    Consider e.g. matrix of "Sphere" object of the humanoid, which should be simple:
    just translation to Z = 1.9. But matrix generated in Collada 1.3.1 file is

      -0.000000 0.000000  1.000000  1.900000
       1.000000 0.000000  0.000000  0.000000
       0.000000 1.000000 -0.000000 -0.000000
      -0.156350 -1.627762 0.000000  1.000000

    which is clearly broken... although 4th column defines some translation with 1.9,
    4th row contains very strange values, and left-upper 3x3 part has "1" in the
    wrong places (they should be only on diagonal, since there's no scale/rotation,
    just like in identity matrix).

    Collada 1.4.1 exporter does correct job, here's what this matrix should look
    like:

        1.0  0.0 0.0 -0.0
        0.0  1.0 0.0 -0.0
       -0.0 -0.0 1.0  1.9
        0.0  0.0 0.0  1.0

  - no-matrix has also some strange problems. Model looks better,
    but still it looks like rotations are not generated correctly.

  IOW, it's not the fault of our engine (or anything else that you may use
  to read these Collada files) that these models are strangely rendered ---
  it's the fault of Blender 1.3.1 exporter, which generated wrong Collada files.

Note for Collada 1.3.1 versions of cart:

  The cart model (without armature) doesn't have above matrix problems,
  both baked and non-baked transformations are generated correctly there.
  Although it still has invalid <accessor> counts, also a problem of Blender
  exporter.

Newer Collada 1.4 exporter doesn't have any problems, see ../ for correct models.
