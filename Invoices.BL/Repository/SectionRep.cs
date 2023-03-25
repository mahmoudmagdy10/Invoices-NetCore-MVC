using AutoMapper;
using Invoices.BL.Interface;
using Invoices.BL.Models;
using Invoices.DAL.Database;
using Invoices.DAL.Entity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Sections.BL.Repository
{
    public class SectionRep : ISectionRep
    {
        #region Fields
        
        private readonly InvoiceContext db;
        private readonly IMapper mapper;

        #endregion

        #region Ctor
        public SectionRep(InvoiceContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        #endregion

        #region Actions
        public IEnumerable<SectionVM> Get()
        {
            var Sections = GetSections();
            var data = mapper.Map<IEnumerable<SectionVM>>(Sections);
            return data;
        }


        public void Create(SectionVM obj)
        {
            var data = mapper.Map<Section>(obj);
            db.Section.Add(data);
            db.SaveChanges();
        }

        public void Delete(SectionVM obj)
        {
            var data = mapper.Map<Section>(obj);
            db.Section.Remove(data);
            db.SaveChanges();
        }

        public void Edit(SectionVM obj)
        {
            var data = mapper.Map<Section>(obj);
            db.Entry(data).State = EntityState.Modified;
            db.SaveChanges();
        }


        public Section GetById(int id)
        {
            var model = db.Section.Where(a => a.Id == id).FirstOrDefault();
            return model;
        }

        #endregion

        #region Refactory Methods
        private IQueryable<Section> GetSections()
        {
            return db.Section.Select(a => a);
        }

        #endregion
    }
}
