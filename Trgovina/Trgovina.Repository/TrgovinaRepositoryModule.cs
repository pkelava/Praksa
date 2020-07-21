using Autofac;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trgovina.Repository.Common;

namespace Trgovina.Repository
{
    public class TrgovinaRepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<TrgovinaRepository>().As<ITrgovinaRepository>();
        }
    }
}
