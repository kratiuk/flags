PROJECT := Flags.csproj
CONFIG := Release
RUNTIME := win-x64
PUBLISH_DIR := bin/$(CONFIG)/net10.0-windows/$(RUNTIME)/publish
INSTALLER_SCRIPT := Flags.iss
ISCC := "C:/Program Files (x86)/Inno Setup 6/ISCC.exe"
VERSION := $(shell powershell -NoProfile -Command "[xml]$$proj = Get-Content '$(PROJECT)'; $$proj.Project.PropertyGroup.Version")

.PHONY: build run publish installer release clean

build:
	dotnet build $(PROJECT)

run:
	dotnet run --project $(PROJECT)

publish:
	dotnet publish $(PROJECT) -c $(CONFIG) -r $(RUNTIME) --self-contained true

installer:
	$(ISCC) /DAppVersion=$(VERSION) $(INSTALLER_SCRIPT)

release: publish installer

clean:
	dotnet clean $(PROJECT)
