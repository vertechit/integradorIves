using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows;

namespace Vertech.Services
{
    public class Processos
    {
        public string MontaCaminhoDir(string dir, string name)
        {
            var s = string.Concat(dir, '\\', name);

            return s;
        }

        public List<string> Listar_arquivos(string ext)
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo dirInfo = new DirectoryInfo(@dir);
            List<string> Arquivo = BuscaArquivos(dirInfo, ext);

            return Arquivo;
        }

        public string[] LerArquivo(string dir, string arq)
        {
            string[] lines = null;

            string s = MontaCaminhoDir(dir, arq);

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

        public void SalvaProtocolo(apiIntegra.integraResponse Response, string arq)
        {
            string dir = Parametros.GetDirArq();
            int n = arq.Length;
            string name = arq.Remove(n - 3, 3);

            name = string.Concat("prot_", name, "dat");

            string s = MontaCaminhoDir(dir, name);

            string[] lines = { Convert.ToString(Response.protocolo), Convert.ToString(Response.protocoloSpecified) };

            System.IO.File.WriteAllLines(@s, lines);
        }

        public int VerificacaoIntegra(string arq)
        {
            string dir = Parametros.GetDirArq();
            DirectoryInfo diretorio = new DirectoryInfo(dir);

            int n = arq.Length;
            string name = arq.Remove(n - 3, 3);

            name = string.Concat("prot_", name, "dat");

            foreach (var item in diretorio.GetFiles())
            {
                if (item.Extension == ".dat")
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

        private List<string> BuscaArquivos(DirectoryInfo dir, string ext)
        {
            List<string> File_Names = new List<string>();

            try
            {
                foreach (FileInfo file in dir.GetFiles())
                {
                    if (file.Extension == ext)
                    {
                        if (file.Name != "logEnvio.txt" && file.Name != "logConsulta.txt")
                            File_Names.Add(file.Name);
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


        public void Mover_Consultado(string filename)
        {
            string origem = Parametros.GetDirArq();
            string destino = Parametros.GetDirFim();

            string o = MontaCaminhoDir(origem, filename);
            string d = MontaCaminhoDir(destino, filename);

            System.IO.File.Move(o, d);

            int n = filename.Length;
            string name = filename.Remove(n - 3, 3);

            int j = name.Length;
            name = name.Remove(j - j, 5);
            name = string.Concat(name, "txt");

            o = MontaCaminhoDir(origem, name);
            d = MontaCaminhoDir(destino, name);

            System.IO.File.Move(o, d);
            // To move an entire directory. To programmatically modify or combine
            // path strings, use the System.IO.Path class.
            //System.IO.Directory.Move(@"C:\Users\Public\public\test\", @"C:\Users\Public\private");
        }

        public void GeraLogConsulta(string filename, string nroprt, string desc, TextWriter w)
        {
            w.Write("\r\nLog consulta: ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("Arquivo: {0}", filename);
            w.WriteLine("Número protocolo: {0}", nroprt);
            w.WriteLine("Descrição: {0}", desc);
            w.WriteLine("-------------------------------");
        }

        public void GeraLogIntegra(string filename, string str, TextWriter w)
        {
            w.Write("\r\nLog integra: ");
            w.WriteLine("{0} {1}", DateTime.Now.ToLongTimeString(),
                DateTime.Now.ToLongDateString());
            w.WriteLine("Arquivo: {0}", filename);
            w.WriteLine(str);
            w.WriteLine("-------------------------------");
        }
    }
}
