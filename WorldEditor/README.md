WorldEditor - simple console tool

This console app creates a small `world` directory and can create, list, and edit location JSON files compatible with the `csServer2` world format.

Build & run (PowerShell):

```powershell
# WorldEditor

Small console tool to create, list and edit world JSON files compatible with the `csServer2` world format.

## Quick start (PowerShell)

```powershell
cd .\WorldEditor
dotnet build
dotnet run --project .\WorldEditor.csproj
```

When the program runs you will see a main menu. Type the name or number of the mode you want to work in.

## Top-level modes
- `1` or `world`  — World editor (create / list / edit location files)
- `2` or `itemdb`  — ItemDB editor (manage global items used by shops and enemy loot)
- `3` or `quests`  — Quest editor (create, show, edit, remove quests inside locations)
- `help` or `?`    — show help
- `exit`           — quit the app

## Command tree

A quick reference of commands and subcommands shown as a tree:

```
main
├─ world | 1
│  ├─ list
│  ├─ new
│  ├─ edit [file|#]
│  │  ├─ enemies> add
│  │  ├─ enemies> remove <index>
│  │  ├─ enemies> loot <index>  (then: add | remove)
│  │  └─ shop> add | remove <index>
│  └─ itemdb   (switch to ItemDB editor)
├─ itemdb | 2
│  ├─ list
│  ├─ add
│  ├─ populate
│  ├─ remove <index>
│  └─ edit <index>
└─ quests | 3
	├─ list
	├─ show [locIndex]
	├─ create
	│  └─ (interactive step builder: text | move | items | enemies)
	├─ edit
	│  └─ (edit metadata + optionally rebuild steps)
	└─ remove
```

## Common workflows

World editor
- `list` — shows available location JSON files (top-level and per-location folders)
- `new`  — interactively create a new location (saved as `world/<Name>/<Name>.json`)
- `edit` — edit a location; while editing you can add/remove enemies and shop items

ItemDB editor
- `list`     — list items in the global ItemDB
- `add`      — add an item (name, icon, description, value, flags)
- `populate` — add a default set of items (will not duplicate existing names)
- `remove` / `edit <index>` — manage items by index

Quest editor
- `list`   — list locations that contain world files
- `show`   — show quests inside a chosen location
- `create` — create a new quest inside a location (quest is appended to the location JSON)
- `edit`   — edit an existing quest (metadata + optionally rebuild steps)
- `remove` — delete a quest from a location

## File layout

- `WorldEditor/Program.cs` — interactive console application
- `WorldEditor/Models.cs`  — POCO models used for JSON serialization
- `WorldEditor/world/`     — output directory where JSON files are stored
	- `WorldEditor/world/ItemDB/items.json` — global item database used by shops/loot

Each location is saved in its own folder: `world/<LocationName>/<LocationName>.json`.
Quests are stored inside the location JSON's `quests` array (no separate per-quest files).

## Notes and behavior

- Quest steps are stored as raw JSON elements to preserve the flexible step shapes used by `csServer2`.
- The editor prevents creating two quests with the same name (case-insensitive) inside the same location.
- Items used by shops and enemy loot must be chosen from the `ItemDB` — the chosen items are cloned into the location file.
- Location names are validated for invalid filesystem characters when creating or renaming.

## Troubleshooting & tips

- If you accidentally edit a file, you can use source control (git) to revert or manually open the JSON and fix it.
- To seed the ItemDB quickly, run the ItemDB editor and use the `populate` command.

## Next steps (suggested)

- Add single-step editing in the quest editor instead of full-step rebuilds.
- Add validation commands to check that `moveTo` steps reference existing location names.
- Build a small GUI wrapper to ease editing for non-technical users.

If you want any of the above implemented, tell me which and I will add it.
