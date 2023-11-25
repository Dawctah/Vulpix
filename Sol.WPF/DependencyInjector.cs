using Knox.Commanding;
using Knox.Mediation;
using Microsoft.Extensions.DependencyInjection;
using Sol.Domain.Commanding;
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
            services.AddTransient(typeof(ICommand<SaveToFileCommandContext>), typeof(SaveToFileCommand));

            services.AddTransient(typeof(ICommand<CreateBookCommandContext>), typeof(CreateBookCommand));
            services.AddTransient(typeof(ICommand<StartReadingBookCommandContext>), typeof(StartReadingBookCommand));
            services.AddTransient(typeof(ICommand<StopReadingBookCommandContext>), typeof(StopReadingBookCommand));
            services.AddTransient(typeof(ICommand<FinishBookCommandContext>), typeof(FinishBookCommand));
            services.AddTransient(typeof(ICommand<DoNotFinishBookCommandContext>), typeof(DoNotFinishBookCommand));
            services.AddTransient(typeof(ICommand<SwapBookOrderCommandContext>), typeof(SwapBookOrderCommand));
            services.AddTransient(typeof(ICommand<ExportTbrToFileCommandContext>), typeof(ExportTbrToFileCommand));

            return services;
        }

        private static IServiceCollection InjectMediator(this IServiceCollection services)
        {
            services
                .AddTransient<ICommandHandler<CreateItemCommand>, CreateItemCommandHandler>()
                .AddTransient<ICommandHandler<SaveHobbiesToFileCommand>, SaveHobbiesToFileCommandHandler>()
                .AddTransient<ICommandHandler<ChangeItemStatusCommand>, ChangeItemStatusCommandHandler>()
                ;

            services.AddTransient<IMediator>((provider) =>
            {
                var mediator = new Mediator();
                mediator
                    .Register(provider.GetRequiredService<ICommandHandler<CreateItemCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<SaveHobbiesToFileCommand>>())
                    .Register(provider.GetRequiredService<ICommandHandler<ChangeItemStatusCommand>>())
                    ;
                return mediator;
            });

            return services;
        }
    }
}