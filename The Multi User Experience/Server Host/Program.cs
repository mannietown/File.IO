using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace Server_Host
{
    static class Program
    {
        public static bool CloseAll = false;
        public static string AppDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\Battleships";
        public static ErrorLogger MasterErrorLog = new ErrorLogger(AppDataFolder + "\\Errors.txt");

        static Game TheGame;

        static Board BlueBoard = new Board();
        static Board RedBoard = new Board();

        static void Main(string[] args)
        {
            TheGame = new Game();
            HttpListener webBlue = null;
            HttpListener webRed = null;

            try
            {
                webBlue = new HttpListener();
                webBlue.Prefixes.Add("http://localhost:8080/");

                Console.WriteLine("Listening at " + Dns.GetHostAddresses(Dns.GetHostName()).Where(x => x.AddressFamily == AddressFamily.InterNetwork).FirstOrDefault().ToString() + "...");
                webBlue.Start();

                HttpListenerContext context = webBlue.GetContext();

                Point SelectedPoint;
                string Input = "";
                while (!GetGridPoint(Input, out SelectedPoint))
                {
                    context.SendTextToForeignSource("Please enter a coordinate to attack (e.g. 1-8):");
                    Input = context.GetTextFromForeignSource();
                }

                Console.WriteLine("Recieved message at " + DateTime.Now.ToString() + ":" + Environment.NewLine + SelectedPoint.ToString()); //Writes recieved message to console
                string ReturnThis = ""; //Send back message

                context.SendTextToForeignSource(ReturnThis);

                //End session
                Console.ReadKey();
            }
            finally
            {
                if (webBlue != null && webBlue.IsListening)
                    webBlue.Stop();

                if (webRed != null && webRed.IsListening)
                    webRed.Stop();
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

    public class Board : IEnumerable
    {
        public enum PositionStatus : byte
        {
            Empty = 0,
            Occupied,
            Hit,
            Miss
        }

        private PositionStatus[,] ThisBoard;

        public PositionStatus this[int Index]
        {
            get
            {
                lock (ThisBoard)
                    return ThisBoard[Index % 8, Index / 8];
            }
            set
            {
                lock (ThisBoard)
                    ThisBoard[Index % 8, Index / 8] = value;
            }
        }

        public PositionStatus this[int X, int Y]
        {
            get
            {
                lock (ThisBoard)
                    return ThisBoard[X, Y];
            }
            set
            {
                lock (ThisBoard)
                    ThisBoard[X, Y] = value;
            }
        }

        public PositionStatus this[Point p]
        {
            get
            {
                return this[p.X, p.Y];
            }
            set
            {
                this[p.X, p.Y] = value;
            }
        }

        public int Count { get { lock (ThisBoard) return ThisBoard.Length; } }

        /// <summary>
        /// Gets the length of a specified axis (X or Y)
        /// </summary>
        /// <param name="XTrueYFalse">Whether to get the length of the X Axis (true) or the Y Axis (false)</param>
        /// <returns></returns>
        public int AxisLength(bool XTrueYFalse)
        {
            lock (ThisBoard)
                return ThisBoard.GetLength(XTrueYFalse ? 0 : 1);
        }

        public bool Defeated
        {
            get
            {
                if (AllPiecesPlaced)
                {
                    foreach (PositionStatus ps in this)
                    {
                        if (ps == PositionStatus.Occupied)
                            return false;
                    }
                    return true;
                }
                else return false;
            }
        }

        public bool AllPiecesPlaced
        {
            get
            {
                int TotalOnBoard = 0; //Should be 30
                foreach (PositionStatus ps in this)
                {
                    if (ps == PositionStatus.Hit || ps == PositionStatus.Occupied)
                    {
                        TotalOnBoard++;
                    }
                }

                return TotalOnBoard == 30; //1x5, 2x4, 3x3, 4x2
            }
        }

        public Board()
        {
            ThisBoard = new PositionStatus[8, 8];
        }

        public enum PieceAngle : byte
        {
            Up = 0,
            Right,
            Down,
            Left
        }

        /// <summary>
        /// Attempts to place a piece on the board. Return true if successful, false if not.
        /// </summary>
        /// <param name="Coordinate">The coordinate to place the head of the piece</param>
        /// <param name="Angle">The direction the piece's tail is relative to the head</param>
        /// <param name="PieceLength">The length of the piece</param>
        /// <returns></returns>
        public bool PlacePiece(Point Coordinate, PieceAngle Angle, int PieceLength)
        {
            if (AllPiecesPlaced)
                return false;

            switch (Angle)
            {
                case PieceAngle.Up:
                    if (Coordinate.Y + PieceLength > AxisLength(false))
                        return false;
                    else
                    {
                        for (int YCoord = Coordinate.Y; YCoord < Coordinate.Y + PieceLength; YCoord++)
                        {
                            if (this[Coordinate.X, YCoord] != PositionStatus.Empty)
                                return false;
                        }

                        for (int YCoord = Coordinate.Y; YCoord < Coordinate.Y + PieceLength; YCoord++)
                            this[Coordinate.X, YCoord] = PositionStatus.Occupied;

                        return true;
                    }
                case PieceAngle.Down:
                    if (Coordinate.Y - PieceLength < 0)
                        return false;
                    else
                    {
                        for (int YCoord = Coordinate.Y; YCoord > Coordinate.Y - PieceLength; YCoord--)
                        {
                            if (this[Coordinate.X, YCoord] != PositionStatus.Empty)
                                return false;
                        }

                        for (int YCoord = Coordinate.Y; YCoord > Coordinate.Y - PieceLength; YCoord--)
                            this[Coordinate.X, YCoord] = PositionStatus.Occupied;

                        return true;
                    }
                case PieceAngle.Left:
                    if (Coordinate.X - PieceLength < 0)
                        return false;
                    else
                    {
                        for (int XCoord = Coordinate.X; XCoord > Coordinate.X - PieceLength; XCoord--)
                        {
                            if (this[XCoord, Coordinate.Y] != PositionStatus.Empty)
                                return false;
                        }

                        for (int XCoord = Coordinate.X; XCoord > Coordinate.X - PieceLength; XCoord--)
                            this[XCoord, Coordinate.Y] = PositionStatus.Occupied;

                        return true;
                    }
                case PieceAngle.Right:
                    if (Coordinate.X - PieceLength < 0)
                        return false;
                    else
                    {
                        for (int XCoord = Coordinate.X; XCoord < Coordinate.X + PieceLength; XCoord++)
                        {
                            if (this[XCoord, Coordinate.Y] != PositionStatus.Empty)
                                return false;
                        }

                        for (int XCoord = Coordinate.X; XCoord < Coordinate.X + PieceLength; XCoord++)
                            this[XCoord, Coordinate.Y] = PositionStatus.Occupied;

                        return true;
                    }
                default: return false;
            }
        }

        /// <summary>
        /// Attempts to make an attack. Returns whether the move is valid. 
        /// Can return false if the coordinate is already attacked or off the edge of the board.
        /// </summary>
        /// <param name="CoordinateToAttack">The coordinate to fire at</param>
        /// <param name="Hit">Whether the attack hit a ship</param>
        /// <returns></returns>
        public bool ComputeAttack(Point CoordinateToAttack, out bool Hit)
        {
            Hit = false;

            if (CoordinateToAttack.X >= AxisLength(true) || CoordinateToAttack.Y >= AxisLength(false) || CoordinateToAttack.X < 0 || CoordinateToAttack.Y < 0)
                return false;

            if (this[CoordinateToAttack] != PositionStatus.Hit && this[CoordinateToAttack] != PositionStatus.Miss) //Already attacked this coordinate
            {
                //Move is value
                Hit = this[CoordinateToAttack] == PositionStatus.Occupied;

                return true;
            }
            else return false;
        }

        public IEnumerator GetEnumerator()
        {
            lock (this)
            {
                for (int IndexX = 0; IndexX < ThisBoard.GetLength(0); IndexX++)
                {
                    for (int IndexY = 0; IndexY < ThisBoard.GetLength(1); IndexY++)
                    {
                        yield return ThisBoard[IndexX, IndexY];
                    }
                }
            }
        }
    }

    public class Game
    {
        public Board[] GameBoards;

        public enum Side : byte
        {
            None = 255,
            Blue = 0,
            Red
        }

        public enum GameState
        {
            SettingUp,
            Playing,
            GameOver
        }

        public GameState CurrentGameState;

        public int BoardCount { get { return GameBoards.Length; } }

        List<Thread> Players;
        public Board this[Side SidesBoardToRetrieve]
        {
            get
            {
                return GameBoards[(int)SidesBoardToRetrieve];
            }
        }

        public bool GameOver
        {
            get
            {
                foreach (Board b in GameBoards)
                {
                    if (b.Defeated)
                        return true;
                }
                return false;
            }
        }

        public Side CurrentTurn = Side.None;

        public Game()
        {
            GameBoards = new Board[Enum.GetNames(typeof(Side)).Length - 1 /*Exclude None Side*/];
            for (int Index = 0; Index < GameBoards.Length; Index++)
                GameBoards[Index] = new Board();

            CurrentGameState = GameState.SettingUp;

            Players = new List<Thread>();
            Players.Add(new Thread(() => thdGameManager(Side.Blue)));
            Players.Add(new Thread(() => thdGameManager(Side.Red)));

        }

        static void thdGameManager(Side MySide)
        {
            HttpListener Conversationalist = null;

            try
            {
                Conversationalist = new HttpListener();
                Conversationalist.Prefixes.Add("http://localhost:8080/");
                Conversationalist.Start();

                HttpListenerContext MyContext = Conversationalist.GetContext();
                MyContext.GetTextFromForeignSource();
            }
            catch (Exception ex)
            {
                Program.MasterErrorLog.LogError(ex, "Unknown error occured in Game Manager thread");
            }
            finally
            {
                if (Conversationalist != null && Conversationalist.IsListening)
                    Conversationalist.Stop();
            }
        }
    }

    public static class ExtensionClass
    {
        public static string GetTextFromForeignSource(this HttpListenerContext Conversationalist)
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

        public static void SendTextToForeignSource(this HttpListenerContext Conversationalist, string MessageToSend)
        {
            using (Stream SendBack = Conversationalist.Response.OutputStream)
            {
                byte[] buffer = Encoding.UTF8.GetBytes(MessageToSend);
                Conversationalist.Response.ContentLength64 = buffer.Length;
                SendBack.Write(buffer, 0, buffer.Length);
            }
        }
    }

    public class Error
    {
        public Exception ex { get; set; }
        public string ShortMessage { get; set; }
        public DateTime DateTimeOccurred { get; set; }

        private static ConcurrentQueue<Error> ErrorQueue = new ConcurrentQueue<Error>();

        public Error(Exception ex, string ShortMessage)
        {
            this.ex = ex;
            this.ShortMessage = ShortMessage;
            DateTimeOccurred = DateTime.Now;
        }

        public override string ToString()
        {
            return "New error occured on " + DateTimeOccurred.ToShortDateString() + " at " +
                DateTimeOccurred.ToShortTimeString() + Environment.NewLine +
            "Error description:" + ShortMessage + Environment.NewLine +
            "Error details: " + ex.ToString() + Environment.NewLine + Environment.NewLine;
        }
    }

    public class ErrorLogger
    {
        /// <summary>
        /// How often to check if the UI thread is still running (mutliply by 50 to get the number of milliseconds between refreshes)
        /// </summary>
        private const int UIExistenceCheckRefreshRate = 10;
        public static bool CloseAllLogs = false;

        private string FileLocation { get; set; }
        private Thread thdErrorLogger;
        public bool ExitLog
        {
            get
            {
                return _ExitLog || CloseAllLogs;
            }
            set
            {
                _ExitLog = value;
            }
        }

        private bool _ExitLog = false;

        public ErrorLogger(string FileLocation)
        {
            this.FileLocation = FileLocation;

            thdErrorLogger = new Thread(ErrorLoggerMethod);
            thdErrorLogger.Start();
        }

        private ConcurrentQueue<Error> ExQueue = new ConcurrentQueue<Error>();

        private void ErrorLoggerMethod()
        {
            string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            using (StreamWriter writer = new StreamWriter(FileLocation))
            {
                int ThreadCountBounce = 0; //Check if UI is still there every so often

                while (!ExitLog && (ThreadCountBounce % UIExistenceCheckRefreshRate != 0 || Process.GetCurrentProcess().MainWindowHandle != null))
                {
                    ThreadCountBounce = 0;
                    if (ExQueue.Count > 0)
                    {
                        Error LatestError;
                        if (ExQueue.TryDequeue(out LatestError))
                            writer.WriteLine(LatestError.ToString());
                    }
                    else
                    {
                        Thread.Sleep(50);
                        if (ThreadCountBounce >= UIExistenceCheckRefreshRate)
                            ThreadCountBounce = 0;
                        else ThreadCountBounce++;
                    }
                }
            }
        }

        public void LogError(Error ex)
        {
            ExQueue.Enqueue(ex);
        }

        public void LogError(Exception ex, string ShortMessage)
        {
            LogError(new Error(ex, ShortMessage));
        }
    }
}
