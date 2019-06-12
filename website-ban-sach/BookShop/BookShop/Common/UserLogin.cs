using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Model.EF;

namespace BookShop
{
    [Serializable]
    public class UserLogin
    {
        
        public long UserID { set; get; }
        public string UserName { set; get; }
        //public string GroupID { set; get; }
        public bool Status { set; get; }
    }
}