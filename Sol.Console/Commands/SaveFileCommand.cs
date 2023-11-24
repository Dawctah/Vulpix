using Knox.ConsoleCommanding;
using Knox.Mediation;
using Sol.Domain.Commands;
using Sol.Domain.Common;
using Sol.Domain.Repositories;

namespace Sol.Console.Commands
{
    public class SaveFileCommand : IConsoleCommand
    {
        private readonly IHobbyFile hobbyFile;
        private readonly IMediator mediator;

        public SaveFileCommand(IHobbyFile hobbyFile, IMediator mediator)
        {
            this.hobbyFile = hobbyFile;
            this.mediator = mediator;
        }

        public string CommandDocumentation => "save";

        public string CommandName => "save";

        public async Task ExecuteAsync(ConsoleCommandContext context)
        {
            await mediator.ExecuteCommandAsync(new SaveHobbiesToFileCommand(Data.Directory, Data.ProfileName(Profile.BookClub), hobbyFile));
        }

        public string SuccessMessage(ConsoleCommandContext context) => $"Successfully saved file to {Data.Directory}";
    }
}
