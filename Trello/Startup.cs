using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(Trello.Startup))]
namespace Trello
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.MapSignalR();
            ConfigureAuth(app);
        }
    }
}
