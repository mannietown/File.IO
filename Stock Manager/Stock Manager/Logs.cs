using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Threading;
using System.Windows.Forms;
using UserManagement;

namespace Stock_Manager
{
    public static class Error_Log
    {
        private static readonly string FileLocation = Program.SharedAppDataFolder + "\\Error Logs.txt";

        private static Thread thdWriter = new Thread(ThreadWriterMethod);
        private static Queue<Exception> ErrorQueue = new Queue<Exception>();

        public enum ShowError
        {
            ShowNone,
            ShowUserFriendlyMessage,
            ShowFull
        }

        private static void ThreadWriterMethod()
        {
            try
            {
                string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));

                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);
                Folder = null;

                using (StreamWriter stream = new StreamWriter(FileLocation, true)) //Minimises errors by keeping the stream always open
                {
                    while (!Program.CloseAll)
                    {
                        if (ErrorQueue.Count > 0)
                        {
                            stream.WriteLine("New error occured at: " + DateTime.Now);

                            try
                            {
                                stream.WriteLine("User: " + Environment.UserDomainName + "\\" + Environment.UserName);
                            }
                            catch (Exception ex) when (ex is PlatformNotSupportedException || ex is InvalidOperationException)
                            {
                                stream.WriteLine("User:" + Environment.UserName);
                            }

                            stream.Write(ErrorQueue.Dequeue().ToString() + Environment.NewLine);

                            stream.WriteLine(""); //Paragragh seperator to make reading logs easier later
                        }
                        else Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    MessageBox.Show("Fatal error with error logs:" + Environment.NewLine + ex.Message + Environment.NewLine + ex.ToString(),
                        "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                catch
                {
                    //It's all over
                    throw ex; //Exit the entire process forcefully. Can be dangerous, but at this point everything is broken anyway :(
                }
                finally
                {
                    Process CurrentProcess = Process.GetCurrentProcess();
                    CurrentProcess.CloseMainWindow();
                    Program.CloseAll = true;
                }
            }
        }

        /// <summary>
        /// Logs the error, then optionally shows the error to the user
        /// </summary>
        /// <param name="ex">The error to log</param>
        /// <param name="ShowToUser">Whether to show the error to the user</param>
        public static void LogError(Exception ex, ShowError ShowToUser)
        {
            LogError(ex, ShowToUser, MessageBoxButtons.OK);
        }

        /// <summary>
        /// Logs the error, then optionally shows the error to the user. Returns the user's response to the error
        /// </summary>
        /// <param name="ex">The error to log</param>
        /// <param name="ShowToUser">Whether to show the error to the user</param>
        /// <param name="ButtonsToShow">What options to give the user in response to the error (e.g. retry)</param>
        /// <returns></returns>
        public static DialogResult LogError(Exception ex, ShowError ShowToUser, MessageBoxButtons ButtonsToShow)
        {
            try
            {
                ErrorQueue.Enqueue(ex);

                if (ShowToUser == ShowError.ShowFull)
                {
                    return MessageBox.Show("We've experienced an unexpected error during operation:" + Environment.NewLine
                        + ex.ToString(), "Error", ButtonsToShow, MessageBoxIcon.Error);
                }
                else if (ShowToUser == ShowError.ShowUserFriendlyMessage)
                {
                    return MessageBox.Show("Error:" + Environment.NewLine
                        + ex.Message, "Error", ButtonsToShow, MessageBoxIcon.Error);
                }
                else //Show none
                    return DialogResult.OK;
            }
            catch (Exception)
            {
                //It's over. Go home. There's nothing more to do.
                throw;
            }
        }
    }

    class ActivityLog
    {
        private static readonly string FileLocation = Program.SharedAppDataFolder + "\\User Logs.txt";

        private static Thread thdWriter = new Thread(ThreadWriterMethod);
        private static Queue<string> ActivityQueue = new Queue<string>();

        private static void ThreadWriterMethod()
        {
            try
            {
                string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));

                if (!Directory.Exists(Folder))
                    Directory.CreateDirectory(Folder);
                Folder = null;

                using (StreamWriter stream = new StreamWriter(FileLocation, true)) //Minimises errors by keeping the stream always open
                {
                    while (!Program.CloseAll)
                    {
                        if (ActivityQueue.Count > 0)
                        {
                            stream.WriteLine("New activity occured at: " + DateTime.Now);
                            try
                            {
                                stream.WriteLine("Windows User ID: " + Environment.UserDomainName + "\\" + Environment.UserName);
                            }
                            catch (Exception ex) when (ex is PlatformNotSupportedException || ex is InvalidOperationException)
                            {
                                stream.WriteLine("Windows User ID: " + Environment.UserName);
                            }

                            stream.WriteLine("User ID: " + User.CurrentUser.LoginID + " (" + User.CurrentUser.Firstname + " " + User.CurrentUser.Surname);

                            #region WriteTraceToPC (So that logs can trace which PC made the change)
                            //Network information (IPv4 address, MAC Address)
                            string IPAddress = null;
                            try
                            {
                                IPAddress = "IP Address: " + (Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(
                                    ip => ip.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)).ToString();
                            }
                            catch (Exception ex) when (ex is ArgumentNullException || ex is ArgumentOutOfRangeException || ex is SocketException || ex is ArgumentException)
                            {
                                IPAddress = "IP Address: Error";
                            }
                            finally
                            {
                                stream.WriteLine(IPAddress);
                            }

                            string MACAddress = null;
                            try
                            {
                                MACAddress = "MAC Address: " + NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up).
                                    Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault();
                            }
                            catch (Exception ex) when (ex is NetworkInformationException || ex is ArgumentNullException)
                            {
                                MACAddress = "MAC Address: Error";
                            }
                            finally
                            {
                                stream.WriteLine(MACAddress);
                            }
                            #endregion

                            stream.Write(ActivityQueue.Dequeue() + Environment.NewLine);

                            stream.WriteLine(""); //Paragragh seperator to make reading logs easier later
                        }
                        else Thread.Sleep(50);
                    }
                }
            }
            catch (Exception ex)
            {
                try
                {
                    Error_Log.LogError(new Exception("Error logging user's activity data", ex), Error_Log.ShowError.ShowUserFriendlyMessage);
                }
                finally
                {
                    Process CurrentProcess = Process.GetCurrentProcess();
                    CurrentProcess.CloseMainWindow();
                    Program.CloseAll = true;
                }
            }
        }

        /// <summary>
        /// Logs the activity
        /// </summary>
        /// <param name="Action">The activity to log</param>
        public static void LogActivity(string Action)
        {
            ActivityQueue.Enqueue(Action);
        }
    }
}
