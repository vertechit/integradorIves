using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.IO;

namespace Vertech.Services
{
    public class SecureFile
    {
        public void Encrypt(string input, string output)
        {
            this.EncryptFile(input, output);

            /*// CRIANDO DIRETORIO HIDDEN
            DirectoryInfo diretorio = Directory.CreateDirectory(@"C:\Diretorio");
            diretorio.Attributes = FileAttributes.Hidden;

            // CRIANDO ARQUIVO HIDDEN
            FileInfo arquivo = new FileInfo(@"C:\Arquivo.txt");
            arquivo.CreateText();
            arquivo.Attributes = FileAttributes.Hidden;
            Obs.Se houver algum problema de escrita ou acesso negado para o arquivo/ diretorio que vc criou use o atributo

            FileAttributes.Normal;
            realize a alteração desejada e novamente utilize o atributo
    
            FileAttributes.Hidden;*/
        }

        public void Descrypt(string input, string output)
        {
            this.DecryptFile(input, output);
        }

        private void EncryptFile(string inputFile, string outputFile)
        {

            try
            {
                string password = @"myKey123"; // Your Key Here
                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                string cryptFile = outputFile;
                FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateEncryptor(key, key),
                    CryptoStreamMode.Write);

                FileStream fsIn = new FileStream(inputFile, FileMode.Open);

                int data;
                while ((data = fsIn.ReadByte()) != -1)
                    cs.WriteByte((byte)data);

                fsIn.Close();
                cs.Close();
                fsCrypt.Close();

                //System.IO.FileInfo fi = new System.IO.FileInfo(inputFile);

                //fi.Delete();

            }
            catch (Exception e)
            {
                System.Windows.MessageBox.Show(e.Message);
            }
        }

        private void DecryptFile(string inputFile, string outputFile)
        {
            try
            {
                string password = @"myKey123"; // Your Key Here

                UnicodeEncoding UE = new UnicodeEncoding();
                byte[] key = UE.GetBytes(password);

                FileStream fsCrypt = new FileStream(inputFile, FileMode.Open);

                RijndaelManaged RMCrypto = new RijndaelManaged();

                CryptoStream cs = new CryptoStream(fsCrypt,
                    RMCrypto.CreateDecryptor(key, key),
                    CryptoStreamMode.Read);

                FileStream fsOut = new FileStream(outputFile, FileMode.Create);

                int data;
                while ((data = cs.ReadByte()) != -1)
                    fsOut.WriteByte((byte)data);

                fsOut.Close();
                cs.Close();
                fsCrypt.Close();

            }
            catch(Exception ex)
            {
                System.Windows.MessageBox.Show(ex.Message);
                //MessageBox.Show("DecryptFile failed!", "Error");
            }
        }
    }
}
