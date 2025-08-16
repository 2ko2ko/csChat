WorldEditor - simple console tool

This console app creates a small `world` directory and can create, list, and edit location JSON files compatible with the `csServer2` world format.

Build & run (PowerShell):

```powershell
cd .\WorldEditor
dotnet build
dotnet run --project .\WorldEditor.csproj
```

- list       : list JSON files in the `world` folder and inside per-location subfolders
- new        : create a new location interactively (creates `world/<Name>/<Name>.json`)
- edit file  : edit basic fields of an existing location (e.g. `edit Cryo-Station.json` or `edit Cryo-Station/Cryo-Station.json` or `edit Cryo-Station`)
- exit       : quit
Commands inside the tool:
Commands inside the tool:
- list       : list JSON files in the `world` folder and inside per-location subfolders
- new        : create a new location interactively (creates `world/<Name>/<Name>.json`)
- edit file  : edit basic fields of an existing location (e.g. `edit Cryo-Station.json` or `edit Cryo-Station/Cryo-Station.json` or `edit Cryo-Station`)
	- while editing you can add/remove enemies and shop items via simple prompts
- exit       : quit

Files created:
- `WorldEditor/Models.cs`  : POCOs matching the world JSON structure
-- `WorldEditor/Program.cs` : interactive console program (supports validate/edit)
- `WorldEditor/world/`      : folder where JSON files are written (each location gets its own subfolder)

Notes:
Notes:
- The tool keeps step objects in quests as raw JSON elements to preserve arbitrary step shapes used by `csServer2`.
- Name validation: new or edited location names are checked for invalid filesystem characters.
-- Import: feature removed. Use manual copy if needed.

Next steps you can ask for:
- full quest-step editor (interactive construction of steps)
- GUI-based editor
- automatic sync back to `csServer2` or CI integration
