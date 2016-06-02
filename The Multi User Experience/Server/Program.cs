using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            HttpListener web = new HttpListener();
            web.Prefixes.Add("http://localhost:8080/");
            Console.WriteLine("Listening..");
            web.Start();
            
            HttpListenerContext context = web.GetContext();
            Console.WriteLine(context);
            HttpListenerRequest request = context.Request;

            Stream output = request.InputStream;
            byte[] buffer = new byte[request.ContentLength64];
            output.Read(buffer, 0, buffer.Length);
            Console.WriteLine(Encoding.UTF8.GetString(buffer));

            Console.WriteLine("What would you like to say back?");
            string ReturnThis = Console.ReadLine();

            Stream SendBack = context.Response.OutputStream;
            buffer = Encoding.UTF8.GetBytes(ReturnThis);
            context.Response.ContentLength64 = buffer.Length;
            SendBack.Write(buffer, 0, buffer.Length);
            
            output.Close();
            web.Stop();
            Console.ReadKey();
        }
    }
}
