using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Collections;
using System.Data.SqlClient;
using System.IO;
using MySql.Data.MySqlClient;

namespace OrderByKioskWebAPI
{

    [ApiController]
    public class StaffController : ControllerBase
    {
        DataBase db;
        Hashtable hashtable;
     
        [Route("Staff/soldOutAdd")]
        [HttpPost]
        public ActionResult<string> SoldOutAdd([FromForm] string mName)
        {
            hashtable = new Hashtable();
            hashtable.Add("_mName",mName);

            db = new DataBase();

            if(db.NonQuery("p_Staff_SoldOutAdd",hashtable))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        

        [Route("Staff/soldOutDelete")]
        [HttpPost]
        public ActionResult<string> SoldOutDelete([FromForm] string mName)
        {
            hashtable = new Hashtable();
            hashtable.Add("_mName",mName);

            db = new DataBase();

            if(db.NonQuery("p_Staff_SoldOutDelete",hashtable))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        [Route("Menu/nameSelect")]
        [HttpPost]
        public ActionResult<ArrayList> NameSelect([FromForm] string cNo)
        {
            db = new DataBase();
            hashtable = new  Hashtable();
            hashtable.Add("_cNo",cNo);
            MySqlDataReader sdr = db.Reader("p_Menu_NameSelect",hashtable);
            ArrayList list = new ArrayList();

            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];
                for (int i = 0; i < sdr.FieldCount; i++)
                {
                    arr[i] = sdr.GetValue(i).ToString();
                }
                list.Add(arr);
            }
            db.ReaderClose(sdr);

            return list;
        }
    }
}
