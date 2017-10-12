using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CryptoLabs.Extensions
{
    public static class ArrayExtenstions
    {
        /// <summary>
        /// Разделяет массив на несколько маленьких.
        /// </summary>
        /// <typeparam name="T">Тип массива.</typeparam>
        /// <param name="array">Массив для разделения.</param>
        /// <param name="size">Размер массивов на которые будет разделяться</param>
        /// <returns>Массив содержащий массивы полученные в результате разделения</returns>
        public static IEnumerable<IEnumerable<T>> Split<T>(this T[] array, int size)
        {
            for (var i = 0; i < (float)array.Length / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        /// <summary>
        /// Разделяет битовый массив на несколько маленьких.
        /// </summary>
        /// <param name="array">Массив для разделения.</param>
        /// <param name="size">Размер массивов на которые будет разделяться</param>
        /// <returns>Массив содержащий битовые массивы полученные в результате разделения</returns>
        public static IEnumerable<BitArray> Split(this BitArray array, int size)
        {
            var chunksCount = (float)array.Length / size;
            var lastChunkOffset = array.Length % size;

            int chunkIndex = 0;
            for (var i = 0; i < chunksCount; i++)
            {
                int chunkSize = size;

                if (++chunkIndex > chunksCount)
                    chunkSize = lastChunkOffset;

                BitArray bitBuffer = new BitArray(chunkSize);

                for (var j = 0; j < chunkSize; j++)
                {
                    bitBuffer.Set(j, array[(i * size) + j]);
                }

                yield return bitBuffer;
            }
        }

        public static BitArray Prepend(this BitArray current, BitArray before)
        {
            var bools = new bool[current.Count + before.Count];
            before.CopyTo(bools, 0);
            current.CopyTo(bools, before.Count);
            return new BitArray(bools);
        }

        public static BitArray Append(this BitArray current, BitArray after)
        {
            var bools = new bool[current.Count + after.Count];
            current.CopyTo(bools, 0);
            after.CopyTo(bools, current.Count);
            return new BitArray(bools);
        }

    }
}
