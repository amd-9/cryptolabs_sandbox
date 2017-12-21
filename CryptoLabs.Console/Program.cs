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
            /****************
             * Base64
             ***************/
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

            /****************
             * XOR
             **************/

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

            /****************
             * XOR 1 Symbol
             **************/

            Console.WriteLineFormatted("\nXOR 1 Symbol", Color.White);

            var XORSingleString = "130c14061143170c4307061017110c1a43020d43060d170a110643130f020d06174d";

            var xor1resultsSingle = XorBy1Symbol(XORSingleString);

            Console.WriteLineFormatted("KeyChar\tProbability\tResult\t", Color.White);

            foreach (var xor1Result in xor1resultsSingle)
            {
                Console.WriteLineFormatted("[{0}]\t[{1}]\t{2}", Color.Gray, new Formatter[]
                {
                    new Formatter(xor1Result.Char.ToString(), Color.Yellow),
                    new Formatter(xor1Result.Probability, Color.Red),
                    new Formatter(xor1Result.ResultString, Color.LawnGreen)
                });
            }

            /************* 
             * XOR 1 from file
             ***************/
            Console.WriteLineFormatted("\nPress any key for next task", Color.Orange);
            Console.ReadKey();

            Console.WriteLineFormatted("\nXOR 1 Symbol (from file)", Color.White);

            List<string> xorStrings = new List<string>()
            {
                "80a79d02f243a8886c4e50f28286f21bd445e256b553787fec814966fc85cb52dc62cf687bb1e1f0b4d9efe72a89cf915a4b2b2ee83a81ac25a6e1c9f9c1eb",
                "4fbd7cb715c0c9d3c0fbe9de13b61c99f44d12f2d07e5a59ada1bdeb7d2dcb341c18d874c7c61a0487e4616484d58510dbf1781199c56107b33877547ddcf3",
                "32c7223b0c420c14e9d869da0d59d5a75e8ba16a5d63c4e570a744d8f8fe25a62780bba04d2b0512037c0b43a7294e99b5eb3e23f35ad27290e0bc38a91642",
                "533516ce3a9bdda56b8820b05b23b5761813a8d091e54c238d69cb83f9004c2f25c10c740441eb515e1bda08eb5a0397689eca762b54b785c88364b50c4d1f",
                "45e8fb5126a2a9c8ef70924ffb4dd2db2db521ca270f79e69f70e3eee432b36630e59ba1100cf04d406f579b098fc5bdfae38be9945768056d8523311070aa",
                "ec56024d92bb3d3ae2e87f0ea46f8fcf6b51995ef49af9adf478e30cfbfaee91ce004374eb8e5a6e73723a35d4c9604f34da6f3cdf8d33c3fa256132465579",
                "6857a4aeb946703445327440166d6a5794ca1c4556623315c08697bab798c9384c43e7427e7e771ce9adb6db3182a46d3c043e5d2695b17b0d60d547ccda9c",
                "e4dc2ad8c8792274817985579a4d2937e13137d5cdfcafe405dbc26f34008e268b82271d4c8e4312e52094d8db0f191dbb6fa582f718568bacd4d79f44f2d3",
                "7653a635c43a79a0bde42c6e4035fe6eb27fec9c4b9b2b963b4460286ee7cc2a94e7186c3257a81827696a0b09663ee42190cda27b08e3dc116cdc365b4452",
                "06daa1c40a0a2f2994ec090109d32347cdb98320da3a95ac241bab3bd14251968f035ba16769e50d0666bec26794f5c7fb95da4e3b45f73b3e5fc02a430e23",
                "0ecb39893107124ec018c9cdc80715c001c4493b10bdb81da67b71500baa08fd84bc3ca585d383bcd8f1c0e0847b96e79fc17424886f83105836d53bcb7379",
                "6db9e85d1d68a8da648bf92c3d9c51008cade56be7052395e1fdd3efc0146d64896ceee5d618b5a126ad73b957ab7c1a20cccd7530f6fd18f738bf9c38ecb5",
                "c201f1f228d3b9210976fe7f09763e038602a0081476b309161a5e731d51f77c0ab282426da30a9a09b565a613d686978ce3e32f9039cff274d3b2006bc531",
                "11b8e74d66f75a886f2ab4640e20e55a57d5fe984722d6b22677f5999bb195073e9d1b072e0c5e0a6b1c0d7e0fc53b9d53a20b64c0af163dd0676903c1cc3a",
                "c843893fefb3b7b3be48bcde02274dccc6daef731fb0a30e9e7c4b4cd789384276e96e0725440404f4ade974f7377a5d0427e07b47d6ab0fdaefbe35f79b52",
                "fd35a621c31780a971acf5542a13b89b3b946b04f40c2b376b5d4d8cd69fb50b07c5cdf8a9546ac8a6126a87e4df28ff469a7443c5795385877f19d7491a04",
                "ed7dd8b0f0cfedfa6521a8c47725ecee52aad5609e2269fdbe19bbdd74e4fafe9773736d6047890eefa4120a7184b918b6e48045bf11499b5f81d5e50997e1",
                "ac497b5d707603fc508c348de967467e794591e049888da8eb006c0a66216051f61336f56b98af057de2c8d62f57529122167eae4ac98f08d3ef62306aea48",
                "469d602ec7954e9d43838f8945a408b22f7e8c43c11d4d8c87c6f7c7c667cb6bc101149f2d666c47fbb43611a62024a81a31c33233d0c8b086170e2a9d073a",
                "0e010b4f080e06014f07061c4f070a031f4f06014f1d0a1c1b001d0601084f1f0a0e0c0a4f0e010b4f051a1c1b060c0a4f1b004f1b070a4f080e030e171665",
                "32ef2cc35f60372c53e200d29d2be245aadb8147a201ac7d4158ab7da2aad140b90be00eaf8b248210854627c31720a16d8fffb58cd053331becfbd5d9b191",
                "35844248d7ab0ee91147f54b1031099326fae4403b9b224f6d7d298e911d2f6f9ed158b0fe724cae43885c91926863fd708a03f89b53502731e82f569b2723",
                "c0dc41829b766cfd8640a96e333798f186ae409e0b3b419859f0b74afde625218500195a58eee549a1d333d868d3be361715392f8951019f55aa9d6a70fb28",
                "5d6854087843eb243f56aab0a2b09123d09d90660ab9b06d73ea965b33bc286bdbe562b8dd76dc3744a6fa051aea761e9ea6d11e6e097ca836b8bd2ce35ebb",
                "bd42a63389aaa90fb8b2e70e54311a246a1924e770f4f94c3bf26138b455e706545657390a58c386e8dcabc255b73f16adad6b2da2837707236f366888e440",
                "641b945735e41667aab349ac52c7e5ac82c418c382c7ba085e11821dd3e76140aa4a90464c98d31f0972ebe598537c9d35e337d0764c89f94631bc49ffee1b",
                "c8a91e473ec2869f675fd68745e27550ed453a279ed1cc3187ddac1667e765bea7bac9361461353aaa35e53aca84705af3b9994ea4d584619c20e5b32f51ed",
                "61ffca66eddf88a7f8775f21d85c50879781d7892342b33863e2bdaf8c75b4a96735f472d8fd564768cf465108bcab21adfd4dc24f17f84b6666d7a47c584a",
                "e6b0fe24bf70e694ea3999b0465d3c67d942a43b9278092d6d58f7a744552cdacb6244f043cfe88bedf8087b1c4a3a2df087726fdf08bf7b0bb2ee4419beb3",
                "47a25949bdc5e362055ff0f701f9515eeb1d07ead836dc4a50fda4ec5d1af52e729f10bf3180f572c603e3773d98d64aad277a9fe6febcf9dd40b5ce227425",
                "0d98f75444e8ee9b13c874149d5f2ea93d2c45c0d84be9e9abc2a05971d76a628a2d8a652f70a85c2ada48d03924c1ff60823948559cec943984b517d4daaa",
                "7d2682b86312a30c061f0623efedea93abf0949f4a4f260ff9f734d1b2dbf66e80762873187433843a0d89ebe5bb63154dfa042f078cc5b9ab400cee0d6dc1",
                "ebf72443b503b560570be3be840a7fb94df03e80d16f365cc083bb2e8efff31a8df5ff99dd20f7b58cee0d646f9d6606c49c09d1e7dd73d67c564f3b06e067",
                "efec194e5a586cf845cadb18a74a611ca37c94f64011efb96f681e2c9783ecc0b21bcffa9fd3ae08801880dfa6aa61fbb9d49dd1f6c5cb82833cd139ebcd1f",
                "0557ad22b7bc60ef7644026eec35102ddf5df923c6e4781ec0d45e9079ea9e0f146f7190cec20b50019169accad7f454c1017b3a10d1d89cf28d5f66e2d45c",
                "fae74aa4a3d13135837b09fdfa6db5592aa39d01559f195d1aee4184d4820d49d0ca979e3d40701d36eb38251c3acccd020793bfb609059e63df3726bcfe81",
                "34d7c16e93af7b1480e9211ab89048dbc9ac7e0f58822286770db911a934651f78a86a9e8e3f5e4d0d5c676886f1d9c382cec30a82dbd4a75fbe9d6b4d1c25",
                "4a5ae08fb2b5f1952cd977210c96b41116d85db1097f3509ed229dcc0da27ceb05dc51bc4674bbcf5490262697f91bca28bef9410a7fef8cd40509e0104a9f",
                "2325c79b02381b6fba6e42524701eb24ad1cdb51461f44f9c5a347446a34388b078dd75c789124feb890e472bcde5ca9aa2448da36b363b49de045e49ba34b",
                "bca2790802d573e253128b13d5958f8378a1779672ae2ec4a5bbccc7da73f08bb4cae66a47e078d558046cdf56899dd8893476f2b6d89bc025581c3a4227d9",
                "1cb093e941595307d43efad4f84845ef359387cbf34e33446d386d1fcd8de2619fcba22e603c8875e99a5d22e4050f2201008ff8c0aa14e9d2a4d09b1a0362",
                "00f63ac477c287cb17556151e2b12108cbfa677b2e4953cdc2ff06c341817da96e24ae4bc71b52b0975fa04332711d4887b7b91f59246c024ea93468cadc0d",
                "9a83e983958d81ae22be19dbdf33421e1f329645c4e534295c2767e79dc86712c0c4920004df42408c0da6c7a725edea0590c2300f69e35e925bc2d27acfee",
                "98b3d4817823cdb26bed4f7815218a1ea9e6c7c822ef92a79172220467ce8f6a8066d284e53212e1b627282377c7f42d4ce9bf5bd7e448be72fb10b3a101e9",
                "5c91f71d1291e410a138469303107b92bf5e45b6e932d48325ebb500ef119e848eb4e25118f3dd7c61ad19b2a00142cc700496ed2ecc126cbadff1e483873f",
                "b4f555d3a206715eff8a23c4de9f22f9646af0e6d09a52a241aea7e0add1ac439c04c2b74e6ccca91ac12d38e9647b592d7cbd78c480542870adebe5870069",
                "eb16afbae3c8bce812701157b0feaf4d66f9b4bef59a1f8a05922c71b1dadba0a92536a16ca14ad8be7514281a6d774a2260a6b9ff43ee1c02aedc40e2885b",
                "6fd3554166cbe173e351c5ede62dfc3077b689fe3435739d2973ad0b24bb632e40b3274fd2f39b70a5a8f8dac97cfa144f226f3425c03628e252bf941ccdb4",
                "8db6978b226bed23ea339aa05efd1cce2e0c24448aacde980dbf6289bb04f65f1d20dfa5e0a8791eb0bc0eda826fd68dc3e19d03997e9d218ee6718d540631",
                "f70424c66f3880e395c67fa8b85b9c1baacc0d9aad9b258d368fc3a9e2d9c081c9514963a67976c0592764c387cc6549a4f509bbc934394ef65fdaa4c62b20",
                "576108dc66208d94cb9ab1766d040791434b3046331ee5082c646f90187a20ecebe2a370742e17e19f338fe4169b8d645ee1c86c883fa865370ce529f80e58",
                "4e600990412f960590a2564ff9a7227125942a82d211111ffb19bd53790751f614d4404185b9343d881ba44f692f33f59eac8324115be2b8c39cffb9cb230b",
                "613f582ecd07c5f241e254f49420ce1b9afad3ad12c160ac5b8166202faa12e6038a465dc84e425b5c78d5daaa144ef2e14529605212f551057deeaa8a692e",
                "7b094c8a1fb20c956df13147f55f07c07257e149adfc225e15b5fdf726c3741abcb5cd4d09828c82d30cfa77d8d873ab4952063e16e1610df7fb77388ed992",
                "9f6936caa3f299f02ed111fcb76a9b474e137c7083cb9704670ac3a7b6ecb780b0438716555e6a222e6506ca30b8614f14a395dff98d1bf1fc3308ab7719e5",
                "1257d2053f680d5d8bcf8e735a9ef8d6744b87d6a4d999c44a051b6864c58d832ddd8f181270c2e2e29ffb437a842c92a8717391a6345727586987807afa5b",
                "b00f6a46538753df6e28a00231dbb9157719a3b3e0eb024ec724dfbd1bc879ca6e0ff15d45d470f33e0ab7b7d4d148e6a2d34cf6b30929ea29223fded02ce4",
                "b9047d1d48508118a1add184aa0485e661a24637745931c01a034656f305f9df20645ed06222bed1b459e93b680fded395bb5bba905213f8a1cb8ed0f8ce30",
                "fb2bc07ee46abfb3b6f55e10b934f2853520cfcc1cb8af64c3b19fe6a7140fe08f3c21a035a4a062a8309a2c1ac5547cb5f3a06f7584254f077fd1937c2b10",
                "614fb2ef4932877652f2062dcee250264add654164351ba9a98226572a60c8211cd77e06a1925ace558e673a483d8ce80ad5be14b1a9ae07614d5bf3d1d70f",
                "79ee61fff79249ce6338ca3fffbac1a6d2b8d5f4bf158c843d1f4072182ec678e5f9f09147b9e15021ba9880cc0d1ad66279da11a40c51e7437c1d3d7f3440"


            };
            List<XOR1Result> xor1ResultsFile = new List<XOR1Result>();

            foreach (var xorString in xorStrings)
            {
                var currentXor1StringResults = XorBy1Symbol(xorString);
                xor1ResultsFile.Add(currentXor1StringResults.First());
            }

            Console.WriteLineFormatted("KeyChar\tProbability\tResult\t", Color.White);

            foreach (var xor1Result in xor1ResultsFile.OrderBy(r => r.Probability))
            {
                Console.WriteLineFormatted("[{0}]\t[{1}]\t{2}", Color.Gray, new Formatter[]
                {
                    new Formatter(xor1Result.Char.ToString(), Color.Yellow),
                    new Formatter(xor1Result.Probability, Color.Red),
                    new Formatter(xor1Result.ResultString, Color.LawnGreen)
                });
            }

            Console.WriteLineFormatted("\nPress any key for next task", Color.Orange);
            Console.ReadKey();

            /*
             * XOR with repeating key
             * */
            Console.WriteLineFormatted("\nXOR Repeating key", Color.White);

            var xorRepeatingKeyString = "THIS IS A MESSAGE";
            var xorKey = "YOU";

            var xorByRepeatingKeyResult = XORByRepeatingKey(Encoding.ASCII.GetBytes(xorRepeatingKeyString), xorKey);

            Console.WriteLineFormatted("[{0}]", Color.Gray, new Formatter[]
            {
                new Formatter(ByteArrayToHexString(xorByRepeatingKeyResult), Color.LawnGreen)
            });

            Console.WriteLineFormatted("\nPress any key for next task", Color.Orange);
            Console.ReadKey();

        }
        
        /// <summary>
        /// XOR по одному символу указанной строки
        /// </summary>
        /// <param name="XORString">строка для XOR по одному символу</param>
        private static IEnumerable<XOR1Result> XorBy1Symbol(string XORString)
        {
            var bytesToXOR = HexToByteArray(XORString);

            //XOR 1 Results
            List<XOR1Result> xor1Results = new List<XOR1Result>();

            for (char c = 'a'; c <= 'z'; c++)
            {
                double probabilityIndex = 0;
                var resultString = Encoding.ASCII.GetString(XORSingleElement(bytesToXOR, c.ToString()));

                probabilityIndex = ProbabilityInString(resultString);

                xor1Results.Add(new XOR1Result
                {
                    Char = c,
                    Probability = probabilityIndex,
                    ResultString = resultString
                });
            }

            //Sort by probability value
            var sortedXor1Results = xor1Results.OrderBy(r => r.Probability);

            return sortedXor1Results;
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

        public static string ByteArrayToHexString(byte[] bytes)
        {
            StringBuilder hex = new StringBuilder(bytes.Length * 2);
            foreach (byte b in bytes)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
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

        private static byte[] XORByRepeatingKey(byte[] bytesToXOR, string XORKey)
        {
            byte[] xorBytes = new byte[bytesToXOR.Length];
            int keyCounter = 0;

            for (var i = 0; i < bytesToXOR.Length; i++)
            {
                xorBytes[i] = Encoding.ASCII.GetBytes(new Char[]{XORKey[keyCounter]})[0];
                keyCounter++;

                if (keyCounter == XORKey.Length)
                    keyCounter = 0;
            }

            Console.WriteLineFormatted("Generated repeating Key: [{0}], length: [{1}]",Color.Aqua,new Formatter[]
            {
                new Formatter(Encoding.ASCII.GetString(xorBytes),Color.Yellow),
                new Formatter(xorBytes.Length,Color.Yellow),
            });

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
                charCount.Add(c, source.Count(f => f == c));
            }

            int totalChars = charCount.Count;

            foreach (var key in charCount.Keys)
            {
                charUsage.Add(key, (double)charCount[key] / (double)totalChars);
            }

            double totalProbability = 0;

            foreach (var key in ENGWordsUsage.Keys)
            {
                var usageValue = ENGWordsUsage[key];
                var currentUsage = charUsage[key] - usageValue;
                totalProbability += Math.Abs(currentUsage);
            }

            return totalProbability;
        }
    }

    public class XOR1Result
    {
        public char Char { get; set; }
        public double Probability { get; set; }
        public string ResultString { get; set; }
    }
}
