using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(RegistroVisitantes.Startup))]
namespace RegistroVisitantes
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
