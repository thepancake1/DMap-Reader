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
            var Version = readAFewBytes(ListOfBytes, 4, "version");
            var doubledWidth = readAFewBytes(ListOfBytes, 4, "doubledwidth");
            var height = readAFewBytes(ListOfBytes, 4, "height");
            var AgeGender = readAFewBytes(ListOfBytes, 4, "agegender");
            var Physique = readAFewBytes(ListOfBytes, 1, "physique");
            var ShapeOrNormals = readAFewBytes(ListOfBytes, 1, "shapeOrNormals");
            var minCol = readAFewBytes(ListOfBytes, 4, "minCol");
            var maxCol = readAFewBytes(ListOfBytes, 4, "maxCol");
            var minRow = readAFewBytes(ListOfBytes, 4, "minRow");
            var maxRow = readAFewBytes(ListOfBytes, 4, "maxRow");
            var robeChannel = readAFewBytes(ListOfBytes, 1, "robeChannel");
            var totalBytesInEncodedMap = readAFewBytes(ListOfBytes, 4, "totalButes");
            var width = int.Parse(maxCol) - int.Parse(minCol) + 1; ;
            Console.WriteLine(width + " width");
            var numScanLines = int.Parse(maxRow) - int.Parse(minRow) + 1;
            Console.WriteLine(numScanLines + " numScanLines");
            for (int i = 0; i < numScanLines; i++)
            {
                RLEArrayOfPixels.Clear();


                var scanLineDataSize = readAFewBytes(ListOfBytes, 2, "scanLineDataSize");
                var mbIsCompressed = readAFewBytes(ListOfBytes, 1, "mbIsCompressed");
                var mRobeChannel = readAFewBytes(ListOfBytes, 1, "mRobeChannel");
                int mUncompressedPixels = 0;
                if (int.Parse(mbIsCompressed) == 0)
                {
                    if (int.Parse(mRobeChannel) == 0)
                    {
                        // If robe is present, robe and skin tight are interleaved.  
                        // i.e. skin tight pixel, robe pixel, skin tight pixel, robe pixel, etc.
                        // Thus each pixel takes 6 bytes.
                        mUncompressedPixels = int.Parse(readAFewBytes(ListOfBytes, width * 6, "mUncompressedPixels"));
                    }
                    else
                    {
                        mUncompressedPixels = int.Parse(readAFewBytes(ListOfBytes, width * 3, "mUncompressedPixels"));
                    }
                }
                else
                {
                    int numIndexes = int.Parse(readAFewBytes(ListOfBytes, 1, "numIndexes") ); // must be > 1

                    // The goal of having index tables (mPixelPosIndexes & mDataPosIndexes) is to provide faster shortcuts 
                    // into the RLE data where run-time client can start decoding data to obtain a value at a particular pixel 
                    // position x without decoding the entire preceding scanline.
                    // See DMap-To-BMP.sc for examples on how to used these indexes.
                    int mPixelPosIndexes = int.Parse(readAFewBytes(ListOfBytes, 2, "mPixelPosIndexes", false));
                    int mDataPosIndexes = int.Parse(readAFewBytes(ListOfBytes, 2, "mDataPosIndexes", false));

                    int headerdatasize = 4 + 1 + (4 * numIndexes);
                    Console.WriteLine("HeaderDataSize " + headerdatasize);
                    // The RLE data is an array if chars, organized as follows:
                    //  Byte 0      : Size of run
                    //  Byte 1,2,3  : Pixel info for skin tight data
                    //  Byte 4,5,6  : Pixel info for robe data (if present)
                    //  Repeat
                    int mRLEArrayOfPixelsVar = int.Parse(readAFewBytes(ListOfBytes, int.Parse(scanLineDataSize) - headerdatasize, "mRLEArrayOfPixelsVar", false));
                    List<int> mRLEArrayOfPixels = RLEArrayOfPixels;

                    Console.WriteLine("mRLEArrayOfPixels.count " + mRLEArrayOfPixels.Count);
                    
                }

            }
        }

        List<int> RLEArrayOfPixels = new List<int>();

        int amountOfBytesRead;
        public string readAFewBytes(byte[] file, int amountOfBytesToRead, string stringToDisplay, bool shouldParseAsWholeOrOneByOne = true)
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
                    if (shouldParseAsWholeOrOneByOne == true)
                    {
                        if (byteHolder.Length == 4)
                        {
                            fewBytes = BitConverter.ToInt32(byteHolder, 0).ToString();

                        }
                        else if (byteHolder.Length == 2)
                        {
                            fewBytes = BitConverter.ToInt16(byteHolder, 0).ToString();
                        }

                        else
                        {
                            fewBytes = byteHolder[0].ToString();
                        }
                    }
                    if (shouldParseAsWholeOrOneByOne == false)
                    {
                        for (int z = 0; z < byteHolder.Length; z++)
                        {
                            fewBytes = 0.ToString();
                            RLEArrayOfPixels.Add(byteHolder[z]);
                        }

                    }
                    Console.WriteLine(fewBytes + " " + stringToDisplay);
                }
            }
            amountOfBytesRead += temporaryBytesRead;
            return fewBytes;
        }


    }
        



    }


