using CadCli.Dominio.Interfaces;
using CadCli.Infra.DataEF.Repositorio;
using CadCli.Servico.Api.Infra;
using Microsoft.Practices.Unity;
using Newtonsoft.Json.Serialization;
using System.Web.Http;
using System.Web.Http.Cors;

namespace CadCli.Servico.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services
            var cors = new EnableCorsAttribute("*", "*", "GET");
            config.EnableCors(cors);

            var container = new UnityContainer();
            container.RegisterType<IClienteRepositorio, ClienteRepositorio>(new HierarchicalLifetimeManager());
            container.RegisterType<IEmpresaRepositorio, EmpresaRepositorio>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);


            // Web API routes
            config.MapHttpAttributeRoutes();

            //Removendo o XML
            var formatters = GlobalConfiguration.Configuration.Formatters;
            formatters.Remove(formatters.XmlFormatter);

            // Modifica a serialização - não dar erro de referência circular
            formatters
                .JsonFormatter.SerializerSettings
                //.PreserveReferencesHandling = Newtonsoft.Json.PreserveReferencesHandling.Objects; //cria um referencia aos objs serializados
                .ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore; //Serializa tudo

            //Indentando o JSON (ver no raw do fiddler)
            var jsonSettings = formatters.JsonFormatter.SerializerSettings;
            jsonSettings.Formatting = Newtonsoft.Json.Formatting.Indented;

            //CamelCase nas propriedades do objeto
            jsonSettings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
        }
    }
}
