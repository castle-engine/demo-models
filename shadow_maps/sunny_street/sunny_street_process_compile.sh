#!/bin/bash
set -eu

# Allow calling this script from it's dir.
if [ -f sunny_street_process.lpr ]; then
  cd ../../../castle_game_engine/
fi

# Call this from ../../ (or just use `make examples').
fpc -dRELEASE @kambi.cfg ../demo_models/shadow_maps/sunny_street/sunny_street_process.lpr

