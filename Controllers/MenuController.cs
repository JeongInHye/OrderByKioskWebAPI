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
    public class MenuController : ControllerBase
    {
        DataBase db;
        Hashtable hashtable;
        string serverUrl="http://192.168.3.50:5000";
        
        [Route("Menu/add")]
        [HttpPost]
        public ActionResult<string> Add([FromForm] string fileName,[FromForm]string fileData,[FromForm] string cNo,[FromForm] string mName,[FromForm] string mPrice,[FromForm] string mImage,[FromForm] string DegreeYn,[FromForm] string SizeYn,[FromForm] string ShotYn,[FromForm] string CreamYn)
        {
            string path = "/root/OrderByKioskWebAPI/wwwroot";  //System.IO.Directory.GetCurrentDirectory();
            //path += "/root/OrderByKioskWebAPI/wwwroot";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            byte[] data = Convert.FromBase64String(fileData);

            try
            {
                string ext = fileName.Substring(fileName.LastIndexOf("."));
                Guid saveName = Guid.NewGuid();
                string fullName = saveName + ext;   // 저장되는 파일명 생성
                string fullPath = string.Format("{0}/{1}",path,fullName);  // 전체경로 + 저장파일명 (주소) 
                FileInfo fileInfo = new FileInfo(fullPath);
                FileStream fileStream = fileInfo.Create();
                fileStream.Write(data,0,data.Length);
                fileStream.Close();

                string url = string.Format("{0}/{1}",serverUrl,fullName);

                hashtable = new Hashtable();
                hashtable.Add("_cNo",cNo);
                hashtable.Add("_mName",mName);
                hashtable.Add("_mPrice",mPrice);
                hashtable.Add("_mImage",url);
                hashtable.Add("_DegreeYn",DegreeYn);
                hashtable.Add("_SizeYn",SizeYn);
                hashtable.Add("_ShotYn",ShotYn);
                hashtable.Add("_CreamYn",CreamYn);

                db = new DataBase();
                if(db.NonQuery("p_Menu_Insert",hashtable))
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
            catch
            {
                // Console.WriteLine("파일 저장 오류");
            }
            return "1";
        }

        [Route("Menu/menuEdeit")]
        [HttpPost]
        public ActionResult<string> MenuEdeit([FromForm] string fileName,[FromForm]string fileData,[FromForm] string mName,[FromForm] string NewmName,[FromForm] string mPrice,[FromForm] string DegreeYn,[FromForm] string SizeYn,[FromForm] string ShotYn,[FromForm] string CreamYn)
        {
            string path = "/root/OrderByKioskWebAPI/wwwroot";	//System.IO.Directory.GetCurrentDirectory();
           // path += "\\wwwroot";

            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
            try
            {
                string url;
                // Console.WriteLine("---------->"+fileData.Length+"<--------------");
                if (fileData.Length>80)
                {
                    string ext = fileName.Substring(fileName.LastIndexOf("."));
                    Guid saveName = Guid.NewGuid();
                    string fullName = saveName + ext;
                    string fullPath = string.Format("{0}/{1}", path, fullName);
                    FileInfo fileInfo = new FileInfo(fullPath);
                    FileStream fileStream = fileInfo.Create();
                    byte[] data = Convert.FromBase64String(fileData);
                    fileStream.Write(data, 0, data.Length);
                    fileStream.Close();

                    url = string.Format("{0}/{1}",serverUrl, fullName);
                }
                else
                {
                    url = fileData;
                }

                hashtable = new Hashtable();
                hashtable.Add("_mName",mName);
                hashtable.Add("_NewmName",NewmName);
                hashtable.Add("_mPrice",mPrice);
                hashtable.Add("_mImage",url);
                hashtable.Add("_DegreeYn",DegreeYn);
                hashtable.Add("_SizeYn",SizeYn);
                hashtable.Add("_ShotYn",ShotYn);
                hashtable.Add("_CreamYn",CreamYn);

                db = new DataBase();

                if(db.NonQuery("p_Menu_MenuEdit",hashtable))
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
            catch
            {
                // Console.WriteLine("파일 저장 오류");
            }
            return "0";
        }

        [Route("Menu/delete")]
        [HttpPost]
        public ActionResult<string> Delete([FromForm] string mName)
        {
            hashtable = new Hashtable();
            hashtable.Add("_mName",mName);

            db = new DataBase();

            if(db.NonQuery("p_Menu_Delete",hashtable))
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
            db.Close(); 
            return list;
        }

        [Route("Menu/menuEdeitSelect")]
        [HttpPost]
        public ActionResult<ArrayList> MenuEdeitSelect([FromForm] string mName)
        {
            db = new DataBase();
            hashtable = new  Hashtable();
            hashtable.Add("_mName",mName);
            MySqlDataReader sdr = db.Reader("p_Menu_MenuEdeitSelect",hashtable);
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
    
        [Route("menu/select")]
        [HttpPost]
        public ActionResult<ArrayList> Select_Menu([FromForm] string cNo)
        {
            db = new DataBase();

            hashtable = new Hashtable();
            hashtable.Add("_cNo",cNo);
            MySqlDataReader sdr = db.Reader("p_Menu_Select",hashtable);
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

        [Route("menu/choice")]
        [HttpPost]
        public ActionResult<ArrayList> Select_ChoiceMenu([FromForm] string mName)
        {
            db = new DataBase();
            hashtable = new Hashtable();

            hashtable.Add("_mName",mName);
            MySqlDataReader sdr = db.Reader("p_Menu_Choice",hashtable);
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

        [Route("menu/image")]
        [HttpPost]
        public ActionResult<string> Select_MenuImage([FromForm] string mName)
        {
            db = new DataBase();
            hashtable = new Hashtable();

            hashtable.Add("_mName",mName);
            MySqlDataReader sdr = db.Reader("p_Menu_image",hashtable);
            string list ="";
            while (sdr.Read())
            {
                list=sdr.GetValue(0).ToString();
            }
            db.ReaderClose(sdr);
            db.Close();
            return list;
        }
    }
}
