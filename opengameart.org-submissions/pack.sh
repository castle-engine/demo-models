#!/bin/bash
set -eu

pack_dir ()
{
  find "$1" '(' -iname '*~' -or -iname '*.blend?' ')' -execdir rm -f '{}' ';'
  zip -r "$1".zip "$1"
}

pack_dir car
pack_dir dragon
pack_dir trees
