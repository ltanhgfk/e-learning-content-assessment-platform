using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class OrderRepository
    {
        private LmpSystemEntities entity;
        public OrderRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Order Order)
        {
            entity.Orders.Add(Order);
        }
        public IQueryable<Order> GetAll()
        {
            IQueryable<Order> query = entity.Orders;
            return query.AsQueryable();
        }
        public void Delete(Order post)
        {
            entity.Orders.Remove(post);
        }
        public void Update(Order Order)
        {
            entity.Entry(Order).State = EntityState.Modified;
        }
        public Order FindByID(int id)
        {
            return entity.Orders.Find(id);
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