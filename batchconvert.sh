#!/usr/bin/bash
for ext in dae
do
	for i in `find . -name "*.${ext}"`
	do
		echo $i
		~/Downloads/castle-model-viewer-5.3.0-win64-x86_64/castle-model-viewer/castle-model-converter.exe $i `dirname $i`/`basename $i .${ext}`.x3d
	done
done

for ext in x3d x3dv gltf glb
do
	for i in `find . -name "*.${ext}"`
	do
		echo $i
		npx x3d-tidy@latest -i $i -o `dirname $i`/`basename $i .${ext}`.x3dj
	done
done
