using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows;

namespace IzendaEncryptor
{
    public partial class MainWindow : Window
    {
        #region Constants
        private const string KEY = "THISISKEY1234567"; //must be at least 16 characters long (128 bits) 

        private const string InitializationVector = "ALDAOQJkdak10314";

        private const int RequiredKeyLength = 16;

        private static readonly AesCryptoServiceProvider Crypto = new AesCryptoServiceProvider();
        #endregion

        #region CTOR
        public MainWindow() => InitializeComponent();
        #endregion

        #region Methods
        private void EncryptButtonClicked(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(rawPasswordTextBox.Text))
                return;

            var rawPassword = rawPasswordTextBox.Text;

            var encrypted = Encrypt(rawPassword);

            resultTextBox.Text = encrypted;
        }

        public static string Encrypt(string raw)
        {
            EnsureKeyLength(KEY);

            var keyBytes = Encoding.ASCII.GetBytes(KEY);
            var ivBytes = Encoding.ASCII.GetBytes(InitializationVector);

            byte[] inBlock = Encoding.UTF8.GetBytes(raw);
            ICryptoTransform xfrm = Crypto.CreateEncryptor(keyBytes, ivBytes);
            byte[] outBlock = xfrm.TransformFinalBlock(inBlock, 0, inBlock.Length);
            return Convert.ToBase64String(outBlock);
        }

        private static void EnsureKeyLength(string key)
        {
            if (key.Length < RequiredKeyLength)
            {
                throw new Exception($"The encryption key must be {RequiredKeyLength} characters long.");
            }
        }
        #endregion
    }
}
