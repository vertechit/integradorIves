using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiConsulta;

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
            Processos processo = new Processos();

            List<string> lista = processo.Listar_arquivos(".dat");

            if (lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    consulta.ConsultaProtocolo(Set_Protocolo(arq_name), arq_name);
                }
            }

            else
                System.Windows.Forms.MessageBox.Show("A pasta selecionada não contem os arquivos necessários para a consulta");
        }

        public void ConsultaProtocolo(apiIntegra.integraResponse Prot, string filename)
        {
            consultaRequest Request = new consultaRequest();
            consultaResponse Response = new consultaResponse();
            Processos processo = new Processos();

            var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "logConsulta.txt");

            Request.protocolo = Prot.protocolo;
            Request.protocoloSpecified = Prot.protocoloSpecified;
            Request.token = Parametros.GetToken();
            Request.grupo = Parametros.GetGrupo();
            Request.grupoSpecified = true;

           
            apiConsulta.EsocialServiceClient req = new apiConsulta.EsocialServiceClient();

            try
            {
                req.Open();
                Response = req.consultaRequest(Request);
                req.Close();

                StreamWriter w = File.AppendText(@s);

                processo.GeraLogConsulta(filename
                    , Response.consultaProtocolo.identificador.protocolo.ToString()
                    , Convert.ToString(Response.consultaProtocolo.status.descResposta)
                    , w);

                w.Close();

                if(Response.consultaProtocolo.status.cdResposta == 3)
                {
                    processo.Mover_Consultado(filename);
                }
            }
            catch(Exception e)
            {
                StreamWriter arq = File.AppendText(@s);

                processo.GeraLogConsulta(filename, "Falha!", e.Message.ToString(), arq);

                arq.Close();
            }

        }

        private apiIntegra.integraResponse Set_Protocolo(string arq_name)
        {
            Processos processo = new Processos();
            apiIntegra.integraResponse response = new apiIntegra.integraResponse();

            var retorno = processo.LerArquivo(Parametros.GetDirArq(), arq_name);

            response.protocolo = Convert.ToUInt32(retorno[0]);
            response.protocoloSpecified = Convert.ToBoolean(retorno[1]);

            return response;
        }
    }
}
