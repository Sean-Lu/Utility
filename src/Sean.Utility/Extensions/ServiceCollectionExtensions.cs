#if NETSTANDARD
using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Sean.Utility.Contracts;
using Sean.Utility.Impls.Log;
using Sean.Utility.Impls.Queue;

namespace Sean.Utility.Extensions
{
    /// <summary>
    /// Extensions for <see cref="IServiceCollection"/>
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        #region SimpleLocalLogger
        public static IServiceCollection AddSimpleLocalLogger(this IServiceCollection services, bool useDefaultDependencyInjection = true, Action<SimpleLocalLoggerOptions> configureOptions = null, IConfiguration configuration = null)
        {
            // Configuration
            services.Configure<SimpleLocalLoggerOptions>((configuration ?? services.BuildServiceProvider().GetService<IConfiguration>()).GetSection(nameof(SimpleLocalLoggerOptions)));
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            if (useDefaultDependencyInjection)
            {
                services.AddSingleton<ISimpleLogger>(new SimpleLocalLogger());
                services.AddTransient(typeof(ISimpleLogger<>), typeof(SimpleLocalLogger<>));
                services.AddTransient(typeof(ISimpleLoggerAsync<>), typeof(SimpleLocalLoggerAsync<>));
            }

            SimpleLocalLoggerBase.ServiceProvider = services.BuildServiceProvider();
            return services;
        }
        #endregion

        #region SimpleQueue
        public static IServiceCollection AddSimpleQueue(this IServiceCollection services, bool useDefaultDependencyInjection = true, Action<SimpleQueueOptions> configureOptions = null, IConfiguration configuration = null)
        {
            // Configuration
            services.Configure<SimpleQueueOptions>((configuration ?? services.BuildServiceProvider().GetService<IConfiguration>()).GetSection(nameof(SimpleQueueOptions)));
            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            if (useDefaultDependencyInjection)
            {
                services.AddSingleton(typeof(ISimpleQueue<>), typeof(SimpleQueue<>));
            }

            SimpleQueueBase.ServiceProvider = services.BuildServiceProvider();
            return services;
        }
        #endregion
    }
}
#endif