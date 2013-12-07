CS_FILES = $(shell find src/ -type f -name *.cs)

all: build_burnsystems build_burnsystems_parser bin/BurnSystems.FlexBG.dll bin/BurnSystems.FlexBG.Test.dll

.PHONY: build_burnsystems
build_burnsystems:
	make -C packages/burnsystems

.PHONY: build_burnsystems_parser
build_burnsystems_parser:
	make -C packages/burnsystems.parser

packages/bin/BurnSystems.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems/bin/* packages/bin/

packages/bin/BurnSystems.Parser.dll: packages/burnsystems/bin/BurnSystems.dll
	mkdir -p packages/bin
	cp packages/burnsystems.parser/bin/* packages/bin/

bin/BurnSystems.FlexBG.dll: $(CS_FILES) packages/bin/BurnSystems.dll packages/bin/BurnSystems.Parser.dll
	xbuild src/BurnSystems.FlexBG/BurnSystems.FlexBG.csproj
	mkdir -p bin/
	cp src/BurnSystems.FlexBG/bin/Debug/* bin/


bin/BurnSystems.FlexBG.Test.dll: $(CS_FILES) bin/BurnSystems.Webserver.dll
	xbuild src/BurnSystems.FlexBG.Test/BurnSystems.FlexBG.Test.csproj
	mkdir -p bin/
	cp -r src/BurnSystems.FlexBG.Test/bin/Debug/* bin/

.PHONY: clean
clean:
	make clean -C packages/burnsystems
	make clean -C packages/burnsystems.parser
	rm -fR src/BurnSystems.FlexBG/bin
	rm -fR src/BurnSystems.FlexBG/obj
	rm -fR src/BurnSystems.FlexBG.Test/bin
	rm -fR src/BurnSystems.FlexBG.Test/obj
	rm -fR bin
