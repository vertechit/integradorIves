using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using FluentNHibernate.Automapping;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using System.IO;

namespace IntegradorCore.NHibernate
{
    public static class AuxiliarNhibernate
    {

        private static ISessionFactory fabricaConexao;
        private static string Conexao = Assembly.GetExecutingAssembly().Location;

        public static void CriarFabricaConexao()
        {
            //var path = Conexao.Replace("\\IntegradorCore.dll", "\\dados.db");

            //string path = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            //path = string.Concat(path, "\\dados.db");

            if (fabricaConexao == null)
            {
                fabricaConexao = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFileWithPassword("C:\\vch\\dados.db", "secret").ShowSql().FormatSql())
                .Mappings(x => x.FluentMappings.AddFromAssembly(Assembly.GetExecutingAssembly()))
                .ExposeConfiguration(GerarSchema)
                .BuildSessionFactory();
            }

        }

        public static void GerarSchema(Configuration cfg)
        {
            new SchemaUpdate(cfg).Execute(true, true);
        }

        public static ISession AbrirSessao()
        {
            CriarFabricaConexao();
            return fabricaConexao.OpenSession();
        }
    }
}
