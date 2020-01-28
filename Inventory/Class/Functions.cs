using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Web;
using System.Web.UI;


namespace Inventory
{
    [DataContract]
    public class Functions
    {
        #region "JavaScript Messages"
        public void MessageDialog(Page sPage, string msg)
        {
            try
            {

                Notify(sPage, msg);
            }
            catch (Exception ex)
            {

            }

        }

        public void Notify(Page sPage, string msg)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append("<script type='text/javascript'>$.notify('" + msg + "')</script>");
                ScriptManager.RegisterStartupScript(sPage, sPage.GetType(), Guid.NewGuid().ToString(), sb.ToString(), false);
            }
            catch (Exception ex)
            {

            }
        }

        public void Warning(Page sPage, string msg)
        {
            try
            {
                string sb = string.Empty;
                sb = "<script type='text/javascript'>$.notify('" + msg + "', {'type' : 'warning'})</script>";
                ScriptManager.RegisterStartupScript(sPage, sPage.GetType(), Guid.NewGuid().ToString(), sb, false);
            }
            catch (Exception ex)
            {

            }
        }

        #endregion
    }
}