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
        ClassException ex = new ClassException();

        public void Job()
        {
            Processos processo = new Processos();

            var s = processo.MontaCaminhoDir(Parametros.GetDirArq(),"logEnvio.txt");

            var lista = processo.Listar_arquivos(".txt");

            if(lista.Count > 0)
            {
                foreach (var arq_name in lista)
                {
                    int ctrl = processo.VerificacaoIntegra(arq_name);

                    if (ctrl == 0)
                    {
                        integraRequest Request = new integraRequest();

                        Request.esocial = Retorna_Esocial(Retorna_Identificador(), arq_name);

                        integraResponse Response = Enviar(Request);

                        if (Response.protocolo > 0)
                        {
                            StreamWriter w = File.AppendText(@s);
                            processo.SalvaProtocolo(Response, arq_name);
                            processo.GeraLogIntegra(arq_name, "Foi enviado com sucesso!", w);
                            w.Close();
                        }
                        else
                        {
                            StreamWriter w = File.AppendText(@s);
                            processo.GeraLogIntegra(arq_name, "Erro no envio, retorno invalido!", w);
                            w.Close();
                        }
                    }
                    else if (ctrl == 1)
                    {
                        StreamWriter w = File.AppendText(@s);
                        processo.GeraLogIntegra(arq_name, "Já foi enviado!", w);
                        w.Close();
                    }
                    else
                    {
                        StreamWriter w = File.AppendText(@s);
                        processo.GeraLogIntegra(arq_name, "É invalido", w);
                        w.Close();
                    }

                    //Thread.Sleep(1000);
                }
            }

            else
            {
                ex.ImprimeMsgDeErro_NoFilesFound(1);
            }

        }

        public identificador Retorna_Identificador()
        {
            identificador Id = new identificador();

            try
            {
                Id.grupo = Parametros.GetGrupo();
                Id.token = Parametros.GetToken();
            }
            catch (Exception e)
            {
                ex.ImprimeException(1, "Erro ao atribuir identificador");
            }

            return Id;
        }

        public esocial Retorna_Esocial(identificador id, string arq)
        {
            var dir = Parametros.GetDirArq();

            esocial eso = new esocial();
            Processos process = new Processos();

            try
            {
                eso.identificador = id;

                eso.registro = process.LerArquivo(dir, arq);
            }
            catch (Exception e)
            {
                ex.ImprimeException(1, "Erro no retorno do arquivo");
            }

            return eso;
        }

        public integraResponse Enviar(integraRequest Request)
        {
            EsocialServiceClient req = new EsocialServiceClient();
            integraResponse Response = new integraResponse();

            try
            {
                req.Open();

                Response = req.integraRequest(Request);

                req.Close();

            }
            catch(Exception e)
            {
                ex.ImprimeException(1, e.Message.ToString());
            }

            return Response;
        }

    }
}
