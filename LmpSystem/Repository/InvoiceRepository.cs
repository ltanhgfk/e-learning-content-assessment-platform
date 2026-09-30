using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class InvoiceRepository
    {
        private LmpSystemEntities entity;
        public InvoiceRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Invoice Invoice)
        {
            entity.Invoices.Add(Invoice);
        }
        public IQueryable<Invoice> GetAll()
        {
            IQueryable<Invoice> query = entity.Invoices;
            return query.AsQueryable();
        }
        public void Delete(Invoice post)
        {
            entity.Invoices.Remove(post);
        }
        public void Update(Invoice Invoice)
        {
            entity.Entry(Invoice).State = EntityState.Modified;
        }
        public Invoice FindByID(int id)
        {
            return entity.Invoices.Find(id);
        }
        public void SaveChanges()
        {
            entity.SaveChanges();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    entity.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}