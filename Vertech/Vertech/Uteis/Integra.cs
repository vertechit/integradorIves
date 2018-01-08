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

        public List<string> Lista_arquivos()
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo dirInfo = new DirectoryInfo(@dir);
            List<string> Arquivo = BuscaArquivos(dirInfo);

            return Arquivo;
        }

        public List<integraResponse> Job()
        {
            string dir = Parametros.GetDirArq();
            var arq = Lista_arquivos();
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
            integraResponse Response = new integraResponse();

            try
            {
                VertechCliente.Open();

                Response = VertechCliente.integraRequest(Request);

                VertechCliente.Close();
            }
            catch(Exception e)
            {
                MessageBox.Show("Erro ao enviar");
            }

            return Response;
        }

        public identificador Retorna_ID()
        {
            identificador Id = new identificador();

            try
            {
                Id.grupo = Parametros.GetGrupo();
                Id.token = Parametros.GetToken();
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao atribuir identificador");
            }

            return Id;
        }

        public esocial Retorna_Esocial(identificador id, string dir, string arq)
        {
            esocial eso = new esocial();

            try
            {
                eso.identificador = id;

                eso.registro = LerArquivo(dir, arq);
            }
            catch(Exception e)
            {
                MessageBox.Show("Erro no retorno do arquivo");
            }

            return eso;
        }

        private List<string> BuscaArquivos(DirectoryInfo dir)
        {
            List<string> File_Names = new List<string>();

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    File_Names.Add(file.Name);
                }

                /*// busca arquivos do proximo sub-diretorio
                foreach (DirectoryInfo subDir in dir.GetDirectories())
                {
                    BuscaArquivos(subDir);
                }*/
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro ao buscar arquivos na pasta indicada");
            }

            return File_Names;
        }

        private string[] LerArquivo(string dir, string arq)
        {
            string[] lines= null;

            string s = string.Concat(dir, '\\', arq);
            try
            {
                lines = System.IO.File.ReadAllLines(@s);
            }
            catch (Exception e)
            {
                MessageBox.Show("Erro na leitura do arquivo");
            }

            return lines;
        }
    }
}
