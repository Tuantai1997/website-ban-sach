using Model.Dao;
using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace BookShop.Controllers
{
    public class ContactController : Controller
    {
        // GET: Contact
        public ActionResult Index()
        {
            var model = new ContactDao().GetActiveContact();
            return View(model);
        }

        [HttpPost]
        public JsonResult Send(string name, string mobile, string address, string email, string content)
        {
            var feedback = new FeedBack();
            feedback.Name = name;
            feedback.Email = email;
            feedback.CreateDate = DateTime.Now;
            feedback.Phone = mobile;
            feedback.Content = content;
            feedback.Address = address;
            if(feedback.Name != "" && feedback.Email != "" && feedback.Phone != "" && feedback.Content != "" && feedback.Address != "")
            {
                feedback.status = true;
                var id = new ContactDao().InsertFeedBack(feedback);
                if (id > 0)
                {
                    return Json(new
                    {

                        //status = true
                    });
                    //send mail
                }
                    return Json(new
                    {
                        //status = false
                    });
            }
            return Json(new { });
        }
    }
}