using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.EsoIntegra;
using System.Threading;
using System.IO;
using System.Net.Sockets;

namespace Vertech.Uteis
{
    public class EsoIntegra
    {
        private esocial Eso { get; set; }
        private EsocialServiceClient Client { get; set; }
        private identificador Identificador { get; set; }
        private integraRequest EsoRequest { get; set; }
        private integraRequest1 EsoRequest1 { get; set; }
        private integraResponse EsoResponse { get; set; }
        private integraResponse1 EsoResponse1 { get; set; }


        public void Init ()
        {
            Identificador.grupo = 1;
            Identificador.token = "8EE07DE66C97D8CFBAE04C47E8F51D76";

            Eso.identificador = Identificador;
            Eso.registro[0] = "S-1020";

            EsoRequest.esocial = Eso;

            EsoRequest1.integraRequest = EsoRequest;

            Client.Open();
            Client.integraRequest(EsoRequest);
        }
    }
}
