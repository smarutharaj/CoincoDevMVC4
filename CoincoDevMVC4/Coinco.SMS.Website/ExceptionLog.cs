using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using System.IO;

namespace Coinco.SMS.WCF
{
    public class ExceptionLog
    {
        public static void LogException(Exception exControllerException, string strUserName)    //
        {
            //******************************************************************************************************** 
            // Date : 20-Nov-2008 
            // Author : C.Subramanian 
            // Description : This procedure writes the error details to error log. 
            // 
            // Revision History : 
            // ------------------ 
            // 
            // Sl.no. Date          Author           Modification Reason 
            // ------ ----------- ----------- ------------------------------------------------- 
            // 1. 25-Apr-2012       Prem            Add User Name to the Exception Log.
            // 2. dd-mmm-yyyy 
            //********************************************************************************************************* 
            string strMessage = null;
            string strSource = null;
            string strStackTrace = null;
            string strFinalPath = null;

            //strMessage = exControllerException.Message;
            //strInnerException = exControllerException.InnerException.ToString();
            strSource = exControllerException.Source;
            strStackTrace = exControllerException.StackTrace;

            StreamWriter sw = default(StreamWriter);
            FileStream fw = default(FileStream);
            try
            {
                strMessage = System.Environment.NewLine + "--------------------------------------------------------------------------------------------------------------------------";
                strMessage = strMessage + System.Environment.NewLine + "An Error Has occured on SMS. The Error Details are";
                strMessage = strMessage + System.Environment.NewLine + " Login User : " + strUserName;
                strMessage = strMessage + System.Environment.NewLine + " Error Occured Date & Time : " + System.DateTime.Now;
                strMessage = strMessage + System.Environment.NewLine + " Error Message : " + exControllerException.Message + "";
                //strMessage = strMessage + System.Environment.NewLine + " Error InnerException : " + strInnerException + "";
                strMessage = strMessage + System.Environment.NewLine + " Error Source : " + strSource + "";
                strMessage = strMessage + System.Environment.NewLine + " Error Stack Trace : " + strStackTrace + "";
                strMessage = strMessage + System.Environment.NewLine + "-------------------------------------------------------------------------------------------------------------------------- ";
                strFinalPath = ConfigurationManager.AppSettings.Get("ErrorLogPath") + "\\SMSErrorLog" + ((string)(System.DateTime.Today.ToShortDateString())).Replace("/", "_") + ".txt";

                if (System.IO.Directory.Exists(ConfigurationManager.AppSettings.Get("ErrorLogPath")) == true)
                {
                    if (File.Exists(strFinalPath))
                    {

                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();
                    }
                    else
                    {
                        fw = File.Create(strFinalPath);
                        fw.Close();
                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();
                    }
                }
                else
                {
                    System.IO.Directory.CreateDirectory(ConfigurationManager.AppSettings.Get("ErrorLogPath") + "\\");
                    if (File.Exists(strFinalPath))
                    {

                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();
                    }
                    else
                    {
                        fw = File.Create(strFinalPath);
                        fw.Close();
                        sw = File.AppendText(strFinalPath);
                        sw.WriteLine(strMessage);
                        sw.Close();

                    }
                }

            }
            catch{}
        
        }
    }
}