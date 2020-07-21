using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using Autofac;
using Trgovina.netapi.Controllers;
using Trgovina.Service;
using Trgovina.Service.Common;
using Trgovina.Repository.Common;
using Trgovina.Repository;
using Autofac.Integration.WebApi;
using System.Reflection;

namespace Trgovina.netapi
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        { 
            GlobalConfiguration.Configure(WebApiConfig.Register);
            
            var builder = new ContainerBuilder();
            var config = GlobalConfiguration.Configuration;

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());

            builder.RegisterModule(new TrgovinaServiceModule());
            builder.RegisterModule(new TrgovinaRepositoryModule());
            
            
            var container = builder.Build();

            GlobalConfiguration.Configuration.DependencyResolver = new AutofacWebApiDependencyResolver(container);

        }
    }
}
