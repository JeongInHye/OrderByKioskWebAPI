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
    public class OrderlistController : ControllerBase
    {
        DataBase db = new DataBase();
        Hashtable ht = new  Hashtable();

        [Route("orderlist/select")]
        [HttpGet]
        public ActionResult<ArrayList> Select_Orderlist()
        {
            db = new DataBase();

            MySqlDataReader sdr = db.Reader("p_Orderlist_select");
            ArrayList list = new ArrayList();
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];

                arr[0] = sdr.GetValue(0).ToString();
                string menu="";
                menu += sdr.GetValue(1).ToString();
                if(sdr.GetValue(2).ToString()!="X")
                {
                    menu+="("+sdr.GetValue(2).ToString()+")";
                }
                if(sdr.GetValue(3).ToString()!="X")
                {
                    string size = sdr.GetValue(3).ToString();
                    menu +="_"+size.Substring(0,1);
                }
                //===================메뉴이름
                arr[1] = menu;
                if(sdr.GetValue(4).ToString()!="-1")   arr[2] = sdr.GetValue(4).ToString();//샷추가
                else                                   arr[2] = "X";
                arr[3]=sdr.GetValue(5).ToString();//휘핑크림
                arr[4] = sdr.GetValue(6).ToString();//수량
                arr[5]=sdr.GetValue(7).ToString();//가격

                list.Add(arr);
            }
            db.ReaderClose(sdr);

            return list;
        }

        [Route("orderlist/selectstaff")]
        [HttpGet]
        public ActionResult<ArrayList> Selectstaff_Orderlist()
        {
            db = new DataBase();

            MySqlDataReader sdr = db.Reader("p_Orderlist_selectstaff");
            ArrayList list = new ArrayList();
            while (sdr.Read())
            {
                //0 ''/1 o.oNo/2 m.mName/3 o.oDegree/4 o.oSize/5 o.oShot/6 o.oCream/7 o.oCount
                string[] arr = new string[sdr.FieldCount];

                arr[0] = sdr.GetValue(0).ToString();
                arr[1] = sdr.GetValue(1).ToString();
                string menu="";
                menu += sdr.GetValue(2).ToString();
                if(sdr.GetValue(3).ToString()!="X")
                {
                    menu+="("+sdr.GetValue(3).ToString()+")";
                }
                if(sdr.GetValue(4).ToString()!="X")
                {
                    string size = sdr.GetValue(4).ToString();
                    menu +="_"+size.Substring(0,1);
                }
                //===================메뉴이름
                arr[2] = menu;
                if(sdr.GetValue(5).ToString()!="-1")   arr[3] = sdr.GetValue(5).ToString();//샷추가
                else                                   arr[3] = "X";
                arr[4]=sdr.GetValue(6).ToString();//휘핑크림
                arr[5] = sdr.GetValue(7).ToString();//수량

                list.Add(arr);
            }
            db.ReaderClose(sdr);

            return list;
        }

        [Route("orderlist/insert")]
        [HttpPost]
        public ActionResult<string> Insert([FromForm] string mName,[FromForm] string oCount,[FromForm] string oDegree,[FromForm] string oSize,[FromForm] string oShot,[FromForm] string oCream)
        {
            db = new DataBase();
            ht = new Hashtable();

            if(oDegree==null) oDegree="X";
            if(oSize==null) oSize="X";
            if(oCream==null) oCream="X";

            ht.Add("_mName", mName);
            ht.Add("_oCount", oCount);
            ht.Add("_oDegree", oDegree);
            ht.Add("_oSize", oSize);
            ht.Add("_oShot", oShot);
            ht.Add("_oCream", oCream);
            Console.WriteLine("이름->"+mName+",수량->"+oCount+",온도->"+oDegree+",사이즈->"+oSize+",샷->"+oShot+",휘핑->"+oCream);
            if (db.NonQuery("p_Orderlist_insert", ht))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }

        [Route("orderlist/complete")]
        [HttpPost]
        public ActionResult<string> Complete([FromForm] string oNo)
        {
            db = new DataBase();
            ht = new Hashtable();

            ht.Add("_oNo", oNo);
            if (db.NonQuery("p_Orderlist_complete", ht))
            {
                return "1";
            }
            else
            {
                return "0";
            }
        }
        
        [Route("orderlist/pay")]
        [HttpPost]
        public ActionResult<string> Pay([FromForm] string oNo)
        {
            db = new DataBase();
            ht = new Hashtable();
            ht.Add("_oNo", oNo);

            if (db.NonQuery("p_Orderlist_pay", ht))
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