using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoLabs.Extensions;
using Console = Colorful.Console;
using Colorful;
using System.Drawing;

namespace CryptoLabs.ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            //Base64
            Console.WriteLineFormatted("Base64\n", Color.White);

            var hexString = "faea8766efd8b295a633908a3c0828b22640e1e9122c3c9cfb7b59b7cf3c9d448bf04d72cde3aaa0";
            var base64result = HexToBase64(hexString);

            Console.WriteLineFormatted("Source hex string:\n\n{0}\n\nEncoded Base64 string:\n\n{1}\n", Color.Gray, new Formatter[]
                                    {
                                        new Formatter(hexString, Color.LightGoldenrodYellow),
                                        new Formatter(base64result, Color.LightGoldenrodYellow)
                                    });

            Console.WriteLineFormatted("Encoded by System.Convert:\n", Color.Gray);


            byte[] data = HexToByteArray(hexString);
            string base64 = Convert.ToBase64String(data);

            Console.WriteLineFormatted("{0}", Color.Gray, new Formatter[]
            {
                new Formatter(base64, Color.LightGoldenrodYellow)
            });

            //XOR
            Console.WriteLineFormatted("\nXOR", Color.White);

            var XORHexString = "8f29336f5e9af0919634f474d248addaf89f6e1f533752f52de2dae0ec3185f818c0892fdc873a69";
            var XORString = "bf7962a3c4e6313b134229e31c0219767ff59b88584a303010ab83650a3b1763e5b314c2f1e2f166";

            var hexStringBytes = HexToByteArray(XORHexString);
            var XORStringBytes = HexToByteArray(XORString);

            var XORresult = XORByteArrays(hexStringBytes, XORStringBytes);

            Console.WriteLineFormatted("\nXOR Result:\n\n{0}", Color.Gray, new Formatter[]
            {
                new Formatter(ByteArrayToString(XORresult, 0, XORresult.Length),Color.LightGoldenrodYellow)
            });


            //XOR 1 Symbol
            Console.WriteLineFormatted("\nXOR 1 Symbol", Color.White);

            var XORSingleString = "0e0d1f09404c040d1a094c1b03024c180409051e4c0a051e1f184c1a050f18031e15";
            var bytesToXOR = HexToByteArray(XORSingleString);

            for (char c = 'a'; c <= 'z'; c++)
            {
                double probabilityIndex = 0;
                var resultString = Encoding.ASCII.GetString(XORSingleElement(bytesToXOR, c.ToString()));

                probabilityIndex = ProbabilityInString(resultString);

                Console.WriteLineFormatted("KeyChar [{0}] Probability [{1}] -> Result:\n{2}\n", Color.Gray, new Formatter[]
                {
                    new Formatter(c.ToString(),Color.Yellow),
                    new Formatter(probabilityIndex, Color.Red),
                    new Formatter(resultString, Color.LawnGreen)
                });
            }

            Console.ReadKey();
        }

        /// <summary>
        /// Преобразование строки в hex формате в строку Base64
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns>Строка в формате Base64</returns>
        static string HexToBase64(string hexString)
        {
            byte[] bytes = HexToByteArray(hexString);
            BitArray stringBits = new BitArray(0);

            foreach (var b in bytes)
            {
                BitArray bitsInByte = new BitArray(new byte[] { b });
                Reverse(bitsInByte);
                stringBits = stringBits.Append(bitsInByte);
            }

            var bitChunks = stringBits.Split(6);

            StringBuilder resultBuilder = new StringBuilder();
            foreach (var bitChunk in bitChunks)
            {
                Reverse(bitChunk);
                StringBuilder bitRepresentationBuiler = new StringBuilder();
                foreach (var bit in bitChunk)
                    bitRepresentationBuiler.Append((bool)bit ? "1" : "0");

                var base64Result = BitChunkToBase64(bitChunk);
                resultBuilder.Append(base64Result);

                Console.WriteLineFormatted("[{0}] -> Base64: {1}", Color.Gray, new Formatter[] {
                    new Formatter(bitRepresentationBuiler.ToString(),Color.Yellow),
                    new Formatter(base64Result,Color.LawnGreen)
                });
            }

            Console.WriteLine();
            return resultBuilder.ToString();
        }

        /// <summary>
        /// Преобразование строки в hex формате в массив байт
        /// </summary>
        /// <param name="hexString"></param>
        /// <returns>Массив байт</returns>
        public static byte[] HexToByteArray(string hexString)
        {
            hexString = hexString.Replace("-", "");

            byte[] resultArray = new byte[hexString.Length / 2];
            for (int i = 0; i < resultArray.Length; i++)
            {
                resultArray[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            }
            return resultArray;
        }

        /// <summary>
        /// Преобразование массива из 6 бит в Base64 кодировку
        /// </summary>
        /// <param name="array">Массив из 6 бит</param>
        /// <returns>строка с одним символом в кодировке Base64</returns>
        public static string BitChunkToBase64(BitArray array)
        {
            if (array.Count > 6)
                throw new ArgumentOutOfRangeException();

            int offset = 6 - array.Count;

            if (offset != 0)
                array = array.Prepend(new BitArray(offset));

            int[] intValues = new int[1];
            array.CopyTo(intValues, 0);

            if (offset == 6)
                return Base64Map[intValues[0]];

            return Base64Map[intValues[0]] + new string('=', offset / 2);
        }

        /// <summary>
        /// Реверс элементов в массиве бит
        /// </summary>
        /// <param name="array"></param>
        public static void Reverse(BitArray array)
        {
            int length = array.Length;
            int mid = (length / 2);

            for (int i = 0; i < mid; i++)
            {
                bool bit = array[i];
                array[i] = array[length - i - 1];
                array[length - i - 1] = bit;
            }
        }

        /// <summary>
        /// Словарь кодировки в Base64
        /// </summary>
        public static Dictionary<int, string> Base64Map = new Dictionary<int, string>
        {
            { 0, "A" },
            { 1, "B" },
            { 2, "C" },
            { 3, "D" },
            { 4, "E" },
            { 5, "F" },
            { 6, "G" },
            { 7, "H" },
            { 8, "I" },
            { 9, "J" },
            { 10, "K" },
            { 11, "L" },
            { 12, "M" },
            { 13, "N" },
            { 14, "O" },
            { 15, "P" },
            { 16, "Q" },
            { 17, "R" },
            { 18, "S" },
            { 19, "T" },
            { 20, "U" },
            { 21, "V" },
            { 22, "W" },
            { 23, "X" },
            { 24, "Y" },
            { 25, "Z" },
            { 26, "a" },
            { 27, "b" },
            { 28, "c" },
            { 29, "d" },
            { 30, "e" },
            { 31, "f" },
            { 32, "g" },
            { 33, "h" },
            { 34, "i" },
            { 35, "j" },
            { 36, "k" },
            { 37, "l" },
            { 38, "m" },
            { 39, "n" },
            { 40, "o" },
            { 41, "p" },
            { 42, "q" },
            { 43, "r" },
            { 44, "s" },
            { 45, "t" },
            { 46, "u" },
            { 47, "v" },
            { 48, "w" },
            { 49, "x" },
            { 50, "y" },
            { 51, "z" },
            { 52, "0" },
            { 53, "1" },
            { 54, "2" },
            { 55, "3" },
            { 56, "4" },
            { 57, "5" },
            { 58, "6" },
            { 59, "7" },
            { 60, "8" },
            { 61, "9" },
            { 62, "+" },
            { 63, "/" }
        };

        /// <summary>
        /// XOR двух массивов байт одинаковой длинны
        /// </summary>
        /// <param name="buffer1">массив байт - буфер 1</param>
        /// <param name="buffer2">массив байт - буфер 2</param>
        /// <returns>Результат XOR</returns>
        public static byte[] XORByteArrays(byte[] buffer1, byte[] buffer2)
        {
            byte[] resultBuffer = new byte[buffer1.Length];

            for (int i = 0; i < buffer2.Length; i++)
            {
                resultBuffer[i] = (byte)(buffer1[i] ^ buffer2[i]);
            }

            return resultBuffer;
        }

        public static string ByteArrayToString(byte[] value, int startIndex, int length)
        {
            int chArrayLength = length * 3;
            char[] chArray = new char[chArrayLength];
            int num1 = startIndex;
            int index = 0;
            while (index < chArrayLength)
            {
                byte num2 = value[num1++];
                chArray[index] = GetHexValue((int)num2 / 16);
                chArray[index + 1] = GetHexValue((int)num2 % 16);
                chArray[index + 2] = '-';
                index += 3;
            }
            return new string(chArray, 0, chArray.Length - 1);
        }

        private static char GetHexValue(int i)
        {
            if (i < 10)
                return (char)(i + 48);
            return (char)(i - 10 + 65);
        }

        private static byte[] XORSingleElement(byte[] bytesToXOR, string XORElement)
        {
            byte[] xorBytes = new byte[bytesToXOR.Length];

            for (var i = 0; i < bytesToXOR.Length; i++)
            {
                xorBytes[i] = Encoding.ASCII.GetBytes(XORElement)[0];
            }

            return XORByteArrays(bytesToXOR, xorBytes);
        }

        private static Dictionary<char, double> ENGWordsUsage = new Dictionary<char, double>
        {
                {'a',8.167},
                {'b',1.492},
                {'c',2.782},
                {'d',4.253},
                {'e',12.702},
                {'f',2.228},
                {'g',2.015},
                {'h',6.094},
                {'i',6.966},
                {'j',0.153},
                {'k',0.772},
                {'l',4.025},
                {'m',2.406},
                {'n',6.749},
                {'o',7.507},
                {'p',1.929},
                {'q',0.095},
                {'r',5.987},
                {'s',6.327},
                {'t',9.056},
                {'u',2.758},
                {'v',0.978},
                {'w',2.360},
                {'x',0.150},
                {'y',1.974},
                {'z',0.074}
    };

        public static double ProbabilityInString(string source)
        {
            Dictionary<char, int> charCount = new Dictionary<char, int>();
            Dictionary<char, double> charUsage = new Dictionary<char, double>();
            for (char c = 'a'; c <= 'z'; c++)
            {
                charCount.Add(c,source.Count(f => f == c));
            }

            int totalChars = charCount.Count;

            foreach (var key  in charCount.Keys)
            {
                charUsage.Add(key, (double)charCount[key] / (double)totalChars);
            }

            double totalProbability = 0;

            foreach(var key in ENGWordsUsage.Keys)
            {
                var usageValue = ENGWordsUsage[key];
                var currentUsage = charUsage[key] - usageValue;
                totalProbability += Math.Abs(currentUsage);
            }

            return totalProbability;
        }
    }
}
