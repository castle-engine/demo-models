#!/usr/bin/bash
for ext in x3d wrl x3dv gltf glb
do
	for i in `find . -name "*.${ext}"`
	do
		echo $i
		npx x3d-tidy@latest -i $i -o `dirname $i`/`basename $i .${ext}`.x3dj
	done
done
