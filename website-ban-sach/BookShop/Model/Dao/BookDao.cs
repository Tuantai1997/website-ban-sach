﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using Model.ViewModel;
using PagedList;

namespace Model.Dao
{
    public class BookDao
    {
        BookShopOnlDbContext db = null;
        public BookDao()
        {
            db = new BookShopOnlDbContext();
        }
        public List<Book> ListAll(long id)
        {
           
            return db.Books.Where(x => x.status == true).OrderByDescending(x=>x.CreateDate).ToList();
        }

        public bool CheckName(string Name)
        {
            var book = (from s in db.Books
                      where s.status == true
                      select s);
            foreach (var item in book)
            {
                if (item.Name == Name)
                    return true;

            }
            return false;
        }
        public List<string> ListName(string keyword)
        {
            return db.Books.Where(x => x.Name.Contains(keyword)).Select(x => x.Name).ToList();
        }
        //public Book GetById(long bookId)
        //{
        //    return db.Books.SingleOrDefault(x => x.ID == bookId);
        //}
        public long Insert(Book entity)
        {
            db.Books.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(Book entity)
        {
            try
            {
                var book = db.Books.Find(entity.ID);
                book.Name = entity.Name;
                book.Images = entity.Images;
                book.Quantity = entity.Quantity;
                book.Price = entity.Price;
                book.PromotionPrice = entity.PromotionPrice;
                book.CategoryID = entity.CategoryID;
                book.AuthorID = entity.AuthorID;
                book.ProcderID = entity.ProcderID;
                book.CreateDate = DateTime.Now;
                book.status = entity.status;
                book.Discriptions = entity.Discriptions;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public IEnumerable<Book> ListAllpaging(String searchString, int page, int pageSize)
        {
            IQueryable<Book> model = db.Books;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
       
        
        public bool Delete(long id)
        {
            try
            {
                var book = db.Books.Find(id);
                db.Books.Remove(book);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public List<BookViewModel> Search(string search, ref int totalRecord, int pageIndex = 1, int pageSize = 2)
        {
            totalRecord = db.Books.Where(x => x.Name == search).Count();
            var model = (from a in db.Books
                         join b in db.BookCategories
                         on a.CategoryID equals b.ID
                         where a.Name.Contains(search)
                         select new
                         {
                             CateMetaTitle = b.MetaTitle,
                             CateName = b.Name,
                             CreatedDate = a.CreateDate,
                             ID = a.ID,
                             Images = a.Images,
                             Name = a.Name,
                             MetaTitle = a.MetaTitle,
                             Price = a.Price
                         }).AsEnumerable().Select(x => new BookViewModel()
                         {
                             CateMetaTitle = x.MetaTitle,
                             CateName = x.Name,
                             CreatedDate = x.CreatedDate,
                             ID = x.ID,
                             Images = x.Images,
                             Name = x.Name,
                             MetaTitle = x.MetaTitle,
                             Price = x.Price
                         });
            model.OrderByDescending(x => x.CreatedDate).Skip((pageIndex - 1) * pageSize).Take(pageSize);
            return model.ToList();
        }
        public List<Book> ListNewBook(int top)
        {
            return db.Books.OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }
        public List<Book> ListHotBook(int top)
        {
            return db.Books.Where(x => x.TopHot != null && x.TopHot>DateTime.Now).OrderByDescending(x => x.CreateDate).Take(top).ToList();
        }

       
        public List<Book> ListRelateBook(long productID)
        {
            var book = db.Books.Find(productID);
            return db.Books.Where(x => x.ID != productID && x.CategoryID==book.CategoryID).ToList();
        }

        public List<Book> ListByCategoryID(long categoryId)
        {
            return db.Books.Where(x => x.CategoryID == categoryId).ToList();
        }
        public List<Book> ListByAuthor(long AuthorId)
        {
            return db.Books.Where(x => x.AuthorID == AuthorId).ToList();
        }
        public List<Book> ListByProducer(long ProId)
        {
            return db.Books.Where(x => x.ProcderID == ProId).ToList();
        }
        public Book ViewDetail(long id)
        {
            return db.Books.Find(id);
        }

       
    }
}
