using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Numerics;

namespace Rsa
{
    // <summary>
    /// Класс для упрощенного RSA шифрования.
    /// </summary>
    public class RsaCipher
    {
        public BigInteger N { get; private set; }
        public BigInteger E { get; private set; }
        public BigInteger D { get; private set; }

        /// <summary>
        /// Генерирует ключи на основе простых чисел p и q.
        /// </summary>
        public void GenerateKeys(int p, int q)
        {
            if (p <= 1 || q <= 1) throw new ArgumentException("Числа должны быть больше 1.");

            BigInteger n = (BigInteger)p * q;
            BigInteger phi = (BigInteger)(p - 1) * (q - 1);
            BigInteger e = 17; // Стандартная экспонента

            // Находим D (обратное число по модулю Phi)
            BigInteger d = CalculateInverse(e, phi);

            N = n;
            E = e;
            D = d;
        }

        private BigInteger CalculateInverse(BigInteger e, BigInteger phi)
        {
            BigInteger k = 1;
            while (true)
            {
                if ((k * phi + 1) % e == 0)
                    return (k * phi + 1) / e;
                k++;
            }
        }

        public BigInteger[] Encrypt(string text)
        {
            if (string.IsNullOrEmpty(text)) throw new ArgumentException("Текст пуст");
            byte[] data = Encoding.UTF8.GetBytes(text);
            BigInteger[] result = new BigInteger[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = BigInteger.ModPow(data[i], E, N);
            return result;
        }

        public string Decrypt(BigInteger[] data)
        {
            if (data == null || data.Length == 0) throw new ArgumentException("Нет данных");
            byte[] result = new byte[data.Length];
            for (int i = 0; i < data.Length; i++)
                result[i] = (byte)BigInteger.ModPow(data[i], D, N);
            return Encoding.UTF8.GetString(result);
        }
    }
}
