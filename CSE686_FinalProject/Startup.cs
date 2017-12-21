using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(CSE686_FinalProject.Startup))]
namespace CSE686_FinalProject
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
