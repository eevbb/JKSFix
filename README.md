# JKSFix

Check the [Steam community thread](https://steamcommunity.com/app/1952250/discussions/0/3718314778560404192/) for more information!

## Building the DLL

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
