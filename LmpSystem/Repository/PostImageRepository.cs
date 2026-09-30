using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class PostImageRepository
    {
        private LmpSystemEntities entity;
        public PostImageRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(PostImage PostImage)
        {
            entity.PostImages.Add(PostImage);
        }
        public IQueryable<PostImage> GetAll()
        {
            IQueryable<PostImage> query = entity.PostImages;
            return query.AsQueryable();
        }
        public void Delete(PostImage post)
        {
            entity.PostImages.Remove(post);
        }
        public void DeleteByPost(int postId)
        {
            entity.PostImages.RemoveRange(entity.PostImages.Where(m => m.PostId == postId));
        }
        public void Update(PostImage PostImage)
        {
            entity.Entry(PostImage).State = EntityState.Modified;
        }
        public PostImage FindByID(int id)
        {
            return entity.PostImages.Find(id);
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