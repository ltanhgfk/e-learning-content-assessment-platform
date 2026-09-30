using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class PostTagRepository
    {
        private LmpSystemEntities entity;
        public PostTagRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Tag Tag)
        {
            entity.Tags.Add(Tag);
        }
        public void Add(Post Tag)
        {
            entity.Posts.Add(Tag);
        }
        public IQueryable<Tag> GetAllTags()
        {
            IQueryable<Tag> query = entity.Tags;
            return query.AsQueryable();
        }
        public IQueryable<Post> GetAllPosts()
        {
            IQueryable<Post> query = entity.Posts;
            return query.AsQueryable();
        }
        //public void Delete(Tag post)
        //{
        //    entity.Tags.Remove(post);
        //}
        //public void Update(PostTag PostTag)
        //{
        //    entity.Entry(PostTag).State = EntityState.Modified;
        //}
        public Tag FindTagByID(int id)
        {
            return entity.Tags.Find(id);
        }
        public Post FindPostByID(int id)
        {
            return entity.Posts.Find(id);
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