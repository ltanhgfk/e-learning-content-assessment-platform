using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace LmpSystem.Repository
{
    public class FeedbackRepository
    {
        private LmpSystemEntities entity;
        public FeedbackRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Feedback Feedback)
        {
            entity.Feedbacks.Add(Feedback);
        }
        public IQueryable<Feedback> GetAll()
        {
            IQueryable<Feedback> query = entity.Feedbacks;
            return query.AsQueryable();
        }

        public void Delete(Feedback Feedback)
        {
            //entity.Tbl_Tags.Remove(Feedback.Tbl_Tags);
            entity.Feedbacks.Remove(Feedback);
        }
        public void Update(Feedback Feedback)
        {
            entity.Entry(Feedback).State = EntityState.Modified;
        }
        public Feedback FindByID(int id)
        {
            return entity.Feedbacks.Find(id);
        }
        public Feedback FindBySlug(string slug)
        {
            return entity.Feedbacks.Where(m => m.FullName == slug).FirstOrDefault();
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