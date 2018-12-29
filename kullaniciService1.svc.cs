using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace WcfService1
{
    // NOT: "kullaniciService1" sınıf adını kodda, svc'de ve yapılandırma dosyasında birlikte değiştirmek için "Yeniden Düzenle" menüsündeki "Yeniden Adlandır" komutunu kullanabilirsiniz.
    // NOT: Bu hizmeti test etmek üzere WCF Test İstemcisi'ni başlatmak için lütfen Çözüm Gezgini'nde kullaniciService1.svc veya kullaniciService1.svc.cs öğesini seçin ve hata ayıklamaya başlayın.
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.PerCall,
         ConcurrencyMode = ConcurrencyMode.Multiple)]
    public class kullaniciService1 : IkullaniciService1
    {
        public class SendData
        {
            public string cpuid;
            public string kullaniciadi;
            public string sifre;
            public string mail;
            public string soruindex;
            public string task1;
            public string task2;
            public string task3;
            public int skor;
            public int gold;
        }
        public class Match
        {

            public int skor;
            public int dogru;
            public int yanlis;
            public int gecilen;
            public bool bitis;


        }
        public Stream gettask1(string cpuid)
        {

            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);

            conn.Open();

            SqlCommand sorgula = new SqlCommand("select * from kullanici where cpuid ='" + cpuid + "'", conn);

            SqlDataReader dr1 = sorgula.ExecuteReader();
            string task1 = "";
            string task2 = "";
            string task3 = "";
            string Soruindex = "";
            string gold = "";
            string kullaniciad = "";
            List<string> data = new List<string>();

            if (dr1.Read())
            {
                //MessageBox.Show(" daha önce bu bilgisayardan kayıt yapımış oto giriş yapıldı");

                //cozulen soruları çağırıyor    
                try
                {

                    if (dr1["task1"].ToString() != "")
                    {
                        task1 = dr1["task1"].ToString();
                        data.Add(task1);
                    }
                    if (dr1["task2"].ToString() != "")
                    {
                        task2 = dr1["task2"].ToString();
                        data.Add(task2);
                    }
                    if (dr1["task3"].ToString() != "")
                    {
                        task3 = dr1["task3"].ToString();
                        data.Add(task3);
                    }
                    if (dr1["Soruindex"].ToString() != "")
                    {
                        Soruindex = dr1["Soruindex"].ToString();
                        data.Add(Soruindex);
                    }

                    if (dr1["gold"].ToString() != "")
                    {
                        gold = dr1["gold"].ToString();
                        data.Add(gold);
                    }
                    if (dr1["kullaniciAd"].ToString() != "")
                    {
                        kullaniciad = dr1["kullaniciAd"].ToString();
                        data.Add(kullaniciad);
                    }

                }
                catch
                {
                    data.Add("hata");
                }

                dr1.Close();
                conn.Close();
            }
            else
            {
                data.Add("hata");


            }
            byte[] resultBytes = Encoding.GetEncoding("windows-1254").GetBytes(JsonConvert.SerializeObject(data));
            WebOperationContext.Current.OutgoingResponse.ContentType = "text";
            return new MemoryStream(resultBytes);

        }
        public string setskor(SendData data)
        {

            string mesaj = "basarili";
            try
            {
                string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
                SqlConnection conn = new SqlConnection(baglanti);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                }

                SqlCommand cmd = new SqlCommand(
                   @"UPDATE kullanici 
                    SET gold = @gold
                    WHERE cpuid=@cpuid ", conn);
                if (data.soruindex != null)
                {
                    cmd = new SqlCommand(
                    @"UPDATE kullanici 
                    SET Soruindex=@Soruindex,
                    task1 = @task1,
                    task2 = @task2,
                    task3 = @task3, 
                    skor = @skor, 
                    gold = @gold
                    WHERE cpuid=@cpuid ", conn);
                    cmd.Parameters.AddWithValue("@Soruindex", data.soruindex);
                    cmd.Parameters.AddWithValue("@task1", data.task1);
                    cmd.Parameters.AddWithValue("@task2", data.task2);
                    cmd.Parameters.AddWithValue("@task3", data.task3);
                    cmd.Parameters.AddWithValue("@skor", data.skor);
                }
                cmd.Parameters.AddWithValue("@cpuid", data.cpuid);
                cmd.Parameters.AddWithValue("@gold", data.gold);
                cmd.ExecuteNonQuery();

                conn.Close();

            }
            catch (Exception e)
            {
                mesaj = "hata " + e.Message;
            }

            return mesaj;
        }
        public string register(SendData data)
        {
            string mesaj = "";
            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            try
            {

              
            
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand sorgula = new SqlCommand("select * from kullanici where kullaniciAd ='" + data.kullaniciadi + "'", conn);
            SqlDataReader dr = sorgula.ExecuteReader();

            if (dr.Read())
            {
                mesaj = "This username is used by an another user. Please choose a different username.";

            }
            else
            {
                dr.Close();
                SqlCommand idsorgula = new SqlCommand("select * from kullanici where cpuid ='" + data.cpuid + "'", conn);
                SqlDataReader dr1 = idsorgula.ExecuteReader();
                if (dr1.Read())
                {
                    mesaj = "You already registered from this device before. Please login.";

                }
                else
                {
                    dr1.Close();
                      

                    SqlCommand cmd = new SqlCommand("insert into kullanici(kullaniciAd, sifre, mail,Soruindex,cpuid,gold,skor,task1,task2,task3) values('" + data.kullaniciadi + "','" + data.sifre + "','" + data.mail + "','" + data.soruindex + "','" + data.cpuid + "','" + data.gold + "','" +data.skor + "','" + data.task1 + "','" + data.task2 + "','" + data.task3 + "')", conn);
                    //object yourMysteryObject = Form1.skor;              
                    cmd.ExecuteNonQuery();
                    mesaj = "Your registration has been added successfully, You are redirected to the MainPage.";
                    conn.Close();

                }

            }
        }
            catch (Exception ex)
            {
                var st = new StackTrace(ex, true);
                // Get the top stack frame
                var frame = st.GetFrame(0);
                // Get the line number from the stack frame
                var line = frame.GetFileLineNumber();
                mesaj = ex.Message + "," + line.ToString();
            }
            return mesaj;
        }
        public string login(SendData dataa)
        {
            string data = "";
            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);

            conn.Open();

            SqlCommand sorgula = new SqlCommand("select * from kullanici where kullaniciAd ='" + dataa.kullaniciadi + "'", conn);
            SqlDataReader dr1 = sorgula.ExecuteReader();

            if (dr1.Read())
            {
                //MessageBox.Show(" daha önce bu bilgisayardan kayıt yapımış oto giriş yapıldı");

                //cozulen soruları çağırıyor    
                try
                {

                    if (dr1["sifre"].ToString() == dataa.sifre)
                    {
                        if (dr1["cpuid"].ToString() == dataa.cpuid)
                        {
                            data = "You have logged in successfully.";
                        }
                        else
                        {
                            data = "Your user name has been registered from an another device. Please try to login from the device which you have registered on.";
                        }
                    }
                    else
                    {

                        data = "You have incorrectly entered your password";
                    }


                }
                catch
                {
                    data = ("Genel hata");
                }

                dr1.Close();
                conn.Close();
            }
            else
            {
                data = ("Your user name or password was entered inaccurately.");
            }
            return data;
        }
        public Stream getrank()
        {

            List<string> skor = new List<string>();
            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();

            }

            //select TOP 100 skor,kullaniciAd from kullanici ORDER BY skor DESC


            SqlCommand sorgula = new SqlCommand("SELECT top 100 num,kullaniciAd,skor FROM(SELECT kullaniciAd,skor, ROW_NUMBER() OVER(ORDER BY skor DESC) AS num From kullanici) AS numbered", conn);

            SqlDataReader dr = sorgula.ExecuteReader();

            while (dr.Read())
            {
               
                skor.Add(dr["kullaniciAd"].ToString() + "," + dr["num"].ToString()+","+dr["skor"]);
                
            }
            dr.Close();
            conn.Close();
            byte[] resultBytes = Encoding.GetEncoding("windows-1254").GetBytes(JsonConvert.SerializeObject(skor));
            WebOperationContext.Current.OutgoingResponse.ContentType = "text";
            return new MemoryStream(resultBytes);
           

        }
        public Stream getuserrank(string kullaniciad)
        {

            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);

            conn.Open();

            SqlCommand sorgula = new SqlCommand("select task1,task2,task3 from kullanici where kullaniciAd ='" + kullaniciad + "'", conn);

            SqlDataReader dr1 = sorgula.ExecuteReader();
            string task1 = "";
            string task2 = "";
            string task3 = "";           
        
            List<string> data = new List<string>();

            if (dr1.Read())
            {
                //MessageBox.Show(" daha önce bu bilgisayardan kayıt yapımış oto giriş yapıldı");

                //cozulen soruları çağırıyor    
                try
                {

                    if ((string)dr1["task1"] != "")
                    {
                        task1 = (string)dr1["task1"];
                        data.Add(task1);
                    }
                    if ((string)dr1["task2"]!= "")
                    {
                        task2 = (string)dr1["task2"];
                        data.Add(task2);
                    }
                    if ((string)dr1["task3"] != "")
                    {
                        task3 = (string)dr1["task3"];
                        data.Add(task3);
                    }
                   
                   
                        data.Add(kullaniciad);
                    

                }
                catch
                {
                    data.Add("hata");
                }

                dr1.Close();
                conn.Close();
            }
            else
            {
                data.Add("hata");
            }
            byte[] resultBytes = Encoding.GetEncoding("windows-1254").GetBytes(JsonConvert.SerializeObject(data));
            WebOperationContext.Current.OutgoingResponse.ContentType = "text";
            return new MemoryStream(resultBytes);
        }
        public Stream getSoru(string soruId)
        {

            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);

            conn.Open();

            SqlCommand sorgula = new SqlCommand("select * from sorular where SoruId LIKE '%" + soruId + "%'", conn);

            SqlDataReader dr1 = sorgula.ExecuteReader();          

            List<Soru.Soru> data = new List<Soru.Soru>();

            while (dr1.Read())
            {
                //MessageBox.Show(" daha önce bu bilgisayardan kayıt yapımış oto giriş yapıldı");

                //cozulen soruları çağırıyor    
                Soru.Soru yenisoru = new Soru.Soru();
                try
                {

                    if (dr1["Soru"].ToString() != "")
                    {
                        yenisoru.soru = dr1["Soru"].ToString();
                       
                    }
                    if (dr1["SoruId"].ToString() != "")
                    {
                        yenisoru.id = dr1["SoruId"].ToString();
                        
                    }
                    if (dr1["Cevap1"].ToString() != "")
                    {
                        yenisoru.cevap1 = dr1["Cevap1"].ToString();
                   
                    }
                    if (dr1["Cevap2"].ToString() != "")
                    {
                        yenisoru.cevap2 = dr1["Cevap2"].ToString();

                    }
                    if (dr1["Cevap3"].ToString() != "")
                    {
                        yenisoru.cevap3 = dr1["Cevap3"].ToString();

                    }
                    if (dr1["Cevap4"].ToString() != "")
                    {
                        yenisoru.cevap4 = dr1["Cevap4"].ToString();

                    }
                    if (dr1["Cevap5"].ToString() != "")
                    {
                        yenisoru.cevap5 = dr1["Cevap5"].ToString();

                    }
                    if (dr1["Dogrucevap"].ToString() != "")
                    {
                        yenisoru.dogrucevap = (int)dr1["Dogrucevap"];
                        
                    }
                    //string yenisorutxt = SerializeToXml(yenisoru);
                    data.Add(yenisoru);
                }
                catch
                {
                   
                }

                
            }
            dr1.Close();
            conn.Close();
            byte[] resultBytes = Encoding.GetEncoding("windows-1254").GetBytes(JsonConvert.SerializeObject(data));
            WebOperationContext.Current.OutgoingResponse.ContentType = "text";
            return new MemoryStream(resultBytes);
        }
        public string getscorerank(string kullaniciAd)
        {

            string skor= "";
            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();

            }

            //SELECT num FROM(SELECT kullaniciAd, ROW_NUMBER() OVER(ORDER BY skor DESC) AS num From kullanici) AS numbered WHERE kullaniciAd ='"+ kullaniciAd+"'"
            SqlCommand sorgula = new SqlCommand("SELECT num,kullaniciAd,skor FROM(SELECT kullaniciAd,skor, ROW_NUMBER() OVER(ORDER BY skor DESC) AS num From kullanici) AS numbered where kullaniciAd='" + kullaniciAd+"'", conn);

            SqlDataReader dr = sorgula.ExecuteReader();

            while (dr.Read())
            {
                //string x = dr["kullaniciAd"] + " " + dr["skor"];
                skor=dr.GetValue(0).ToString();
                
            }
            dr.Close();
            conn.Close();
            return skor;

        }
        public string haftaliksetskor(SendData data)
        {

            string mesaj = "basarili";
            try
            {
                string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
                SqlConnection conn = new SqlConnection(baglanti);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                }

                SqlCommand cmd = new SqlCommand(
                    @"UPDATE haftalik 
                    SET cozulensoruindexleri=@Soruindex,
                    match = @match,                   
                    toplamskor = @skor
                    WHERE kullaniciAd=@kullaniciAd ", conn);
                    cmd.Parameters.AddWithValue("@Soruindex", data.soruindex);
                    cmd.Parameters.AddWithValue("@match", data.task1);                 
                    cmd.Parameters.AddWithValue("@skor", data.skor);                
                cmd.Parameters.AddWithValue("@kullaniciAd", data.kullaniciadi);
                //cmd.Parameters.AddWithValue("@gold", data.gold);
                cmd.ExecuteNonQuery();

                conn.Close();
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                }
                cmd = new SqlCommand("UPDATE kullanici SET gold = @gold WHERE kullaniciAd=@kullaniciAd ", conn);
                cmd.Parameters.AddWithValue("@kullaniciAd", data.kullaniciadi);
                cmd.Parameters.AddWithValue("@gold", data.gold);
                cmd.ExecuteNonQuery();

                conn.Close();


            }
            catch (Exception e)
            {
                mesaj = "hata " + e.Message;
            }

            return mesaj;
        }
        public string aylikistatistikekle(SendData data)
        {

            string mesaj = "basarili";
            try
            {
                string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
                SqlConnection conn = new SqlConnection(baglanti);
                if (conn.State == ConnectionState.Closed)
                {
                    conn.Open();

                }

                SqlCommand cmd = new SqlCommand(
                    @"select * from aylik where kullaniciAd=@kullaniciAd", conn);                            
                cmd.Parameters.AddWithValue("@kullaniciAd", data.kullaniciadi);
                //cmd.Parameters.AddWithValue("@gold", data.gold);
                SqlDataReader dr1 = cmd.ExecuteReader();
                List<Match> gelenlist = new List<Match>();
                if (dr1.Read())
                {
                    gelenlist = JsonConvert.DeserializeObject<List<Match>>((string)dr1["matchs"]);
                    conn.Close();
                    dr1.Close();
                    gelenlist.Add(JsonConvert.DeserializeObject<Match>(data.task1));

                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();

                    }
                    cmd = new SqlCommand("update aylik set matchs= @matchs WHERE kullaniciAd=@kullaniciAd ", conn);
                    cmd.Parameters.AddWithValue("@kullaniciAd", data.kullaniciadi);
                    cmd.Parameters.AddWithValue("@matchs", JsonConvert.SerializeObject(gelenlist));
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
                else
                {
                    conn.Close();
                    if (conn.State == ConnectionState.Closed)
                    {
                        conn.Open();

                    }
                    // SqlCommand cmd = new SqlCommand("insert into kullanici(kullaniciAd, sifre, mail,Soruindex,cpuid,gold) values('" + data.kullaniciadi + "','" + data.sifre + "','" + data.mail + "','" + data.soruindex + "','" + data.cpuid + "','" + data.gold + "')", conn);
                    gelenlist.Clear();
                    gelenlist.Add(JsonConvert.DeserializeObject<Match>(data.task1));

                    cmd = new SqlCommand("insert into aylik(kullaniciAd, matchs) values('"+data.kullaniciadi + "','" + JsonConvert.SerializeObject(gelenlist)+ "')", conn);
                    cmd.ExecuteNonQuery();
                    conn.Close();
                }
               

              


            }
            catch (Exception e)
            {
                mesaj = "hata " + e.Message;
            }

            return mesaj;
        }
        public Stream getcompatetionSoru()
        {

            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);

            conn.Open();

            SqlCommand sorgula = new SqlCommand("select * from compitationsorular", conn);

            SqlDataReader dr1 = sorgula.ExecuteReader();          

            List<Soru.Soru> data = new List<Soru.Soru>();

            while (dr1.Read())
            {   
                Soru.Soru yenisoru = new Soru.Soru();
                try
                {

                    if (dr1["Soru"].ToString() != "")
                    {
                        yenisoru.soru = dr1["Soru"].ToString();
                       
                    }
                    if (dr1["SoruId"].ToString() != "")
                    {
                        yenisoru.id = dr1["SoruId"].ToString();
                        
                    }
                    if (dr1["Cevap1"].ToString() != "")
                    {
                        yenisoru.cevap1 = dr1["Cevap1"].ToString();
                   
                    }
                    if (dr1["Cevap2"].ToString() != "")
                    {
                        yenisoru.cevap2 = dr1["Cevap2"].ToString();

                    }
                    if (dr1["Cevap3"].ToString() != "")
                    {
                        yenisoru.cevap3 = dr1["Cevap3"].ToString();

                    }
                    if (dr1["Cevap4"].ToString() != "")
                    {
                        yenisoru.cevap4 = dr1["Cevap4"].ToString();

                    }
                    if (dr1["Cevap5"].ToString() != "")
                    {
                        yenisoru.cevap5 = dr1["Cevap5"].ToString();

                    }
                    if (dr1["Dogrucevap"].ToString() != "")
                    {
                        yenisoru.dogrucevap = (int)dr1["Dogrucevap"];
                        
                    }
                    //string yenisorutxt = SerializeToXml(yenisoru);
                    data.Add(yenisoru);
                }
                catch
                {
                   
                }

                
            }
            dr1.Close();
            conn.Close();
            byte[] resultBytes = Encoding.GetEncoding("windows-1254").GetBytes(JsonConvert.SerializeObject(data));
            WebOperationContext.Current.OutgoingResponse.ContentType = "text";
            return new MemoryStream(resultBytes);
        }
        public string join(SendData data)
        {
            string mesaj ="false";
            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
                conn.Open();
            SqlCommand sorgula = new SqlCommand("select * from haftalik where kullaniciAd='" + data.kullaniciadi + "'", conn);
            SqlDataReader dr = sorgula.ExecuteReader();

            if (dr.Read())
            {
                mesaj = "true";

            }
            else
            {
                dr.Close();    
                    SqlCommand cmd = new SqlCommand("insert into haftalik (kullaniciAd,toplamskor,match,cozulensoruindexleri) VALUES ('" + data.kullaniciadi + "','" + data.skor + "','" + data.task1 + "','" + data.soruindex + "')", conn);
                    //object yourMysteryObject = Form1.skor;              
                    cmd.ExecuteNonQuery();
                    mesaj = "true";
                    conn.Close();

         }

            
            return mesaj;
        }
        public string startkontrol()
        {
            string mesaj = "";
            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand sorgula = new SqlCommand("Select COUNT(*) From haftalik", conn);            
            int count = (int)sorgula.ExecuteScalar();
            mesaj = count.ToString();
            conn.Close();            
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
             sorgula = new SqlCommand("SELECT top 3 num,kullaniciAd,toplamskor FROM(SELECT kullaniciAd,toplamskor, ROW_NUMBER() OVER(ORDER BY toplamskor DESC) AS num From haftalik) AS numbered", conn);
            SqlDataReader dr = sorgula.ExecuteReader();

            while (dr.Read())
            {
                mesaj += "," + dr["kullaniciAd"];
            }

            return mesaj;
        }
        public string joinkontrol(string kullaniciAd)
        {
            
            string mesaj = "";
            try
            {

        
            List<string> data = new List<string>();
            string baglanti = "Data Source=.;Initial Catalog=testyourcovabulary;User ID=sa;Password=148150;Min Pool Size=5;Max Pool Size=300;Pooling=true;";
            SqlConnection conn = new SqlConnection(baglanti);
            if (conn.State == ConnectionState.Closed)
            {
                conn.Open();
            }
            SqlCommand sorgula = new SqlCommand("select * from haftalik where kullaniciAd='"+ kullaniciAd, conn);
            SqlDataReader dr = sorgula.ExecuteReader();

            if (dr.Read())
            {
                data.Add((string)dr["cozulensoruindexleri"]);
                data.Add((string)dr["match"]);
                mesaj = JsonConvert.SerializeObject(data);
            }
            else
            {
                mesaj = "hata";
            }
            }
            catch (Exception e)
            {

                mesaj = "hata";
            }
            return mesaj;
        } 
    }



}
