#!/bin/bash
set -eu

# Call this script from this directory,
# or from base castle_game_engine directory.
# Or just do "make examples" in base castle_game_engine directory.

# Allow calling this script from it's dir.
if [ -f castle_with_trees_process.lpr ]; then cd ../../../castle_game_engine/; fi

fpc -dRELEASE @castle-fpc.cfg ../demo_models/shadow_maps/castle_with_trees/castle_with_trees_process.lpr
