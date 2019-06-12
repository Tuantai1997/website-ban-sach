using BotDetect.Web.UI.Mvc;
using Facebook;
using Model.Dao;
using Model.EF;
using BookShop.Common;
using BookShop.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Xml.Linq;

namespace BookShop.Controllers
{
    public class UserController : Controller
    {
        private Uri RedirectUri
        {
            get
            {
                var uriBuilder = new UriBuilder(Request.Url);
                uriBuilder.Query = null;
                uriBuilder.Fragment = null;
                uriBuilder.Path = Url.Action("FacebookCallback");
                return uriBuilder.Uri;
            }
        }

        // GET: User
        [HttpGet]
        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }
        public ActionResult LoginFacebook()
        {
            var fb = new FacebookClient();
            var loginUrl = fb.GetLoginUrl(new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                response_type = "code",
                scope = "email",
            });
            return Redirect(loginUrl.AbsoluteUri);
        }

        public ActionResult FacebookCallback(string code)
        {
            var fb = new FacebookClient();
            dynamic result = fb.Post("oauth/access_token", new
            {
                client_id = ConfigurationManager.AppSettings["FbAppId"],
                client_secret = ConfigurationManager.AppSettings["FbAppSecret"],
                redirect_uri = RedirectUri.AbsoluteUri,
                code = code
            });


            var accessToken = result.access_token;
            if (!string.IsNullOrEmpty(accessToken))
            {
                fb.AccessToken = accessToken;
                // Get the user's information, like email, first name, middle name etc
                dynamic me = fb.Get("me?fields=first_name,middle_name,last_name,id,email");
                string email = me.email;
                string userName = me.email;
                string firstname = me.first_name;
                string middlename = me.middle_name;
                string lastname = me.last_name;

                var user = new Customer();
                user.Email = email;
                user.UserName = email;
                user.status = true;
                user.Name = firstname + " " + middlename + " " + lastname;
                user.CreateDate = DateTime.Now;
                var resultInsert = new CustomerDao().InsertForFacebook(user);
                if (resultInsert > 0)
                {
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    Session.Add(CommonConstants.USER_SESSION, userSession);
                }
            }
            return Redirect("/");
        }
        public ActionResult Logout()
        {
            Session[CommonConstants.USER_SESSION] = null;
            Session[CommonConstants.CartSession] = null;
            //Session.Add(CartController.CartSession, "CartSession");
            return Redirect("/");
        }
        [HttpPost]
        public ActionResult Login(LoginModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDao();
                var result = dao.Login(model.UserName, Encryptor.MD5Hash(model.Password));
                if (result == 1)
                {
                    var user = dao.GetById(model.UserName);
                    //khi đăng nhập thành công, lưu thông tin đăng nhập vào Session
                    var userSession = new UserLogin();
                    userSession.UserName = user.UserName;
                    userSession.UserID = user.ID;
                    userSession.Status = user.status;
                    Session[CartController.CartSession] = null;
                    Session.Add(CommonConstants.USER_SESSION, userSession);

                    return Redirect("/");
                }
                else if (result == 0)
                {
                    ModelState.AddModelError("", "Tài khoản không tồn tại.");
                }
                else if (result == -1)
                {
                    ModelState.AddModelError("", "Tài khoản đang bị khoá.");
                }
                else if (result == -2)
                {
                    ModelState.AddModelError("", "Mật khẩu không đúng.");
                }
                else
                {
                    ModelState.AddModelError("", "đăng nhập không đúng.");
                }
            }
            return View(model);
        }
        [HttpPost]
        [CaptchaValidation("CaptchaCode", "registerCapcha", "Mã xác nhận không đúng!")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                var dao = new CustomerDao();
                if (dao.CheckUserName(model.UserName))
                {
                    ModelState.AddModelError("", "Tên đăng nhập đã tồn tại");
                }
                else if (dao.CheckEmail(model.Email))
                {
                    ModelState.AddModelError("", "Email đã tồn tại");
                }
                else
                {
                    var user = new Customer();
                    user.UserName = model.UserName;
                    user.Name = model.Name;
                    user.Password = Encryptor.MD5Hash(model.Password);
                    user.Phone = model.Phone;
                    user.Email = model.Email;
                    user.Address = model.Address;
                    user.CreateDate = DateTime.Now;
                    user.status = true;

                    var result = dao.Insert(user);
                    if (result > 0)
                    {
                        ViewBag.Success = "Đăng ký thành công";
                        model = new RegisterModel();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Đăng ký không thành công.");
                    }
                }
            }
            return RedirectToAction("Login","User");
        }
        public ActionResult UserProfile()
        {
            var session = (UserLogin)Session[BookShop.Common.CommonConstants.USER_SESSION];
            var dao = new CustomerDao();
            var userProfile = new UserProfile();
            try
            {
                var currentUser = dao.GetById(session.UserName);
                userProfile.UserName = currentUser.UserName;
                userProfile.Name = currentUser.Name;
                userProfile.OldPassword = currentUser.Password;
                userProfile.Phone = currentUser.Phone;
                userProfile.Email = currentUser.Email;
                userProfile.Address = currentUser.Address;
                return View(new UserProfile
                {
                    UserName = userProfile.UserName,
                    Name = userProfile.Name,
                    Phone = userProfile.Phone,
                    Email = userProfile.Email,
                    Address = userProfile.Address
                });
            }catch(Exception e)
            {
                return View();
            }
            
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult UserProfile(UserProfile model)
        {
            var session = (UserLogin)Session[BookShop.Common.CommonConstants.USER_SESSION];
            var customerDao = new CustomerDao();
            try
            {
                if (ModelState.IsValid)
                {
                    var currentUser = customerDao.GetById(model.UserName);
                    if (currentUser != null)
                    {
                        currentUser.Name = model.Name;
                        currentUser.Phone = model.Phone;
                        currentUser.Email = model.Email;
                        currentUser.Address = model.Address;

                        var oldPassword = Encryptor.MD5Hash(model.OldPassword);
                        var newPassword = Encryptor.MD5Hash(model.NewPassword);
                        var currentPassword = currentUser.Password;
                        var confirmPassword = Encryptor.MD5Hash(model.ConfirmPassword);
                        if (newPassword != oldPassword && oldPassword != null && newPassword != null && currentPassword == oldPassword
                            && newPassword == confirmPassword)
                        {
                            currentUser.Password = newPassword;
                            bool result = customerDao.Update(currentUser);
                            if (result == true)
                            {
                                ViewBag.Success("Cập nhật tài khoản thành công");
                                model = new UserProfile();
                                return View(model);
                            }
                            else
                            {
                                ModelState.AddModelError("", "Cập nhật tài khoản không thành công.");
                            }
                        }                          
                        else
                        {
                            if (oldPassword == null)
                                ModelState.AddModelError("", "Vui lòng nhập mật khẩu cũ");
                            if (currentPassword != oldPassword)
                            {
                                ModelState.AddModelError("", "Mật khẩu sai,vui lòng nhập lại");
                            }
                            if (model.NewPassword == null)
                            {
                                ModelState.AddModelError("", "Vui lòng nhập mật khẩu mới");
                            }
                            if (oldPassword == newPassword)
                            {
                                ModelState.AddModelError("", "Mật khẩu mới không được giống mật khẩu cũ");
                            }
                            if(newPassword != confirmPassword)
                            {
                                ModelState.AddModelError("", "Xác nhận mật khẩu không đúng, vui lòng nhập lại");
                            }
                        }
                    }
                    
                }
            }catch(Exception ex)
            {
                return View(model);
            }
            return View(model);
        }
    }
}