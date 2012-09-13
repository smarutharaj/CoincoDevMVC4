using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Collections;
using System.Web.Mvc;
using StructureMap;
using Coinco.SMS.AXWrapper;

namespace Coinco.SMS.Website.Models
{
    public class Site
    {
        public string Username { get; set; }
        public string SiteID { get; set; }
        public string SiteName { get; set; }
        public string IsDefault { get; set; }
        public SelectList SiteList { get; set; }
        public Site()
        {
        }


        public Site(string userName, string siteID, string siteName, string isDefault)
        {
            this.Username = userName;
            this.SiteID = siteID;
            this.SiteName = siteName;
            this.IsDefault = isDefault;
        }

        //- To get the GetSites  for sites and Service Order Process Page 

        public IEnumerable<Site> GetSitesListByUsername(string userName)
        {
            List<Site> siteList = new List<Site> { };
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();

            try
            {

                DataTable resultTable = axHelper.GetDefaultSitesByUsername(userName);

                foreach (DataRow row in resultTable.Rows)
                {
                    Site currentSite = new Site();
                    currentSite.Username = row["User"].ToString();
                    currentSite.SiteID = row["Sites"].ToString();
                    currentSite.SiteName = row["SitesName"].ToString();
                    currentSite.IsDefault = row["Default"].ToString();
                    siteList.Add(currentSite);
                }
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return siteList.AsEnumerable<Site>();
        }
    }


}