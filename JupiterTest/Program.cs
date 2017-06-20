using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace JupiterTest
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpWebRequest request = (HttpWebRequest) WebRequest.Create("https://login.jupitered.com/login/index.php");
            request.Credentials = CredentialCache.DefaultCredentials;
            request.Method = WebRequestMethods.Http.Post;
            request.ContentLength = 110;
            request.ContentType = "application/x-www-form-urlencoded";
            request.Host = "login.jupitered.com";
            request.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            CookieContainer cookieContainer = new CookieContainer();
            request.CookieContainer = cookieContainer;
            Stream dataStream = request.GetRequestStream();
            using (BinaryWriter bw = new BinaryWriter(dataStream))
            {
                bw.Write("doit=checkparent&screensize=1280x720&language1=&district=26214&studid1=Joseph+Wythe&password1=&pagecomplete=1");
                dataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();
            HttpWebRequest request2 = (HttpWebRequest)WebRequest.Create("https://login.jupitered.com/student/0.php?sz=4b38a3f5&w=3.2998877.1");
            request2.CookieContainer = cookieContainer;
            request2.Credentials = CredentialCache.DefaultCredentials;
            request2.Method = WebRequestMethods.Http.Post;
            request2.ContentLength = 110;
            request2.ContentType = "application/x-www-form-urlencoded";
            request2.Host = "login.jupitered.com";
            request2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            // parse response for session id and other params
            //build class for courses
            Stream dataStream2 = request.GetRequestStream();
            using (BinaryWriter bw = new BinaryWriter(dataStream2))
            {
                bw.Write("doit=checkparent&screensize=1280x720&language1=&district=26214&studid1=Joseph+Wythe&password1=&pagecomplete=1");
                dataStream2.Close();
            }
        }
    }
}
