using LmpSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace LmpSystem.Repository
{
    public class UserScoreRepository
    {
        private readonly LmpSystemEntities entity;// = new QUANLYCONGVIECEntities();
        public UserScoreRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(UserScore UserScore)
        {
            entity.UserScores.Add(UserScore);
        }

        public UserScore FindByID(int id)
        {
            UserScore u = entity.UserScores.Find(id);
            return u;
        }
        public IQueryable<UserScore> GetAll()
        {
            IQueryable<UserScore> query = entity.UserScores;
            return query.AsQueryable();
        }
        public void Delete(UserScore UserScore)
        {
            entity.UserScores.Remove(UserScore);
        }
        public void Update(UserScore u)
        {
            entity.Entry(u).State = EntityState.Modified;
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