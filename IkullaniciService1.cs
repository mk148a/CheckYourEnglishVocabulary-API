using System.IO;
using System.ServiceModel;
using System.ServiceModel.Web;


namespace WcfService1
{
    // NOT: "IkullaniciService1" arabirim adını kodda ve yapılandırma dosyasında birlikte değiştirmek için "Yeniden Düzenle" menüsündeki "Yeniden Adlandır" komutunu kullanabilirsiniz.
    [ServiceContract]
    public interface IkullaniciService1
    {
       

        [WebInvoke(
              Method = "GET",
              //ResponseFormat = WebMessageFormat.Json,
              BodyStyle = WebMessageBodyStyle.Bare,
              UriTemplate = "/gettask1/{cpuid}"
              )]
        [OperationContract]
        Stream gettask1(string cpuid);

        [WebInvoke(UriTemplate = "/setskor",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json, Method = "POST")]       
        [OperationContract]
        string setskor(kullaniciService1.SendData data);
 [WebInvoke(UriTemplate = "/aylikistatistikekle",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json, Method = "POST")]       
        [OperationContract]
        string aylikistatistikekle(kullaniciService1.SendData data);
 [WebInvoke(UriTemplate = "/haftaliksetskor",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json, Method = "POST")]       
        [OperationContract]
        string haftaliksetskor(kullaniciService1.SendData data);
        [WebInvoke(UriTemplate = "/register",
           RequestFormat = WebMessageFormat.Json,
           ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        [OperationContract]
        string register(kullaniciService1.SendData data);
        [WebInvoke(UriTemplate = "/join",
          RequestFormat = WebMessageFormat.Json,
          ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        [OperationContract]
        string join(kullaniciService1.SendData data);

        [WebInvoke(UriTemplate = "/login",
       RequestFormat = WebMessageFormat.Json,
       ResponseFormat = WebMessageFormat.Json, Method = "POST")]
        [OperationContract]
        string login(kullaniciService1.SendData dataa);
        [WebInvoke(
            Method = "GET",
            //ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Bare,
            UriTemplate = "/getrank"
            )]
        [OperationContract]
        Stream getrank();

        [WebInvoke(
           Method = "GET",
           //ResponseFormat = WebMessageFormat.Json,
           BodyStyle = WebMessageBodyStyle.Bare,
           UriTemplate = "/getuserrank/{kullaniciad}"
           )]
        [OperationContract]
        Stream getuserrank(string kullaniciad);

        [WebInvoke(
         Method = "GET",
         ResponseFormat = WebMessageFormat.Json,
         BodyStyle = WebMessageBodyStyle.Bare,
         UriTemplate = "/getscorerank/{kullaniciad}"
         )]
        [OperationContract]
        string getscorerank(string kullaniciad);

        [WebInvoke(
      Method = "GET",
      //ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "/getSoru/{soruId}"
      )]
        [OperationContract]
        Stream getSoru(string soruId);
        [WebInvoke(
      Method = "GET",
      //ResponseFormat = WebMessageFormat.Json,
      BodyStyle = WebMessageBodyStyle.Bare,
      UriTemplate = "/getcompatetionSoru"
      )]
        [OperationContract]
        Stream getcompatetionSoru();
        [WebInvoke(
     Method = "GET",
     ResponseFormat = WebMessageFormat.Json,
     BodyStyle = WebMessageBodyStyle.Bare,
     UriTemplate = "/startkontrol"
     )]
        [OperationContract]
        string startkontrol();
        [WebInvoke(
    Method = "GET",
    ResponseFormat = WebMessageFormat.Json,
    BodyStyle = WebMessageBodyStyle.Bare,
    UriTemplate = "/joinkontrol/{kullaniciAd}"
    )]
        [OperationContract]
        string joinkontrol(string kullaniciAd);

    }
}
