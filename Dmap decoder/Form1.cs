using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Dmap_decoder
{
    public partial class Form1 : Form
    {
        FileAttributes FileInfo;
        public List<FileAttributes> Unused = new List<FileAttributes>();

        public Form1()
        {
            InitializeComponent();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            ReadFile();
        }

        public void ReadFile()
        {
            string path = @"C:\\S4_DB43E069_00000000_DB7D590C08AF3CDB%%+UNKN.bnry";
            var ListOfBytes = File.ReadAllBytes(path);
            var Version = readAFewBytes(ListOfBytes, 4);
            var doubledWidth = readAFewBytes(ListOfBytes, 4);
            var height = readAFewBytes(ListOfBytes, 4);
            var AgeGender = readAFewBytes(ListOfBytes, 4);

        }

        int amountOfBytesRead;
        public string readAFewBytes(byte[] file, int amountOfBytesToRead)
        {
            string fewBytes = System.String.Empty;
            var temporaryBytesRead = 0;
            for (int i = 0 + amountOfBytesRead; i < amountOfBytesToRead + amountOfBytesRead + 1; i++)
            {
                //Console.WriteLine(amountOfBytesToRead + amountOfBytesRead);
                Console.WriteLine(amountOfBytesRead);
                fewBytes += file[i];
                temporaryBytesRead++;
            }
            amountOfBytesRead += temporaryBytesRead;
            return fewBytes;
        }


    }
    public class FileAttributes
    {
        public int Physique, ShapeOrNormals, RobeChannel, version, dmap_doubled_width, height, AgeGender;
        FileAttributes(char _Physique, char _ShapeOrNormals, char _RobeChannel, int _version, int _dmap_doubled_width, int _height, int _AgeGender)
        {
            Physique = _Physique;
            ShapeOrNormals = _ShapeOrNormals;
            RobeChannel = _RobeChannel;
            version = _version;
            dmap_doubled_width = _dmap_doubled_width;
            height = _height;
            AgeGender = _AgeGender;

        }

        



    }
}

