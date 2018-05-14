using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.Services;
using System.IO;
using System.Windows;
using Vertech.DAO;
using Vertech.Modelos;

namespace Vertech.Services
{
    public class ClassException
    {
        
        public void ImprimeException(int tipo, string msg)
        {
            Processos processo = new Processos();
            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            strArr = stri.Split(splitchar);

            string hora = strArr[1];
            string data = strArr[0];
            switch (tipo)
            {

                case 1:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        

                        Log.AddLogEnvia(new LogEnvia { Id = 0, Msg = msg, NomeArquivo = "Integra", Data = data, Hora = hora });
                        /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logEnvio.log");
                        StreamWriter vWriter = new StreamWriter(@s, true);
                        vWriter.WriteLine("");
                        vWriter.WriteLine("Ocorrencia Integra");
                        vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                        vWriter.WriteLine("Descrição: " + msg);
                        vWriter.WriteLine("");
                        vWriter.Flush();
                        vWriter.Close();*/
                    }
                    else
                    {
                        MessageBox.Show(msg);
                    }
                    break;

                case 2:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        Log.AddLogConsulta(new LogConsulta
                        {
                            Id = 0
                                                       ,
                            NomeArquivo = "Consulta"
                                                       ,
                            Protocolo = "0"
                                                       ,
                            Msg = msg
                                                       ,
                            Data = data
                                                       ,
                            Hora = hora
                        });
                        /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logConsulta.log");
                        StreamWriter vWriter = new StreamWriter(@s, true);

                        vWriter.WriteLine("");
                        vWriter.WriteLine("Ocorrencia Consulta");
                        vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                        vWriter.WriteLine("Descrição: " + msg);
                        vWriter.WriteLine("");
                        vWriter.Flush();
                        vWriter.Close();*/
                    }
                    else
                    {
                        MessageBox.Show(msg);
                    }
                    break;
            }
            
        }
        public void ImprimeMsgDeErro_NoFilesFound(int tp)
        {
            Processos processo = new Processos();

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            strArr = stri.Split(splitchar);

            string hora = strArr[1];
            string data = strArr[0];

            switch (tp)
            {
                case 1:
                    if(Parametros.GetTipoApp() == "Service")
                    {
                        try
                        {
                            Log.AddLogEnvia(new LogEnvia { Id = 0, Msg = "A pasta selecionada não contem os arquivos necessários para o envio", NomeArquivo = "Integra", Data = data, Hora = hora });
                            /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logEnvio.log");
                            StreamWriter vWriter = new StreamWriter(@s, true);
                            vWriter.WriteLine("");
                            vWriter.WriteLine("Ocorrencia Integra");
                            vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                            vWriter.WriteLine("Descrição: A pasta selecionada não contem os arquivos necessários para o envio");
                            vWriter.WriteLine("");
                            vWriter.Flush();
                            vWriter.Close();*/
                        }
                        catch(Exception ex)
                        {
                            Exception_Open_Files(999, ex.Message);
                        }
                        
                    }
                    else
                    {
                        try
                        {
                            Log.AddLogEnvia(new LogEnvia { Id = 0, Msg = "A pasta selecionada não contem os arquivos necessários para o envio", NomeArquivo = "Integra", Data = data, Hora = hora });
                        }
                        catch (Exception ex)
                        {

                        }
                        
                        /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logEnvio.log");
                        StreamWriter vWriter = new StreamWriter(@s, true);
                        vWriter.WriteLine("");
                        vWriter.WriteLine("A pasta selecionada não contem os arquivos necessários para o envio");
                        vWriter.WriteLine("");
                        vWriter.Flush();
                        vWriter.Close();
                        //MessageBox.Show("A pasta selecionada não contem os arquivos necessários para o envio");*/
                    }
                        break;
                case 2:
                    if(Parametros.GetTipoApp() == "Service")
                    {
                        try
                        {
                            Log.AddLogConsulta(new LogConsulta
                            {
                                Id = 0
                                                       ,
                                NomeArquivo = "Consulta"
                                                       ,
                                Protocolo = "0"
                                                       ,
                                Msg = "A pasta selecionada não contem os arquivos necessários para a consulta"
                                                       ,
                                Data = data
                                                       ,
                                Hora = hora
                            });
                            /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logConsulta.log");
                            StreamWriter vWriter = new StreamWriter(@s, true);

                            vWriter.WriteLine("");
                            vWriter.WriteLine("Ocorrencia Consulta");
                            vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                            vWriter.WriteLine("Descrição: A pasta selecionada não contem os arquivos necessários para a consulta");
                            vWriter.WriteLine("");
                            vWriter.Flush();
                            vWriter.Close();*/
                        }
                        catch (Exception ex)
                        {
                            Exception_Open_Files(999, ex.Message);
                        }
                    }
                    else
                    {
                        try
                        {
                            Log.AddLogConsulta(new LogConsulta
                            {
                                Id = 0
                                                       ,
                                NomeArquivo = "Consulta"
                                                       ,
                                Protocolo = "0"
                                                       ,
                                Msg = "A pasta selecionada não contem os arquivos necessários para a consulta"
                                                       ,
                                Data = data
                                                       ,
                                Hora = hora
                            });
                        }
                        catch (Exception ex)
                        {

                        }
                        
                        /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logConsulta.log");
                        StreamWriter w = File.AppendText(@s);
                        w.WriteLine("");
                        w.WriteLine("A pasta selecionada não contem os arquivos necessários para a consulta");
                        w.WriteLine("");
                        w.Flush();
                        w.Close();
                        //MessageBox.Show("A pasta selecionada não contem os arquivos necessários para a consulta");*/
                    }
                    break;
            }
        }

        public void ExProcessos(int codErro, string msg)
        {
            Processos processo = new Processos();

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            strArr = stri.Split(splitchar);

            string hora = strArr[1];
            string data = strArr[0];

            if (Parametros.GetTipoApp() == "Service")
            {
                try
                {
                    Log.AddLogErro(new LogErros { Id = 0, Servico = "Processos", CodErro = codErro.ToString(), Msg = msg, Data = data, Hora = hora });
                    /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logException.log");
                    StreamWriter vWriter = new StreamWriter(@s, true);

                    vWriter.WriteLine("");
                    vWriter.WriteLine("Ocorrencia: Processos");
                    vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                    vWriter.WriteLine("Codigo do erro: " + codErro.ToString());
                    vWriter.WriteLine("Descrição: " + msg);
                    vWriter.WriteLine("");
                    vWriter.Flush();
                    vWriter.Close();*/
                }
                
                catch (Exception ex)
                {
                    Exception_Open_Files(999, ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Codigo do erro: " + codErro.ToString() + "\n" + msg);
            }
        }

        public void ExSQLite(int codErro, string msg)
        {
            Processos processo = new Processos();

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            strArr = stri.Split(splitchar);

            string hora = strArr[1];
            string data = strArr[0];

            if (Parametros.GetTipoApp() == "Service")
            {
                try
                {
                    Log.AddLogErro(new LogErros { Id = 0, Servico = "SQLite", CodErro = codErro.ToString(), Msg = msg, Data = data, Hora = hora });
                    /*var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logSQL.log");
                    StreamWriter vWriter = new StreamWriter(@s, true);

                    vWriter.WriteLine("");
                    vWriter.WriteLine("Ocorrencia: SQLite");
                    vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                    vWriter.WriteLine("Codigo do erro: " + codErro.ToString());
                    vWriter.WriteLine("Descrição: " + msg);
                    vWriter.WriteLine("");
                    vWriter.Flush();
                    vWriter.Close();*/
                }
                catch (Exception ex)
                {
                    Exception_Open_Files(999, ex.Message);
                }

            }
            else
            {
                MessageBox.Show("Codigo do erro: " + codErro.ToString() + "\n" + msg);
            }
        }

        public void ExSecureFile(int codErro, string msg)
        {
            var p = new Processos();

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            strArr = stri.Split(splitchar);

            string hora = strArr[1];
            string data = strArr[0];

            if (Parametros.GetTipoApp() == "Service")
            {
                Log.AddLogErro(new LogErros { Id = 0, Servico = "SecureFile", CodErro = codErro.ToString(), Msg = msg, Data = data, Hora = hora });
                /*var s = p.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logException.log");
                StreamWriter vWriter = new StreamWriter(@s, true);

                vWriter.WriteLine("");
                vWriter.WriteLine("Ocorrencia: Secure File");
                vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                vWriter.WriteLine("Codigo do erro: " + codErro.ToString());
                vWriter.WriteLine("Descrição: " + msg);
                vWriter.WriteLine("");
                vWriter.Flush();
                vWriter.Close();*/
            }
            else
            {
                MessageBox.Show("Codigo do erro: " + codErro.ToString() + "\n" + msg);
            }
        }

        public void Exception_Open_Files(int tipo, string msg)
        {
            var p = new Processos();

            string stri = Convert.ToString(DateTime.Now);
            string[] strArr = null;
            char[] splitchar = { ' ' };

            strArr = stri.Split(splitchar);

            string hora = strArr[1];
            string data = strArr[0];

            Log.AddLogErro(new LogErros { Id = 0, Servico = "SQLite", CodErro = "999", Msg = msg, Data = data, Hora = hora });
            /*var s = p.MontaCaminhoDir(Parametros.GetDirArq(), "\\logs\\logException.log");
            StreamWriter vWriter = new StreamWriter(@s, true);

            vWriter.WriteLine("");
            vWriter.WriteLine("Ocorrencia: Open Files");
            vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
            vWriter.WriteLine("Codigo do erro: " + tipo.ToString());
            vWriter.WriteLine("Descrição: " + msg);
            vWriter.WriteLine("");
            vWriter.Flush();
            vWriter.Close();*/
        }
    }
}
