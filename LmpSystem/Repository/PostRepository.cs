using LmpSystem.Models;
using LmpSystem.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class PostRepository
    {
        private LmpSystemEntities entity;
        public PostRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Post post)
        {
            entity.Posts.Add(post);
        }
        public IQueryable<Post> GetAll()
        {
            IQueryable<Post> query = entity.Posts;
            return query.AsQueryable();
        }
        
        public void Delete(Post post)
        {
            //entity.Tbl_Tags.Remove(post.Tbl_Tags);
            entity.Posts.Remove(post);           
        }
        public void Update(Post post)
        {
            entity.Entry(post).State = EntityState.Modified;
        }
        public Post FindByID(int id)
        {
            return entity.Posts.Find(id);
        }

        //public PostDetailViewModel GetPostDetailById(int id)
        //{
        //    return entity.Posts.SqlQuery();
        //}
        //public Post FindBySlug(string slug)
        //{
        //    return entity.Posts.Where(m => m.Name == slug).FirstOrDefault();
        //}
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