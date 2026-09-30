using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class PostReviewRepository
    {
        private LmpSystemEntities entity;
        public PostReviewRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(PostReview PostReview)
        {
            entity.PostReviews.Add(PostReview);
        }
        public IQueryable<PostReview> GetAll()
        {
            IQueryable<PostReview> query = entity.PostReviews;
            return query.AsQueryable();
        }
        public void Delete(PostReview post)
        {
            entity.PostReviews.Remove(post);
        }
        public void DeleteByPost(int postId)
        {
            entity.PostReviews.RemoveRange(entity.PostReviews.Where(m => m.PostId == postId));
        }
        public void Update(PostReview PostReview)
        {
            entity.Entry(PostReview).State = EntityState.Modified;
        }
        public PostReview FindByID(int id)
        {
            return entity.PostReviews.Find(id);
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