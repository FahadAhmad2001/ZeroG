# ZeroG

### A LAN multiplayer mod for Antigraviator

### Current status:
#### What works:
- Player Movement
- Syncing start of races
- Easy server setup
- Aand, thats about it!
#### What doesn't work / known issues 
- Player positions do not work
- Laps do not work/no end of race
- Sometimes player and opponent ships get stuck but the player/opponent UI doesnt and 
- Player names do not sync
- Currently have to install mod manually by injecting startup method into Assembly-CSharp.dll
### Installation
#### Simple Installation
Run the ZeroG installer found in the zip in the releases section. The server files are saved to a directory in the desktop.
#### Manual Installation
This can be done by injecting the Load method in the Init class in the ZeroG assembly into Assembly-CSharp.dll
in the SplashScreen class's Awake method (it can also be injected into other classes that contain a method that starts at runtime).
### Help and support
If you encounter any problems or need help, please create an issue in the issues tab
