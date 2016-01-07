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
            var Physique = readAFewBytes(ListOfBytes, 1);
            var ShapeOrNormals = readAFewBytes(ListOfBytes, 1);
            var minCol = readAFewBytes(ListOfBytes, 4);
            var maxCol = readAFewBytes(ListOfBytes, 4);
            var minRow = readAFewBytes(ListOfBytes, 4);
            var maxRow = readAFewBytes(ListOfBytes, 4);

        }

        int amountOfBytesRead;
        public string readAFewBytes(byte[] file, int amountOfBytesToRead)
        {
            string fewBytes = System.String.Empty;
            var temporaryBytesRead = 0;
            byte[] byteHolder;
            List<byte> temporaryByteHolder = new List<byte>();
            for (int i = 0 + amountOfBytesRead; i < amountOfBytesToRead + amountOfBytesRead; i++)
            {
                //Console.WriteLine(amountOfBytesToRead);
                //Console.WriteLine(amountOfBytesRead);
                temporaryBytesRead++;
                temporaryByteHolder.Add(file[i]);
                if (((amountOfBytesToRead + amountOfBytesRead) - i) == 1)
                {
                    byteHolder = temporaryByteHolder.ToArray();
                    if (byteHolder.Length != 1)
                    {
                        fewBytes = BitConverter.ToInt32(byteHolder, 0).ToString();
                    }

                    else
                    {
                        fewBytes = byteHolder[0].ToString();
                    }
                    Console.WriteLine(fewBytes);
                }
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

