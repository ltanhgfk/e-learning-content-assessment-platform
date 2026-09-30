using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class CategoryRepository
    {
        private LmpSystemEntities entity;
        public CategoryRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Category Category)
        {
            entity.Categories.Add(Category);
        }
        public IQueryable<Category> GetAll()
        {
            IQueryable<Category> query = entity.Categories;
            return query.AsQueryable();
        }
        public void Delete(Category post)
        {
            entity.Categories.Remove(post);
        }
        public void Update(Category Category)
        {
            entity.Entry(Category).State = EntityState.Modified;
        }
        public Category FindByID(int id)
        {
            return entity.Categories.Find(id);
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