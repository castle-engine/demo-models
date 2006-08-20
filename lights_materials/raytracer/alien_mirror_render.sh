#!/bin/bash
set -eu

rayhunter classic 10 1600 1200 alien_mirror.wrl alien_two_mirrors_1.png \
   --scene-bg-color 0.057 0.221 0.4
convert alien_two_mirrors_1.png \
  -filter Gaussian -geometry 800x600 alien_two_mirrors_2.png