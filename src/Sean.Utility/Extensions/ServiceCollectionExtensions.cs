#if NETSTANDARD || NET5_0_OR_GREATER
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
            return services.GetImplementationInstance<IConfiguration>() ?? services.BuildServiceProvider().GetService<IConfiguration>();
        }

        /// <summary>
        /// Get <see cref="ServiceDescriptor.ImplementationInstance"/> of type T from <see cref="IServiceCollection"/>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static T GetImplementationInstance<T>(this IServiceCollection services)
        {
            var serviceDescriptor = services.FirstOrDefault(c => c.ServiceType == typeof(T));
            if (serviceDescriptor == null && typeof(T).IsGenericType)
            {
                serviceDescriptor = services.FirstOrDefault(c => c.ServiceType.IsGenericType && c.ServiceType.GetGenericTypeDefinition() == typeof(T).GetGenericTypeDefinition());
            }
            var instance = serviceDescriptor?.ImplementationInstance;
            return instance != null ? (T)instance : default;
        }

        #region SimpleLocalLogger
        /// <summary>
        /// 默认依赖注入(<paramref name="useDefaultDependencyInjection"/> == true)：
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
            var provider = services.BuildServiceProvider();
            SimpleLocalLoggerBase.ServiceProvider = provider;

            if (configuration is IConfigurationSection section)
            {
                services.Configure<SimpleLocalLoggerOptions>(section);
            }
            else
            {
                services.Configure<SimpleLocalLoggerOptions>((configuration ?? provider.GetService<IConfiguration>())?.GetSection(nameof(SimpleLocalLoggerOptions)));
            }

            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            if (useDefaultDependencyInjection)
            {
                services.AddTransient(typeof(ISimpleLogger), typeof(SimpleLocalLogger));
                services.AddTransient(typeof(ISimpleLogger<>), typeof(SimpleLocalLogger<>));
                services.AddTransient(typeof(ISimpleLoggerAsync<>), typeof(SimpleLocalLoggerAsync<>));
            }

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
            var provider = services.BuildServiceProvider();
            SimpleQueueBase.ServiceProvider = provider;

            if (configuration is IConfigurationSection section)
            {
                services.Configure<SimpleQueueOptions>(section);
            }
            else
            {
                services.Configure<SimpleQueueOptions>((configuration ?? provider.GetService<IConfiguration>())?.GetSection(nameof(SimpleQueueOptions)));
            }

            if (configureOptions != null)
            {
                services.Configure(configureOptions);
            }

            if (useDefaultDependencyInjection)
            {
                services.AddSingleton(typeof(ISimpleQueue<>), typeof(SimpleQueue<>));
            }

            return services;
        }
        #endregion
    }
}
#endif