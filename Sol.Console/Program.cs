using Knox.ConsoleCommanding;
using Knox.Extensions;
using Sol.Console.Commands;
using Sol.Domain.Commands;
using Sol.Domain.Utilities;

var running = true;

var commands = new List<IConsoleCommand>()
{
    new ConvertLegacyToModernConsoleCommand(new LegacyLoadFromFileCommand(), new SaveHobbiesToFileCommandHandler(new FileExporter()))
};

while (running)
{
    foreach (var command in commands)
    {
        WriteLine(command.CommandDocumentation);
    }

    Write("\n] ");
    var inputRaw = Console.ReadLine().Wrap().UnwrapOrExchange(string.Empty)!;
    var input = inputRaw.Split(" ");
    var first = input.First().Wrap().UnwrapOrExchange(string.Empty);

    switch (first)
    {
        case "exit":
            WriteLine("Closing...");
            running = false;
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
                        await command.ExecuteAsync(context);

                        WriteSuccess($"{command.SuccessMessage(context)}");

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