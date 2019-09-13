using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class AboutDao
    {
        BookShopOnlDbContext db=null;
    public AboutDao()
    {
        db = new BookShopOnlDbContext();
    }
        public About GetByID(long id)
    {
        return db.Abouts.Find(id);
    }
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.AboutTags
                         on a.ID equals b.TagID
                         where b.AboutID == contentId
                         select new
                         {
                             ID = b.TagID,
                             Name = a.Name
                         }).AsEnumerable().Select(x => new Tag()
                         {
                             ID = x.ID,
                             Name = x.Name
                         });
            return model.ToList();
        }
        public long Insert(About entity)
        {
            db.Abouts.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(About entity)
        {
            try
            {
                var about = db.Abouts.Find(entity.ID);
                about.Name = entity.Name;
                about.MetaTitle = entity.MetaTitle;
                about.Discriptions = entity.Discriptions;
                about.Images = entity.Images;
                about.Detail = entity.Detail;
                about.CreateDate=DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
            
        }
        public IEnumerable<About> ListAllpaging(String searchString, int page, int pageSize)
        {
            IQueryable<About> model = db.Abouts;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Discriptions.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        //public User GetByID(String UserName)
        //{
        //    return db.Users.SingleOrDefault(x => x.UserName==UserName);
        //}
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }
        public IEnumerable<About> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Abouts
                         join b in db.AboutTags
                         on a.ID equals b.AboutID
                         where b.TagID == tag
                         select new
                         {
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Image = a.Images,
                             Description = a.Discriptions,
                             CreatedDate = a.CreateDate,
                             CreatedBy = a.CreateBy,
                             ID = a.ID

                         }).AsEnumerable().Select(x => new About()
                         {
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Images = x.Image,
                             Discriptions = x.Description,
                             CreateDate = x.CreatedDate,
                             CreateBy = x.CreatedBy,
                             ID = x.ID
                         });
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public About ViewDetail(long id)
        {
            return db.Abouts.Find(id);
        }
       
        public bool Delete(int id)
        {
            try
            {
                var about = db.Abouts.Find(id);
                db.Abouts.Remove(about);
                db.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
            
        }
    }
}
