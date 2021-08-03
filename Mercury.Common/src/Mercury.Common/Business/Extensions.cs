using Mercury.Common.Settings;
using Microsoft.Extensions.DependencyInjection;

namespace Mercury.Common.Business
{
    public static class Extensions
    {
        public static IServiceCollection AddComponent<T>(this IServiceCollection services) where T : IEntity
        {
            services.AddSingleton<IBaseComponent<T>>(ServiceProvider => 
            {
                var repository = ServiceProvider.GetService<IRepository<T>>();
                return new BaseComponent<T>(repository);
            });

            return services;
        }
    }
}