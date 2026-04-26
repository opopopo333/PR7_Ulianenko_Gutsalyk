using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Numerics;

namespace Rsa
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private RsaCipher _cipher = new RsaCipher();
        private BigInteger[] _lastEncrypted;

        public MainWindow() => InitializeComponent();

        private void BtnGenerate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int p = int.Parse(TxtP.Text);
                int q = int.Parse(TxtQ.Text);
                _cipher.GenerateKeys(p, q);
                MessageBox.Show($"Ключи созданы!\nN: {_cipher.N}\nE: {_cipher.E}\nD: {_cipher.D}", "Успех");
            }
            catch (Exception ex) { MessageBox.Show("Ошибка параметров: " + ex.Message); }
        }

        private void BtnEncrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_cipher.N == 0) throw new Exception("Сначала сгенерируйте ключи!");
                _lastEncrypted = _cipher.Encrypt(InputText.Text);
                OutputText.Text = string.Join(", ", _lastEncrypted);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void BtnDecrypt_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_lastEncrypted == null) throw new Exception("Нет данных для расшифровки!");
                OutputText.Text = _cipher.Decrypt(_lastEncrypted);
            }
            catch (Exception ex) { MessageBox.Show(ex.Message); }
        }
    }
}
