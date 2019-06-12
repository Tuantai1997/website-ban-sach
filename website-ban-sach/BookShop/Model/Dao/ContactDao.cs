using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContactDao
    {
        BookShopOnlDbContext db = null;
        public ContactDao()
        {
            db = new BookShopOnlDbContext();
        }

        public Contact GetActiveContact()
        {
            return db.Contacts.Single(x => x.Status == true);
        }

        public int InsertFeedBack(FeedBack fb)
        {
            db.FeedBacks.Add(fb);
            db.SaveChanges();
            return fb.ID;
        }
    }
}
