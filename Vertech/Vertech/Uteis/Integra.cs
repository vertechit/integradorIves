using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiIntegra;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Xml;
using System.Windows;
using Vertech.Uteis;

namespace Vertech.Uteis
{
    public class Integra
    {

        public Integra()
        {

        }

        public List<string> Lista_arq()
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo dirInfo = new DirectoryInfo(@dir);
            List<string> Arquivo = BuscaArquivos(dirInfo);

            return Arquivo;
        }

        public List<integraResponse> Job()
        {
            string dir = Parametros.GetDirArq();
            var arq = Lista_arq();
            var Lista = new List<integraResponse>();

            foreach (var arq_name in arq)
            {
                integraRequest Request = new integraRequest();
                Request.esocial = Retorna_Esocial(Retorna_ID(), dir, arq_name);
                Lista.Add(Enviar(Request));
            }

            return Lista;
        }

        public integraResponse Enviar(integraRequest Request)
        {
            EsocialServiceClient VertechCliente = new EsocialServiceClient();
            //identificador Id = this.Retorna_ID();
            //esocial Eso = this.Retorna_Esocial(Id, dir, arq);

            //integraRequest1 Integra1 = new integraRequest1();
            //integraRequest Integra = new integraRequest();

            //Integra.esocial = Eso;
            //Integra1.integraRequest = Integra;


            integraResponse Response = new integraResponse();
            //integraResponse1 Response1 = new integraResponse1();

            //Response1.integraResponse = Response;

            VertechCliente.Open();

            Response = VertechCliente.integraRequest(Request);

            VertechCliente.Close();

            return Response;
        }

        public identificador Retorna_ID()
        {
            identificador Id = new identificador();

            Id.grupo = Parametros.GetGrupo();
            Id.token = Parametros.GetToken();

            return Id;
        }

        public esocial Retorna_Esocial(identificador id, string dir, string arq)
        {
            esocial eso = new esocial();

            eso.identificador = id;

            eso.registro = LerArquivo(dir, arq);

            return eso;
        }

        private List<string> BuscaArquivos(DirectoryInfo dir)
        {
            List<string> File_Names = new List<string>();

            foreach (FileInfo file in dir.GetFiles())
            {
                File_Names.Add(file.Name);
            }

            /*// busca arquivos do proximo sub-diretorio
            foreach (DirectoryInfo subDir in dir.GetDirectories())
            {
                BuscaArquivos(subDir);
            }*/

            return File_Names;
        }

        private string[] LerArquivo(string dir, string arq)
        {
            string[] lines= null;

            string s = string.Concat(dir, '\\', arq);

            lines = System.IO.File.ReadAllLines(@s);

            return lines;
        }
    }
}
