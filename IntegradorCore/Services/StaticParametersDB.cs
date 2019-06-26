using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Services
{
    public static class StaticParametersDB
    {
        private static string Driver = null;
        private static string Host = null;
        private static string Port = null;
        private static string ServiceName = null;
        private static string User = null;
        private static string Password = null;
        private static string Trusted_Conn = null;

        #region Sets
        public static void SetDriver(string driver)
        {
            Driver = driver;
        }

        public static void SetHost(string host)
        {
            Host = host;
        }

        public static void SetPort(string port)
        {
            Port = port;
        }

        public static void SetServiceName(string serviceName)
        {
            ServiceName = serviceName;
        }

        public static void SetUser(string user)
        {
            User = user;
        }

        public static void SetPassword(string password)
        {
            Password = password;
        }

        public static void SetTrustedCon(string trusted_conn)
        {
            Trusted_Conn = trusted_conn;
        }
        #endregion

        #region Gets
        public static string GetDriver()
        {
            return Driver;
        }

        public static string GetHost()
        {
            return Host;
        }

        public static string GetPort()
        {
            return Port;
        }

        public static string GetServiceName()
        {
            return ServiceName;
        }

        public static string GetUser()
        {
            return User;
        }

        public static string GetPassword()
        {
            return Password;
        }

        public static string GetTrustedConn()
        {
            return Trusted_Conn;
        }

        #endregion

    }
}
