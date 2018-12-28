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
        DataBase db;
        Hashtable ht;

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
                arr[6] = sdr.GetValue(8).ToString();//oNo주문번호를 보내줌
                arr[7] = sdr.GetValue(9).ToString();
		list.Add(arr);
            }
            db.ReaderClose(sdr);
            db.Close();
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
                //0 ''/1 o.oNum/2 m.mName/3 o.oDegree/4 o.oSize/5 o.oShot/6 o.oCream/7 o.oCount
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
            db.Close();
            return list;
        }

        [Route("orderlist/selectpay")]
        [HttpGet]
        public ActionResult<ArrayList> Select_OrderlistPay()
        {
            db = new DataBase();

            MySqlDataReader sdr = db.Reader("p_Orderlist_selectpay");
            ArrayList list = new ArrayList();
            while (sdr.Read())
            {
                string[] arr = new string[sdr.FieldCount];

                //arr[0] = sdr.GetValue(0).ToString();
                string menu="";
                menu += sdr.GetValue(0).ToString();
                if(sdr.GetValue(1).ToString()!="X")
                {
                    menu+="("+sdr.GetValue(2).ToString()+")";
                }
                if(sdr.GetValue(2).ToString()!="X")
                {
                    string size = sdr.GetValue(2).ToString();
                    menu +="_"+size.Substring(0,1);
                }
                //===================메뉴이름
                arr[0] = menu;
                if(sdr.GetValue(3).ToString()!="-1")   arr[1] = sdr.GetValue(3).ToString();//샷추가
                else                                   arr[1] = "X";
                arr[2]=sdr.GetValue(4).ToString();//휘핑크림
                arr[3] = sdr.GetValue(5).ToString();//수량
                arr[4]=sdr.GetValue(6).ToString();//가격

                list.Add(arr);
            }
            db.ReaderClose(sdr);
            db.Close();
            return list;
        }

        [Route("orderlist/insert")]
        [HttpPost]
        public ActionResult<string> Insert([FromForm] string mName,[FromForm] string oNum,[FromForm] string oCount,[FromForm] string oDegree,[FromForm] string oSize,[FromForm] string oShot,[FromForm] string oCream)
        {
            db = new DataBase();
            ht = new Hashtable();

            if(oDegree==null) oDegree="X";
            if(oSize==null) oSize="X";
            if(oCream==null) oCream="X";

            ht.Add("_mName", mName);
            ht.Add("_oNum",oNum);
            ht.Add("_oCount", oCount);
            ht.Add("_oDegree", oDegree);
            ht.Add("_oSize", oSize);
            ht.Add("_oShot", oShot);
            ht.Add("_oCream", oCream);
            Console.WriteLine("이름->"+mName+"주문번호->"+oNum+",수량->"+oCount+",온도->"+oDegree+",사이즈->"+oSize+",샷->"+oShot+",휘핑->"+oCream);
            if (db.NonQuery("p_Orderlist_insert", ht))
            {
                db.Close();
                return "1";
            }
            else
            {
                db.Close();
                return "0";
            }
        }

        [Route("orderlist/comYn")]
        [HttpPost]
        public ActionResult<string> ComYn([FromForm] string oNum)
        {
            db = new DataBase();
            ht = new Hashtable();

            ht.Add("_oNum", oNum);
            if (db.NonQuery("p_Staff_ComYn", ht))
            {
                db.Close();
                return "1";
            }
            else
            {
                db.Close();
                return "0";
            }
        }
        
        [Route("orderlist/orderYn")]
        [HttpPost]
        public ActionResult<string> OrderYn([FromForm] string oNum)
        {
            db = new DataBase();
            ht = new Hashtable();
            ht.Add("_oNum", oNum);

            if (db.NonQuery("p_Orderlist_orderYn", ht))
            {
                db.Close();
                return "1";
            }
            else
            {
                db.Close();
                return "0";
            }
        }
    
        [Route("orderlist/selectMaxoNum")]
        [HttpGet]
        public ActionResult<int> SelectMaxoNum_Orderlist()
        {
            db = new DataBase();

            MySqlDataReader sdr = db.Reader("p_Orderlist_selectMaxoNum");

            while (sdr.Read())
            {
                if (sdr.GetValue(0) == System.DBNull.Value)
                {
                    return 1;
                }
                return (int)sdr.GetValue(0)+1;
            }
            db.ReaderClose(sdr);
            db.Close();
            return 0;
        }

        [Route("orderlist/deleteOrder")]
        [HttpPost]
        public ActionResult<string> DeleteOrder([FromForm] string oNo)
        {
            db = new DataBase();
            ht = new Hashtable();
            ht.Add("_oNo", oNo);

            if (db.NonQuery("p_Orderlist_deleteOrder", ht))
            {
                db.Close();
                return "1";
            }
            else
            {
                db.Close();
                return "0";
            }
        }

        [Route("orderlist/deleteOrderAll")]
        [HttpPost]
        public ActionResult<string> DeleteOrderAll([FromForm] string oNum)
        {
            db = new DataBase();
            ht = new Hashtable();
            ht.Add("_oNum", oNum);

            if (db.NonQuery("p_Orderlist_deleteOrderAll", ht))
            {
                db.Close();
                return "1";
            }
            else
            {
                db.Close();
                return "0";
            }
        }
    
        [Route("orderlist/selectBill")]
        [HttpPost]
        public ActionResult<ArrayList> Select_Bill([FromForm] string oNum)
        {
            db = new DataBase();
            ht = new Hashtable();
            ht.Add("_oNum",oNum);
            MySqlDataReader sdr = db.Reader("p_Orderlist_selectBill",ht);
            
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
                arr[2] = sdr.GetValue(4).ToString();//단가
                arr[3]=sdr.GetValue(5).ToString();//수량
                arr[4] = sdr.GetValue(6).ToString();//금액
                list.Add(arr);
            }
            db.ReaderClose(sdr);
            db.Close();
            return list;
        }
    }
}
