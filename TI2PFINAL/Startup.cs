using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

using Owin;
using TI2PFINAL.Models;

namespace TI2PFINAL
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);

            inic();
        }

        private void inic() {

            ApplicationDbContext db = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(db));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(db));

            //criar a Role 'Admin'
            if (!roleManager.RoleExists("Admin")) {
                //não existe a 'role'
                //cria a 'role'
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
            }

            //criar a Role 'User'
            if (!roleManager.RoleExists("User"))
            {
                //não existe a 'role'
                //cria a 'role'
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }


            //cria um utilizador 'Admin' 
            var user = new ApplicationUser();
            user.UserName = "l.thorne@gmail.com";
            user.Email = "l.thorne@gmail.com";

            string userPWD = "RutePaz16*";
            var chkUser = userManager.Create(user, userPWD);

            //adiciona o utilizador à respetiva Role-Admin
            if (chkUser.Succeeded) {
                var result = userManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
