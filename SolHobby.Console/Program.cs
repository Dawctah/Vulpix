// See https://aka.ms/new-console-template for more information
Console.WriteLine("Hello, World!");

var running = true;

var loadCommand = new LoadFromFileCommand();
var saveFile = loadCommand.Execute(new(Data.Directory, Data.FullName(Sol.Domain.Repositories.Profile.Personal)));
var saveToFileCommand = new SaveToFileCommand(new FileExporter());

var commands = LoadCommands();

while (running)
{
    foreach (var command in commands)
    {
        WriteLine(command.CommandDocumentation);
    }

    Write("\n] ");
    var inputRaw = Console.ReadLine().ToMaybe().GetOrElse(string.Empty)!;
    var input = inputRaw.Split(" ");
    var first = input.First().ToMaybe().GetOrElse(string.Empty);

    switch (first)
    {
        case "exit":
            WriteLine("Saving data...");
            Save(new(input));
            WriteLine("Closing...");
            running = false;
            break;

        case "list":
            WriteLine("To Be Read:");
            foreach (var book in saveFile.GetToBeRead())
            {
                WriteLine(book.ToString());
            }

            WriteLine("\nCurrently Reading:");
            foreach (var book in saveFile.GetCurrentlyReading())
            {
                WriteLine(book.ToString());
            }

            WriteLine("\nDid Not Finish:");
            foreach (var book in saveFile.GetDidNotFinish())
            {
                WriteLine(book.ToString());
            }

            WriteLine("\nFinished:");
            foreach (var book in saveFile.GetFinished())
            {
                WriteLine(book.ToString());
            }

            WriteLine();
            break;

        default:
            try
            {
                var success = false;
                foreach (var command in commands)
                {
                    if (first == command.CommandName)
                    {
                        var context = new ConsoleCommandContext(input);
                        if (command.CommandName == "save")
                        {
                            Save(context);
                        }
                        else
                        {
                            command.Execute(context);
                        }

                        WriteSuccess($"{command.SuccessMessage}");

                        success = true;
                        break;
                    }
                }

                if (!success)
                {
                    WriteError($"Could not find command matching pattern {inputRaw}");
                }
            }
            catch (Exception ex)
            {
                WriteError($"Could not execute command matching pattern \"{inputRaw}\"");
                WriteError(ex.Message);
            }
            finally
            {
                WriteLine(string.Empty);
            }

            break;
    }
}


static void WriteError(string error) => WriteLine(error, ConsoleColor.Red);
static void WriteSuccess(string success) => WriteLine(success, ConsoleColor.DarkGreen);

static void WriteLine(string line = "", ConsoleColor color = ConsoleColor.Gray)
{
    Console.ForegroundColor = color;
    Console.WriteLine(line);
    Console.ResetColor();
}

static void Write(string line, ConsoleColor color = ConsoleColor.Gray)
{
    Console.ForegroundColor = color;
    Console.Write(line);
    Console.ResetColor();
}