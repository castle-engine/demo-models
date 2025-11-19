# Autogenerate some files here.

ALL_OUTPUT=animation/gltf_skinned_animation/fox_from_khronos/Fox.x3d \
           animation/gltf_skinned_animation/fox_from_khronos/Fox.x3dv

.PHONY: all
all: $(ALL_OUTPUT)

.PHONY: clean
clean:
	rm -f $(ALL_OUTPUT)

%.x3d: %.gltf
	castle-model-converter $< $@
%.x3dv: %.gltf
	castle-model-converter $< $@
