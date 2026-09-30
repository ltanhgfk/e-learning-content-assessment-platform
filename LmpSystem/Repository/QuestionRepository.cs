using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class QuestionRepository
    {
        private LmpSystemEntities entity;
        public QuestionRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }
        public void Add(Question Question)
        {
            entity.Questions.Add(Question);
        }
        public IQueryable<Question> GetAll()
        {
            IQueryable<Question> query = entity.Questions;
            return query.AsQueryable();
        }
        public void Delete(Question question)
        {
            entity.Questions.Remove(question);
        }
        public void DeleteByPost(int postId)
        {
            entity.Questions.RemoveRange(entity.Questions.Where(m => m.PostId == postId));
        }
        public void DeleteByLesson(int postId, int lessonId)
        {
            entity.Questions.RemoveRange(entity.Questions.Where(m => m.LessonId == lessonId && m.PostId == postId));
        }
        public void Update(Question Question)
        {
            entity.Entry(Question).State = EntityState.Modified;
        }
        public Question FindByID(int id)
        {
            return entity.Questions.Find(id);
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