using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace CometTester
{

    public partial class FileBrowser : Form
    {
        List<string> listFiles = new List<string>();
        XmlDocument _xmlDoc;
        string _path;

        public FileBrowser()
        {
            //InitializeComponent();

            Stream fileStream;
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "XML|*.xml|All files (*.*)|*.*";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    if ((fileStream = ofd.OpenFile()) != null)
                    {
                        _path = ofd.FileName;
                    }
                }
            }
        }

        public string GetPath()
        {
            return _path;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            /*listFiles.Clear();
            listView.Items.Clear();
            using (FolderBrowserDialog fd = new FolderBrowserDialog() {Description = "Select your path."})
            {
                if (fd.ShowDialog() == DialogResult.OK)
                {
                    txtPath.Text = fd.SelectedPath;
                    foreach (string item in Directory.GetFiles(fd.SelectedPath))
                    {
                        imageList.Images.Add(System.Drawing.Icon.ExtractAssociatedIcon(item));
                        FileInfo fi = new FileInfo(item);
                        listFiles.Add(fi.FullName);
                        listView.Items.Add(fi.Name, imageList.Images.Count - 1);
                    }
                }
            }*/
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "XML|*.xml";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                this._xmlDoc = new XmlDocument();
                _xmlDoc.Load(ofd.FileName);                
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listView.FocusedItem != null)
                Process.Start(listFiles[listView.FocusedItem.Index]);
        }
    }
}
