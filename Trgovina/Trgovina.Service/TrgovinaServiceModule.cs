using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Trgovina.Service.Common;

namespace Trgovina.Service
{
    public class TrgovinaServiceModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrgovinaService>().As<ITrgovinaServices>();
        }
    }
}
