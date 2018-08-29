using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Collections.Specialized;
using System.Xml.Linq;
using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using Imgur.API.Models;

namespace WF_ImgurLoader
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            openFileDialog1.Filter = "Image Files(*.BMP; *.JPG; *.GIF;*.PNG)| *.BMP; *.JPG; *.GIF;*.PNG | All files(*.*) | *.*";
        }

        private void btnPath_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.Cancel)
                return;
            string filename = openFileDialog1.FileName;

            string path = filename;
            tBFilePath.Text = path;
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void btnUpload_Click(object sender, EventArgs e)
        {
            var client = new ImgurClient("Client ID", "Client Secret Key"); //Client ID - add your app client id, Client Secret Key-your app secret key
            var endpoint = new ImageEndpoint(client);			    // check https://api.imgur.com/#authentication
            IImage image;
            string path = tBFilePath.Text;
            try
            {

                if (File.Exists(path))
                {

                    using (var fs = new FileStream(path, FileMode.Open))
                    {
                        image = endpoint.UploadImageStreamAsync(fs).GetAwaiter().GetResult();
                    }
                    MessageBox.Show("image upload + url\n");
                    tbUrl.Text = image.Link;
                }
            }
            catch (Exception)
            {
                MessageBox.Show("Произошла ошибка при загрузке изображения");
            }


        }

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                Clipboard.Clear();
                Clipboard.SetText(tbUrl.Text);
            }
            catch (Exception) { MessageBox.Show("Сначала загрузите изображение"); }
        }

        private void Form1_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void Form1_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string pathname in s)
                    tBFilePath.Text += pathname;
            }
        }

        private void tBFilePath_DragEnter(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                e.Effect = DragDropEffects.Copy;
            }
        }

        private void tBFilePath_DragDrop(object sender, DragEventArgs e)
        {
            if (e.Data.GetDataPresent(DataFormats.FileDrop))
            {
                string[] s = (string[])e.Data.GetData(DataFormats.FileDrop);
                foreach (string pathname in s)
                    tBFilePath.Text += pathname;
            }
        }
    }
}
