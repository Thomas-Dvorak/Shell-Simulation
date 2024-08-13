using System.Diagnostics.Tracing;
using System.Security.Cryptography;

public class SimpleConsole {
    private readonly string Version = "1.8.2";
    private readonly string ConsoleName = "Devo Command Line";
    private readonly List<string> UserInputHistory = new();
    private string UserInput = "default_text";
    private string[] UserCommand;
    private string[] RawUserCommand;
    private string CurrentDirectory = "src";
    private string RootDirectory = "src";
    private readonly Random RNG = new();

    public void Main() {
        Console.WriteLine(ConsoleName + $"\nv{Version}");
        while (!UserInput.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) {
            Console.Write($"{CurrentDirectory}> ");
            UserInput = Console.ReadLine();
            UserInputHistory.Add(UserInput);
            RawUserCommand = UserInput.Split(" ");
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
            } else if (UserCommand.Length == 2 && UserCommand[0].Equals("ls", StringComparison.CurrentCultureIgnoreCase)) {
                ListContentsOfCurrentDirectory();
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
        Console.WriteLine("2. echo <message> -r <repeat_times> | Prints <message> <repeat_times> times. Can leave out -r and <repeat_times> for a one-time print of <message>.");
        Console.WriteLine("3. help | Displays this help menu.");
        Console.WriteLine("4. ls [-i or -o] | Lists all the files and directories in the current directory. -i shows only files, -o shows only folders. NEEDS TO BE IMPLEMENTED");
        Console.WriteLine("5. mkfile <filename.extension> | Creates a file with name <filename> and extension <extension> in the current directory.");
        Console.WriteLine("6. rmfile <filename.extension> | Deletes a file with name <filename> and extension <extension> in the current directory.");
        Console.WriteLine("7. mkfolder <foldername> | Creates a folder with name <foldername>.");
        Console.WriteLine("8. rmfolder <foldername> | Deletes a folder with name <foldername> AND ALL ITS CONTENTS RECURSIVELY.");
        Console.WriteLine("==================================");
    }
    private void ChangeDirectory() {
        if (!UserCommand[1].Contains("../") && UserCommand.Length == 2) {
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            // is Path.Exists(path) not case sensitive?
            if (Path.Exists(BaseDir.Remove(BaseDir.IndexOf("\\bin\\Debug\\net8.0\\")) + $"\\{CurrentDirectory}\\{RawUserCommand[1]}")) {
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
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            FileStream NewFile = File.Create(BaseDir.Remove(BaseDir.IndexOf("\\bin\\Debug\\net8.0\\")) + $"\\{CurrentDirectory}\\{NewFileInfo[0]}.{NewFileInfo[1]}");
            NewFile.Close();
            Console.WriteLine("File created successfully.");
        }
    }
    private void RemoveFile() {
        // get the base directory
        string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
        if (UserCommand.Length == 2 && Path.Exists(BaseDir.Remove(BaseDir.IndexOf("\\bin\\Debug\\net8.0\\")) + $"\\{CurrentDirectory}\\{RawUserCommand[1]}")) {
            try {
                // try deleting it if it exists
                File.Delete(BaseDir.Remove(BaseDir.IndexOf("\\bin\\Debug\\net8.0\\")) + $"\\{CurrentDirectory}\\{RawUserCommand[1]}");
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
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            Directory.CreateDirectory(BaseDir.Remove(BaseDir.IndexOf("\\bin\\Debug\\net8.0\\")) + $"\\{CurrentDirectory}\\{RawUserCommand[1]}");
            Console.WriteLine("Folder created successfully.");
        } catch (Exception e) {
            Console.WriteLine("Error in creating folder. Path may not exist, or other directories may be invalid. ");
            Console.WriteLine(e.Message);
        }
    }
    private void RemoveFolder() {
        try {
            string BaseDir = AppDomain.CurrentDomain.BaseDirectory;
            Directory.Delete(BaseDir.Remove(BaseDir.IndexOf("\\bin\\Debug\\net8.0\\")) + $"\\{CurrentDirectory}\\{RawUserCommand[1]}", true);
            Console.WriteLine("Folder deleted successfully.");
        } catch (Exception e) {
            Console.WriteLine("Error in deleting folder. Path may not exist, or other directories may be invalid. ");
            Console.WriteLine(e.Message);
        }
    }
}