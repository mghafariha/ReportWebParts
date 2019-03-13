using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Newtonsoft.Json;
using Microsoft.SharePoint;
using System.DirectoryServices.AccountManagement;
using Microsoft.SharePoint.Utilities;
namespace Reporting.TableWP
{
    public partial class TableWPUserControl : UserControl
    {

        // Fields
        protected Literal Literal1;

        // Methods
        private object getSpData(string spName)
        {
            SPQuery query;
            SPWeb web = SPContext.Current.Web;
            string[] strArray = spName.Split(new char[] { ',' });
            object[] parameterValues = new object[strArray.Length - 1];
            string sPName = strArray[0].ToString();

            SPGroup gpAll = web.SiteGroups.GetByID(345);
            SPGroup gpArea1 = web.SiteGroups.GetByID(346);
            SPGroup gpArea2 = web.SiteGroups.GetByID(347);
            SPGroup gpArea3 = web.SiteGroups.GetByID(348);
            SPGroup gpArea4 = web.SiteGroups.GetByID(349);
            SPGroup gpArea5 = web.SiteGroups.GetByID(350);
            SPGroup gpArea6 = web.SiteGroups.GetByID(351);
            //SPGroup gpAll = web.SiteGroups.GetByID(320);
            //SPGroup gpArea1 = web.SiteGroups.GetByID(321);
            //SPGroup gpArea2 = web.SiteGroups.GetByID(321);
            //SPGroup gpArea3 = web.SiteGroups.GetByID(321);
            //SPGroup gpArea4 = web.SiteGroups.GetByID(321);
            //SPGroup gpArea5 = web.SiteGroups.GetByID(321);
            //SPGroup gpArea6 = web.SiteGroups.GetByID(321);
            if (strArray[1].ToString().ToUpper() == "NULL")
            {
                if (gpAll.ContainsCurrentUser)
                    strArray[1] = "NULL";
                else if (gpArea1.ContainsCurrentUser)
                    strArray[1] = "1";
                else if (gpArea2.ContainsCurrentUser)
                    strArray[1] = "2";
                else if (gpArea3.ContainsCurrentUser)
                    strArray[1] = "3";
                else if (gpArea4.ContainsCurrentUser)
                    strArray[1] = "4";
                else if (gpArea5.ContainsCurrentUser)
                    strArray[1] = "5";
                else if (gpArea6.ContainsCurrentUser)
                    strArray[1] = "6";
                else
                    return null;
            }





            //SPUser currentUser = web.CurrentUser;

            ////var domainName = "jnasr";
            ////var queryUser = "nasr\\SPS_Farm_Prd";
            ////var queryUserPassword = "rnk@Qk7fqH";

            //string str = "";
            //var domainName = "nasr2";
            //var queryUser = "nasr2\\spadmin";
            //var queryUserPassword = "Nsr!dm$n!Sp";
            //var principalContext = new PrincipalContext(ContextType.Domain, domainName, queryUser, queryUserPassword);

            //GroupPrincipal managerPrincipal = GroupPrincipal.FindByIdentity(principalContext, "Epm-managers");
            //GroupPrincipal directorPrincipal = GroupPrincipal.FindByIdentity(principalContext, "Epm-Directors_pmis");
            //GroupPrincipal contractorPrincipal = GroupPrincipal.FindByIdentity(principalContext, "Epm-contractors");
            //GroupPrincipal engineerPrincipal = GroupPrincipal.FindByIdentity(principalContext, "Epm-engineers");

            //UserPrincipal user = UserPrincipal.FindByIdentity(principalContext, currentUser.LoginName);

            //string userdomain = "";
            //if (currentUser.LoginName.Contains("|") == true)
            //    userdomain = currentUser.LoginName.Split('|')[1].Split('\\')[0];
            //else
            //    userdomain = currentUser.LoginName;
            //if (strArray[1].ToString().ToUpper() == "NULL")
            //{
            //    if (userdomain != "nasr" && (user.IsMemberOf(managerPrincipal) || user.IsMemberOf(directorPrincipal)))
            //    {
            //        query = new SPQuery();
            //        SPListItem item = SPContext.Current.Web.GetList("/Lists/Areas").Items[0];
            //        parameterValues[0] = item.ID;
            //    }
            //    else
            //    {
            //        parameterValues[0] = DBNull.Value;
            //    }
            //}
            //else
            //{
            //    parameterValues[0] = int.Parse(strArray[1].ToString());
            //}
            //if (strArray[2].ToString().ToUpper() == "NULL")
            //{
            //    if (userdomain != "nasr" && (user.IsMemberOf(contractorPrincipal) || user.IsMemberOf(engineerPrincipal)))
            //    {

            //        query = new SPQuery();
            //        SPListItem item2 = SPContext.Current.Web.GetList("/Lists/Contracts").Items[0];
            //        parameterValues[1] = item2.ID;
            //    }
            //    else
            //    {
            //        parameterValues[1] = DBNull.Value;
            //    }
            //}
            //else
            //{
            //    parameterValues[0] = int.Parse(strArray[1].ToString());
            //}

            //if (strArray.Length > 3)
            //{
            for (int i = 0; i < strArray.Length - 1; i++)
            {
                if (strArray[i + 1].ToString().ToUpper() == "NULL")
                {
                    parameterValues[i] = DBNull.Value;
                }
                else
                {
                    int result = 0;
                    if (int.TryParse(strArray[i + 1].ToString(), out result))
                    {
                        parameterValues[i] = result;
                    }
                    else
                    {
                        parameterValues[i] = strArray[i + 1].ToString();
                    }
                }
            }


            DataAccessBase base2 = new DataAccessBase();
            return JsonConvert.SerializeObject(base2.ReaderSp(sPName, parameterValues));
        }

        private bool IsMemberOfGroup(SPWeb web, string groupName)
        {
            bool flag = false;
            bool isMemberOfGroup = false;
            string siteUrl = web.Site.Url;
            SPSecurity.RunWithElevatedPrivileges(delegate
            {
                using (SPSite site = new SPSite(siteUrl))
                {
                    using (SPWeb web1 = site.OpenWeb())
                    {
                        SPPrincipalInfo[] infoArray = SPUtility.GetPrincipalsInGroup(web1, groupName, 0x3e8, out flag);
                        foreach (SPPrincipalInfo info in infoArray)
                        {
                            if (info.LoginName == web.CurrentUser.LoginName)
                            {
                                isMemberOfGroup = true;
                                return;
                            }
                        }
                    }
                }
            });
            return isMemberOfGroup;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!base.IsPostBack)
            {
                try
                {
                    this.Literal1.Text = string.Concat(new object[] { "<script>var columns=", this.webPart.Columns, " ;var Data=", this.getSpData(this.webPart.SpName), " ;var pageTitle='", this.webPart.Title, "'</script>" });
                }
                catch (Exception exception)
                {
                    this.Literal1.Text = "<script>console.log('error:'" + exception.Message + ") ;</script>";
                }
            }
        }

        // Properties
        public Reporting.TableWP.TableWP webPart { get; set; }
    }


}
