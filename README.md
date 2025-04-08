# RandomBackgroundInConemu
## I'm tired of constantly changing the background image in the ConEmu console emulator, so I created a Windows service that automatically changes it when the application starts. Using System.Thread, I search for the ConEmu process. If it's running, I go to the directory with the ConEmu .xml file and change the line responsible for the image file path. 
