X Rebirth Player Position Mod
=======================

A mod for X Rebirth that exports the player position at regular intervals.
Created by Tyrant597


### Setup Instructions ###
#### Installation ####
Install this mod like you would any other mod.<br/>
Basically it comes down to copying the `PlayerPosition` directory to the `extensions` directory.

You can find this folder where you installed steam, if it does not exist you'll have to create it.

    ..\Steam\SteamApps\common\X Rebirth\extensions
    
When you start the game, you should see it in the `extensions` menu.
    
    
#### Enabling the debug log ####
At the time of writing the only way to output data is through the debug log file.<br/>
Hopefully in the futher there will be other methods of exporting the information.

To enable the debug log you need to start the game with the following arguments:

    -debug all -logfile debug.log

If you are using Steam, right click the game in your game library and select `properties`.<br/>
Then click the `Set launch options` button and add the arguments there.

Next time you start the game a file will be created in your personal X Rebirth folder.

    {MyDocuments}\Egosoft\X Rebirth\{UniqueId}\debug.log
    
With the mod enabled it will contain entries indicating the position of the player at regular intervals.
