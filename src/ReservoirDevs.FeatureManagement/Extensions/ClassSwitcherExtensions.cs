using Microsoft.Extensions.DependencyInjection;

namespace ReservoirDevs.FeatureManagement.Extensions
{
    public static class ClassSwitcherExtensions
    {
        public static IServiceCollection AddSwitchableClasses<TInterface, TEnabledClass, TDisabledClass>(this IServiceCollection serviceCollection, string featureFlag)
            where TInterface : class
            where TEnabledClass : class, TInterface
            where TDisabledClass : class, TInterface
        {
            serviceCollection.Configure<ClassSwitcherOptions>(options =>
            {
                options.Flag = featureFlag;
                options.Interface = typeof(TInterface);
            });

            serviceCollection.AddTransient<TInterface, TEnabledClass>();
            serviceCollection.AddTransient<TInterface, TDisabledClass>();

            return serviceCollection.AddTransient<ClassSwitcher<TInterface, TEnabledClass, TDisabledClass>>();
        }
    }
}
