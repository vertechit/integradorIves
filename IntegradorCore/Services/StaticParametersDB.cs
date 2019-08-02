using IntegradorCore.Modelos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IntegradorCore.Services
{
    public static class StaticParametersDB
    {
        private static List<ParametroDB> ListBanco = new List<ParametroDB>();
        private static ParametroDB current;
        private static string Driver = null;
        private static string Host = null;
        private static string Port = null;
        private static string ServiceName = null;
        private static string User = null;
        private static string Password = null;
        private static string Trusted_Conn = null;
        private static string Id = null;

        public static void SetListBanco(ParametroDB banco)
        {
            ListBanco.Add(banco);
        }
        public static ParametroDB getListBanco(long? id)
        {
            foreach(var b in ListBanco)
            {
                if (id == b.Id)
                    return b;
            }
            return new ParametroDB();
        }

        public static List<ParametroDB> getAllListBanco()
        {
            return ListBanco;
        }

        public static void clearListBanco()
        {
            try
            {
                ListBanco = new List<ParametroDB>();
            }
            catch (Exception e)
            {

            }
        }

        public static void Setcurrent(long? id)
        {
            foreach (var b in ListBanco)
            {
                if (id == b.Id)
                    current = b;
            }

            Processos process = new Processos();

            StaticParametersDB.SetDriver(current.Driver);
            StaticParametersDB.SetHost(current.Host);
            StaticParametersDB.SetPort(current.Port);
            StaticParametersDB.SetServiceName(current.ServiceName);
            StaticParametersDB.SetUser(current.User);
            StaticParametersDB.SetPassword(AESThenHMAC.SimpleDecryptWithPassword(current.Password, process.GetMacAdress()));
            StaticParametersDB.SetTrustedCon(current.Trusted_Conn);
            StaticParametersDB.SetId(current.Id.ToString());
        }

        public static void clearAllStatic()
        {
            current = new ParametroDB();
            Driver = null;
            Host = null;
            Port = null;
            ServiceName = null;
            User = null;
            Password = null;
            Trusted_Conn = null;
            Id = null;

        }
        public static ParametroDB Getcurrent()
        {
            return current;
        }


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
        public static void SetId(string id)
        {
            Id = id;
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

        public static string GetId()
        {
            return Id;
        }
        #endregion

    }
}
