using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MediatRTest
{
    /// <summary>
    /// 全局存储IServiceProvider
    /// </summary>
    public class ServiceLocator
    {
        /// <summary>
        /// IOC中的IServiceProvider对象接口
        /// </summary>
        public static IServiceProvider SerivcePovider { get; private set; }

        /// <summary>
        /// 赋值IServiceProvider到静态变量中
        /// </summary>
        /// <param name="provider">IServiceProvider对象接口</param>
        public static void ConfigService(IServiceProvider provider)
        {
            SerivcePovider = provider;
        }

        /// <summary>
        /// 获取指定服务接口实例
        /// </summary>
        /// <returns></returns>
        public static T GetService<T>()
        {
            return SerivcePovider.GetService<T>();
        }
    }
}
