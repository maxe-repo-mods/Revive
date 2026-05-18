# Revive

Revive all dead players with a single keypress (host only). Configurable HP restore, cost, and invincibility.

R.E.P.O. BepInEx mod. Host-side only.

## Features

- Press a configurable key (default F1) to revive all dead players at once
- Set the HP amount restored to each revived player
- Optional HP cost deducted from the host player when reviving
- Configurable invincibility duration after revive
- Optional distance-based filtering (only revive players near the host)

## Installation

Requires [BepInEx 5.x](https://thunderstore.io/c/repo/p/BepInEx/BepInExPack/).

Place `Revive.dll` in `BepInEx/plugins/`.

Configuration file is generated at `BepInEx/config/maxenterme.Revive.cfg` on first launch.

## Configuration

Edit `BepInEx/config/maxenterme.Revive.cfg` to customize behavior.

| Section | Key | Default | Range | Description |
|---------|-----|---------|-------|-------------|
| Settings | ReviveKey | F1 | - | Key to press for reviving all dead players (host only) |
| Settings | ReviveHealth | 50 | 1-100 | HP amount restored to each revived player |
| Settings | UseHPCost | false | - | Enable HP cost deducted from host when reviving |
| Settings | HPCost | 25 | 1-100 | HP cost per revived player (if UseHPCost is enabled) |
| Settings | InvincibilityDuration | 3 | 0-30 | Seconds of invincibility granted after revive |
| Settings | UseMaxDistance | false | - | Only revive dead players within MaxDistance of the host |
| Settings | MaxDistance | 5.0 | 1-50 | Maximum distance (in meters) to revive from host (if UseMaxDistance is enabled) |

## Build

```bash
dotnet build -c Release
```

The compiled DLL will be available at:
```
bin/Release/netstandard2.1/Revive.dll
```

## License

MIT
