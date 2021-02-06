#!/bin/sh
set -eu

FULL_WIDTH=200  #1600
FULL_HEIGHT=200 #1200

rayhunter classic 3 $FULL_WIDTH $FULL_HEIGHT forest-final.wrl \
  forest-nogamma-aliased.rgbe
pfilt -1 -x /2 -y /2 forest-nogamma-aliased.rgbe > forest-nogamma.rgbe

# gamma correction not needed.
# imgConvert --gamma 2.0 forest-nogamma.rgbe forest.rgbe
cp forest-nogamma.rgbe forest.rgbe
