using Autofac;
using NLog.Web;
using System.Reflection.Metadata;
using wm.common.CommonUtils;

namespace wm.api
{
    public class AutofacModule : Module
    {
        
        protected override void Load(ContainerBuilder builder)
        {
            
            //把服务的注入规则写在这里
            builder.re<ILoggerBase>().As>();
        }
    }
}
