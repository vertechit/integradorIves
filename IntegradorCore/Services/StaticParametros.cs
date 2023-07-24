using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IntegradorCore.Modelos;

namespace IntegradorCore.Services
{
    public static class StaticParametros
    {
        #region Atributos
        //Todos parametros são obrigatorios para o pleno funcionamento do app e serviço
        private static string DirArq = null; //Diretorio de arquivos que serão integrados/Consultados
        private static string DirFim = null; //Diretorio onde será movido os arquivos já enviados e consultados
        private static string DirToke = null; //Diretorio onde está localizado o token
        private static long Grupo = 0; //Deve ser definido com a leitura do arquivo token
        private static string Token = null; //Deve ser definido com a leitura do arquivo token
        private static string TipoApp = null; //Define se está executando um app ou um serviço
        private static long Ambiente = 0; //Ambiente de envio do evento definido pelo usuário
        private static bool Base = false; //Base de dados definida pelo usuário
        private static bool IntegraBanco = false;
        private static bool LockVariavel = false;
        private static readonly string Versao = "22.07.1";
        private static bool GeraLogs = true;
        private static string UrlProd = null;
        private static string UrlQa = null;
        private static string UrlTeste = null;
        #endregion

        #region Param
        private static ParamVPGP VPGP;
        private static ParamVPGT VPGT;
        private static ParamVTGT VTGT;
        private static ParamVPGQ VPGQ;
        private static string DirOrigem = null;

        public static string GetDirOrigem()
        {
            return DirOrigem;
        }

        public static void SetDirOrigem(string dir)
        {
            DirOrigem = dir;
        }

        public static ParamVPGP GetVPGP()
        {
            return VPGP;
        }

        public static void SetVPGP()
        {
            VPGP = new ParamVPGP(string.Concat(GetDirOrigem(), "\\", "IPGP"));
        }

        public static ParamVPGT GetVPGT()
        {
            return VPGT;
        }

        public static void SetVPGT()
        {
            VPGT = new ParamVPGT(string.Concat(GetDirOrigem(), "\\", "IPGT"));
        }

        public static ParamVTGT GetVTGT()
        {
            return VTGT;
        }

        public static void SetVTGT()
        {
            VTGT = new ParamVTGT(string.Concat(GetDirOrigem(), "\\", "ITGT"));
        }

        public static ParamVPGQ GetVPGQ()
        {
            return VPGQ;
        }

        public static void SetVPGQ()
        {
            VPGQ = new ParamVPGQ(string.Concat(GetDirOrigem(), "\\", "IPGQ"));
        }
        #endregion

        #region gets/sets

        public static string GetVersao()
        {
            return Versao;
        }

        public static bool GetLockVariavel()
        {
            return LockVariavel;
        }

        public static void SetLockVariavel(bool lockV)
        {
            LockVariavel = lockV;
        }

        public static bool GetIntegraBanco()
        {
            return IntegraBanco;
        }

        public static void SetIntegraBanco(bool integraBanco)
        {
            IntegraBanco = integraBanco;
        }

        public static bool GetBase()
        {
            return Base;
        }

        public static void SetBase(bool basea)
        {
            Base = basea;
        }

        public static long GetAmbiente()
        {
            return Ambiente;
        }

        public static void SetAmbiente(long ambiente)
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


        public static bool GetGeraLogs()
        {
            return GeraLogs;
        }

        public static void SetGeraLogs(bool gera)
        {
            GeraLogs = gera;
        }

        public static string GetUrlProd()
        {
            return UrlProd;
        }

        public static void SetUrlProd(string a)
        {
            UrlProd = a;
        }

        public static string GetUrlQa()
        {
            return UrlQa;
        }

        public static void SetUrlQa(string a)
        {
            UrlQa = a;
        }

        public static string GetUrlTeste()
        {
            return UrlTeste;
        }

        public static void SetUrlTeste(string a)
        {
            UrlTeste = a;
        }

        #endregion
    }
}
