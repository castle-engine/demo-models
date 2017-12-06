ALL_OUTPUT := castle_with_trees_processed.x3dv castle_with_trees_process

.PHONY: all
all: $(ALL_OUTPUT)

castle_with_trees_processed.x3dv: castle_with_trees.wrl castle_with_trees_process
	./castle_with_trees_process castle_with_trees.wrl > castle_with_trees_processed.x3dv

castle_with_trees_process: castle_with_trees_process.lpr
	castle-engine simple-compile castle_with_trees_process.lpr

# Clean stuff that should not be in tar.gz
.PHONY: clean
clean:
	rm -Rf castle_with_trees_process castle_with_trees_process.res lib/

# Clean all auto-generated stuff
.PHONY: cleanmore
cleanmore:
	rm -f $(ALL_OUTPUT)
