using BookShop.Common;
using BookShop.Models;
using Common;
using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.IO;

namespace BookShop.Controllers
{
    public class CartController : Controller
    {
        public const string CartSession = "CartSession";// key này là 1 hằng số, ko thể thay đổi
        //
        // GET: /Cart/
        public ActionResult Index()
        {
            var session = (UserLogin)Session[BookShop.Common.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var cart_user = Session[CommonConstants.CartSession];
                var list_user = new List<CartItem>();
                if (cart_user != null) //kiểm tra giỏ hàng đã có sp
                {
                    list_user = (List<CartItem>)cart_user;
                }
                return View(list_user);
            }
            var cart = Session[CartSession];
            var list = new List<CartItem>();
            if (cart != null) //kiểm tra giỏ hàng đã có sp
            {
                list = (List<CartItem>)cart;
            }
            return View(list);
        }
        public ActionResult AddItem(long bookId, int quantity, string username)
        {
            var book = new BookDao().ViewDetail(bookId);
            var session = (UserLogin)Session[BookShop.Common.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var cart_user = Session[CommonConstants.CartSession]; //kiểm tra key của session
                if (cart_user != null)// đã có key và username
                {
                    var list_user = (List<CartItem>)cart_user;  //ép kiểu của Cart
                    if (list_user.Exists(x => x.Book.ID == bookId)) // th giỏ hàng đã có sách vừa chọn
                    {
                        foreach (var item in list_user)
                        {
                            item.UserName = username;
                            if (item.Book.ID == bookId)
                            {
                                item.Quantity += quantity;
                            }
                        }
                    }
                    else // nếu chưa có, thì add mới sách vào list
                    {
                        //tạo mới đối tượng cart item
                        var item = new CartItem();
                        item.UserName = username;
                        item.Quantity = quantity;
                        item.Book = book;
                        list_user.Add(item);
                    }
                    //gán vào session
                    Session[CommonConstants.CartSession] = list_user;
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Quantity = quantity;
                    item.Book = book;
                    item.UserName = username;
                    var list = new List<CartItem>();
                    list.Add(item);
                    //gán dl vào session
                    Session[CommonConstants.CartSession] = list;

                }
            }
            else
            {
                var cart = Session[CartSession]; //kiểm tra key của session
                if (cart != null)// đã có key
                {
                    var list = (List<CartItem>)cart;  //ép kiểu của Cart
                    if (list.Exists(x => x.Book.ID == bookId)) // th giỏ hàng đã có sách vừa chọn
                    {
                        foreach (var item in list)
                        {
                            if (item.Book.ID == bookId)
                            {
                                item.Quantity += quantity;
                            }
                        }
                    }
                    else // nếu chưa có, thì add mới sách vào list
                    {
                        //tạo mới đối tượng cart item
                        var item = new CartItem();
                        item.Quantity = quantity;
                        item.Book = book;
                        list.Add(item);
                    }
                    //gán vào session
                    Session[CartSession] = list;
                }
                else
                {
                    //tạo mới đối tượng cart item
                    var item = new CartItem();
                    item.Quantity = quantity;
                    item.Book = book;

                    var list = new List<CartItem>();
                    list.Add(item);
                    //gán dl vào session
                    Session[CartSession] = list;

                }                
            }
            return RedirectToAction("Index");
        }
        
        
        public JsonResult Delete(long id)//id lấy ở ajax truyền lên
        {
            var sessionCart = (List<CartItem>)Session[CartSession];
            sessionCart.RemoveAll(x => x.Book.ID == id);
            Session[CartSession] = sessionCart;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult DeleteAll()
        {
            Session[CartSession] = null;
            return Json(new
            {
                status = true
            });
        }
        public JsonResult Update(String cartModel)
        {
            var jsonCart = new JavaScriptSerializer().Deserialize<List<CartItem>>(cartModel);
            var sessionCart = (List<CartItem>)Session[CartSession];
           
                foreach (var item in sessionCart)
                {
                    var jsonItem = jsonCart.SingleOrDefault(x => x.Book.ID == item.Book.ID);
                    if (jsonItem != null)
                    {
                        item.Quantity = jsonItem.Quantity;
                    }
                }
                Session[CartSession] = sessionCart;//cập nhật lại cart
            

            return Json(new
            {
                status = true
            });
        }
        
        [HttpGet]
        public ActionResult Payment()
        {
            var list = new List<CartItem>();
            var session = (UserLogin)Session[BookShop.Common.CommonConstants.USER_SESSION];
            if (session != null)
            {
                var cusDao = new CustomerDao();
                var customer = cusDao.GetById(session.UserName);
                ViewBag.ShipName = customer.Name;
                ViewBag.ShipPhone = customer.Phone;
                ViewBag.ShipAddress = customer.Address;
                ViewBag.ShipEmail = customer.Email;
            }
                var cart = Session[CartSession];
                ViewBag.ngaymua = DateTime.Now;
                int tien = 0;
            if (cart != null)
            {
                list = (List<CartItem>)cart;
                foreach (var item in list)
                {
                    tien += Convert.ToInt32(item.Book.Price.GetValueOrDefault(0) * item.Quantity);
                }
                ViewBag.tongtien = tien;
            }
            return View(list);
        }
        [HttpPost]
        public ActionResult Payment(string shipname, string address, string mobile, string email)
        {
            var order = new Order();
            order.CreateDate = DateTime.Now;
            order.ShipAddress = address;
            order.ShipPhone = mobile;
            order.ShipName = shipname;
            order.ShipEmail = email;
            if (order.ShipAddress != null && order.ShipPhone != null && order.ShipName != null &&
                order.ShipAddress != "" && order.ShipPhone != "" && order.ShipName != "")
            {
                var id = new OrderDao().Insert(order);
                var cart = (List<CartItem>)Session[CartSession];
                var detailDao = new Model.Dao.OrderDetailDao();
                decimal total = 0;
                foreach (var item in cart)
                {
                    var orderdetail = new OrderDetail();
                    orderdetail.BookID = item.Book.ID;
                    orderdetail.OrderID = id;
                    orderdetail.Price = item.Book.Price;
                    orderdetail.Quantity = item.Quantity;
                    detailDao.Insert(orderdetail);                    

                    total += (item.Book.Price.GetValueOrDefault(0) * item.Quantity);
                    Session[CommonConstants.CartSession] = null;
                }
                string content = System.IO.File.ReadAllText(Server.MapPath("~/assets/client/template/neworder.html"));

                content = content.Replace("{{CustomerName}}", shipname);
                content = content.Replace("{{Phone}}", mobile);
                content = content.Replace("{{Email}}", email);
                content = content.Replace("{{Address}}", address);
                content = content.Replace("{{Total}}", total.ToString("N0"));
                var toEmail = ConfigurationManager.AppSettings["ToEmailAddress"].ToString();

                new MailHelper().SendMail(email, "Đơn hàng mới từ OnlineShop", content);
                new MailHelper().SendMail(toEmail, "Đơn hàng mới từ OnlineShop", content);
            }
            else
            {
                return Redirect("/loi-mua-hang");
            }
            
            return Redirect("/hoan-thanh");
        }
        public ActionResult Success()
        {
            return View();
        }
        public ActionResult Fail()
        {
            return View();
        }
    }
}