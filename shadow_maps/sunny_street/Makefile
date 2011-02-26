ALL_OUTPUT := sunny_street_processed.x3dv sunny_street_process

.PHONY: all
all: $(ALL_OUTPUT)

sunny_street_processed.x3dv: sunny_street.wrl sunny_street_process
	./sunny_street_process sunny_street.wrl > sunny_street_processed.x3dv

sunny_street_process: sunny_street_process.lpr
	fpcdebug sunny_street_process.lpr

.PHONY: clean
clean:
	rm -f $(ALL_OUTPUT)
