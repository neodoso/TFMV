# TFMV Neo 0.4
Team Fortress 2 Model Viewer

DOWNLOAD HERE: http://neodem.net/TFMV/TFMV_Neo_0.4.zip

-------------------------------------------------------------------------------------------------------------------------------------------------------

PATCH NOTES:

-------------------------------------------------------------------------------------------------------------------------------------------------------

0.4 (10/18/2022)
   
-------------------------------------------------------------------------------------------------------------------------------------------------------

-Fixed a bug preventing HLMV from starting up relating to the HLMV recent files list change from 0.1 (thanks Wurlmon!)
-Updated exe assembly information (TFMV Neo 0.3 was mistakenly referred to as TFMV 1.9 - Community Edition).

-------------------------------------------------------------------------------------------------------------------------------------------------------

0.3 (10/18/2022)
   
-------------------------------------------------------------------------------------------------------------------------------------------------------

NEW FEATURES:

-Added a checkbox to disable jiggle bones on loaded items.
-Added Taunt Prop item filter type. This means you can load any ingame taunt prop alongside regular items.
-Added option to expand item list on startup.
-Added option to replace default Valve cubemap with one from 2fort. This feature will be expanded upon in the next release.

IMPROVEMENTS:

-Updated "Save current as default" button to also save Light and Background color settings.
-Added options to disable TFMV overrides for Light and Window settings.
-Added reset buttons to Background and Window settings.
-Improved fixed head texture mipmap functionality. Now TFMV fixes them automatically instead of storing uncompressed versions of the head textures.
-Made some non-tournament medals visible in the item selection panel. Notably, this prevents the Made Man not appearing in Spy's list of cosmetic items.
(the Gentle Manne's Service Medal, Employee Badges and Dueling Badges are still disabled for now)

OTHER CHANGES:

-Fixed a bug preventing the TFMV folder in the Custom folder from being deleted when closing TFMV.
-Removed PDA2 item filter type. It didn't work properly and both PDA2 items in the game are duplicates of existing items. The Taunt Prop filter replaces it.
-Changed default HLMV window size to 800x600.
-Added warning/instructions when a user attempts to load a Voodoo-Cursed Zombie Soul.
-Added "Latest Patch Notes" to About tab.

-------------------------------------------------------------------------------------------------------------------------------------------------------

0.2 (05/08/2022)
   
-------------------------------------------------------------------------------------------------------------------------------------------------------

-TFMV now asks for an API Key the first time you launch it, so you're no longer relying on a key that could be invalidated at some point in the future.


-------------------------------------------------------------------------------------------------------------------------------------------------------

0.1 (05/06/2022)
   
-------------------------------------------------------------------------------------------------------------------------------------------------------

-TFMV now auto-installs fixed head textures into the custom folder when putting a loadout into HLMV (no more pixelated eyebrows!)
-TFMV no longer deletes the HLMV recent files list (good for those of us that still launch HLMV the old fashioned way sometimes).

-------------------------------------------------------------------------------------------------------------------------------------------------------


User Guide / More information: https://steamcommunity.com/sharedfiles/filedetails/?id=158547475

TFMV is a program that adds functionality and automation to the Source Engine HLMV.exe model viewer (of Team Fortress 2).

TFMV (Team Fortress Model Viewer) is a tool for Windows that makes it easier to load and preview TF2 player item loadouts in the model viewer "HLMV" 
and also helps for testing and developing workshop items by automating file loading and adding features on top of HLMV for tasks which are otherwise 
impossible or require tedious manual file managing and editing.

The tool also lists the TF2 items so you can easily pick and load items by their icon instead of having to search and pick the models by file name.
TFMV downloads the items list and icons from the official servers, so it's always up to date and the latest items can be loaded.

TFMV manages the models and materials(aka skins) of each item through a visual interface, rather than having search, load or edit files and dependencies manually.
It also makes it possible to switch skins(red/blue) on model attachments, change paint colors, easily edit skins with the material editor and test material 
changes in real time, take screenshots with specific resolutions, take screenshots with transparency, automatically capture screenshots for each of item's paint color, 
load workshop .zip compiled items, etc
