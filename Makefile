# Autogenerate some files here.

ALL_OUTPUT=animation/gltf_skinned_animation/fox_from_khronos/Fox.x3d \
           animation/gltf_skinned_animation/fox_from_khronos/Fox.x3dv \
					 x3d/matrix_transform.x3d

.PHONY: all
all: $(ALL_OUTPUT)

.PHONY: clean
clean:
	rm -f $(ALL_OUTPUT)

# Use --float-precision 4 to make output the same regardless of the platform,
# and FPC / Delphi version (which may output floats with different precision
# by default).
CASTLE_MODEL_CONVERTER=castle-model-converter --float-precision 4

%.x3d: %.gltf
	$(CASTLE_MODEL_CONVERTER) $< $@
%.x3dv: %.gltf
	$(CASTLE_MODEL_CONVERTER) $< $@
%.x3d: %.x3dv
	$(CASTLE_MODEL_CONVERTER) $< $@
