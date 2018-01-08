using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vertech.Uteis
{
    public static class Parametros
    {
        private static string DirArq = null; //Diretorio de arquivos que serão integrados
        private static long Grupo;
        private static string Token = null;


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
