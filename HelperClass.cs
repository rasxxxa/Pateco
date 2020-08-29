using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication2
{

    class HelperClass
    {
        //Algoritam za pretvaranje celog broja u niz bitova
        public static byte[] IntToByte(int br)
        {
            byte[] array = new byte[8];
            int i = 7;
            while (br != 0)
            {
                array[i] = (byte)(br % 2);
                br /= 2;
                i--;
            }
            for (int j = i; j >= 0; j--)
            {
                array[j] = 0;
            }


            return array;
        }
        public static Tuple<byte,byte> IntToDoubleByte(int number)
        {
            byte[] lowerBytes = new byte[8];
            byte[] higherBytes = new byte[8];
            int tempNum = number;
            for (int i = 7; i>=0;i--)
            {
                lowerBytes[i] = (byte)(number % 2);
                number = number / 2;
            }
            for (int i = 7; i>=0;i--)
            {
                higherBytes[i] = (byte)(number % 2);
                number = number / 2;
            }
            int num1 = ByteToInt(lowerBytes);
            int num2 = ByteToInt(higherBytes);
            return new Tuple<byte, byte>((byte)num2,(byte)num1);
        }
        public static void LongToTripleByte(long number,out byte byte1, out byte byte2, out byte byte3)
        {
            byte1 = 0;
            byte2 = 0;
            byte3 = 0;
            byte[] lowerBytes = new byte[8];
            byte[] middleBytes = new byte[8];
            byte[] higherBytes = new byte[8];
            long tempNum = number;
            for (int i = 7; i >= 0; i--)
            {
                lowerBytes[i] = (byte)(number % 2);
                number = number / 2;
            }
            for (int i = 7; i >= 0; i--)
            {
                middleBytes[i] = (byte)(number % 2);
                number = number / 2;
            }
            for (int i = 7; i >= 0; i--)
            {
                higherBytes[i] = (byte)(number % 2);
                number = number / 2;
            }
            byte3 = (byte)ByteToInt(lowerBytes);
            byte2 = (byte)ByteToInt(middleBytes);
            byte1 = (byte)ByteToInt(higherBytes);
        }
        public static long TripleByteToInt(byte byte1, byte byte2, byte byte3)
        {
            byte[] higherBytes = IntToByte(byte1);
            byte[] middleBytes = IntToByte(byte2);
            byte[] lowerBytes = IntToByte(byte3);
            byte[] merged = new byte[24];
            for (int i = 0; i < 8; i++)
            {
                merged[i] = higherBytes[i];
            }
            for (int i = 8; i < 16; i++)
            {
                merged[i] = middleBytes[i - 8];
            }
            for (int i = 16;i<24;i++)
            {
                merged[i] = lowerBytes[i - 16];
            }

            long sum = 0;
            for (int i = 0; i < merged.Length; i++)
            {
                if (merged[i] != 0)
                {
                    sum += (int)Math.Pow(2, 23 - i);
                }

            }
            return sum;
        }
        public static int DoubleByteToInt(byte num1, byte num2)
        {
            byte[] higherBytes = IntToByte(num1);
            byte[] lowerBytes = IntToByte(num2);
            byte[] merged = new byte[16];
            for (int i = 0; i<8;i++)
            {
                merged[i] = higherBytes[i];
            }
            for (int i = 8;i<16;i++)
            {
                merged[i] = lowerBytes[i - 8];
            }

            int sum = 0;
            for (int i = 0; i < merged.Length; i++)
            {
                if (merged[i] != 0)
                {
                    sum += (int)Math.Pow(2, 15 - i);
                }

            }
            return sum;
        }
        // Algoritam za pretvaranje niza bitova u ceo broj
        public static int ByteToInt(byte[] array)
        {
            int sum = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (array[i] != 0)
                {
                    sum += (int)Math.Pow(2, 7 - i);
                }

            }
            return sum;
        }
        public static byte[] IntTo8Bytes(int broj)
        {

            byte[] bajt = IntToByte(broj);
            byte[] array1 = new byte[8], array2 = new byte[8], array3 = new byte[8], array4 = new byte[8];
            for (int i = 0; i < 2; i++)
            {
                array1[i] = bajt[i];
            }
            for (int i = 2; i < 4; i++)
            {
                array2[i - 2] = bajt[i];
            }
            for (int i = 4; i < 6; i++)
            {
                array3[i - 4] = bajt[i];
            }
            for (int i = 6; i < 8; i++)
            {
                array4[i - 6] = bajt[i];
            }
            for (int i = 2; i < 8; i++)
            {
                array1[i] = 0;
                array2[i] = 0;
                array3[i] = 0;
                array4[i] = 0;
            }
            byte[] result = new byte[4];
            result[0] = (byte)ByteToInt(array1);
            result[1] = (byte)ByteToInt(array2);
            result[2] = (byte)ByteToInt(array3);
            result[3] = (byte)ByteToInt(array4);

            return result;
        }
        public static byte[] TextToSound(string text, byte[] wav, int compressionLevel)
        {
            int channels = wav[22];     // cuva se informacija da li je mono ili stereo 0 mono, 1 stereo

            int pos = 12;   // Preskacemo chunk informacije, i trazimo sekvencu "data" (d hex = 64, a hex = 61, t hex = 74)
            // medjutim posto radimo sa byte podacima, odnosno int, trazicemo sekvencu pomocu integera

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            // Dodatnih 8 praznih chunkova
            pos += 8;
            // Pos postaje direktni pointer na data;
            //TODO: Do some work with data and image
            wav[pos++] = 2;
            int textlengt = text.Length;
            var result = IntToDoubleByte(textlengt);
            wav[pos] = (byte)compressionLevel;
            pos++;
            wav[pos] = (byte)result.Item1;
            pos++;
            wav[pos] = (byte)(result.Item2);
            pos++;
            for (int i = 0; i < text.Length; i++)
            {
                int value = (int)text.ElementAt(i);
                byte[] byteValue = IntToByte(value);
                int steps = 8 / compressionLevel;
                for (int j = 0; j < steps; j++)
                {
                    byte[] wavValue = IntToByte(wav[pos]);
                    for (int k = (8 - compressionLevel); k < 8; k++)
                    {
                        wavValue[k] = byteValue[j * compressionLevel + 7 - k];
                    }
                    var wavIntValue = ByteToInt(wavValue);
                    wav[pos] = (byte)wavIntValue;
                    pos++;
                }


            }
            return wav;
        }
        // Za ubacivanje teksta u zvuk
        public static string SoundToText(byte[] wav)
        {
            int pos = 12;   // Preskacemo chunk informacije, i trazimo sekvencu "data" (d hex = 64, a hex = 61, t hex = 74)
            // medjutim posto radimo sa byte podacima, odnosno int, trazicemo sekvencu pomocu integera

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }

            // Dodatnih 8 praznih chunkova
            pos += 9;
            var conversion = wav[pos];
            pos++;
            byte higherBite = wav[pos];
            pos++;
            byte lowerBite = wav[pos];
            pos++;
            var duzina = DoubleByteToInt(higherBite, lowerBite);
            string final = "";
            for (int i = 0; i < duzina; i++)
            {
                byte[] bite = new byte[8];

                int steps = 8 / conversion;
                for (int j = 0; j < steps; j++)
                {
                    var firstPos = wav[pos];
                    var converted = IntToByte(firstPos);
                    for (int k = (8 - conversion); k < 8; k++)
                    {
                        bite[j * conversion + 7 - k] = converted[k];
                    }
                    pos++;
                }
                final += (char)ByteToInt(bite);
            }
            return final;
        }
        // Za ubacivanje slike u zvuk

        public static int ReturnTypeOFSoundDeconversion(byte[]wav)
        {
            int channels = wav[22];     // cuva se informacija da li je mono ili stereo 0 mono, 1 stereo

            int pos = 12;   // Preskacemo chunk informacije, i trazimo sekvencu "data" (d hex = 64, a hex = 61, t hex = 74)
            // medjutim posto radimo sa byte podacima, odnosno int, trazicemo sekvencu pomocu integera

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            // Dodatnih 8 praznih chunkova
            pos += 8;
            return wav[pos];
        }
        public static byte[] ImageToSound(Bitmap image,byte[] wav,int compressionLevel, int qualityLevel)
        {
            int channels = wav[22];     // cuva se informacija da li je mono ili stereo 0 mono, 1 stereo

            int pos = 12;   // Preskacemo chunk informacije, i trazimo sekvencu "data" (d hex = 64, a hex = 61, t hex = 74)
            // medjutim posto radimo sa byte podacima, odnosno int, trazicemo sekvencu pomocu integera

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }
            // Dodatnih 8 praznih chunkova
            pos += 8;
            // Pos postaje direktni pointer na data;
            //TODO: Do some work with data and image
            wav[pos++] = 1;
            wav[pos] = (byte)compressionLevel;
            pos++;
            wav[pos] = (byte)qualityLevel;
            pos++;
            var width = IntToDoubleByte(image.Width);
            var height = IntToDoubleByte(image.Height);
            wav[pos] = (byte)(width.Item1);
            pos++;
            wav[pos] = (byte)(width.Item2);
            pos++;
            wav[pos] = (byte)(height.Item1);
            pos++;
            wav[pos] = (byte)(height.Item2);
            pos++;
            int steps = 8 / compressionLevel;
            for (int i = 0;i<image.Width;i++)
            {
                for (int j = 0;j<image.Height;j++)
                {
                    var pixel = image.GetPixel(i, j);
                    int red = pixel.R;
                    int green = pixel.G;
                    int blue = pixel.B;
                    byte[] redByte = IntToByte(red);
                    byte[] greenByte = IntToByte(green);
                    byte[] blueByte = IntToByte(blue);
                    if (qualityLevel == 0)
                    {
                        for (int k = 0; k < steps; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                wavValue[l] = redByte[k * compressionLevel +  l - (8 - compressionLevel)];
                            }
                            var wavIntValue = ByteToInt(wavValue);
                            wav[pos] = (byte)wavIntValue;
                            pos++;
                        }
                        for (int k = 0; k < steps; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                wavValue[l] = greenByte[k * compressionLevel + l - (8 - compressionLevel)];
                            }
                            var wavIntValue = ByteToInt(wavValue);
                            wav[pos] = (byte)wavIntValue;
                            pos++;
                        }
                        for (int k = 0; k < steps; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                wavValue[l] = blueByte[k * compressionLevel + l - (8 - compressionLevel)];
                            }
                            var wavIntValue = ByteToInt(wavValue);
                            wav[pos] = (byte)wavIntValue;
                            pos++;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < steps/2; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                wavValue[l] = redByte[k * compressionLevel + l - (8 - compressionLevel)];
                            }
                            var wavIntValue = ByteToInt(wavValue);
                            wav[pos] = (byte)wavIntValue;
                            pos++;
                        }
                        for (int k = 0; k < steps/2; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                wavValue[l] = greenByte[k * compressionLevel + l - (8 - compressionLevel)];
                            }
                            var wavIntValue = ByteToInt(wavValue);
                            wav[pos] = (byte)wavIntValue;
                            pos++;
                        }
                        for (int k = 0; k < steps/2; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                wavValue[l] = blueByte[k * compressionLevel + l - (8 - compressionLevel)];
                            }
                            var wavIntValue = ByteToInt(wavValue);
                            wav[pos] = (byte)wavIntValue;
                            pos++;
                        }
                    }



                }
            }
            return wav;
        }
        public static Bitmap SoundToImage(byte[] wav)
        {
            int pos = 12;   // Preskacemo chunk informacije, i trazimo sekvencu "data" (d hex = 64, a hex = 61, t hex = 74)
            // medjutim posto radimo sa byte podacima, odnosno int, trazicemo sekvencu pomocu integera

            // Keep iterating until we find the data chunk (i.e. 64 61 74 61 ...... (i.e. 100 97 116 97 in decimal))
            while (!(wav[pos] == 100 && wav[pos + 1] == 97 && wav[pos + 2] == 116 && wav[pos + 3] == 97))
            {
                pos += 4;
                int chunkSize = wav[pos] + wav[pos + 1] * 256 + wav[pos + 2] * 65536 + wav[pos + 3] * 16777216;
                pos += 4 + chunkSize;
            }

            // Dodatnih 8 praznih chunkova
            pos += 9;
            var compressionLevel = wav[pos];
            pos++;
            var qualityLevel = wav[pos];
            pos++;
            var strongWidth = wav[pos];
            pos++;
            var weakWidtk = wav[pos];
            pos++;
            var strongHeight = wav[pos];
            pos++;
            var weakHeight = wav[pos];
            pos++;
            var width = DoubleByteToInt(strongWidth, weakWidtk);
            var height = DoubleByteToInt(strongHeight, weakHeight);
            Bitmap bitmap = new Bitmap(width,height);
            for (int i = 0; i< width;i++)
            {
                for (int j = 0; j < height; j++)
                {
                    byte[] red = new byte[8];
                    byte[] green = new byte[8];
                    byte[] blue = new byte[8];
                    var steps = 8 / compressionLevel;
                    if (qualityLevel == 0)
                    {
                        for (int k = 0; k < steps; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                red[k * compressionLevel + l - (8 - compressionLevel)] = wavValue[l];
                            }
                            pos++;
                        }
                        for (int k = 0; k < steps; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                green[k * compressionLevel + l - (8 - compressionLevel)] = wavValue[l];
                            }
                            pos++;
                        }
                        for (int k = 0; k < steps; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                blue[k * compressionLevel + l - (8 - compressionLevel)] = wavValue[l];
                            }
                            pos++;
                        }
                    }
                    else
                    {
                        for (int k = 0; k < steps/2; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                red[k * compressionLevel + l - (8 - compressionLevel)] = wavValue[l];
                            }
                            pos++;
                        }
                        for (int k = 0; k < steps/2; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                green[k * compressionLevel + l - (8 - compressionLevel)] = wavValue[l];
                            }
                            pos++;
                        }
                        for (int k = 0; k < steps/2; k++)
                        {
                            byte[] wavValue = IntToByte(wav[pos]);
                            for (int l = (8 - compressionLevel); l < 8; l++)
                            {
                                blue[k * compressionLevel + l - (8 - compressionLevel)] = wavValue[l];
                            }
                            pos++;
                        }
                    }
                    bitmap.SetPixel(i, j, Color.FromArgb(ByteToInt(red),ByteToInt(green),ByteToInt(blue)));

                }
            }


            return bitmap;
        }
        public static Bitmap SoundInImage(byte[] wav, int width, int height, int compression,int quality, List<byte> imageByte)
        {
            int pos = 0;
            imageByte[pos++] = 4;
            imageByte[pos++] = (byte)compression;
            imageByte[pos++] = (byte)quality;
            byte byte1, byte2, byte3;
            LongToTripleByte(wav.Length,out byte1,out byte2,out byte3);
            imageByte[pos++] = byte1;
            imageByte[pos++] = byte2;
            imageByte[pos++] = byte3;
            long result = TripleByteToInt(byte1, byte2, byte3);
            int posToInsert = (8 - compression);

            for (int i = 0; i < wav.Length; i++)
            {
                byte[] soundSample = IntToByte(wav[i]);
                int step = 0;
                if (i < 1024)
                {
                    step = 8;
                }else
                {
                    step = 8 - quality;
                }
                for (int j = 0; j<step;j++)
                {
                    byte[] arr = IntToByte(imageByte[pos]);
                    arr[posToInsert++] = soundSample[j];
                    imageByte[pos] = (byte)ByteToInt(arr);
                    if (posToInsert >= 8)
                    {
                        pos++;
                        posToInsert = (8 - compression);
                    }

                }
            }
            Bitmap bitmap = new Bitmap(width, height);
            int insertedPos = 0;
            for (int i = 0;i<width;i++)
            {
                for (int j = 0;j<height;j++)
                {
                    var r = imageByte[insertedPos++];
                    var g = imageByte[insertedPos++];
                    var b = imageByte[insertedPos++];
                    bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;
        }

        public static byte[] SoundOutImage(List<byte> imageBytes,byte[] header)
        {
            int pos = 1;
            byte compression = imageBytes[pos++];
            byte quality = imageBytes[pos++];
            byte byte1 = (byte)imageBytes[pos++];
            byte byte2 = (byte)imageBytes[pos++];
            byte byte3 = (byte)imageBytes[pos++];
            long length = TripleByteToInt(byte1, byte2, byte3);
            byte[] wav = new byte[length];
            int posToInsert = (8 - compression);
            for (int i = 0; i < wav.Length; i++)
            {
                byte[] soundSample = new byte[8];
                int step = 0;
                if (i < 1024)
                {
                    step = 8;
                }
                else
                {
                    step = 8 - quality;
                }
                for (int j = 0; j < step; j++)
                {
                    byte[] arr = IntToByte(imageBytes[pos]);
                    soundSample[j] = arr[posToInsert++];
                    if (posToInsert >= 8)
                    {
                        pos++;
                        posToInsert = (8 - compression);
                    }

                }
                wav[i] = (byte)ByteToInt(soundSample);

            }

            return wav;
        }
        internal static Image MultiPictureInsert(List<byte> mainImageBytes, int width, int height, List<List<byte>> smallImageBytes, List<int> qualityBytes, List<Tuple<int, int>> resolutions, int v)
        {
            byte typeOfCompression = 1;
            int pos = 0;
            mainImageBytes[pos++] = typeOfCompression;
            mainImageBytes[pos++] = (byte)v;
            mainImageBytes[pos++] = (byte)smallImageBytes.Count;

            for (int i = 0; i < smallImageBytes.Count; i++)
            {
                var widthPicture = IntToDoubleByte(resolutions[i].Item1);
                var heightPicture = IntToDoubleByte(resolutions[i].Item2);
                mainImageBytes[pos++] = widthPicture.Item1;
                mainImageBytes[pos++] = widthPicture.Item2;
                mainImageBytes[pos++] = heightPicture.Item1;
                mainImageBytes[pos++] = heightPicture.Item2;
                mainImageBytes[pos++] = (byte)qualityBytes[i];
                int posToInsert = (8 - v);
                var picture = smallImageBytes[i];

                for (int j = 0; j < picture.Count; j++)
                {
                    byte[] pictureSample = IntToByte(picture[j]);
                    int step = 8 - qualityBytes[i];

                    for (int k = 0; k < step; k++)
                    {
                        byte[] arr = IntToByte(mainImageBytes[pos]);
                        arr[posToInsert++] = pictureSample[k];
                        mainImageBytes[pos] = (byte)ByteToInt(arr);
                        if (posToInsert >= 8)
                        {
                            pos++;
                            posToInsert = (8 - v);
                        }

                    }

                }
                pos++;
            }


            Bitmap bitmap = new Bitmap(width, height);
            int insertedPos = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var r = mainImageBytes[insertedPos++];
                    var g = mainImageBytes[insertedPos++];
                    var b = mainImageBytes[insertedPos++];
                    bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;


        }
        public static Image MultiSoundInImage(List<byte> imageByte, int width, int height, List<byte[]> sounds, List<byte> qualityBytes, int v)
        {

            int pos = 0;
            imageByte[pos++] = 3;
            imageByte[pos++] = (byte)v;
            imageByte[pos++] = (byte)sounds.Count;
            for (int i = 0;i<sounds.Count;i++)
            {
                byte byte1, byte2, byte3;
                LongToTripleByte(sounds[i].Length, out byte1, out byte2, out byte3);
                imageByte[pos++] = byte1;
                imageByte[pos++] = byte2;
                imageByte[pos++] = byte3;
                imageByte[pos++] = qualityBytes[i];
                int posToInsert = (8 - v);
                byte[] sound = sounds[i];
                for (int j = 0; j<sound.Length;j++)
                {
                    byte[] soundSample = IntToByte(sound[j]);
                    int step = 0;
                    if (j < 1024)
                    {
                        step = 8;
                    }
                    else
                    {
                        step = 8 - qualityBytes[i];
                    }
                    for (int k = 0; k < step; k++)
                    {
                        byte[] arr = IntToByte(imageByte[pos]);
                        arr[posToInsert++] = soundSample[k];
                        imageByte[pos] = (byte)ByteToInt(arr);
                        if (posToInsert >= 8)
                        {
                            pos++;
                            posToInsert = (8 - v);
                        }

                    }

                }
                pos++;
            }


            Bitmap bitmap = new Bitmap(width, height);
            int insertedPos = 0;
            for (int i = 0; i < width; i++)
            {
                for (int j = 0; j < height; j++)
                {
                    var r = imageByte[insertedPos++];
                    var g = imageByte[insertedPos++];
                    var b = imageByte[insertedPos++];
                    bitmap.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return bitmap;
        }

        public static List<byte[]> MultiSoundDeconversion(List<byte> imageByte)
        {
            int pos = 1;
            byte v = imageByte[pos++];
            byte count = imageByte[pos++];
            List<byte[]> sound = new List<byte[]>();

            for (int i = 0; i < count; i++)
            {
                long length = TripleByteToInt(imageByte[pos], imageByte[pos+1], imageByte[pos+2]);
                pos += 3;
                byte quality = imageByte[pos++];
                byte[] wav = new byte[length];
                int posToInsert = (8 - v);
                //for (int i = 0;i<44;i++)
                //{
                //    wav[i] = header[i];
                //}
                for (int j = 0; j < wav.Length; j++)
                {
                    byte[] soundSample = new byte[8];
                    int step = 0;
                    if (j < 1024)
                    {
                        step = 8;
                    }
                    else
                    {
                        step = 8 - quality;
                    }
                    for (int k = 0; k < step; k++)
                    {
                        byte[] arr = IntToByte(imageByte[pos]);
                        soundSample[k] = arr[posToInsert++];
                        if (posToInsert >= 8)
                        {
                            pos++;
                            posToInsert = (8 - v);
                        }

                    }
                    wav[j] = (byte)ByteToInt(soundSample);

                }
                pos++;
                sound.Add(wav);
            }

            return sound;
        }

        internal static List<Image> MultiPictureDeconversion(List<byte> imageBytes)
        {
            int pos = 1;
            int typeOfCompression = imageBytes[pos++];
            int numOfPictures = imageBytes[pos++];
            List<Image> pictures = new List<Image>();
            for (int i = 0; i < numOfPictures; i++)
            {
                var byteWidth1 = imageBytes[pos++];
                var byteWidth2 = imageBytes[pos++];
                var byteHeight1 = imageBytes[pos++];
                var byteHeight2 = imageBytes[pos++];
                var byteQuality = imageBytes[pos++];
                int widthPicture = DoubleByteToInt(byteWidth1, byteWidth2);
                int heightPicture = DoubleByteToInt(byteHeight1, byteHeight2);
                int posToInsert = 8 - typeOfCompression;
                Bitmap bmp = new Bitmap(widthPicture, heightPicture);
                for (int j = 0;j<widthPicture;j++)
                {
                    for (int k = 0; k<heightPicture;k++)
                    {
                        byte[] R = new byte[8];
                        byte[] G = new byte[8];
                        byte[] B = new byte[8];
                        int step = 8 - byteQuality;
                        for (int l = 0; l<step;l++)
                        {
                            var byteAtPos = IntToByte(imageBytes[pos]);
                            R[l] = byteAtPos[posToInsert++];
                            if (posToInsert>=8)
                            {
                                posToInsert = 8 - typeOfCompression;
                                pos++;
                            }
                        }
                        for (int l = 0; l < step; l++)
                        {
                            var byteAtPos = IntToByte(imageBytes[pos]);
                            G[l] = byteAtPos[posToInsert++];
                            if (posToInsert >= 8)
                            {
                                posToInsert = 8 - typeOfCompression;
                                pos++;
                            }
                        }
                        for (int l = 0; l < step; l++)
                        {
                            var byteAtPos = IntToByte(imageBytes[pos]);
                            B[l] = byteAtPos[posToInsert++];
                            if (posToInsert >= 8)
                            {
                                posToInsert = 8 - typeOfCompression;
                                pos++;
                            }
                        }

                        bmp.SetPixel(j, k, Color.FromArgb(ByteToInt(R), ByteToInt(G), ByteToInt(B)));
                    }
                }
                pos++;
                pictures.Add(bmp);
            }

            return pictures;
        }

        internal static Image TextToPicture(List<byte> imageBytes, int width, int height, string v1, int v2)
        {
            int pos = 0;
            imageBytes[pos++] = 2;
            imageBytes[pos++] = (byte)v2;
            byte byte1, byte2, byte3;
            LongToTripleByte(v1.Length,out byte1, out byte2, out byte3);
            imageBytes[pos++] = byte1;
            imageBytes[pos++] = byte2;
            imageBytes[pos++] = byte3;
            int posToInsert = 8 - v2;
            for (int i = 0;i < v1.Length;i++)
            {
                var character = v1[i];
                var assciCodeOfChar = (int)character;
                byte[] arrayOfChar = IntToByte(assciCodeOfChar);
                for (int j = 0;j<8;j++)
                {
                    var intValueOfImageByte = IntToByte(imageBytes[pos]);
                    intValueOfImageByte[posToInsert++] = arrayOfChar[j];
                    imageBytes[pos] = (byte)ByteToInt(intValueOfImageByte);
                    if (posToInsert >= 8)
                    {
                        posToInsert = 8 - v2;
                        pos++;
                    }
                }
            }
            pos = 0;
            Bitmap bmp = new Bitmap(width,height);
            for (int i = 0; i<width;i++)
            {
                for (int j = 0;j<height;j++)
                {
                    var r = imageBytes[pos++];
                    var g = imageBytes[pos++];
                    var b = imageBytes[pos++];
                    bmp.SetPixel(i, j, Color.FromArgb(r, g, b));
                }
            }
            return bmp;
        }

        internal static string ImageWithText(List<byte> imageBytes)
        {
            string text = "";
            int pos = 1;
            int quality = (int)imageBytes[pos++];
            byte byte1 = imageBytes[pos++];
            byte byte2 = imageBytes[pos++];
            byte byte3 = imageBytes[pos++];
            long textSize = TripleByteToInt(byte1, byte2, byte3);
            int posToInsert = 8 - quality;
            for (int i = 0;i<textSize;i++)
            {
                byte[] newCharacter = new byte[8];
                for (int j = 0; j<8;j++)
                {
                    byte[] arrayValue = IntToByte(imageBytes[pos]);
                    newCharacter[j] = arrayValue[posToInsert++];
                    if (posToInsert >= 8)
                    {
                        posToInsert = 8 - quality;
                        pos++;
                    }
                }
                int newCharacterAsciiValue = ByteToInt(newCharacter);
                text += (char)newCharacterAsciiValue;
            }

            return text;
        }

    }
}
