using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace viewPatientServer
{
    /// <summary>
    /// 公開函式:包括cooper 預設路徑
    /// </summary>
    public class PublicInfo // must be instanced in program beginning
    {
        //public static Boolean ClientSide;
        /// <summary>
        /// cooper 路徑
        /// </summary>
        public static string Cooper_Path = "";
        //public static entryCoop_cli Coop_cli_dbf = new entryCoop_cli();

        /*
        public static void GetCoop_cli()
        {
            //ClientSide = IsClientSide();
            //Coop_cli_dbf.QueryData();
        }
        */
        
        public static void CheckClientSide()
        {
            try
            {
                String strFile = Path.Combine(Directory.GetCurrentDirectory(), "CP_SERVER.txt");
                if (File.Exists(strFile))
                {
                    Cooper_Path = System.IO.File.ReadAllText(strFile);
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
           
            //String strFile = Path.Combine(Directory.GetCurrentDirectory(), "CLINIC.DBF");
            //if (File.Exists(@strFile))
            //{
            //    ClientSide = false; //單機或 server 端
            //    Cooper_Path = Path.Combine(Directory.GetCurrentDirectory());
            //}
            //else
            //{
            //    //strFile = Path.Combine(Environment.GetEnvironmentVariable("windir"), "COOP_CLI.DBF");

            //    String[] Coop_cli_ini = ReadIni();

            //    foreach (String Line in Coop_cli_ini)
            //    {
            //        if (Line.IndexOf("COOP_CLI PATH=EXE", StringComparison.CurrentCultureIgnoreCase) >= 0)
            //        {
            //            strFile = Path.Combine(Directory.GetCurrentDirectory(), "COOP_CLI.DBF");
                        
            //            Coop_cli_dbf.QueryData();
            //            Cooper_Path = Coop_cli_dbf.Coopdata;
            //            break;
            //        }
            //    }

            //    if (File.Exists(@strFile)) ClientSide = true;
            //    else ClientSide = false; // true:client; false:server
            //}
        }

        //public static String[] ReadIni()
        //{
        //    //Coop_cli_ini;
        //    String FileName_Coop_cli_ini = Path.Combine(Directory.GetCurrentDirectory(), "Coop_cli.ini");
        //    if (File.Exists(@FileName_Coop_cli_ini))
        //    {
        //        String[] Coop_cli_ini = File.ReadAllLines(@FileName_Coop_cli_ini);
        //        return Coop_cli_ini;
        //    }
        //    else
        //    {
        //        String[] NullString = {""};
        //        return NullString;
        //    }
        //}

        public static Boolean CopyFromServer(String NeedCopyFile)
        {
            String SourceFile = Path.Combine(PublicInfo.Cooper_Path, NeedCopyFile);
            String TargetFile = Path.Combine(Directory.GetCurrentDirectory(), NeedCopyFile);

            if (File.Exists(@SourceFile)) File.Copy(SourceFile, TargetFile);

            if (File.Exists(Path.Combine(Directory.GetCurrentDirectory(), NeedCopyFile))) return true; else return false;
        } // CopyFromServer

        public static String PassCode2(byte[] Pass2)
        {
            String Pass1 = "";
            int AscCode = 0, Seed1 = 0, Seed2 = 0, Pass2Length = Pass2.Length;

            for (int i = Pass2Length; i >= 1; i--)
            {
                AscCode = Pass2[i - 1];
                Seed1 = (100 + (Pass2Length - i + 1)) % 256;
                Seed2 = 256 - Seed1;
                if (AscCode >= Seed1)
                    Pass1 = Pass1 + Convert.ToChar(AscCode - Seed1);
                else
                    Pass1 = Pass1 + Convert.ToChar(AscCode + Seed2);
            }
            return Pass1.Trim();
        }

        public static String Padl(String strSource, int ResultLength, Char PadCharacter)
        {
            if (strSource.Length >= ResultLength) return strSource.Substring(0, ResultLength);

            String RetVal = strSource;
            for (int i = 1; i <= (ResultLength - strSource.Length); i++)
                RetVal = PadCharacter + RetVal;
            return RetVal;
        } // Padl

        
        public static String Decryptdata(byte[] Encrypt, int Seedpos)
        {
            //Char SeedChar = DecryptOneChar((Encrypt[Seedpos - 1]), (short)'C');
            Byte[] Destr = new byte[Encrypt.Length];
            Byte SeedChar = DecryptOneChar((Encrypt[Seedpos - 1]), (short)'C');
            short DecryptChar;

            for (int i = 1; i <= Seedpos - 1; i++)
            {
                DecryptChar = (short)Encrypt[i - 1];
                Destr[i - 1] = DecryptOneChar(DecryptChar, (short)(SeedChar));
            }

            //Destr = Destr + SeedChar.ToString();
            Destr[Seedpos - 1] = SeedChar;

            for (int i = Seedpos; i <= Encrypt.Length - 1; i++)
            {
                DecryptChar = (short)Encrypt[i];
                //Destr = Destr + DecryptOneChar(DecryptChar, (short)(SeedChar)).ToString();
                Destr[i] = DecryptOneChar(DecryptChar, (short)(SeedChar));
            }

            //return Destr;
            return Encoding.Default.GetString(Destr).Trim();
        } //Decryptdata


        private static Byte DecryptOneChar(short EncryptChar, short SeedChar)
        {
            if (EncryptChar == 32) return 32; //empty

            if (EncryptChar == SeedChar) EncryptChar = 32;

            //int Ascii_EncryptChar = Convert.ToInt32(EncryptChar);
            //int Ascii_SeedChar = Convert.ToInt32(SeedChar);

            if (EncryptChar < SeedChar) EncryptChar += (short)256 ;

            //EncryptChar = (short)(EncryptChar - SeedChar);

            //return (byte)EncryptChar;
            return (byte)(EncryptChar - SeedChar);
        } //DecryptOneChar

    } // class PublicInfo
} // NameSpace
