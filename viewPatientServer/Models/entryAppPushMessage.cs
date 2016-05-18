using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace viewPatientServer.Models
{
    public class entryAppPushMessage:commonDB
    {
        public string strName
        {
            get;
            set;
        }

        public string strRegistID
        {
            get;
            set;
        }

        public string strPlatform
        {
            get;
            set;
        }

        public bool registedApp()
        {
            try
            {
                List<columnsData> liColumnDataObjs = new List<columnsData>();

                liColumnDataObjs.Add(new columnsData()
                {
                    strFileName = "Name",
                    oledbTypeValue = System.Data.OleDb.OleDbType.Char,
                    strValue = "joyce"
                });

                liColumnDataObjs.Add(new columnsData()
                {
                    strFileName = "Appid",
                    oledbTypeValue = System.Data.OleDb.OleDbType.Char,
                    strValue = strRegistID,
                });

                liColumnDataObjs.Add(new columnsData()
                {
                    strFileName = "Platform",
                    oledbTypeValue = System.Data.OleDb.OleDbType.Char,
                    strValue = strPlatform,
                });

                commonDB.addWithParameter("apppushmessage", liColumnDataObjs);
                return true;
            }
            catch (Exception ex)
            {
                return false;
                //throw new Exception(ex.Message);
            }
        }

    }
}