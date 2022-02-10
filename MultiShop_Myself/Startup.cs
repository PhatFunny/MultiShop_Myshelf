using Microsoft.Owin;
using Owin;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using MultiShop_Myself.Models;

[assembly: OwinStartupAttribute(typeof(MultiShop_Myself.Startup))]
namespace MultiShop_Myself
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            //InitUserRole();
        }

        private void InitUserRole()
        {
            ApplicationDbContext context = new ApplicationDbContext();
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
            //Tạo role Admin
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);
                //Tạo user
                var user = new ApplicationUser();
                user.UserName = "admin@qlbh.com";
                user.Email = "admin@qlbh.com";
                string pass = "123456";
                var chkUser = userManager.Create(user, pass);
                //Đưa user qlbh vào role Admin
                if(chkUser.Succeeded)
                    userManager.AddToRole(user.Id, "Admin");
            }
        }
    }
}
