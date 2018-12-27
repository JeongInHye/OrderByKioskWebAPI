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
    public class CategoryController : ControllerBase
    {
        [Route("category/select")]
        [HttpGet]
        public ActionResult<ArrayList> Select()
        {
            DataBase db = new DataBase();
            
            MySqlDataReader sdr = db.Reader("p_Category_Select");
            
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