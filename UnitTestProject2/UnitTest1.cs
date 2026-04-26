using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Rsa;
using System.Numerics;

namespace UnitTestProject2
{
    [TestClass]
    public class RsaCipherTests
    {
        private RsaCipher _cipher;

        // Метод выполняется перед каждым тестом
        [TestInitialize]
        public void Setup()
        {
            _cipher = new RsaCipher();
        }

        [TestMethod]
        [Description("Проверка корректности генерации ключей N, E и D для известных простых чисел.")]
        public void GenerateKeys_ValidPrimes_CalculatesCorrectParameters()
        {
            // Arrange (Подготовка)
            int p = 61;
            int q = 53;

            // Act (Действие)
            _cipher.GenerateKeys(p, q);

            // Assert (Проверка)
            Assert.AreEqual(new BigInteger(3233), _cipher.N, "Модуль N рассчитан неверно.");
            Assert.AreEqual(new BigInteger(17), _cipher.E, "Экспонента E должна быть 17.");
            Assert.AreEqual(new BigInteger(2753), _cipher.D, "Закрытый ключ D рассчитан неверно.");
        }

        [TestMethod]
        [Description("Проверка обратимости: расшифрованный текст должен совпадать с исходным.")]
        public void EncryptAndDecrypt_NormalString_ReturnsOriginalText()
        {
            // Arrange
            _cipher.GenerateKeys(61, 53);
            string originalText = "Hello RSA 123!";

            // Act
            var encrypted = _cipher.Encrypt(originalText);
            var decrypted = _cipher.Decrypt(encrypted);

            // Assert
            Assert.AreEqual(originalText, decrypted, "Текст после дешифрования не совпадает с оригиналом.");
        }

        [TestMethod]
        [Description("Проверка работы с кириллицей и спецсимволами.")]
        public void EncryptAndDecrypt_CyrillicAndSymbols_ReturnsOriginalText()
        {
            // Arrange
            _cipher.GenerateKeys(17, 19);
            string originalText = "Проверка связи! @#№$%";

            // Act
            var encrypted = _cipher.Encrypt(originalText);
            var decrypted = _cipher.Decrypt(encrypted);

            // Assert
            Assert.AreEqual(originalText, decrypted);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [Description("Негативный тест: шифрование пустой строки должно вызывать исключение.")]
        public void Encrypt_EmptyString_ThrowsArgumentException()
        {
            // Arrange
            _cipher.GenerateKeys(11, 13);

            // Act
            _cipher.Encrypt("");

            // Assert — обрабатывается атрибутом ExpectedException
        }

        [TestMethod]
        [Description("Проверка, что зашифрованные данные действительно отличаются от исходных.")]
        public void Encrypt_String_ResultIsDifferentFromInput()
        {
            // Arrange
            _cipher.GenerateKeys(61, 53);
            string originalText = "A"; // Код 'A' = 65

            // Act
            var encrypted = _cipher.Encrypt(originalText);

            // Assert
            // (65^17) mod 3233 = 2790
            Assert.AreNotEqual((BigInteger)originalText[0], encrypted[0], "Шифрование не изменило данные.");
            Assert.AreEqual(new BigInteger(2790), encrypted[0]);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        [Description("Негативный тест: дешифрование пустого массива.")]
        public void Decrypt_NullArray_ThrowsArgumentException()
        {
            // Act
            _cipher.Decrypt(null);
        }
    }
}
