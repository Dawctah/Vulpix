using Sol.Console.Commanding;
using Sol.Domain.Commanding;
using Sol.Domain.Commands;
using Sol.Domain.Common.Maybes;
using Sol.Domain.Models;
using Sol.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sol.Console.Commands
{
    public class CreateBookConsoleCommand : IConsoleCommand
    {
        private readonly ICommand<CreateBookCommandContext> createBookCommand;

        public string CommandDocumentation => $"{CommandName} [TITLE]";

        public string CommandName => "create-book";

        public string SuccessMessage => "Book created";

        public CreateBookConsoleCommand(ICommand<CreateBookCommandContext> createBookCommand)
        {
            this.createBookCommand = createBookCommand;
        }

        public void Execute(ConsoleCommandContext context)
        {
            var title = string.Empty;
            foreach(var arg in context.Arguments)
            {
                if (arg == CommandName)
                {
                    continue;
                }

                title += $"{arg} ";
            }

            title = title.Trim();

            var commandContext = new CreateBookCommandContext(title, new SaveFile());
            createBookCommand.Execute(commandContext);
        }
    }
}
