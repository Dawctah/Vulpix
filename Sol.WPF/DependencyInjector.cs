using Knox.Commanding;
using Knox.Mediation;
using Microsoft.Extensions.DependencyInjection;
using Sol.Domain.Commands;
using Sol.Domain.Utilities;

namespace Sol.WPF
{
    public static class DependencyInjector
    {
        public static IServiceCollection InjectAll(this IServiceCollection services)
        {
            services
                .InjectCommands()
                .InjectMediator()
                .AddSingleton<IExporter, FileExporter>()
                ;

            return services;
        }

        public static IServiceCollection InjectCommands(this IServiceCollection services)
        {
            services
                .AddTransient<ICommandHandler<CreateItemCommand>, CreateItemCommandHandler>()
                .AddTransient<ICommandHandler<SaveHobbiesToFileCommand>, SaveHobbiesToFileCommandHandler>()
                .AddTransient<ICommandHandler<ChangeItemStatusCommand>, ChangeItemStatusCommandHandler>()
                .AddTransient<ICommandHandler<DeleteItemCommand>, DeleteItemCommandHandler>()
                .AddTransient<ICommandHandler<BumpItemCommand>, BumpItemCommandHandler>()
                .AddTransient<ICommandHandler<ExportNotStartedListCommand>, ExportNotStartedListCommandHandler>()
                .AddTransient<ICommandHandler<CompleteItemCommand>, CompleteItemCommandHandler>()
                ;

            return services;
        }

        private static IServiceCollection InjectMediator(this IServiceCollection services)
        {
            services.AddTransient<IMediator>((provider) =>
            {
                var mediator = new Mediator();
                mediator
                    .Register(provider.GetRequiredService<ICommandHandler<CreateItemCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<SaveHobbiesToFileCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<ChangeItemStatusCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<DeleteItemCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<BumpItemCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<ExportNotStartedListCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<CompleteItemCommand>>())
                    ;
                return mediator;
            });

            return services;
        }
    }
}