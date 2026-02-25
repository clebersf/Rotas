using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Vale.Tops.Application.Presentation.AssetManager.Startup))]
namespace Vale.Tops.Application.Presentation.AssetManager
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
