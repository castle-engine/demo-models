#!/bin/bash
set -eu

# ----------------------------------------------------------------------------
# Regenerate, using view3dscene, proper screenshots for all models.
# Run with
# - no arguments (regenerate everything here)
# - directory name as argument (regenerate everything there, recursively)
# - file name as argument (regenerate this one file)
# ----------------------------------------------------------------------------

# Allows to properly handle filenames with spaces,
# "for" iteration over a filename with 1-filename per line will be reliable.
IFS=$'\n'

# Functions ------------------------------------------------------------------

# Regenerate screenshot for file $1
do_file ()
{
  MODEL_FILE="$1"

  # Use bash expansion to strip extensions.
  #
  # Note: we could use %% to strip even double extensions like .wrl.gz,
  # but then filename like ../x3d-tests/aa.x3d fails.
  #
  # See https://stackoverflow.com/questions/965053/extract-filename-and-extension-in-bash
  # and https://www.gnu.org/software/bash/manual/html_node/Shell-Parameter-Expansion.html#Shell-Parameter-Expansion
  SCREENSHOT_FILE="${1%.*}.png"

  echo "Making screenshot ${MODEL_FILE} -> ${SCREENSHOT_FILE}"
  view3dscene "${MODEL_FILE}" --geometry 1600x900 --screenshot 0 "${SCREENSHOT_FILE}"
}

# Regenerate screenshot for everything in directory $1
do_dir ()
{
  find "$1" \
    '(' -type d -iname 'errors' -prune ')' -or \
    '(' -type f -name '*test_temporary*' ')' -or \
    '(' -type f '(' -iname '*.wrl' -or \
                    -iname '*.wrz' -or \
                    -iname '*.wrl.gz' -or \
                    -iname '*.x3d' -or \
                    -iname '*.x3dz' -or \
                    -iname '*.x3d.gz' -or \
                    -iname '*.x3dv' -or \
                    -iname '*.x3dvz' -or \
                    -iname '*.x3dv.gz' -or \
                    -iname '*.3ds' -or \
                    -iname '*.dae' -or \
                    -iname '*.gltf' -or \
                    -iname '*.glb' -or \
                    -iname '*.plist' -or \
                    -iname '*.starling-xml' ')' \
                -print ')' > \
          /tmp/cge-demo-models-regenerate-screenshots.sh.txt
  echo 'Found files: '`wc -l < /tmp/cge-demo-models-regenerate-screenshots.sh.txt`
  for FILE in `cat /tmp/cge-demo-models-regenerate-screenshots.sh.txt`; do
    do_file "$FILE"
  done
}

# main -----------------------------------------------------------------------

if [ $# = 0 ]; then
  do_dir .
else
  if [ -f "$1" ]; then
    do_file "$1"
  else
    do_dir "$1"
  fi
fi
