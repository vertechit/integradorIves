using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.apiConsulta;
//using Vertech.apiIntegra;
using Vertech.Uteis;
using System.Threading;
using System.IO;
using System.Net.Sockets;
using System.Windows;

namespace Vertech.Uteis
{
    public class Consulta
    {
        public void ConsultaProtocolo(List<apiIntegra.integraResponse> Prot)
        {
            consultaRequest Request = new consultaRequest();

            Request.protocolo = Prot[0].protocolo;
            Request.protocoloSpecified = Prot[0].protocoloSpecified;
            Request.token = Parametros.GetToken();
            Request.grupo = Parametros.GetGrupo();
            Request.grupoSpecified = true;
            Request.cdEvento = "S-1020";


            EsocialServiceClient req = new EsocialServiceClient();

            req.Open();
            consultaResponse Response = req.consultaRequest(Request);
            req.Close();

            MessageBox.Show(Response.consultaProtocolo.status.msg);
        }
    }
}
