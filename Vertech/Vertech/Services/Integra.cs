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
using Vertech.Services;

namespace Vertech.Services
{
    public class Integra
    {

        public Integra()
        {

        }

        public List<string> Listar_arquivos(string ext)
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo dirInfo = new DirectoryInfo(@dir);
            List<string> Arquivo = BuscaArquivos(dirInfo, ext);

            return Arquivo;
        }

        public void Job()
        {
            string dir = Parametros.GetDirArq();
            var L_arq = Listar_arquivos(".txt");

            if(L_arq.Count > 0)
            {
                foreach (var arq_name in L_arq)
                {
                    int ctrl = Verificacao(arq_name);

                    if (ctrl == 0)
                    {
                        integraRequest Request = new integraRequest();
                        Request.esocial = Retorna_Esocial(Retorna_ID(), dir, arq_name);
                        integraResponse Response = Enviar(Request);

                        if (Response.protocolo > 0)
                        {
                            SalvaProtocolo(Response, arq_name);
                            MessageBox.Show("Arquivo " + arq_name + " foi enviado com sucesso!");
                        }
                        else
                            MessageBox.Show("Erro no envio, retorno invalido!");
                    }
                    else if(ctrl == 1)
                    {
                        MessageBox.Show("O arquivo " + arq_name +" já foi enviado!");
                    }
                    else
                        MessageBox.Show("O arquivo " + arq_name + " é invalido");

                }
            }

            else
                MessageBox.Show("A pasta selecionada não contem os arquivos necessários para o envio");

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
                MessageBox.Show(e.Message);
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

        private List<string> BuscaArquivos(DirectoryInfo dir, string ext)
        {
            List<string> File_Names = new List<string>();

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension == ext)
                    {
                        File_Names.Add(file.Name);
                        //MessageBox.Show(file.Name);
                    }
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

        private int Verificacao(string arq)
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo diretorio = new DirectoryInfo(dir);

            int n = arq.Length;
            string name = arq.Remove(n - 3, 3);

            name = string.Concat("prot_", name, "dat");

            foreach (var item in diretorio.GetFiles())
            {
                if(item.Extension == ".dat")
                {
                    if (item.Name == name)
                        return 1;
                }
            }

            int tam = 0;
            string[] lines = LerArquivo(dir, arq);

            tam = lines.Length;

            if (tam >= 3)
            {
                if (lines[0].Contains("|OPEN|") && lines[tam - 1].Contains("|CLOSE|"))
                {
                    return 0;
                }

                else
                {
                    return 2;
                }
            }

            else
            {
                return 2;
            }
        }

        public string[] LerArquivo(string dir, string arq)
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

        private void SalvaProtocolo(integraResponse Response, string arq)
        {
            string dir = Parametros.GetDirArq();
            int n = arq.Length;
            string name = arq.Remove(n - 3, 3);

            name = string.Concat("prot_",name,"dat");

            string s = string.Concat(dir, '\\', name);

            string[] lines = { Convert.ToString(Response.protocolo), Convert.ToString(Response.protocoloSpecified)};
            
            System.IO.File.WriteAllLines(@s, lines);
        }
    }
}
