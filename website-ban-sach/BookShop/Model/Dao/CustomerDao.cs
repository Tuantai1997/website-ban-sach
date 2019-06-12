using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model.EF;
using PagedList;

namespace Model.Dao
{
    public class CustomerDao
    {
        
        BookShopOnlDbContext db=null;
    public CustomerDao()
    {
        db = new BookShopOnlDbContext();
    }
        public long Insert(Customer entity)
        {
            try { 
                using (db)
                {
                    db.Customers.Add(entity);
                    db.SaveChanges();
                    return entity.ID;
                }
            }
            catch (DbEntityValidationException ex)
            {
                // Retrieve the error messages as a list of strings.
                var errorMessages = ex.EntityValidationErrors
                .SelectMany(x => x.ValidationErrors)
                .Select(x => x.ErrorMessage);

                // Join the list to a single string.
                var fullErrorMessage = string.Join("; ", errorMessages);

                // Combine the original exception message with the new one.
                var exceptionMessage = string.Concat(ex.Message, " The validation errors are: ", fullErrorMessage);

                // Throw a new DbEntityValidationException with the improved exception message.
                throw new DbEntityValidationException(exceptionMessage, ex.EntityValidationErrors);
            }

        }
        public long InsertForFacebook(Customer entity)
        {
            var user = db.Customers.SingleOrDefault(x => x.UserName == entity.UserName);
            if (user == null)
            {
                db.Customers.Add(entity);
                db.SaveChanges();
                return entity.ID;
            }
            else
            {
                return user.ID;
            }

        }
        public bool Update(Customer entity)
        {
            try
            {
                var customer = db.Customers.Find(entity.ID);
                customer.Name = entity.Name;
                customer.Phone = entity.Phone;
                customer.Address = entity.Address;
                customer.Email = entity.Email;
                customer.CreateDate = DateTime.Now;
                db.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
            
        }
        public IEnumerable<Customer> ListAllpaging(String searchString, int page, int pageSize)
        {
            IQueryable<Customer> model = db.Customers;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString) || x.Email.Contains(searchString));
            }
            return model.OrderByDescending(x => x.CreateDate).ToPagedList(page, pageSize);
        }
        public Customer GetById(string userName)
        {
            return db.Customers.SingleOrDefault(x => x.UserName == userName);
        }
        public Customer ViewDetail(long id)
        {
            return db.Customers.Find(id);
        }
       
        public bool Delete(int id)
        {
            try
            {
                var customer = db.Customers.Find(id);
                db.Customers.Remove(customer);
                db.SaveChanges();
                return true;
            }
            catch (Exception )
            {
                return false;
            }
            
        }
        public int Login(string userName, string passWord)
        {
            var result = db.Customers.SingleOrDefault(x => x.UserName == userName);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if (result.status == false)
                {
                    return -1;
                }
                else
                {
                    if (result.Password == passWord)
                        return 1;
                    else
                        return -2;
                }
            }
        }
        public bool CheckUserName(string userName)
        {
            return db.Customers.Count(x => x.UserName == userName) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Customers.Count(x => x.Email == email) > 0;
        }
        public bool ChangeStatus(long id)
        {
            var user = db.Customers.Find(id);
            user.status = !user.status;
            db.SaveChanges();
            return user.status;
        }
    }
}
