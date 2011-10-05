#!/bin/bash
set -eu

# Call this script from this directory,
# or from base castle_game_engine directory.
# Or just do "make examples" in base castle_game_engine directory.

# Allow calling this script from it's dir.
if [ -f sunny_street_process.lpr ]; then cd ../../../castle_game_engine/; fi

fpc -dRELEASE @castle-fpc.cfg ../demo_models/shadow_maps/sunny_street/sunny_street_process.lpr
