public class SimpleConsole {
    private readonly string Version = "1.8.2";
    private readonly string ConsoleName = "Devo Command Line";
    private string UserInput = "default_text";
    private string[] UserCommand;
    private string[] RawUserCommand;
    private string CurrentDirectory = "src";
    private readonly string RootDirectory = "src";
    private readonly string BaseDirectory = AppDomain.CurrentDomain.BaseDirectory.Remove(AppDomain.CurrentDomain.BaseDirectory.IndexOf("\\bin\\Debug\\net8.0"));
    private readonly Random RNG = new();

    public void Main() {
        Console.WriteLine(ConsoleName + $"\nv{Version}");
        while (!UserInput.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) {
            Console.Write($"{CurrentDirectory}> ");
            UserInput = Console.ReadLine();
            RawUserCommand = UserInput.Split(" ");
            // raw user command is not changed to lowercase for printing messages such as echo
            // user command is lowercase for cd, echo, mkfile so that the command reads properly
            UserInput = UserInput.ToLower();
            UserCommand = UserInput.Split(" ");
            if (UserCommand[0].Equals("echo", StringComparison.CurrentCultureIgnoreCase)) {
                // echo <message> -r <repeat-times>
                Echo();
            } else if (UserCommand[0].Equals("mkfile", StringComparison.CurrentCultureIgnoreCase)) {
                // mkfile <filename.extension>
                if (UserCommand.Length == 2) {
                    CreateNewFile(RawUserCommand[1]);
                } else {
                    Console.WriteLine("Invalid use of mkdir.");
                }
            } else if (UserCommand[0].Equals("help", StringComparison.CurrentCultureIgnoreCase)) {
                // <help> 
                DisplayHelp();
            } else if (UserCommand[0].Equals("cd", StringComparison.CurrentCultureIgnoreCase)) {
                // cd <dir>
                // cd ../
                // FIXME: Not case sensitive
                ChangeDirectory();
            } else if (UserCommand[0].Equals("rmfile", StringComparison.CurrentCultureIgnoreCase)) {
                // rmfile <filename.extension>
                RemoveFile();
            } else if (UserCommand.Length == 2 && UserCommand[0].Equals("mkfolder", StringComparison.CurrentCultureIgnoreCase)) {
                // mkfolder <foldername>
                CreateFolder();
            } else if (UserCommand.Length == 2 && UserCommand[0].Equals("rmfolder", StringComparison.CurrentCultureIgnoreCase)) {
                // rmfolder <foldername>
                RemoveFolder();
            } else if (UserCommand[0].Equals("ls", StringComparison.CurrentCultureIgnoreCase)) {
                // ls [-i or -o]
                ListContentsOfCurrentDirectory();
            } else if (UserCommand[0].Equals("copyto", StringComparison.CurrentCultureIgnoreCase)) {
                // copyto <file> <directory>
                // so, we must validate the file exists in the current directory and that the directory exists
                CopyFileToDestination();
            } else {
                if (!UserCommand[0].Equals("exit", StringComparison.CurrentCultureIgnoreCase)) {
                    Console.WriteLine($"Unknown command {UserCommand[0]}");
                } else {
                    Console.WriteLine("Program Terminated Successfully.");
                }
            }
        }
    }
    private void DisplayHelp() {
        Console.WriteLine("Command List:");
        Console.WriteLine("==================================");
        Console.WriteLine("1. cd <directory> | Goes to the <directory> specified.");
        Console.WriteLine("2. copyto <filename> <destination> | Copies <filename> to <destination>.");
        Console.WriteLine("3. echo <message> -r <repeat_times> | Prints <message> <repeat_times> times. Can leave out -r and <repeat_times> for a one-time print of <message>.");
        Console.WriteLine("4. help | Displays this help menu.");
        Console.WriteLine("5. ls [-i or -o] | Lists all the files and directories in the current directory. -i shows only files, -o shows only folders.");
        Console.WriteLine("6. mkfile <filename.extension> | Creates a file with name <filename> and extension <extension> in the current directory.");
        Console.WriteLine("7. rmfile <filename.extension> | Deletes a file with name <filename> and extension <extension> in the current directory.");
        Console.WriteLine("8. mkfolder <foldername> | Creates a folder with name <foldername>.");
        Console.WriteLine("9. rmfolder <foldername> | Deletes a folder with name <foldername> AND ALL ITS CONTENTS RECURSIVELY.");
        Console.WriteLine("==================================");
    }
    private void ChangeDirectory() {
        if (!UserCommand[1].Contains("../") && UserCommand.Length == 2) {
            // is Path.Exists(path) not case sensitive?
            if (Path.Exists(BaseDirectory + $"\\{CurrentDirectory}\\{RawUserCommand[1]}")) {
                CurrentDirectory = $"{CurrentDirectory}\\{RawUserCommand[1]}";
            } else {
                Console.WriteLine("Invalid use of cd. Path may not exist.");
            }
        } else {
            if (UserCommand.Length == 2 && UserCommand[1].Length == 3 && UserCommand[1].Equals("../", StringComparison.CurrentCultureIgnoreCase)) {
                if (!CurrentDirectory.Equals(RootDirectory, StringComparison.CurrentCultureIgnoreCase)) {
                    CurrentDirectory = CurrentDirectory.Remove(CurrentDirectory.LastIndexOf('\\'));
                } else {
                    Console.WriteLine("You are in the root directory. You cannot go further back.");
                }
            } else {
                Console.WriteLine("Invalid use of cd.");
            }
        }
    }
    private void Echo() {
        // check to see if the user's input is valid
        if (UserCommand.Length == 4 && UserCommand[2].Equals("-r", StringComparison.CurrentCultureIgnoreCase)) {
            // if it's valid, we will try to parse the number 
            // and print <message> <repeat_times> times
            bool ConvertSuccess = long.TryParse(UserCommand[3], out long RepeatTimes);
            for (int i = 0; i < RepeatTimes; i++) {
                Console.WriteLine(RawUserCommand[1]);
            }
        } else if (UserCommand.Length == 3) {
            // if <repeat_times> is not specified, then repeat the message a random amount of times within reason
            Console.WriteLine("A repeat time (-r) was not specified. In the command, use: echo <message> -r <repeat_times>. The program will now repeat the message a set random number of times.");
            for (int i = 0; i < RNG.NextInt64(1, 1000); i++) {
                Console.WriteLine(UserCommand[1]);
            }
        } else {
            // if none of it is, just repeat it once
            Console.WriteLine(UserCommand[1]);
        }
    }
    private void ListContentsOfCurrentDirectory() {
        string Path = BaseDirectory + $"\\{CurrentDirectory}";
        if (UserCommand.Length == 1) {
            string[] Folders = Directory.GetDirectories(Path);
            string[] Files = Directory.GetFiles(Path);
            Console.WriteLine("FOLDERS:");
            for (int i = 0; i < Folders.Length; i++) {
                Console.WriteLine(Folders[i].Substring(Path.Length - CurrentDirectory.Length));
            }
            Console.WriteLine("FILES:");
            for (int i = 0; i < Files.Length; i++) {
                Console.WriteLine(Files[i].Substring(Path.Length - CurrentDirectory.Length));
            }
        } else if (UserCommand.Length == 2 && UserCommand[1].Equals("-i", StringComparison.CurrentCultureIgnoreCase)) {
            string[] Files = Directory.GetFiles(Path);
            Console.WriteLine("FILES:");
            for (int i = 0; i < Files.Length; i++) {
                Console.WriteLine(Files[i].Substring(Path.Length - CurrentDirectory.Length));
            }
        } else if (UserCommand.Length == 2 && UserCommand[1].Equals("-o", StringComparison.CurrentCultureIgnoreCase)) {
            string[] Folders =  Directory.GetDirectories(Path);
            Console.WriteLine("FOLDERS:");
            for (int i = 0; i < Folders.Length; i++) {
                Console.WriteLine(Folders[i].Substring(Path.Length - CurrentDirectory.Length));
            }
        } else {
            Console.WriteLine("Invalid use of ls.");
        }
    }
    private void CreateNewFile(string FileNameAndExtension) {
        // split the file's info into the name and extension
        string[] NewFileInfo = FileNameAndExtension.Split(".");
        if (NewFileInfo.Length > 2) {
            // there's multiple extensions
            Console.WriteLine($"Error in file creation. The name {FileNameAndExtension} should contain only 1 period. Multiple Periods are not supported yet.");
        } else if (NewFileInfo.Length <= 1) {
            // there's no extension
            Console.WriteLine($"Error: Please specify file extension.");
        } else {
            // get the base directory on anyone's computer
            // make a new file in the user's current directory
            FileStream NewFile = File.Create(BaseDirectory + $"\\{CurrentDirectory}\\{NewFileInfo[0]}.{NewFileInfo[1]}");
            NewFile.Close();
            Console.WriteLine("File created successfully.");
        }
    }
    private void RemoveFile() {
        // get the base directory
        if (UserCommand.Length == 2 && Path.Exists(BaseDirectory + $"\\{CurrentDirectory}\\{RawUserCommand[1]}")) {
            try {
                // try deleting it if it exists
                File.Delete(BaseDirectory + $"\\{CurrentDirectory}\\{RawUserCommand[1]}");
                Console.WriteLine("File successfully deleted.");
            } catch (Exception e) {
                // there was a problem deleting the file
                Console.WriteLine("Error in removing file. File may not exist, or the directory containing the file cannot be found.");
                Console.WriteLine(e.Message);
            }
        } else {
            Console.WriteLine("Invalid use of rmfile. File may not exist.");
        }
    }
    private void CreateFolder() {
        try {
            Directory.CreateDirectory(BaseDirectory + $"\\{CurrentDirectory}\\{RawUserCommand[1]}");
            Console.WriteLine("Folder created successfully.");
        } catch (Exception e) {
            Console.WriteLine("Error in creating folder. Path may not exist, or other directories may be invalid. ");
            Console.WriteLine(e.Message);
        }
    }
    private void RemoveFolder() {
        try {
            Directory.Delete(BaseDirectory + $"\\{CurrentDirectory}\\{RawUserCommand[1]}", true);
            Console.WriteLine("Folder deleted successfully.");
        } catch (Exception e) {
            Console.WriteLine("Error in deleting folder. Path may not exist, or other directories may be invalid. ");
            Console.WriteLine(e.Message);
        }
    }
    private void CopyFileToDestination() {
        string CurrentFile = BaseDirectory + $"\\{CurrentDirectory}\\{RawUserCommand[1]}";
        string Destination = BaseDirectory + $"\\{RawUserCommand[2]}";
        if (UserCommand.Length == 3) {
            if (Path.Exists(CurrentFile)) {
                // validate the file
                if (Path.Exists(Destination)) {
                    // validate the folder it's going to
                    try {
                        File.Copy(CurrentFile, Destination + $"\\{RawUserCommand[1]}");
                        Console.WriteLine("File successfully copied to destination!");
                    } catch (Exception e) {
                        Console.WriteLine("Error in copying file. File and directory are valid, but there was an error in copying the contents.");
                        Console.WriteLine(e.Message);
                    }
                } else {
                    Console.WriteLine("Directory does not exist.");
                }
            } else {
                Console.WriteLine("File does not exist.");
            }
        } else {
            Console.WriteLine("Invalid use of copyto.");
        }
    }
}