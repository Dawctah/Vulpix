using Microsoft.Extensions.DependencyInjection;
using Sol.Domain.Commanding;
using Sol.Domain.Commands;
using Sol.Domain.Repositories;
using Sol.Domain.Utilities;

namespace Sol.WPF
{
    public static class DependencyInjector
    {
        public static IServiceCollection InjectAll(this IServiceCollection services)
        {
            services.AddSingleton(typeof(ISaveFile), typeof(SaveFile));

            services.InjectCommands();

            services.AddSingleton(typeof(IExporter), typeof(FileExporter));

            return services;
        }

        public static IServiceCollection InjectCommands(this IServiceCollection services)
        {
            services.AddTransient(typeof(ICommand<SaveToFileCommandContext>), typeof(SaveToFileCommand));
            services.AddTransient(typeof(ICommand<LoadFromFileCommandContext, ISaveFile>), typeof(LoadFromFileCommand));

            services.AddTransient(typeof(ICommand<CreateBookCommandContext>), typeof(CreateBookCommand));
            services.AddTransient(typeof(ICommand<StartReadingBookCommandContext>), typeof(StartReadingBookCommand));
            services.AddTransient(typeof(ICommand<StopReadingBookCommandContext>), typeof(StopReadingBookCommand));
            services.AddTransient(typeof(ICommand<FinishBookCommandContext>), typeof(FinishBookCommand));
            services.AddTransient(typeof(ICommand<DoNotFinishBookCommandContext>), typeof(DoNotFinishBookCommand));
            services.AddTransient(typeof(ICommand<SwapBookOrderCommandContext>), typeof(SwapBookOrderCommand));
            services.AddTransient(typeof(ICommand<ExportTbrToFileCommandContext>), typeof(ExportTbrToFileCommand));

            return services;
        }
    }
}