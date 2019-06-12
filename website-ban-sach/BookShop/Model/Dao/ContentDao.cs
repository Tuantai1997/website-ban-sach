using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;

namespace Model.Dao
{
    public class ContentDao
    {
        BookShopOnlDbContext db = null;
    public ContentDao()
    {
        db = new BookShopOnlDbContext();
    }
    public Content GetByID(long id)
    {
        return db.Contents.Find(id);
    }
    public long Insert(Content entity)
    {
        db.Contents.Add(entity);
        db.SaveChanges();
        return entity.ID;
    }
    public IEnumerable<Content> ListAllPaging(int page, int pageSize)
    {
        IQueryable<Content> model = db.Contents;
        return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
    }
        public bool Update(Content entity)
    {
        try
        {
            var content = db.Contents.Find(entity.ID);
            content.Name = entity.Name;
            content.MetaTitle = entity.MetaTitle;
            content.Discriptions = entity.Discriptions;
            content.Images = entity.Images;
            content.Detail = entity.Detail;
            content.CreateDate = DateTime.Now;
            content.ModifiedDate = DateTime.Now;
            content.ModifiedBy = entity.ModifiedBy;
            content.status = entity.status;
            db.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
        public Tag GetTag(string id)
        {
            return db.Tags.Find(id);
        }   
        public IEnumerable<Content> ListAllpaging(String searchString, int page, int pageSize)
    {
        IQueryable<Content> model = db.Contents;
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
    public Content ViewDetail(long id)
    {
        return db.Contents.Find(id);
    }

    public bool Delete(int id)
    {
        try
        {
            var content = db.Contents.Find(id);
            db.Contents.Remove(content);
            db.SaveChanges();
            return true;
        }
        catch (Exception)
        {
            return false;
        }

    }
        public List<Tag> ListTag(long contentId)
        {
            var model = (from a in db.Tags
                         join b in db.ContentTags
                         on a.ID equals b.TagID
                         where b.ContentID == contentId
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
        public IEnumerable<Content> ListAllByTag(string tag, int page, int pageSize)
        {
            var model = (from a in db.Contents
                         join b in db.ContentTags
                         on a.ID equals b.ContentID
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

                         }).AsEnumerable().Select(x => new Content()
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
    }
}
