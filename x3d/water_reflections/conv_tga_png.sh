for F in *.tga; do convert $F `basename $F .tga`.png; done
