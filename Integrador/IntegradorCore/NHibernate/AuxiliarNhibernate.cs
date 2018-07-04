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

namespace IntegradorCore.NHibernate
{
    public static class AuxiliarNhibernate
    {

        private static ISessionFactory fabricaConexao;

        public static void CriarFabricaConexao()
        {
            if (fabricaConexao == null)
            {
                fabricaConexao = Fluently.Configure()
                .Database(SQLiteConfiguration.Standard.UsingFile("c:\\vch\\dados.db").ShowSql().FormatSql())
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
