using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;

namespace OrderByKioskWebAPI
{
    [ApiController]
    public class AdminController : ControllerBase
    {
        DataBase db;
        [Route("Admin/soldoutList")]
        [HttpGet]
        public ActionResult<ArrayList> SoldoutList()
        {
            db = new DataBase();
            
            MySqlDataReader sdr = db.Reader("p_SoldoutList");
            
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
            db.Close();
            return list;
        }

        [Route("admin/passwd")]
        [HttpGet]
        public ActionResult<string> Passwd()
        {
            db = new DataBase();

            MySqlDataReader sdr = db.Reader("p_Admin_Passwd");

            while (sdr.Read())
            {
                return sdr.GetValue(0).ToString();
            }
            db.ReaderClose(sdr);
            db.Close();
            return "";
        }
    
        [Route("admin/selectMonthIncome")]
        [HttpPost]
        public ActionResult<ArrayList> Select_Admin_monthIncome([FromForm] string startdate,[FromForm] string enddate)
        {
            db = new DataBase();
            Hashtable ht = new Hashtable();
            ht.Add("_startdate",startdate);
            ht.Add("_enddate",enddate);
            MySqlDataReader sdr = db.Reader("p_Admin_monthIncome",ht);
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
            db.Close();
            return list;
        }
    
        [Route("admin/selectMenuIncome")]
        [HttpPost]
        public ActionResult<ArrayList> Select_Admin_menuIncome([FromForm] string startdate,[FromForm] string enddate)
        {
            db = new DataBase();
            Hashtable ht = new Hashtable();
            ht.Add("_startdate",startdate);
            ht.Add("_enddate",enddate);
            MySqlDataReader sdr = db.Reader("p_Admin_menuIncome",ht);
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
            db.Close();
            return list;
        }
    }
}