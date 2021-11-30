#if NETSTANDARD
using System;
using System.Linq;
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
        /// <summary>
        /// Get <see cref="IConfiguration"/> from <see cref="IServiceCollection"/>
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IConfiguration GetConfiguration(this IServiceCollection services)
        {
            //return services.BuildServiceProvider().GetService<IConfiguration>();
            return (IConfiguration)services.FirstOrDefault(p => p.ServiceType == typeof(IConfiguration))?.ImplementationInstance;
        }

        #region SimpleLocalLogger
        /// <summary>
        /// <para><see cref="ISimpleLogger"/></para>
        /// <para><see cref="ISimpleLogger{T}"/></para>
        /// <para><see cref="ISimpleLoggerAsync{T}"/></para>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="useDefaultDependencyInjection"></param>
        /// <param name="configureOptions"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSimpleLocalLogger(this IServiceCollection services, bool useDefaultDependencyInjection = true, Action<SimpleLocalLoggerOptions> configureOptions = null, IConfiguration configuration = null)
        {
            if (configuration is IConfigurationSection section)
            {
                services.Configure<SimpleLocalLoggerOptions>(section);
            }
            else
            {
                services.Configure<SimpleLocalLoggerOptions>((configuration ?? services.GetConfiguration()).GetSection(nameof(SimpleLocalLoggerOptions)));
            }

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
        /// <summary>
        /// <see cref="ISimpleQueue{T}"/>
        /// </summary>
        /// <param name="services"></param>
        /// <param name="useDefaultDependencyInjection"></param>
        /// <param name="configureOptions"></param>
        /// <param name="configuration"></param>
        /// <returns></returns>
        public static IServiceCollection AddSimpleQueue(this IServiceCollection services, bool useDefaultDependencyInjection = true, Action<SimpleQueueOptions> configureOptions = null, IConfiguration configuration = null)
        {
            if (configuration is IConfigurationSection section)
            {
                services.Configure<SimpleQueueOptions>(section);
            }
            else
            {
                services.Configure<SimpleQueueOptions>((configuration ?? services.GetConfiguration()).GetSection(nameof(SimpleQueueOptions)));
            }

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