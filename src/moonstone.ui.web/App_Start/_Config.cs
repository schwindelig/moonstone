namespace moonstone.ui.web
{
    public class Config
    {
        public Connection Connection { get; set; }

        public Routes Routes { get; set; }

        public static Config Get()
        {
            return new Config()
            {
                Connection = new Connection()
                {
                    Server = ".",
                    Database = "moonstone_dev",
                    Username = "moonstone_ui",
                    Password = "moonstone_ui"
                },
                Routes = new Routes()
            };
        }
    }

    public class Connection
    {
        public string Database { get; set; }
        public string Password { get; set; }
        public string Server { get; set; }
        public string Username { get; set; }
    }

    public class Routes
    {
        public string Error = "error";
        public string Home = "home";
        public string Logout = "logout";

        public static Routes Get()
        {
            return new Routes();
        }
    }
}