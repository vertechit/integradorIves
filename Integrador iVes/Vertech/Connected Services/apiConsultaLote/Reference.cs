﻿//------------------------------------------------------------------------------
// <auto-generated>
//     O código foi gerado por uma ferramenta.
//     Versão de Tempo de Execução:4.0.30319.42000
//
//     As alterações ao arquivo poderão causar comportamento incorreto e serão perdidas se
//     o código for gerado novamente.
// </auto-generated>
//------------------------------------------------------------------------------

namespace Vertech.apiConsultaLote {
    
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ServiceModel.ServiceContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
        "Processamento/v1_1_0", ConfigurationName="apiConsultaLote.ServicoConsultarLoteEventos")]
    public interface ServicoConsultarLoteEventos {
        
        // CODEGEN: Gerando contrato de mensagem porque o nome do elemento consulta no namespace http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retornoProcessamento/v1_1_0 não está marcado como nulo
        [System.ServiceModel.OperationContractAttribute(Action="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventos", ReplyAction="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventosResponse")]
        Vertech.apiConsultaLote.ConsultarLoteEventosResponse ConsultarLoteEventos(Vertech.apiConsultaLote.ConsultarLoteEventosRequest request);
        
        [System.ServiceModel.OperationContractAttribute(Action="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventos", ReplyAction="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0/ServicoConsultarLoteEventos/ConsultarLoteEventosResponse")]
        System.Threading.Tasks.Task<Vertech.apiConsultaLote.ConsultarLoteEventosResponse> ConsultarLoteEventosAsync(Vertech.apiConsultaLote.ConsultarLoteEventosRequest request);
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultarLoteEventosRequest {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultarLoteEventos", Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0", Order=0)]
        public Vertech.apiConsultaLote.ConsultarLoteEventosRequestBody Body;
        
        public ConsultarLoteEventosRequest() {
        }
        
        public ConsultarLoteEventosRequest(Vertech.apiConsultaLote.ConsultarLoteEventosRequestBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
        "Processamento/v1_1_0")]
    public partial class ConsultarLoteEventosRequestBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement consulta;
        
        public ConsultarLoteEventosRequestBody() {
        }
        
        public ConsultarLoteEventosRequestBody(System.Xml.Linq.XElement consulta) {
            this.consulta = consulta;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.ServiceModel.MessageContractAttribute(IsWrapped=false)]
    public partial class ConsultarLoteEventosResponse {
        
        [System.ServiceModel.MessageBodyMemberAttribute(Name="ConsultarLoteEventosResponse", Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
            "Processamento/v1_1_0", Order=0)]
        public Vertech.apiConsultaLote.ConsultarLoteEventosResponseBody Body;
        
        public ConsultarLoteEventosResponse() {
        }
        
        public ConsultarLoteEventosResponse(Vertech.apiConsultaLote.ConsultarLoteEventosResponseBody Body) {
            this.Body = Body;
        }
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
    [System.Runtime.Serialization.DataContractAttribute(Namespace="http://www.esocial.gov.br/servicos/empregador/lote/eventos/envio/consulta/retorno" +
        "Processamento/v1_1_0")]
    public partial class ConsultarLoteEventosResponseBody {
        
        [System.Runtime.Serialization.DataMemberAttribute(EmitDefaultValue=false, Order=0)]
        public System.Xml.Linq.XElement ConsultarLoteEventosResult;
        
        public ConsultarLoteEventosResponseBody() {
        }
        
        public ConsultarLoteEventosResponseBody(System.Xml.Linq.XElement ConsultarLoteEventosResult) {
            this.ConsultarLoteEventosResult = ConsultarLoteEventosResult;
        }
    }
    
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public interface ServicoConsultarLoteEventosChannel : Vertech.apiConsultaLote.ServicoConsultarLoteEventos, System.ServiceModel.IClientChannel {
    }
    
    [System.Diagnostics.DebuggerStepThroughAttribute()]
    [System.CodeDom.Compiler.GeneratedCodeAttribute("System.ServiceModel", "4.0.0.0")]
    public partial class ServicoConsultarLoteEventosClient : System.ServiceModel.ClientBase<Vertech.apiConsultaLote.ServicoConsultarLoteEventos>, Vertech.apiConsultaLote.ServicoConsultarLoteEventos {
        
        public ServicoConsultarLoteEventosClient() {
        }
        
        public ServicoConsultarLoteEventosClient(string endpointConfigurationName) : 
                base(endpointConfigurationName) {
        }
        
        public ServicoConsultarLoteEventosClient(string endpointConfigurationName, string remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoConsultarLoteEventosClient(string endpointConfigurationName, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(endpointConfigurationName, remoteAddress) {
        }
        
        public ServicoConsultarLoteEventosClient(System.ServiceModel.Channels.Binding binding, System.ServiceModel.EndpointAddress remoteAddress) : 
                base(binding, remoteAddress) {
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        Vertech.apiConsultaLote.ConsultarLoteEventosResponse Vertech.apiConsultaLote.ServicoConsultarLoteEventos.ConsultarLoteEventos(Vertech.apiConsultaLote.ConsultarLoteEventosRequest request) {
            return base.Channel.ConsultarLoteEventos(request);
        }
        
        public System.Xml.Linq.XElement ConsultarLoteEventos(System.Xml.Linq.XElement consulta) {
            Vertech.apiConsultaLote.ConsultarLoteEventosRequest inValue = new Vertech.apiConsultaLote.ConsultarLoteEventosRequest();
            inValue.Body = new Vertech.apiConsultaLote.ConsultarLoteEventosRequestBody();
            inValue.Body.consulta = consulta;
            Vertech.apiConsultaLote.ConsultarLoteEventosResponse retVal = ((Vertech.apiConsultaLote.ServicoConsultarLoteEventos)(this)).ConsultarLoteEventos(inValue);
            return retVal.Body.ConsultarLoteEventosResult;
        }
        
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Advanced)]
        System.Threading.Tasks.Task<Vertech.apiConsultaLote.ConsultarLoteEventosResponse> Vertech.apiConsultaLote.ServicoConsultarLoteEventos.ConsultarLoteEventosAsync(Vertech.apiConsultaLote.ConsultarLoteEventosRequest request) {
            return base.Channel.ConsultarLoteEventosAsync(request);
        }
        
        public System.Threading.Tasks.Task<Vertech.apiConsultaLote.ConsultarLoteEventosResponse> ConsultarLoteEventosAsync(System.Xml.Linq.XElement consulta) {
            Vertech.apiConsultaLote.ConsultarLoteEventosRequest inValue = new Vertech.apiConsultaLote.ConsultarLoteEventosRequest();
            inValue.Body = new Vertech.apiConsultaLote.ConsultarLoteEventosRequestBody();
            inValue.Body.consulta = consulta;
            return ((Vertech.apiConsultaLote.ServicoConsultarLoteEventos)(this)).ConsultarLoteEventosAsync(inValue);
        }
    }
}
