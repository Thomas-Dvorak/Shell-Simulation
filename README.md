# Shell-Simulation
---
## Project Details
The purpose/motivation of this project is to gain a basic understanding of how modern computers across any operating system and users use a terminal. 

It is also to gain experience with using both GitHub and C# with .NET.

The project provides a root directory `src` where users can enter 8 different commands:
-`echo <message> -r <repeat-times>` - Prints `<message>` once if `-r` is not specified and if so, prints `<message>` `<repeat-times>` times. If `-r` is specified and `<repeat-times>` is not, then it prints `<message>` anywhere from 1 to 1000 times. 

-`mkfile <filename.extension>` - Creates a file with `<filename` name and extention `.extension>` in the current directory. If there is no extension or filename specified, it does not work. 

-`rmfile <filename.extension>` - Removes/Deletes a file with `<filename` name and extension `.extension>`, given it exists. 

-`mkfolder <foldername>` - Similar to `mkfile` but with a folder instead.

-`rmfolder <foldername>` - Similar to `rmfile` but with a folder instead. 

-`ls [-i or -o]` - Lists all the files and folders in the current directory. If `-i` is specified, lists only all the files. If `-o` is specified, lists only all the folders. Both cannot be used at the same time. 

-`copyto <file> <directory>` - Copies a file with name `<file>` and copies it to directory `<directory>`. 

-`help` - Prints all commands with their functionality and parameters. 

This project is still in development.
