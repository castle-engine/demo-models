ALL_OUTPUT := sunny_street_processed.x3dv sunny_street_process

.PHONY: all
all: $(ALL_OUTPUT)

sunny_street_processed.x3dv: sunny_street.wrl sunny_street_process
	./sunny_street_process sunny_street.wrl > sunny_street_processed.x3dv

sunny_street_process: sunny_street_process.lpr
	fpcdebug sunny_street_process.lpr

# Clean stuff that should not be in tar.gz
.PHONY: clean
clean:
	rm -f sunny_street_process

# Clean all auto-generated stuff
.PHONY: cleanmore
cleanmore:
	rm -f $(ALL_OUTPUT)
