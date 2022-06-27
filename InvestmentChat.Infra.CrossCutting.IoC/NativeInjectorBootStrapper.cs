using MediatR;
using Microsoft.Extensions.DependencyInjection;

namespace InvestmentChat.Infra.CrossCutting.IoC
{
    public static partial class NativeInjectorBootStrapper
    {
        public static void RegisterServices(this IServiceCollection services)
        {
            services.RegisterMediatR();
        }

        public static IServiceCollection RegisterMediatR(this IServiceCollection services)
        {
            return services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}
