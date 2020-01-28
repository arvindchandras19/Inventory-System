using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Inventory.Common
{
    public class CommonLibrary:System.Web.UI.Page
    {
        public void IsValidContext()
        {

        }
        public void isValidSiteMasterContext()
        {
            if (Session["UserName"] == null)
            {
                Response.Redirect("Default.aspx");
            }

        }
    }
}