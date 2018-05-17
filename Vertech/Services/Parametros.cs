using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertech.Services
{
    public static class Parametros
    {
        private static string DirArq = null; //Diretorio de arquivos que serão integrados/Consultados
        private static string DirFim = null;
        private static string DirToke = null;
        private static long Grupo = 0;
        private static string Token = null;
        private static string TipoApp = null;
        private static string Ambiente = null;


        public static string GetAmbiente()
        {
            return Ambiente;
        }

        public static void SetAmbiente(string ambiente)
        {
            Ambiente = ambiente;
        }

        public static string GetTipoApp()
        {
            return TipoApp;
        }

        public static void SetTipoApp(string tipo)
        {
            TipoApp = tipo;
        }

        public static string GetDirToke()
        {
            return DirToke;
        }

        public static void SetDirToke(string dir)
        {
            DirToke = dir;
        }

        public static string GetDirFim()
        {
            return DirFim;
        }

        public static void SetDirFim(string dir)
        {
            DirFim = dir;
        }

        public static string GetDirArq()
        {
            return DirArq;
        }

        public static void SetDirArq(string dir)
        {
            DirArq = dir;
        }

        public static long GetGrupo()
        {
            return Grupo;
        }

        public static void SetGrupo(long grupo)
        {
            Grupo = grupo;
        }

        public static string GetToken()
        {
            return Token;
        }

        public static void SetToken(string token)
        {
            Token = token;
        }
    }
}
