using eVote360Pro.Core.Application.Interfaces.Infrastructure;
using eVote360Pro.Core.Domain.Settings;
using eVote360Pro.Infraestructure.Shared.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace eVote360Pro.Infraestructure.Shared
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddSharedInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));
            services.AddTransient<IEmailService, EmailService>();
            services.AddTransient<IOcrService, OcrService>();
            services.AddTransient<IUploadService, UploadService>();
            return services;
        }
    }
}
