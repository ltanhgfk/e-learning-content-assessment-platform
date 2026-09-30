using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class LessonRepository
    {
        private LmpSystemEntities entity;
        public LessonRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Lesson Lesson)
        {
            entity.Lessons.Add(Lesson);
        }
        public IQueryable<Lesson> GetAll()
        {
            IQueryable<Lesson> query = entity.Lessons;
            return query.AsQueryable();
        }
        public void Delete(Lesson post)
        {
            entity.Lessons.Remove(post);
        }
        public void Update(Lesson Lesson)
        {
            entity.Entry(Lesson).State = EntityState.Modified;
        }
        public Lesson FindByID(int id)
        {
            return entity.Lessons.Find(id);
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