﻿using System;
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
            string[] array = Directory.GetFiles(@"C:\Users\user\Desktop\thingy");
            for (int i = 0; i < array.Length; i++)
            {
                amountOfBytesRead = 0;
                currentFile = array[i];

            ReadFile();
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// 
        public string currentFile;
        public void ReadFile()
        {


            var ListOfBytes = File.ReadAllBytes(currentFile);
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
                        mUncompressedPixels = int.Parse(readAFewBytes(ListOfBytes, width * 6, "mUncompressedPixels", false, true, int.Parse(mRobeChannel), true));
                    }
                    else
                    {
                        mUncompressedPixels = int.Parse(readAFewBytes(ListOfBytes, width * 3, "mUncompressedPixels", false, true, int.Parse(mRobeChannel), true));
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
        List<int> mEditedRLEArrayOfPixels = new List<int>();

        List<int> RLEArrayOfPixels = new List<int>();

        int amountOfBytesRead;
        public string readAFewBytes(byte[] file, int amountOfBytesToRead, string stringToDisplay, bool shouldParseAsWholeOrOneByOne = true, bool RLEArray = false, int notRobeChannel = 0, bool uncompressed = false)
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
                        fewBytes = 0.ToString();

                        if (uncompressed == false)
                        {
                            for (int z = 0; z < byteHolder.Length; z++)
                            {

                                if (RLEArray == true)
                                {
                                    int numberToDivide;
                                    if (notRobeChannel == 0)
                                    {
                                        numberToDivide = 7;

                                    }
                                    else
                                    {
                                        numberToDivide = 4;

                                    }
                                    if (z % numberToDivide == 0)
                                    {
                                        mEditedRLEArrayOfPixels.Add(byteHolder[z]);
                                    }
                                    else
                                    {
                                        var multiplied = MultiplyThenRound(byteHolder[z]);
                                        Console.WriteLine(multiplied);
                                        using (var stream = new FileStream(currentFile, FileMode.Open, FileAccess.ReadWrite))
                                        {
                                            stream.Position = ((i - byteHolder.Length) + z) + 1;
                                            stream.WriteByte(Convert.ToByte(multiplied));
                                            stream.Close();
                                        }
                                    }
                                    RLEArrayOfPixels.Add(byteHolder[z]);
                                }
                            }
                        }



                        else if (uncompressed == true)
                        {
                            for (int z = 0; z < byteHolder.Length; z++)
                            {
                                var multiplied = MultiplyThenRound(byteHolder[z]);
                                Console.WriteLine("Final number is " + multiplied);
                                using (var stream = new FileStream(currentFile, FileMode.Open, FileAccess.ReadWrite))
                                {
                                    stream.Position = ((i - byteHolder.Length) + z) + 1;
                                    stream.WriteByte(Convert.ToByte(multiplied));
                                    stream.Close();
                                }
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

           // Console.WriteLine("Input number is " + a);

            float multi = a;
            float distanceFrom128 = a - 128;


            if(a != 128)
                multi = a * ((a / 128) * (a / 120));

           
           // Console.WriteLine("Multiplied is " + multi);
           

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


