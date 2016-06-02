using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Client
{
    class Program
    {
        static void Main(string[] args)
        {
            WebRequest wr = WebRequest.Create("http://localhost:8080/");
            //wr.Credentials = new NetworkCredential("user", "pass1");
            wr.Method = "POST";
            wr.ContentType = "application/x-www-form-urlencoded";

            Console.WriteLine("What do you want to send?");
            string SendThis = Console.ReadLine();
            byte[] buffer = Encoding.UTF8.GetBytes(SendThis);
            wr.ContentLength = buffer.Length;
            Stream s = wr.GetRequestStream();
            s.Write(buffer, 0, buffer.Length);
            Console.WriteLine("Sent. Awaiting reponse... (BETA)");

            WebResponse response = wr.GetResponse();
            Stream s2 = response.GetResponseStream();
            buffer = new byte[response.ContentLength];
            s2.Read(buffer, 0, buffer.Length);
            Console.Write(Encoding.UTF8.GetString(buffer));

            Console.ReadLine();
        }
    }
}
