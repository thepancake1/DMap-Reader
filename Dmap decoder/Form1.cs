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
            //Console.WriteLine(width + " width");
            var numScanLines = int.Parse(maxRow) - int.Parse(minRow) + 1;
            //Console.WriteLine(numScanLines + " numScanLines");
            for (int i = 0; i < numScanLines; i++)
            {
                RLEArrayOfPixels.Clear();


                var scanLineDataSize = int.Parse(readAFewBytes(ListOfBytes, 2, "scanLineDataSize"));
                //Console.WriteLine("scanLineDataSize " + scanLineDataSize.ToString());
                var mbIsCompressed = readAFewBytes(ListOfBytes, 1, "mbIsCompressed");
                var mRobeChannel = readAFewBytes(ListOfBytes, 1, "mRobeChannel");
                int mUncompressedPixels = 0;
                if (int.Parse(mbIsCompressed) == 0)
                {
                    if (int.Parse(mRobeChannel) == 0)
                    {
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

                    for (int x = 0; x < numIndexes; x++)
                    {
                        int mPixelPosIndexes = int.Parse(readAFewBytes(ListOfBytes, 2, "mPixelPosIndexes", false));

                    }
                    for (int x = 0; x < numIndexes; x++)
                    {
                        int mDataPosIndexes = int.Parse(readAFewBytes(ListOfBytes, 2, "mDataPosIndexes", false));

                    }

                    int headerdatasize = 4 + 1 + (4 * numIndexes);
                    //Console.WriteLine("HeaderDataSize " + headerdatasize);
                   
                    //Console.WriteLine("scanLineDataSize " + scanLineDataSize.ToString());
                    int mRLEArrayOfPixelsVar = int.Parse(readAFewBytes(ListOfBytes,scanLineDataSize - headerdatasize, "mRLEArrayOfPixelsVar", false, true, int.Parse(mRobeChannel)));
                    List<int> mRLEArrayOfPixels = RLEArrayOfPixels;

                    for (int x = 0;x < mRLEArrayOfPixels.Count; x++)
                    {
                        //Console.WriteLine("mRLEArrayOfPixels " + mRLEArrayOfPixels[x]);

                    }

                }

            }
            Console.WriteLine("done!");
        }
        string path = @"C:\\S4_DB43E069_00000000_8C373912264C8EE1%%+UNKN.bnry";

        List<int> mEditedRLEArrayOfPixels = new List<int>();

        List<int> RLEArrayOfPixels = new List<int>();

        int amountOfBytesRead;
        public string readAFewBytes(byte[] file, int amountOfBytesToRead, string stringToDisplay, bool shouldParseAsWholeOrOneByOne = true, bool RLEArray = false, int notRobeChannel = 0)
        {
            ////Console.WriteLine("amountOfBytesToRead " + amountOfBytesToRead);
            string fewBytes = System.String.Empty;
            var temporaryBytesRead = 0;
            byte[] byteHolder;
            List<byte> temporaryByteHolder = new List<byte>();
            for (int i = 0 + amountOfBytesRead; i < amountOfBytesToRead + amountOfBytesRead; i++)
            {
                ////Console.WriteLine(amountOfBytesToRead);
                ////Console.WriteLine(amountOfBytesRead);
                ////Console.WriteLine(file[i]);

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
                            if (RLEArray == true)
                            {
                                int numberToDivide;
                                if (notRobeChannel == 0)
                                {
                                    numberToDivide = 5;

                                }
                                else
                                {
                                    numberToDivide = 3;

                                }
                                if(z % numberToDivide == 0)
                                {
                                    mEditedRLEArrayOfPixels.Add(byteHolder[z]);
                                }
                                else
                                { var multiplied = MultiplyThenRound(byteHolder[z]);
                                    mEditedRLEArrayOfPixels.Add(multiplied);
                                    Console.WriteLine(multiplied);
                                    using (var stream = new FileStream(path, FileMode.Open, FileAccess.ReadWrite))
                                    {
                                        stream.Position =(i - byteHolder.Length) + z;
                                        stream.WriteByte(Convert.ToByte(byteHolder[z]));
                                        stream.Close();
                                    }
                                }
                                RLEArrayOfPixels.Add(byteHolder[z]);

                            }
                        }

                    }
                    //Console.WriteLine(stringToDisplay + " " +  fewBytes.ToString());
                }
            }
            amountOfBytesRead += temporaryBytesRead;
            return fewBytes;
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }
        int MultiplyThenRound(float a)
        {



            float multi = 0;


            if (a > 128)
            {
                multi = a * 1.5f;
            }
            if (a == 128)
            {
                multi = 128;

            }
            if (a < 128)
            {
                multi = a * 0.667f;
            }
            if (multi > 255)
            {
                multi = 255;
            }
            if (multi < 0)
            {
                multi = 0;
            }
            return Convert.ToInt32(Math.Round(multi));


        }







    }




}


