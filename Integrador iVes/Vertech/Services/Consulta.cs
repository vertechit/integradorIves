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
using Vertech.DAO;
using Vertech.Modelos;


namespace Vertech.Services
{
    public class Consulta
    {
        
        public void Job()
        {
            ClassException ex = new ClassException();
            //Consulta consulta = new Consulta();
            Processos processo = new Processos();
            int i = 0;
            List<string> lista = processo.Listar_arquivos(".txt");

            if (lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    bool valida = Helper.ExistsProtocolo(arq_name);
                    if(valida == true)
                    {
                        var p = Helper.GetProtocolo(arq_name);
                        ConsultaProtocolo(Set_Protocolo(p), p.NomeArquivo);
                        i++;
                    }
                    
                    //Thread.Sleep(1000);
                }
                if (i == 0)
                {
                    ex.ImprimeMsgDeErro_NoFilesFound(2);
                }
            }

            else
            {
                ex.ImprimeMsgDeErro_NoFilesFound(2);
            }
            
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

            try
            {
                EsocialServiceClient req = new EsocialServiceClient();
                req.Open();
                Response = req.consultaRequest(Request);
                req.Close();

                StreamWriter w = File.AppendText(@s);

                processo.GeraLogConsulta(filename
                    , Response.consultaProtocolo.identificador.protocolo.ToString()
                    , Convert.ToString(Response.consultaProtocolo.status.descResposta)
                    , w);

                processo.GeraLogDetalhado(filename, Response);

                w.Close();

                if(Response.consultaProtocolo.status.cdResposta == 3)
                {
                    processo.Mover_Consultado(filename);
                }
            }
            catch(Exception e)
            {
                StreamWriter arq = File.AppendText(@s);

                processo.GeraLogConsulta(filename, Request.protocolo.ToString(), e.Message.ToString(), arq);

                arq.Close();
            }

        }

        private apiIntegra.integraResponse Set_Protocolo(Protocolo p)
        {
            Processos processo = new Processos();
            apiIntegra.integraResponse response = new apiIntegra.integraResponse();

            //var retorno = processo.LerArquivo(Parametros.GetDirArq(), arq_name);

            response.protocolo = Convert.ToUInt32(p.NroProtocolo);
            response.protocoloSpecified = true; 

            return response;
        }
    }
}
