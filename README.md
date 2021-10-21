# NASB Mirror Match Mod

This mod will recolor duplicate characters by simply tinting the model.

![image](https://user-images.githubusercontent.com/27714637/136115464-9b238b9e-0df1-437c-8271-4c572d629cba.png)


## 🚀 Installation

### Slime Mod Manager

Download the latest version of this mod from the [Slime Mod Manager](https://github.com/legoandmars/SlimeModManager/releases/latest)!

### Manual Installation

*If your game isn't modded with BepinEx, DO THAT FIRST!*
Simply go to the [latest BepinEx release](https://github.com/BepInEx/BepInEx/releases) and extract `BepinEx_x64_VERSION.zip` directly into your game's folder, then run the game once to install BepinEx properly.

Next, go to the [latest release of this mod](https://github.com/megalon/nick-mirror-match-mod/releases/latest) and extract the zip in your game's install directory.
This will place the dll in `BepInEx\plugins\`.

That's it!

## 📝 Configuration

**Run the game once to generate a config file!**

The file will be placed in
`BepInEx\config\megalon.nick_mirror_match_mod.cfg`

Here you can edit the color hex values.
The format is `RRGGBBAA`

* `TintColor1` is for the first duplicate player
* `TintColor2` is the second
* `TintColor3` is the third

```
[Colors]

# Setting type: Color
# Default value: 8245FFA8
TintColor1 = 8245FFA8

# Setting type: Color
# Default value: FFED3BA8
TintColor2 = FFED3BA8

# Setting type: Color
# Default value: 29F2D1A8
TintColor3 = 29F2D1A8
```

## 🔧 Developing

Clone the project, then create a file in the root of the project directory named:

`NickCustomMusicMod.csproj.user`

Here you need to set the `GameDir` property to match your install directory.

Example:
```xml
<?xml version="1.0" encoding="utf-8"?>
<Project>
  <PropertyGroup>
    <GameDir>D:\SteamLibrary\steamapps\common\Nickelodeon All-Star Brawl</GameDir>
  </PropertyGroup>
</Project>
```

Now when you build the mod, it should resolve your references automatically, and the build event will copy the plugin into your `BepInEx\plugins` folder!
