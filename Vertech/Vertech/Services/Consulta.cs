using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiConsulta;
//using Vertech.apiIntegra;
using Vertech.Services;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Windows;

namespace Vertech.Services
{
    public class Consulta
    {
        public void Job()
        {
            Consulta consulta = new Consulta();
            Integra integra = new Integra();

            List<string> lista = integra.Listar_arquivos(".dat");
            if (lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    var str = integra.LerArquivo(Parametros.GetDirArq(), arq_name);
                    apiIntegra.integraResponse response = new apiIntegra.integraResponse();
                    response.protocolo = Convert.ToUInt32(str[0]);
                    response.protocoloSpecified = Convert.ToBoolean(str[1]);
                    consulta.ConsultaProtocolo(response, arq_name);
                }
            }

            else
                System.Windows.Forms.MessageBox.Show("A pasta selecionada não contem os arquivos necessários para a consulta");
        }

        public void ConsultaProtocolo(apiIntegra.integraResponse Prot, string filename)
        {
            consultaRequest Request = new consultaRequest();
            consultaResponse Response = new consultaResponse();

            string dir = Parametros.GetDirArq();
            var s = string.Concat(dir, '\\', "logConsulta.txt");

            Request.protocolo = Prot.protocolo;
            Request.protocoloSpecified = Prot.protocoloSpecified;
            Request.token = Parametros.GetToken();
            Request.grupo = Parametros.GetGrupo();
            Request.grupoSpecified = true;

            //MessageBox.Show(Request.token.ToString() + "\n" + Request.protocolo.ToString() + "\n" + Request.grupo.ToString() + "\n" + Request.protocoloSpecified.ToString());

            apiConsulta.EsocialServiceClient req = new apiConsulta.EsocialServiceClient();
            try
            {
                req.Open();
                Response = req.consultaRequest(Request);
                req.Close();

                StreamWriter w = File.AppendText(@s);

                GeraLog(filename
                    , Response.consultaProtocolo.identificador.protocolo.ToString()
                    , Convert.ToString(Response.consultaProtocolo.status.descResposta)
                    , w);

                w.Close();

                if(Response.consultaProtocolo.status.cdResposta == 3)
                {
                    Mover_Consultado(filename);
                }
            }
            catch(Exception e)
            {

                /*StreamWriter arq = File.AppendText(@s);
                //MessageBox.Show(e.TargetSite.ToString());
                GeraLog(filename
                    , Response.consultaProtocolo.identificador.protocolo.ToString()
                    , e.Message.ToString()
                    , arq);
                arq.Close();*/
            }

        }

        private void Mover_Consultado(string filename)
        {
            string origem = Parametros.GetDirArq();
            string destino = Parametros.GetDirFim();

            string o = string.Concat(origem,'\\', filename);
            string d = string.Concat(destino, '\\', filename);

            System.IO.File.Move(o, d);

            int n = filename.Length;
            string name = filename.Remove(n - 3, 3);

            int j = name.Length;
            name = name.Remove(j - j, 5);
            name = string.Concat(name, "txt");
            
            o = string.Concat(origem, '\\', name);
            d = string.Concat(destino, '\\', name);

            System.IO.File.Move(o, d);
            // To move an entire directory. To programmatically modify or combine
            // path strings, use the System.IO.Path class.
            //System.IO.Directory.Move(@"C:\Users\Public\public\test\", @"C:\Users\Public\private");
        }

        private void GeraLog(string filename, string nroprt, string desc, TextWriter w)
        {
            w.Write("\r\nLog Entry : ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("Arquivo: {0}", filename);
            w.WriteLine("Número protocolo: {0}", nroprt);
            w.WriteLine("Descrição: {0}", desc);
            w.WriteLine("-------------------------------");
        }
    }
}
