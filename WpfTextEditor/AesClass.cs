﻿using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace WpfTextEditor
{
    public static class AesClass
    {
        public static string Encrypt(string plainText)
        {
            if (plainText == null)
                throw new ArgumentNullException("plainText");

            //encrypt data
            var data = Encoding.Unicode.GetBytes(plainText);
            byte[] encrypted = ProtectedData.Protect(data, null, DataProtectionScope.LocalMachine);

            //return as base64 string
            return Convert.ToBase64String(encrypted);
        }

        public static string Decrypt(string cipher)
        {
            if (cipher == null)
                throw new ArgumentNullException("cipher");

            //parse base64 string
            byte[] data = Convert.FromBase64String(cipher);

            //decrypt data
            byte[] decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.LocalMachine);
            return Encoding.Unicode.GetString(decrypted);
        }
    }
}
