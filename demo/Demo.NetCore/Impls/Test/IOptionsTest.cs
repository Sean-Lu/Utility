using System;
using Demo.NetCore.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Sean.Utility.Contracts;

namespace Demo.NetCore.Impls.Test
{
    public class IOptionsTest : ISimpleDo
    {
        public void Execute()
        {
            //https://www.cnblogs.com/wenhx/p/ioptions-ioptionsmonitor-and-ioptionssnapshot.html
            //ASP.NET Core引入了Options模式，使用类来表示相关的设置组。简单的来说，就是用强类型的类来表达配置项，这带来了很多好处。
            //初学者会发现这个框架有3个主要的面向消费者的接口：IOptions<TOptions>、IOptionsMonitor<TOptions>以及IOptionsSnapshot<TOptions>。
            //1. IOptions<> 是单例(Singleton)，因此一旦生成了，除非通过代码的方式更改，它的值是不会更新的。
            //2. IOptionsMonitor<> 也是单例(Singleton)，但是它通过IOptionsChangeTokenSource<> 能够和配置文件一起更新，也能通过代码的方式更改值。
            //3. IOptionsSnapshot<> 是范围(Scoped)，所以在配置文件更新的下一次访问，它的值会更新，但是它不能跨范围通过代码的方式更改值，只能在当前范围（请求）内有效。
            //一般来说，如果你依赖配置文件，那么首先考虑IOptionsMonitor<>，如果不合适接着考虑IOptionsSnapshot<>，最后考虑IOptions<>。

            var configuration = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: true, reloadOnChange: true).Build(); //注意最后一个参数值，true表示配置文件更改时会重新加载。
            var services = new ServiceCollection();
            services.AddOptions();
            //services.Configure<TestOptions>(configuration); //这里通过配置文件绑定TestOptions
            //services.Configure<TestOptions>(configuration.GetSection("TestOptions")); //这里通过配置文件绑定TestOptions
            services.Configure<TestOptions>(options =>
            {
                options.Name = "Test 0";
            });
            var provider = services.BuildServiceProvider();
            Console.WriteLine("修改前：");
            Print(provider);

            Change(provider); //使用代码修改Options值。
            Console.WriteLine("使用代码修改后：");
            Print(provider);

            Console.WriteLine("请修改配置文件。");
            Console.ReadLine(); //等待手动修改appsettings.json配置文件。
            Console.WriteLine("修改appsettings.json文件后：");
            Print(provider);
        }

        private void Print(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var sp = scope.ServiceProvider;
                var options1 = sp.GetRequiredService<IOptions<TestOptions>>();
                var options2 = sp.GetRequiredService<IOptionsMonitor<TestOptions>>();
                var options3 = sp.GetRequiredService<IOptionsSnapshot<TestOptions>>();
                Console.WriteLine("IOptions值: {0}", options1.Value.Name);
                Console.WriteLine("IOptionsMonitor值: {0}", options2.CurrentValue.Name);
                Console.WriteLine("IOptionsSnapshot值: {0}", options3.Value.Name);
                Console.WriteLine();
            }
        }

        private void Change(IServiceProvider provider)
        {
            using (var scope = provider.CreateScope())
            {
                var sp = scope.ServiceProvider;
                sp.GetRequiredService<IOptions<TestOptions>>().Value.Name = "IOptions Test 1";
                sp.GetRequiredService<IOptionsMonitor<TestOptions>>().CurrentValue.Name = "IOptionsMonitor Test 1";
                sp.GetRequiredService<IOptionsSnapshot<TestOptions>>().Value.Name = "IOptionsSnapshot Test 1";
            }
        }
    }
}
