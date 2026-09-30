using LmpSystem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace LmpSystem.Repository
{
    public class LmpInfoRepository
    {
        private readonly LmpSystemEntities entity;// = new LmpSystemEntities();
        public LmpInfoRepository(LmpSystemEntities context)
        {
            this.entity = context;
        }

        public IQueryable<LmpInfo> GetAll()
        {
            IQueryable<LmpInfo> query = entity.LmpInfoes;
            return query.AsQueryable();
        }

        public void Delete(LmpInfo LmpInfo)
        {
            //entity.Tbl_Tags.Remove(Feedback.Tbl_Tags);
            entity.LmpInfoes.Remove(LmpInfo);
        }

        public LmpInfo FindByID(int id = 1)
        {
            LmpInfo u = entity.LmpInfoes.Find(id);
            return u;
        }

        public LmpInfo GetLmpInfo(int id = 1)
        {
            LmpInfo u = entity.LmpInfoes.Find(id);
            return u;
        }

        public LmpInfo GetLmpInfoByCode(string code = "", bool status = true)
        {
            LmpInfo u = entity.LmpInfoes.Where(m => m.Code == code && m.Status == status).Take(1).SingleOrDefault();
            return u;
        }

        public void Update(LmpInfo u)
        {
            entity.Entry(u).State = EntityState.Modified;
        }

        public void Add(LmpInfo info)
        {            
            entity.LmpInfoes.Add(info);
        }
    }
}