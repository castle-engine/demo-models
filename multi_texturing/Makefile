ALL_OUTPUT=*.x3d *_screen.png

.PHONY: default
default:
	./make_x3d_xml_and_screens.sh

.PHONY: multi_texturing_tests.zip
multi_texturing_tests.zip:
	cd ../ && rm -f multi_texturing_tests.zip
	cd ../ && zip multi_texturing_tests.zip \
	  $(shell cd ../ && find multi_texturing/ '(' -name '.svn' -prune ')' -or '(' -type f -and '(' -not -name '*~' ')' -and -print ')')

.PHONY: clean
clean:
	rm -f $(ALL_OUTPUT)
