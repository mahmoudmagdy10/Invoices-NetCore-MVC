using Invoices.BL.Interface;
using Invoices.BL.Models;
using Invoices.DAL.Extend;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.BL.Repository
{
    public class RolesRep : IRolesRep
    {
        #region Fields
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        #endregion

        #region Ctor
        public RolesRep(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
            this.userManager = userManager;
        }
        #endregion

        #region Actions

        public IEnumerable<IdentityRole> Get()
        {
            var roles = roleManager.Roles;
            return roles;
        }

        public async Task<IdentityRole> Create(IdentityRole model)
        {
            var role = new IdentityRole()
            {
                Name = model.Name,
                NormalizedName = model.Name.ToUpper()
            };

            var result = await roleManager.CreateAsync(role);

            if (result.Succeeded)
            {
                return role;
            }

            return new IdentityRole()
            {
                Name = null,
                NormalizedName = null
            };
        }

        //public async Task<IdentityRole> Edit(IdentityRole model)
        //{
        //    try
        //    {
        //        var role = await roleManager.FindByIdAsync(model.Id);

        //        role.Name = model.Name;
        //        role.NormalizedName = model.Name.ToUpper();

        //        await roleManager.UpdateAsync(role);

        //        return role;
        //    }
        //    catch (Exception)
        //    {
        //        return new IdentityRole()
        //        {
        //            Name = null,
        //            NormalizedName = null
        //        };
        //    }
        //}

        public async Task<List<UserInRoleVM>> AddOrRemoveUsers(string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);
            var Users = new List<UserInRoleVM>();

            foreach (var user in userManager.Users)
            {
                var UserInRole = new UserInRoleVM()
                {
                    UserId = user.Id,
                    UserName = user.UserName
                };

                if (await userManager.IsInRoleAsync(user, role.Name))
                {
                    UserInRole.IsSelected = true;
                }
                else
                {
                    UserInRole.IsSelected = false;
                }

                Users.Add(UserInRole);

            }

            return Users;
        }

        public async Task<IdentityRole> GetById(string Id)
        {
            try
            {
                var role = await roleManager.FindByIdAsync(Id);
                return role;
            }
            catch (Exception)
            {
                return new IdentityRole()
                {
                    Name = null,
                    NormalizedName = null
                };
            }
        }

        public async Task<IdentityRole> Delete(IdentityRole model)
        {
            var role = await roleManager.FindByIdAsync(model.Id);

            await roleManager.DeleteAsync(role);
            return role;
        }

        public async Task EditUserInRole(List<UserInRoleVM> model, string RoleId)
        {
            var role = await roleManager.FindByIdAsync(RoleId);

            for (int i = 0; i < model.Count; i++)
            {
                var user = await userManager.FindByIdAsync(model[i].UserId);


                if (model[i].IsSelected && !(await userManager.IsInRoleAsync(user, role.Name)))
                {
                    await userManager.AddToRoleAsync(user, role.Name);
                }
                else if (model[i].IsSelected == false && (await userManager.IsInRoleAsync(user, role.Name)))
                {
                    await userManager.RemoveFromRoleAsync(user, role.Name);
                }
                else
                {
                    continue;
                }

                if (i < model.Count)
                    continue;
            }
        }
        #endregion

    }
}
