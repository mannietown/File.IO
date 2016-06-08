using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

//TODO Clean. Seriously this is a mess. The heirarchy makes sense but it's so much effort to use!
namespace UserManagement
{
    [Serializable]
    public class User
    {
        public static readonly User SystemDefaultAdmin = new User("SysAdmin", "System", "SysAdmin", null, Permissions.GenerateDefault(Permissions.PermissionLevel.FullAccess));
        public static User CurrentUser;

        Permissions mypermissions; //TODO Encrypt permissions when saving

        //TODO Create default permissions
        public Permissions MyPermissions { get { return mypermissions; } }

        private int? UID { get; set; }

        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string LoginID { get; set; }
        public byte[] Password { get; set; }

        //TODO Add user clock (login time, logout time)

        /// <summary>
        /// Auto-hash string, then set password to the result
        /// </summary>
        public string PasswordString { set { Password = new SHA512Managed().ComputeHash(Encoding.UTF8.GetBytes(value)); } }

        private static string FileLocation = Stock_Manager.Program.AppDataFolder + "\\Users.dat";

        /// <summary>
        /// For creating a new user
        /// </summary>
        /// <param name="Permissions">The permissions and access privelages associated with the user</param>
        public User(string Firstname, string Surname, string LoginID, string Password, Permissions MyPermissions)
        {
            this.UID = NextAvailableUID();

            this.Firstname = Firstname;
            this.Surname = Surname;
            this.LoginID = LoginID;

            PasswordString = Password;

            mypermissions = MyPermissions;
        }

        private User(int UID, string Firstname, string Surname, string LoginID, byte[] Password, Permissions MyPermissions)
        {
            this.UID = UID;
            this.Firstname = Firstname;
            this.Surname = Surname;
            this.LoginID = LoginID;

            this.Password = Password;

            mypermissions = MyPermissions;
        }

