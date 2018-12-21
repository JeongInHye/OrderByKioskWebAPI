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
    }
}
