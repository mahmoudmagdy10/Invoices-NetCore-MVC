using Invoices.BL.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.BL.Interface
{
    public interface IRolesRep
    {
        IEnumerable<IdentityRole> Get();
        Task<IdentityRole> Create(IdentityRole model);
        //Task<IdentityRole> Edit(IdentityRole model);
        Task<List<UserInRoleVM>> AddOrRemoveUsers(string RoleId);
        Task EditUserInRole(List<UserInRoleVM> model, string RoleId);
        Task<IdentityRole> GetById(string Id);
        Task<IdentityRole> Delete(IdentityRole model);
    }
}