        public static bool Login(string Username, string Password)
        {
            if (!File.Exists(FileLocation))
                throw new FileNotFoundException("The users data file could not be found. Please restart this program");

            using (StreamReader reader = new StreamReader(FileLocation))
            {
                string Dataline = reader.ReadLine();

                while (Dataline != null)
                {
                    List<Permissions.SiteAccess> SitesAccess = new List<Permissions.SiteAccess>();
                    //Skip past permissions
                    while (Dataline != null && Dataline != "")
                    {
                        //TODO Load permissions

                        Dataline = reader.ReadLine();
                    }

                    if (Dataline == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    //UID
                    Dataline = reader.ReadLine();
                    if (Dataline == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");
                    int UID;

                    try
                    {
                        UID = Convert.ToInt32(Dataline);
                    }
                    catch (InvalidCastException ex)
                    {
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt", ex);
                    }

                    //First name
                    string Firstname = reader.ReadLine();
                    if (Firstname == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    //Surname
                    string Surname = reader.ReadLine();
                    if (Surname == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    string FileUsername = reader.ReadLine();
                    if (FileUsername == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    //Username
                    if (Username == FileUsername)
                    {
                        //Password
                        Dataline = reader.ReadLine();
                        if (Dataline == null)
                            throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                        using (SHA512 shaM = new SHA512Managed())
                        {
                            byte[] Hash = shaM.ComputeHash(Encoding.UTF8.GetBytes(Password));
                            if (Encoding.UTF8.GetBytes(Dataline) == Hash)
                            {
                                //Success - user logged in
                                CurrentUser = new User(UID, Firstname, Surname, Username, Hash, new Permissions(SitesAccess));
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                    else
                    {
                        //else skip to next user

                        //Password
                        Dataline = reader.ReadLine();
                        if (Dataline == null)
                            throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");
                    }

                    //Next user
                }

                return false;
            }
        }

        /// <summary>
        /// Saves a new user. If the UID is taken, generates a new one for the user. If the save fails because the LoginID is already in use, it returns false.
        /// </summary>
        /// <param name="UserToSave"></param>
        /// <returns></returns>
        public static bool SaveNewUser(User UserToSave)
        {
            List<User> OldUsers = LoadAllUsers();

            foreach (User u in OldUsers)
            {
                if (u.UID == UserToSave.UID)
                    UserToSave.UID = NextAvailableUID();

                if (u.LoginID == UserToSave.LoginID)
                    return false;
            }

            //Save new user
            using (StreamWriter stream = new StreamWriter(FileLocation, true))
            {
                foreach (Permissions.SiteAccess sa in UserToSave.MyPermissions.SitePermissions)
                {
                    stream.WriteLine(sa.ToString()); //TODO Save permissions
                }
                stream.WriteLine(""); //Mark end of permissions

                stream.WriteLine(UserToSave.UID.ToString());

                stream.WriteLine(UserToSave.Firstname);

                stream.WriteLine(UserToSave.Surname);

                stream.WriteLine(UserToSave.LoginID);

                stream.WriteLine(Encoding.UTF8.GetString(UserToSave.Password));
            }

            return true;
        }

        /// <summary>
        /// Updates an existing user with new user information. Returns false if the old user could not be found
        /// </summary>
        /// <param name="UserToUpdate"></param>
        /// <returns></returns>
        public static bool UpdateSavedUser(User UserToUpdate)
        {
            List<User> OldUsers = LoadAllUsers();

            bool UserFound = false;

            for (int UserIndex = 0; UserIndex < OldUsers.Count; UserIndex++)
                if (OldUsers[UserIndex].UID == UserToUpdate.UID)
                {
                    OldUsers[UserIndex] = UserToUpdate;
                    UserFound = true;
                    break;
                }

            if (!UserFound)
                return false;

            SaveAllUsers(OldUsers);
            return true;
        }

        private static int NextAvailableUID()
        {
            List<User> AllUsers = LoadAllUsers();
            AllUsers.OrderBy(x => x.UID);

            for (int Index = 0; Index < AllUsers.Count; Index++)
                if (AllUsers[Index].UID != Index)
                    return Index;

            return AllUsers.Count + 1;
        }

        /// <summary>
        /// Loads all users excluding their passwords
        /// </summary>
        /// <returns></returns>
        public static List<User> LoadAllUsers()
        {
            if (!File.Exists(FileLocation))
                throw new FileNotFoundException("The users data file could not be found. Please restart this program");

            List<User> ReturnValue = new List<User>();

            using (StreamReader reader = new StreamReader(FileLocation))
            {
                string Dataline = reader.ReadLine();

                while (Dataline != null)
                {
                    List<Permissions.SiteAccess> Permissions = new List<Permissions.SiteAccess>();

                    //Load permissions
                    while (Dataline != null && Dataline != "")
                    {
                        //TODO Load permissions

                        Dataline = reader.ReadLine();
                    }

                    if (Dataline == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    //UID
                    Dataline = reader.ReadLine();
                    if (Dataline == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");
                    int UID;

                    try
                    {
                        UID = Convert.ToInt32(Dataline);
                    }
                    catch (InvalidCastException ex)
                    {
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt", ex);
                    }

                    //First name
                    string Firstname = reader.ReadLine();
                    if (Firstname == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    //Surname
                    string Surname = reader.ReadLine();
                    if (Surname == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    //Username
                    string LoginID = reader.ReadLine();
                    if (LoginID == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    //Password
                    byte[] Password = Encoding.UTF8.GetBytes(reader.ReadLine());
                    if (Password == null)
                        throw new FileLoadException("Data loading error whereby the users data file appears to be corrupt");

                    ReturnValue.Add(new User(UID, Firstname, Surname, LoginID, Password, new UserManagement.Permissions(Permissions)));
                    //Next user
                }
            }

            return ReturnValue;
        }

        /// <summary>
        /// Replaces the old users with a new list of users. Use with caution
        /// </summary>
        /// <param name="UsersToSave"></param>
        private static void SaveAllUsers(List<User> UsersToSave)
        {
            string Folder = FileLocation.Substring(0, FileLocation.LastIndexOf('\\'));
            if (!Directory.Exists(Folder))
                Directory.CreateDirectory(Folder);

            File.Create(FileLocation);

            foreach (User u in UsersToSave)
                SaveNewUser(u);
        }

        /// <summary>
        /// Gets whether the user has the required permissions for an activity
        /// </summary>
        /// <param name="AreaToCheck">The area to check whether the user has access to</param>
        /// <param name="MinimumPermission">The minimum access the user needs</param>
        /// <returns></returns>
        public bool HasAccess(Permissions.SiteAccess.AreaOfAccess.AccessArea AreaToCheck, Stock_Manager.Stock StockToAccess, Permissions.PermissionLevel MinimumPermission)
        {
            foreach (Permissions.SiteAccess sa in MyPermissions)
            {
                if (sa.SiteID == StockToAccess.SiteID)
                {
                    foreach(var aa in sa.AreasOfAccess)
                    {
                        if (AreaToCheck == aa.Area)
                        {
                            return (byte)aa.pl >= (byte)MinimumPermission;
                        }
                    }
                    return false;
                }
            }

            return false; //Area of Access not found. Could be an old user, or invalid AreaToCheck value.
        }

        /// <summary>
        /// Gets whether the user has the required permissions for an activity
        /// </summary>
        /// <param name="AreaToCheck">The area to check whether the user has access to</param>
        /// <param name="StocksToAccess">The stocks to check whether the user has access. Returns true if the user has access to any - not all</param>
        /// <param name="MinimumPermission">The minimum access the user needs</param>
        /// <returns></returns>
        public bool HasAccess(Permissions.SiteAccess.AreaOfAccess.AccessArea AreaToCheck, List<Stock_Manager.Stock> StocksToAccess, Permissions.PermissionLevel MinimumPermission)
        {
            foreach(Stock_Manager.Stock s in StocksToAccess)
            {
                if (HasAccess(AreaToCheck, s, MinimumPermission))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Gets whether the user has the required permissions for an activity
        /// </summary>
        /// <param name="AreaToCheck">The area to check whether the user has access to</param>
        /// <param name="StocksToAccess">The stocks to check whether the user has access. Returns true if the user has access to all - not any</param>
        /// <param name="MinimumPermission">The minimum access the user needs</param>
        /// <returns></returns>
        public bool HasAccessToAll(Permissions.SiteAccess.AreaOfAccess.AccessArea AreaToCheck, List<Stock_Manager.Stock> StocksToAccess, Permissions.PermissionLevel MinimumPermission)
        {
            foreach (Stock_Manager.Stock s in StocksToAccess)
            {
                if (!HasAccess(AreaToCheck, s, MinimumPermission))
                    return false;
            }

            return true;
        }

        public UnauthorizedAccessException PermissionDeniedMessage(string SiteTryingToAccess, Permissions.SiteAccess.AreaOfAccess.AccessArea AreaTryingToAccess, string Action, Permissions.PermissionLevel PermissionsNeeded)
        {
            Permissions.PermissionLevel pl = 0;
            bool plFound = false;

            foreach (Permissions.SiteAccess sa in MyPermissions)
            {
                if (sa.SiteID == SiteTryingToAccess)
                {
                    foreach (Permissions.SiteAccess.AreaOfAccess aa in sa.AreasOfAccess)
                    {
                        if (aa.Area == AreaTryingToAccess)
                        {
                            pl = aa.pl;
                            plFound = true;
                            break;
                        }
                    }
                }
            }

            if (!plFound)
                pl = Permissions.PermissionLevel.NoAccess;

            return new UnauthorizedAccessException(
                "The current user (" + User.CurrentUser.LoginID +
                ") does not have the required permissions (" +
                Permissions.PermissionLevelToString(PermissionsNeeded) + ") for the requested action (" +
                Action + " in " + Permissions.SiteAccess.AreaOfAccess.AccessAreaToString(AreaTryingToAccess) + ")." + Environment.NewLine +
                "The user's permissions in this area are restricted to " + Permissions.PermissionLevelToString(pl));
        }


    }

    [Serializable]
    public class Permissions : IEnumerable
    {
        public List<SiteAccess> SitePermissions { get; set; }

        /// <summary>
        /// Creates a new Permissions entry with no permissions granted
        /// </summary>
        public Permissions()
        {
            SitePermissions = new List<SiteAccess>();
        }

        /// <summary>
        /// Creates a new Permissions entry from a predefined arrangement of SiteAccesses
        /// </summary>
        /// <param name="SitePermissions"></param>
        public Permissions(List<SiteAccess> SitePermissions)
        {
            this.SitePermissions = SitePermissions;
        }

        /// <summary>
        /// Loads encrypted permissions
        /// </summary>
        /// <param name="EncryptedPermissions">The encrypted permissions to load</param>
        public Permissions(byte EncryptedPermissions)
        {
            //TODO Populate

        }

        public static Permissions GenerateDefault(List<string> Sites, PermissionLevel pl) //TODO Generate default user for the built-in admin
        {
            List<SiteAccess> SiteAccessToSet = new List<SiteAccess>();
            foreach (string site in Sites)
                SiteAccessToSet.Add(new SiteAccess(site, pl));

            return new Permissions(SiteAccessToSet);
        }

        public static Permissions GenerateDefault(PermissionLevel PermissionsToGrant)
        {
            return GenerateDefault(PermissionsToGrant, SiteAccess.GetAllSites());
        }

        public static Permissions GenerateDefault(PermissionLevel PermissionToGrant, List<string> SitesToGrantAccess)
        {
            List<SiteAccess> SitesToGrant = new List<SiteAccess>();
            foreach (string Site in SitesToGrantAccess)
            {
                SitesToGrant.Add(new SiteAccess(Site, PermissionToGrant));
            }
            return new Permissions(SitesToGrant);
        }

        public enum PermissionLevel : byte
        {
            /// <summary>
            /// No access privelages granted. Cannot access this area of the system.
            /// </summary>
            NoAccess = 0,

            /// <summary>
            /// Can read this area of the system, but cannot make changes
            /// </summary>
            ReadOnly,

            /// <summary>
            /// Can read and edit this area of the system, but cannot delete or create entries
            /// </summary>
            ReadAndEdit,

            /// <summary>
            /// Full access to this area of the system
            /// </summary>
            FullAccess
        }

        public static string PermissionLevelToString(PermissionLevel pl)
        {
            switch (pl)
            {
                case PermissionLevel.NoAccess:
                    return "No Access";
                case PermissionLevel.ReadOnly:
                    return "Read Only";
                case PermissionLevel.ReadAndEdit:
                    return "Read and Edit";
                case PermissionLevel.FullAccess:
                    return "Full Access";
                default:
                    throw new ArgumentOutOfRangeException("PermissionLevel ID: " + (byte)pl + " not found");
            }
        }

        public IEnumerator GetEnumerator()
        {
            return SitePermissions.GetEnumerator();
        }

        [Serializable]
        public class SiteAccess
        {
            public string SiteID { get { return siteid; } }
            string siteid;
            private AreaOfAccess[] areasofaccess;
            public AreaOfAccess[] AreasOfAccess { get { return areasofaccess; } }

            public SiteAccess(string SiteID, PermissionLevel pl)
            {
                siteid = SiteID;
                areasofaccess = new AreaOfAccess[AreaOfAccess.AccessAreasCount()];

                for(byte Index = 0; Index < areasofaccess.Length; Index++)
                    areasofaccess[Index] = new AreaOfAccess((AreaOfAccess.AccessArea)Index, pl);
            }

            public SiteAccess(string SiteID, AreaOfAccess[] AreasThisUserCanAccess)
            {
                siteid = SiteID;
                areasofaccess = AreasThisUserCanAccess;
            }

            public bool HasAccess
            {
                get
                {
                    foreach(AreaOfAccess aa in AreasOfAccess)
                    {
                        if ((byte)aa.pl  > (byte)PermissionLevel.NoAccess) //Permission greater than no access
                        {
                            return true;
                        }
                    }
                    return false;
                }
            }

            public static List<string> GetAllSites()
            {
                throw new NotImplementedException();
            }

            public static void CreateNewSite(string SiteID)
            {
                throw new NotImplementedException();
            }

            public override string ToString()
            {
                return SiteID;
            }

            [Serializable]
            public class AreaOfAccess
            {
                public enum AccessArea : byte
                {
                    /// <summary>
                    /// Access to edit, view, create or delete users
                    /// </summary>
                    Users = 0,

                    /// <summary>
                    /// Access to the admin panel section which includes things like login times for users
                    /// </summary>
                    AdminSettings,

                    /// <summary>
                    /// Immediate orders such as over-the-counter orders
                    /// </summary>
                    ImmediateOrders,

                    /// <summary>
                    /// Scheduled outgoing customer orders
                    /// </summary>
                    OutgoingOrders,

                    /// <summary>
                    /// Scheduled incoming orders, such as stock bolstering orders
                    /// </summary>
                    IncomingOrders,

                    /// <summary>
                    /// Current item stocks
                    /// </summary>
                    ItemStocks
                }

                public AccessArea Area { get; set; }
                public PermissionLevel pl { get; set; }

                public AreaOfAccess(AccessArea Area, PermissionLevel pl)
                {
                    this.Area = Area;
                    this.pl = pl;
                }

                public static byte AccessAreasCount()
                {
                    return (byte)Enum.GetNames(typeof(AreaOfAccess.AccessArea)).Length;
                }

                public string AccessAreaToString()
                {
                        return AccessAreaToString(Area);
                }

                public static string AccessAreaToString(AccessArea area)
                {
                    switch (area)
                    {
                        case AccessArea.Users:
                            return "Logins";
                        case AccessArea.AdminSettings:
                            return "Administrator Settings";
                        case AccessArea.ImmediateOrders:
                            return "Over-The-Counter Orders";
                        case AccessArea.IncomingOrders:
                            return "Stock Orders";
                        case AccessArea.OutgoingOrders:
                            return "Customer Delivery Orders";
                        case AccessArea.ItemStocks:
                            return "Stock Management";
                        default:
                            throw new ArgumentOutOfRangeException("AccessArea ID: " + (byte)area + " not found");
                    }
                }

                public string PermissionLevelToString()
                {
                    return Permissions.PermissionLevelToString(pl);
                }

                public override string ToString()
                {
                    return ((byte)Area).ToString() + "\a" + ((byte)pl).ToString();
                }
            }
        }
    }

    [Serializable]
    public class UserGroup
    {
        //TODO Populate

        Permissions UserGroupPermissions { get; set; }
        string GroupID { get; set; }

        public UserGroup(string GroupID, Permissions UserGroupPermissions)
        {
            this.GroupID = GroupID;
            this.UserGroupPermissions = UserGroupPermissions;
        }
    }
}
