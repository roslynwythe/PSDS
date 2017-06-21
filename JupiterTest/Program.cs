using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using HtmlAgilityPack;
using System.Collections.Specialized;

namespace JupiterTest
{
    class assignment
    {
        public string id;
        public string name;
    }

    class course
    {
        public string name;
        public string classMenuId;
        public List<assignment> assignments;
    }
    
    class Program
    {
                                                                                      
        static void Main(string[] args)
        {
            NameValueCollection outgoingQueryString = new NameValueCollection();
            List<course> courses = new List<course>();
            //List<htmlFormField> jupiterFormFields = new List<htmlFormField> { new htmlFormField { name = "session", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "server", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "district", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "stud", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "contact", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "server", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "class1", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "track", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "term", type = "hidden", value=""},
                                                                                        //new htmlFormField { name = "io", type = "hidden", value=""} };

            HttpWebRequest loginRequest = (HttpWebRequest) WebRequest.Create("https://login.jupitered.com/login/index.php");
            loginRequest.Credentials = CredentialCache.DefaultCredentials;
            loginRequest.Method = WebRequestMethods.Http.Post;
            loginRequest.ContentLength = 110;
            loginRequest.ContentType = "application/x-www-form-urlencoded";
            loginRequest.Host = "login.jupitered.com";
            loginRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
            CookieContainer cookieContainer = new CookieContainer();
            loginRequest.CookieContainer = cookieContainer;
            Stream loginDataStream = loginRequest.GetRequestStream();
            using (BinaryWriter bw = new BinaryWriter(loginDataStream))
            {
                bw.Write("doit=checkparent&screensize=1280x720&language1=&district=26214&studid1=Joseph+Wythe&password1=&pagecomplete=1");
                loginDataStream.Close();
            }
            HttpWebResponse response = (HttpWebResponse)loginRequest.GetResponse();
            Stream receiveStream = response.GetResponseStream();
            // Pipes the stream to a higher level stream reader with the required encoding format. 
            StreamReader readStream = new StreamReader(receiveStream, Encoding.UTF8);
            HtmlAgilityPack.HtmlDocument htmlDoc = new HtmlAgilityPack.HtmlDocument();
            htmlDoc.Load(readStream);

            if (htmlDoc.ParseErrors != null && htmlDoc.ParseErrors.Count() > 0)
            {
                // Handle any parse errors as required
            }
            else
            {
                if (htmlDoc.DocumentNode != null)
                {
                    HtmlAgilityPack.HtmlNode bodyNode = htmlDoc.DocumentNode.SelectSingleNode("//body");

                    if (bodyNode != null)
                    {
                        HtmlAgilityPack.HtmlNode formNode = new HtmlAgilityPack.HtmlNode(HtmlAgilityPack.HtmlNodeType.Element,);
                        HtmlAgilityPack.HtmlNode test = bodyNode;
                        foreach (HtmlNode node in bodyNode.SelectNodes("//form//input"))
                        {
                            outgoingQueryString.Add(node.GetAttributeValue("name", ""), node.GetAttributeValue("value", null));
                        }
                        foreach (HtmlNode node in bodyNode.SelectNodes("//select[@name='classmenu']//option"))
                        {
                            string name = node.GetAttributeValue("value", "");
                            string name2 = node.InnerHtml;
                            //courses.Add(new course {name = node.GetAttributeValue("value", ""), id = node.GetAttributeValue("name", null));
                            courses.Add(new course {name = node.InnerHtml, classMenuId = node.GetAttributeValue("name", null));
                        }

                        //foreach (HtmlNode node in bodyNode.SelectNodes("//form//input"))
                        //{
                        //    foreach (htmlFormField formField in jupiterFormFields)
                        //    {
                        //        string name = node.GetAttributeValue("name",null);
                        //        if ((name != null) & (name == formField.name))
                        //        {
                        //            formField.value = node.GetAttributeValue("value", "");
                        //            formField.type = node.GetAttributeValue("type", "");
                        //        }
                        //    }
                        //}
                    }
                }
            }
            readStream.Close();
            receiveStream.Close();
            response.Close();
            // now loop over all courses, retrieving and storing assignments
            foreach (course c in courses)
            {
                //set value of class menu id
                outgoingQueryString.Set("classmenu", c.classMenuId);
                HttpWebRequest gradeRequest = (HttpWebRequest)WebRequest.Create("https://login.jupitered.com/student/0.php?sz=4b38a3f5&w=3.2998877.1");
                gradeRequest.CookieContainer = cookieContainer;
                gradeRequest.Credentials = CredentialCache.DefaultCredentials;
                gradeRequest.Method = WebRequestMethods.Http.Post;
                gradeRequest.ContentType = "application/x-www-form-urlencoded";
                gradeRequest.Host = "login.jupitered.com";
                gradeRequest.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,*/*;q=0.8";
                Stream gradeRequestStream = gradeRequest.GetRequestStream();

                using (BinaryWriter bw = new BinaryWriter(gradeRequestStream))
                {
                    bw.Write(outgoingQueryString.ToString());
                    gradeRequest.ContentLength = gradeRequestStream.Length;
                    gradeRequestStream.Close();
                }
                HttpWebResponse gradeResponse = (HttpWebResponse)loginRequest.GetResponse();
                Stream gradeResponseStream = gradeResponse.GetResponseStream();
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readGradeStream = new StreamReader(gradeResponseStream, Encoding.UTF8);
                HtmlAgilityPack.HtmlDocument htmlGradeDoc = new HtmlAgilityPack.HtmlDocument();
                htmlDoc.Load(readGradeStream);

                if (htmlGradeDoc.ParseErrors != null && htmlGradeDoc.ParseErrors.Count() > 0)
                {
                    // Handle any parse errors as required
                }
                else
                {
                    if (htmlGradeDoc.DocumentNode != null)
                    {
                        HtmlAgilityPack.HtmlNode bodyNode = htmlGradeDoc.DocumentNode.SelectSingleNode("//body");

                        if (bodyNode != null)
                        {
                            foreach (HtmlNode node in bodyNode.SelectNodes(@"//tr[starts-with(@onclick, ""goassign"")]"))
                            ////*[starts-with(@class,"atag")]
                            {
                                c.assignments.Add(new assignment { })
                                outgoingQueryString.Add(node.GetAttributeValue("name", ""), node.GetAttributeValue("value", null));
                            }
                            foreach (HtmlNode node in bodyNode.SelectNodes("//select[@name='classmenu']//option"))
                            {
                                string name = node.GetAttributeValue("value", "");
                                string name2 = node.InnerHtml;
                                //courses.Add(new course {name = node.GetAttributeValue("value", ""), id = node.GetAttributeValue("name", null));
                                courses.Add(new course { name = node.InnerHtml, classMenuId = node.GetAttributeValue("name", null));
                            }
                        }


        }
    }
}
