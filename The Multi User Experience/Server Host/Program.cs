using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Net;
using System.Text;

namespace Server_Host
{
    static class Program
    {
        static void Main(string[] args)
        {
            HttpListener web = null;
            try
            {
                web = new HttpListener();
                web.Prefixes.Add("http://localhost:8080/");
                Console.WriteLine("Listening..");
                web.Start();

                HttpListenerContext context = web.GetContext();

                Point SelectedPoint;
                string Input = "";
                while (!GetGridPoint(Input, out SelectedPoint))
                {
                    SendTextToForeignSource(context, "Please enter a coordinate to attack (e.g. 1-8):")
                    GetTextFromForeignSource(context);
                }

                Console.WriteLine("Recieved message at " + DateTime.Now.ToString() + ":" + Environment.NewLine + SelectedPoint.ToString()); //Writes recieved message to console
                string ReturnThis = ""; //Send back message

                SendTextToForeignSource(context, ReturnThis);

                //End session
                Console.ReadKey();
            }
            finally
            {
                if (web != null && web.IsListening)
                    web.Stop();
            }
        }

        static string GetTextFromForeignSource(HttpListenerContext Conversationalist)
        {
            HttpListenerRequest request = Conversationalist.Request;
            byte[] buffer;
            using (Stream output = request.InputStream)
            {
                buffer = new byte[request.ContentLength64];
                output.Read(buffer, 0, buffer.Length);
            }

            return Encoding.UTF8.GetString(buffer);
        }

        static void SendTextToForeignSource(HttpListenerContext Conversationalist, string MessageToSend)
        {
            using (Stream SendBack = Conversationalist.Response.OutputStream)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(MessageToSend);
                Conversationalist.Response.ContentLength64 = buffer.Length;
                SendBack.Write(buffer, 0, buffer.Length);
            }
        }

        static bool GetGridPoint(string Input, out Point SelectedPoint)
        {
            if (Input == null || Input == "")
            {
                SelectedPoint = default(Point);
                return false;
            }

            try
            {
                string[] InputSplit = Input.Split(new char[] { ' ', ',', '-', '.' }, StringSplitOptions.RemoveEmptyEntries);
                if (Input.Length != 2)
                    throw new InvalidCastException();
                else
                {
                    int X = Convert.ToInt32(Input[0]);
                    int Y = Convert.ToInt32(Input[1]);
                    SelectedPoint = new Point(X, Y);
                    return true;
                }
            }
            catch (InvalidCastException)
            {
                SelectedPoint = default(Point);
                return false;
            }
        }
    }
}
