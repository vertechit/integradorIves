﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vertech.Services;
using System.IO;
using System.Windows;

namespace Vertech.Services
{
    public class ClassException
    {
        
        public void ImprimeException(int tipo, string msg)
        {
            Processos processo = new Processos();
            switch (tipo)
            {
                case 1:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "logEnvio.txt");
                        StreamWriter vWriter = new StreamWriter(@s, true);
                        vWriter.WriteLine("");
                        vWriter.WriteLine("Ocorrencia Integra");
                        vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                        vWriter.WriteLine("Descrição: " + msg);
                        vWriter.WriteLine("");
                        vWriter.Flush();
                        vWriter.Close();
                    }
                    else
                    {
                        MessageBox.Show(msg);
                    }
                    break;

                case 2:
                    if (Parametros.GetTipoApp() == "Service")
                    {
                        var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "logConsulta.txt");
                        StreamWriter vWriter = new StreamWriter(@s, true);

                        vWriter.WriteLine("");
                        vWriter.WriteLine("Ocorrencia Consulta");
                        vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                        vWriter.WriteLine("Descrição: " + msg);
                        vWriter.WriteLine("");
                        vWriter.Flush();
                        vWriter.Close();
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
            switch (tp)
            {
                case 1:
                    if(Parametros.GetTipoApp() == "Service")
                    {
                        var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "logEnvio.txt");
                        StreamWriter vWriter = new StreamWriter(@s, true);
                        vWriter.WriteLine("");
                        vWriter.WriteLine("Ocorrencia Integra");
                        vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                        vWriter.WriteLine("Descrição: A pasta selecionada não contem os arquivos necessários para o envio");
                        vWriter.WriteLine("");
                        vWriter.Flush();
                        vWriter.Close();
                    }
                    else
                    {
                        MessageBox.Show("A pasta selecionada não contem os arquivos necessários para o envio");
                    }
                        break;
                case 2:
                    if(Parametros.GetTipoApp() == "Service")
                    {
                        var s = processo.MontaCaminhoDir(Parametros.GetDirArq(), "logConsulta.txt");
                        StreamWriter vWriter = new StreamWriter(@s, true);

                        vWriter.WriteLine("");
                        vWriter.WriteLine("Ocorrencia Consulta");
                        vWriter.WriteLine("Data/Hora: " + DateTime.Now.ToString());
                        vWriter.WriteLine("Descrição: A pasta selecionada não contem os arquivos necessários para a consulta");
                        vWriter.WriteLine("");
                        vWriter.Flush();
                        vWriter.Close();
                    }
                    else
                    {
                        MessageBox.Show("A pasta selecionada não contem os arquivos necessários para a consulta");
                    }
                    break;
            }
        }

        public void ExProcessos(int codErro, string msg)
        {
            if(Parametros.GetTipoApp() == "Service")
            {

            }
            else
            {
                MessageBox.Show("Codigo do erro: " + codErro.ToString() + "\n" + msg);
            }
        }
    }
}