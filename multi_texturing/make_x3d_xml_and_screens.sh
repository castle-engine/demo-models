#!/bin/bash
set -eu

for FILE_NAME in *.x3dv; do
  BASE_NAME=`basename $FILE_NAME .x3dv`
  echo 'Processing '"$FILE_NAME"
  tovrmlx3d --encoding xml "$FILE_NAME" > "${BASE_NAME}.x3d"
  view3dscene "$FILE_NAME" --screenshot 0 "${BASE_NAME}_screen.png"
done
