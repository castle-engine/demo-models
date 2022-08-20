# X3D models using deprecated (in Castle Game Engine) CommonSurfaceShader

The `CommonSurfaceShader` node is documented on
https://castle-engine.io/x3d_implementation_texturing_extensions.php#section_ext_common_surface_shader .
It has been quite important X3D node, for Castle Game Engine, in the past --
it replaced the X3D 3 material node with a different approach with much more capabilities
(normal maps, affect everything by textures).

It was originally introduced in InstantReality,
see e.g. http://doc.instantreality.org/tutorial/commonsurfaceshader/ .
It is also implemented in X3DOM.
Castle Game Engine implementation is largely compatible to them both.

In the past, we also maintained a Blender exporter to X3D that could generate
`CommonSurfaceShader`.

However, in X3D 4, we have extended the `Material` node with lots of new fields
that make `CommonSurfaceShader` obsolete.
Moreover, we added `UnlitMaterial` and `PhysicalMaterial` and encourage all
new models to use `PhysicalMaterial` instead of `Material` for a realistic
physical lighting model.

Thus, `CommonSurfaceShader` becomes obsolete. All its use-cases are now covered
by X3D 4 features.
