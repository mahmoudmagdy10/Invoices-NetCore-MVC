using Invoices.BL.Models;
using Invoices.DAL.Extend;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Invoices.BL.Interface
{
    public interface IUserRep
    {
        IEnumerable<ApplicationUser> Get();
        Task<ApplicationUser> GetById(string id);
        Task<ApplicationUser> Create(RegisterVM obj);
        Task<ApplicationUser> Edit(ApplicationUser obj);
        Task<ApplicationUser> Delete(ApplicationUser obj);
    }
}
