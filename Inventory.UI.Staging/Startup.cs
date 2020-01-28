using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Inventory.UI.Staging.Startup))]
namespace Inventory.UI.Staging
{
    public partial class Startup {
        public void Configuration(IAppBuilder app) {
            ConfigureAuth(app);
        }
    }
}
