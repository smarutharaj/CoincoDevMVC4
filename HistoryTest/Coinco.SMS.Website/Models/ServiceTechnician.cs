using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Web.Mvc;
using StructureMap;
using Coinco.SMS.AXWrapper;

namespace Coinco.SMS.Website.Models
{
    public class ServiceTechnician
    {
        public string ServiceTechnicianName { get; set; }
        public string ServiceTechnicianNo { get; set; }

        public SelectList ServiceTechnicianList { get; set; } 
        public ServiceTechnician()
        {

        }

        public ServiceTechnician(string serviceTechnicianName, string serviceTechnicianNo)
        {
            this.ServiceTechnicianName = serviceTechnicianName;
            this.ServiceTechnicianNo = serviceTechnicianNo;
        }

        //- To get the GetTechnicians for Check In Page 

        public IEnumerable<ServiceTechnician> GetTechnicians(string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<ServiceTechnician> techniciansList = new List<ServiceTechnician>();
            try
            {
                DataTable resultTable = axHelper.GetTechnicians(userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    ServiceTechnician technicianObject = new ServiceTechnician();
                    technicianObject.ServiceTechnicianName = row["Name"].ToString();
                    technicianObject.ServiceTechnicianNo = row["Number"].ToString();

                    techniciansList.Add(technicianObject);

                }
            }
            catch (Exception ex)
            {
                throw ex;

            }
            return techniciansList.AsEnumerable<ServiceTechnician>();

        }

        //- To get the GetTechniciansParts for Service Order Process Page 

        public List<ServiceTechnician> GetTechniciansServiceOrderProcess(string transactionType, string specialityCode, string userName)
        {
            IAXHelper axHelper = ObjectFactory.GetInstance<IAXHelper>();
            List<ServiceTechnician> techniciansList = new List<ServiceTechnician>();
            try
            {
                DataTable resultTable = axHelper.GetTechniciansServiceOrderProcess(transactionType, specialityCode, userName);


                foreach (DataRow row in resultTable.Rows)
                {
                    ServiceTechnician technicianObject = new ServiceTechnician();
                    technicianObject.ServiceTechnicianName = row["Name"].ToString();
                    technicianObject.ServiceTechnicianNo = row["Number"].ToString();
                    techniciansList.Add(technicianObject);
                }

            }
            catch (Exception ex)
            {
                throw ex;

            }
            return techniciansList;

        }

    }
}