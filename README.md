# JKSFix

A Samurai Maiden mod.

## Screenshots!

* [Get as close as you like, characters won't disappear~](./Screenshots/image0.png?raw=1)
* [Pantsu fully uncensored!](./Screenshots/image1.png?raw=1)
* [New menu with lots of options!](./Screenshots/image2.png?raw=1)
* [Camera angles unlocked in equipment mode!](./Screenshots/image3.png?raw=1)
* [Anti VFX option](./Screenshots/image4.png?raw=1)

Big thanks and hugs to XenoMars for buying the game for me so I can keep modding it! 😊💖

## Download and instructions

[Download 💖](../../releases/latest)

Extract straight into the game folder, then run the game. First time running the game after installing the mod can take a few minutes. Afterwards it should be fast as usual.

### Steam Deck users

After copying the mod to your game folder you will need to add the following launch option to the game's properties on Steam:
```
WINEDLLOVERRIDES="winhttp.dll=n,b" %command%
```
(Thanks to Kou-kun for figuring that out~)

### For ultrawide/custom resolutions

Follow [this guide](https://steamcommunity.com/sharedfiles/filedetails/?id=2699973520) to set your preferred resolution. If the black bars fix doesn't work, user DT reported disabling the Steam overlay for the game helped.

### To change hotkeys

First run the latest version of the mod at least once so it generates a config file. Then, simply open the file BepInEx\config\JKSFix.cfg in notepad, and change the keybinds there.

### Advanced graphics settings

Like hotkeys, these can only be changed manually via config file, in the Rendering section.

Any value set to the default of -1 is ignored by the mod (so the game's default is applied instead).

Values are not validated or sanitized so make sure you know the values you're setting are supported! (Watch out, some of them correspond to Unity enums, too!)

There's also a hotkey to reload all Rendering settings during runtime.

## Quick troubleshooting and disclaimer

If the mod doesn't work, make sure to install VC++ redistributable if you haven't already! [Find it here.](https://aka.ms/vs/17/release/vc_redist.x64.exe)

I don't know if this is compatible with other mods, and I haven't played the game a lot so there could be bugs or issues, use at your own risk!

## Building the DLL (for developers!)

1. Install Samurai Maiden on your computer

2. Add BepInEx for Unity IL2CPP 6.0.0 to your Samurai Maiden installation, then run it at least once afterwards

   * If you already ran a previous version of JKSFix on your Samurai Maiden installation, you can skip this step!

   * [Here is a guide](https://docs.bepinex.dev/master/articles/user_guide/installation/unity_il2cpp.html) on the installation.
  
   * Development was done with `BepInEx-Unity.IL2CPP-win-x64-6.0.0-be.664+0b23557`, but newer versions will probably work too.

3. Clone this repo, and create a file named `User.proj` in the `BepInEx` folder, with the following content:

    ```xml
    <Project>
      <PropertyGroup>
        <GameDir>C:\Program Files\Steam\steamapps\common\SAMURAI MAIDEN\</GameDir>
      </PropertyGroup>
    </Project>
    ```

    But replace the directory with the one where *your* copy of Samurai Maiden is installed.

4. Open the project in Visual Studio and build away!

## Changelog

* New in 3.6
  * Replaced death animation toggle with a brand new animations menu! You can now preview any animation you want. It only works on stages. Make sure to click unset when you're done or you won't be able to move!

* New in 3.5
  * Added multiple graphics settings. [See above!](#advanced-graphics-settings)

* New in 3.4
  * Added option to disable black bars, for ultrawide resolutions.
  * Added option to disable chromatic aberration.

* New in 3.3
  * Fixed cursor getting stuck in latest version of the game~
  * Hotkeys can now be configured! And Frame Advance has a hotkey again! ([see above for more info](#to-change-hotkeys))
  * Added a toggle to see the death animation whenever 🤭🍑 OTL

* New in 3.2
  * Added Field of View slider.
  * Added toggle to stun yourself 🙃

* New in 3.1
  * Fixed eye highlights disappearing.
  * Added cheat to continuously keep HP maxed.

* New in 3.0
  * Big rewrite! More performance! 🏇
  * Skirt physics option, so the whole skirt has physics instead of just the hem! Be warned it causes some clipping, and a *ton* of pantyshots 🤣
  * Anti VFX option, disables footstep "dust" visual effect, as well as the bright "soft-censor" light on Komimi's special move ([see pic above!](#screenshots)).

* New in 2.1
  * The mod now remembers if the UI was open or closed.

* New in 2.0
  * New menu with lots of options! Press Delete to open.
  * Unlocked angles in equipment mode. Enjoy looking from low angles~ (Can zoom in further too!)

* New in 1.1.1
  * Fixed overlapping character animations when starting a stage.

* New in 1.1
  * Fixed skirts flattening and other bones resetting when zooming in on characters.
  * Added frame advance. Press the Delete key when the game is paused to advance a single frame 🧐 It may have weird results!
  * BepInEx console hidden by default

