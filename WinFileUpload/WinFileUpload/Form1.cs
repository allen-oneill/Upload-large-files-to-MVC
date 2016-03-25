using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Shared;
using System.Net.Http;
using System.Net;
using System.Net.Http.Headers;

// sample image file: http://hubblesite.org/newscenter/archive/releases/2004/15/image/b/
namespace WinFileUpload
{
    public partial class Form1 : Form
    {
        string CurrentFolder = AppDomain.CurrentDomain.BaseDirectory;

        public Form1()
        {
            InitializeComponent();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            // use a LARGE file to test :)
            DialogResult rslt = openFileDialog1.ShowDialog();
            if (rslt == DialogResult.OK)
            {
                Utils ut = new Utils();
                ut.FileName = openFileDialog1.FileName;
                ut.TempFolder = Path.Combine(CurrentFolder, "Temp");
                if (!Directory.Exists(ut.TempFolder))
                    Directory.CreateDirectory(ut.TempFolder);
                ut.MaxFileSizeMB = 1;
                ut.SplitFile();
                foreach (string File in ut.FileParts) // improvement - this is sequential, make threaded
                {
                    UploadFile(File);
                }
                MessageBox.Show("Upload complete!");
            }
        }

        public bool UploadFile(string FileName)
        {
            bool rslt = false;
            using (var client = new HttpClient())
            {
                using (var content = new MultipartFormDataContent())
                {
                    var fileContent = new ByteArrayContent(System.IO.File.ReadAllBytes(FileName));
                    fileContent.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                    {
                        FileName = Path.GetFileName(FileName)
                    };
                    content.Add(fileContent);

                    var requestUri = "http://localhost:8170/Home/UploadFile/";
                    try
                    {
                        var result = client.PostAsync(requestUri, content).Result;
                        rslt = true;
                    }
                    catch (Exception ex)
                    {
                        // log error
                        rslt = false;
                    }
                }
            }
           return rslt;
        }
        
    }

}
