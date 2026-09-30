using LmpSystem.Models;
using System;
using System.Data.Entity;
using System.Linq;

namespace LmpSystem.Repository
{
    public class UserRepository
    {
        private readonly LmpSystemEntities entity;// = new QUANLYCONGVIECEntities();
        public UserRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(User user)
        {
            entity.Users.Add(user);
        }
        public User FindByUsername(string user)
        {
            User u = entity.Users.Where(x => x.Username.Equals(user)).FirstOrDefault();
            return u;
        }
        public User FindByID(int id)
        {
            User u = entity.Users.Find(id);
            return u;
        }
        public IQueryable<User> GetAll()
        {
            IQueryable<User> query = entity.Users;
            return query.AsQueryable();
        }
        public void Delete(User user)
        {
            entity.Users.Remove(user);
        }
        public void Update(User u)
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