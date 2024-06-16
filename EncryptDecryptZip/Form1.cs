using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncryptDecryptZip
{
    public partial class Form1 : Form
    {
        public string outputFilePath = string.Empty;
        public string fileName = string.Empty;
        public Form1()
        {
            InitializeComponent();
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void webBrowser1_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog fdlg = new OpenFileDialog();
            if (fdlg.ShowDialog() == DialogResult.OK)
            {
                lblFilePath.Text = fdlg.FileName;
                fileName = fdlg.SafeFileName.Split('.')[0];
            }
        }

        private void fdlg_FileOk(object sender, CancelEventArgs e)
        {

        }

        private void btnEncrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text == string.Empty || fileName == string.Empty || fdlgOutputPath == null)
                {
                    MessageBox.Show("Please enter all the required details.");
                }
                else
                {
                    string password = txtPassword.Text; // Your Key Here
                    UnicodeEncoding UE = new UnicodeEncoding();
                    byte[] key = UE.GetBytes(password);

                    string cryptFile = outputFilePath + "\\" + fileName + "_Enc.zip";
                    FileStream fsCrypt = new FileStream(cryptFile, FileMode.Create);

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateEncryptor(key, key),
                        CryptoStreamMode.Write);

                    FileStream fsIn = new FileStream(lblFilePath.Text, FileMode.Open);

                    int data;
                    while ((data = fsIn.ReadByte()) != -1)
                        cs.WriteByte((byte)data);

                    fsIn.Close();
                    cs.Close();
                    fsCrypt.Close();
                    MessageBox.Show("File Encrypted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occurred, please try again later.");
            }
            finally
            {
                resetForm();
            }
        }

        private void btnOutputPath_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd1 = new FolderBrowserDialog();
            if (fbd1.ShowDialog() == DialogResult.OK)
            {
                outputFilePath = fbd1.SelectedPath;
            }
        }

        private void btnDecrypt_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtPassword.Text == string.Empty || fileName == string.Empty || fdlgOutputPath == null)
                {
                    MessageBox.Show("Please enter all the required details.");
                }
                else
                {
                    string password = txtPassword.Text; // Your Key Here

                    UnicodeEncoding UE = new UnicodeEncoding();
                    byte[] key = UE.GetBytes(password);

                    FileStream fsCrypt = new FileStream(lblFilePath.Text, FileMode.Open);

                    RijndaelManaged RMCrypto = new RijndaelManaged();

                    CryptoStream cs = new CryptoStream(fsCrypt,
                        RMCrypto.CreateDecryptor(key, key),
                        CryptoStreamMode.Read);

                    FileStream fsOut = new FileStream(outputFilePath + "\\" + fileName +  "_Dec.zip", FileMode.Create);

                    int data;
                    while ((data = cs.ReadByte()) != -1)
                        fsOut.WriteByte((byte)data);

                    fsOut.Close();
                    cs.Close();
                    fsCrypt.Close();
                    MessageBox.Show("File Decrypted successfully.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Some error occurred, please try again later.");
            }
            finally
            {
                resetForm();
            }
        }
        private void resetForm()
        {
            lblFilePath.Text = string.Empty;
            txtPassword.Text = string.Empty;
            outputFilePath = string.Empty;
            fbd1.SelectedPath = string.Empty;
            fdlg.FileName = string.Empty;
            fileName = string.Empty;
        }
    }
}
