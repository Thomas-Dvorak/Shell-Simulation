using System.Security.Cryptography;

public class SimpleConsole {
    private readonly string Version = "1.0.0";
    private readonly string ConsoleName = "Devo Command Line";
    private readonly List<string> UserInputHistory = new();
    private string UserInput = "default_text";
    private string[] UserCommand;
    private readonly Random RNG = new();

    public void Main() {
        Console.WriteLine(ConsoleName + $"\nv{Version}");
        while (!UserInput.Equals("exit", StringComparison.CurrentCultureIgnoreCase)) {
            Console.Write("Enter A Command: \n==> ");
            UserInput = Console.ReadLine();
            UserInputHistory.Add(UserInput);
            UserInput = UserInput.ToLower();
            UserCommand = UserInput.Split(" ");
            if (UserCommand[0].Equals("echo", StringComparison.CurrentCultureIgnoreCase)) {
                if (UserCommand.Length == 4 && UserCommand[2].Equals("-r", StringComparison.CurrentCultureIgnoreCase)) {
                    bool ConvertSuccess = long.TryParse(UserCommand[3], out long RepeatTimes);
                    for (int i = 0; i < RepeatTimes; i++) {
                        Console.WriteLine(UserCommand[1]);
                    }
                } else if (UserCommand.Length == 3) {
                    Console.WriteLine("A repeat time (-r) was not specified. In the command, use: echo <message> -r <repeat_times>. The program will now repeat the message a set random number of times.");
                    for (int i = 0; i < RNG.NextInt64(1, 1000); i++) {
                        Console.WriteLine(UserCommand[1]);
                    }
                } else {
                    Console.WriteLine(UserCommand[1]);
                }
            } else if (UserCommand[0].Equals("help", StringComparison.CurrentCultureIgnoreCase)) {
                DisplayHelp();
            }
        }
        Console.WriteLine("Program Terminated Successfully.");
    }
    private void DisplayHelp() {
        Console.WriteLine("Command List:");
        Console.WriteLine("==================================");
        Console.WriteLine("1. echo <message> -r <repeat_times> : Prints the message <repeat_times> times. Can leave out -r and <repeat_times> for a one-time print of the message.");
    }
}