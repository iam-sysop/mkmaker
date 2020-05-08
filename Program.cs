using System;
using System.Text;
using System.Security.Cryptography;

namespace mkmaker
{
    class Program
    {

        static void Main(string[] args)
        {
            con("MachineKey Generator v0.1 for ASP.NET / xsiteman WebForms Application");
            con("");

            string _decKeyMode = "AES";
            string _hashMode = "HMACSHA512";

            if (args.Length != 0) {

                if ((args.Length != 2) || (args[0] == "-h") || (args[0] == "--help"))
                {
                    error(-1);
                }

                if ((args[0] != "AES") && (args[0] != "DES") && (args[0] != "3DES"))
                {
                    error(1);

                }
                if ((args[1] != "MD5") && (args[1] != "SHA1") && (args[1] != "HMACSHA256") && (args[1] != "HMACSHA384") && (args[1] != "HMACSHA512"))
                {
                    error(2);
                }

                _decKeyMode = args[0];
                _hashMode = args[1];

                con("FOUND OPTIONS: " + args[0] + ", " + args[1]);
            } 
            else
            {
                con("USING DEFAULTS: AES + HMACSHA512");
            }

            con("");

            string _decKey;
            string _hashKey;

            switch (_decKeyMode) {
                case "3DES":
                    TripleDESCryptoServiceProvider _3DES = new TripleDESCryptoServiceProvider();
                    _3DES.GenerateKey();
                    _decKey = BinToHexStr(_3DES.Key);
                    _3DES.Dispose();
                    break;
                case "DES":
                    DESCryptoServiceProvider _DES = new DESCryptoServiceProvider();
                    _DES.GenerateKey();
                    _decKey = BinToHexStr(_DES.Key);
                    _DES.Dispose();
                    break;
                default:
                    AesCryptoServiceProvider _AES = new AesCryptoServiceProvider();
                    _AES.GenerateKey();
                    _decKey = BinToHexStr(_AES.Key);
                    _AES.Dispose();
                    break;
            }

            switch (_hashMode)
            {
                case "MD5":
                    HMACMD5 _MD5 = new HMACMD5();
                    _hashKey = BinToHexStr(_MD5.Key);
                    _MD5.Dispose();
                    break;
                case "SHA1":
                    HMACSHA1 _SHA1 = new HMACSHA1();
                    _hashKey = BinToHexStr(_SHA1.Key);
                    _SHA1.Dispose();
                    break;
                case "SHA256":
                    HMACSHA256 _SHA256 = new HMACSHA256();
                    _hashKey = BinToHexStr(_SHA256.Key);
                    _SHA256.Dispose();
                    break;
                case "SHA384":
                    HMACSHA384 _SHA384 = new HMACSHA384();
                    _hashKey = BinToHexStr(_SHA384.Key);
                    _SHA384.Dispose();
                    break;
                default:
                    HMACSHA512 _SHA512 = new HMACSHA512();
                    _hashKey = BinToHexStr(_SHA512.Key);
                    _SHA512.Dispose();
                    break;
            }


            string _mkstring = string.Concat("<machineKey decryption=\"", _decKeyMode, "\" decryptionKey=\"", _decKey, "\" validation=\"", _hashMode, "\" validationKey=\"", _hashKey, "\" />");
            
            con(_mkstring);


        }

        public static void con(string strValue)
        {
            Console.Out.WriteLine(strValue);
        }

        public static void error(int ErrorCode)
        {
            switch (ErrorCode)
            {
                case -1:
                    displayHelp();
                    break;

                case 0:
                    con("ERROR: Invalid Parameters Specified");
                    displayHelp();
                    break;

                case 1:
                    con("PARAMETER 1 ERROR");
                    displayHelp();
                    break;

                case 2:
                    con("PARAMETER 2 ERROR");
                    displayHelp();
                    break;

                default:
                    con("SUCCESS");
                    break;
            }
            Environment.Exit(0);
        }

        public static void displayHelp()
        {
            con("ERROR: Please Supply Args:");
            con("Param 1: AES (MS Default), DES, or 3DES");
            con("");
            con("Param 2: MD5 (legacy), SHA1 (legacy), HMACSHA256 (MS Default), HMACSHA384, HMACSHA512");
            con("");
        }


        static string BinToHexStr(byte[] bytes)
        {
            var hexString = new StringBuilder(64);
            foreach (byte t in bytes)
            {
                hexString.Append(String.Format("{0:X2}", t));
            }
            return hexString.ToString();
        }

    }
}

