using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace HSRP
{
    public class Common
    {
        public enum IsSuccess { Sucess, NoSuccess };
        public enum InDatabaseInsertData { Zero, One, Two };
        public static bool IsPunchMasterCallClosedButton = false;
        public const string Success = "Success";
        public const string SomeError = "SomeError";
        public const int Quentity = 60;
        public const int MatchThreshold = 14000;
        public const int Timer = 0;
        public enum LoginStatus { Correct, NameANDPasswordValidDateNOValid, EveryThingWrong }
        public enum UserRoles { Admin, Users, SuperAdmin,SuperUsers };
        public const int DataInsertSuccessfully = 0, IsValidateFinger = 0, Zero = 0, IsSameFinger = 0;
        /// <summary>
        /// Developer Joginder Singh
        /// Dated : 17-12-2015
        /// This is a common function help to slove another problem.
        /// </summary>
        /// <param name="encodedServername"></param>
        /// <returns></returns>
        /// 



        public static string GetMACAddress()
        {
            NetworkInterface[] nics = NetworkInterface.GetAllNetworkInterfaces();
            String sMacAddress = string.Empty;
            foreach (NetworkInterface adapter in nics)
            {
                if (sMacAddress == String.Empty)// only return MAC Address from first card  
                {
                    IPInterfaceProperties properties = adapter.GetIPProperties();
                    sMacAddress = adapter.GetPhysicalAddress().ToString();
                }
            } return sMacAddress;
        }


        public static void CloseMantraConnection()
        {
            //try
            //{
            //   // MantraStopWorking ObjImageAndPunchMaster = new MantraStopWorking();
            //    //ObjImageAndPunchMaster.StopCapture();
            //}
            //catch(Exception ex)
            //{
            //    throw ex;
            //}
            //finally
            //{
            //    GC.Collect();
            //}
        }
        /// <summary>
        /// Create a bitmap from raw data in row/column format.
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="width"></param>
        /// <param name="height"></param>
        /// <returns></returns>
        public static Bitmap CreateBitmap(byte[] bytes, int width, int height)
        {
            byte[] rgbBytes = new byte[bytes.Length * 3];
            for (int i = 0; i <= bytes.Length - 1; i++)
            {
                rgbBytes[(i * 3)] = bytes[i];
                rgbBytes[(i * 3) + 1] = bytes[i];
                rgbBytes[(i * 3) + 2] = bytes[i];
            }
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            BitmapData data = bmp.LockBits(new Rectangle(0, 0, bmp.Width, bmp.Height), ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            for (int i = 0; i <= bmp.Height - 1; i++)
            {
                IntPtr p = new IntPtr(data.Scan0.ToInt64() + data.Stride * i);
                System.Runtime.InteropServices.Marshal.Copy(rgbBytes, i * bmp.Width * 3, p, bmp.Width * 3);
            }
            bmp.UnlockBits(data);
            return bmp;
        }
        public static string Decode(string StringValue)
        {
            try
            {
                return Encoding.UTF8.GetString(Convert.FromBase64String(StringValue));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string Encode(string StringValue)
        {
            try
            {
                return Convert.ToBase64String(Encoding.UTF8.GetBytes(StringValue));
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static byte[] ConvertStringToByteArray(object Name)
        {
            try
            {
                byte[] bytes = new byte[Name.ToString().Length * sizeof(char)];
                System.Buffer.BlockCopy(Name.ToString().ToCharArray(), 0, bytes, 0, bytes.Length);
                return bytes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static byte[] FromBase64Bytes(byte[] base64Bytes)
        {
            try
            {
                string base64String = Encoding.UTF8.GetString(base64Bytes, 0, base64Bytes.Length);
                return Convert.FromBase64String(base64String);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string FixBase64ForImage(string Image)
        {
            try
            {
                System.Text.StringBuilder sbText = new System.Text.StringBuilder(Image, Image.Length);
                sbText.Replace("\r\n", String.Empty); sbText.Replace(" ", String.Empty);
                return sbText.ToString();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string ConvertByteArrayToString(byte[] bytes)
        {
            try
            {
                char[] chars = new char[bytes.Length / sizeof(char)];
                System.Buffer.BlockCopy(bytes, 0, chars, 0, bytes.Length);
                return new string(chars);
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Return the Month Name Like 01-Dec-2015
        /// </summary>
        /// <param name="Date"></param>
        /// <returns></returns>
        public static string ConvertMonth(string Date)
        {
            try
            {
                if (ValidateStringValue(Date))
                {
                    Date = Date.Replace('/', '-');
                    string[] Dateformat = Date.Split('-');
                    Date = Dateformat[0] + "-" + MonthName(Convert.ToInt32(Dateformat[1])) + "-" + Dateformat[2];
                    return Date;
                }
                else
                    return "No Values";
            }
            catch (Exception)
            {
                throw;
            }
        }
        private static string MonthName(int m)
        {
            try
            {
                string res;
                switch (m)
                {
                    case 1:
                        res = "Jan";
                        break;
                    case 2:
                        res = "Feb";
                        break;
                    case 3:
                        res = "Mar";
                        break;
                    case 4:
                        res = "Apr";
                        break;
                    case 5:
                        res = "May";
                        break;
                    case 6:
                        res = "Jun";
                        break;
                    case 7:
                        res = "Jul";
                        break;
                    case 8:
                        res = "Aug";
                        break;
                    case 9:
                        res = "Sep";
                        break;
                    case 10:
                        res = "Oct";
                        break;
                    case 11:
                        res = "Nov";
                        break;
                    case 12:
                        res = "Dec";
                        break;
                    default:
                        res = "Null";
                        break;
                }
                return res;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static bool ValidateStringValue(string StringValue)
        {
            try
            {
                if (StringValue != string.Empty && StringValue != "" && StringValue != null)
                    return true;
                else
                    return false;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /// <summary>
        /// Add Row Data in the tables 
        /// </summary>
        /// <param name="TempraryTable"></param>
        /// <param name="NodeName"></param>
        /// <param name="conditionValue"></param>
        /// <param name="doc"></param>
        /// <returns></returns>
        public static DataTable AddRowData(DataTable TempraryTable, string NodeName, string conditionValue, XmlDocument doc)
        {
            try
            {
                XmlNodeList ConList = null;
                for (int i = 0; i < doc.GetElementsByTagName("application").Count; i++)
                {
                    if (conditionValue != null)
                        ConList = doc.GetElementsByTagName("application").Item(i).SelectNodes(NodeName).Item(0).SelectNodes("covdet");
                    else
                        ConList = doc.GetElementsByTagName("application").Item(i).SelectNodes(NodeName);
                    foreach (XmlNode xn in ConList)
                    {
                        DataRow Row = TempraryTable.NewRow();
                        for (int j = 0; j < TempraryTable.Columns.Count; j++)
                        {
                            Row[TempraryTable.Columns[j].ToString()] = xn[TempraryTable.Columns[j].ToString()].InnerText;
                        }
                        TempraryTable.Rows.Add(Row);
                    }
                }
                return TempraryTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static DataTable InsertColumnsInTables(DataTable dataTable, string[] stringArray)
        {
            try
            {
                foreach (var ColumnName in stringArray)
                {
                    if (!dataTable.Columns.Contains(ColumnName))
                    {
                        DataColumn col = new DataColumn(ColumnName);
                        dataTable.Columns.Add(col);
                    }
                }
                return dataTable;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string ConvertToUpperCase(string text)
        {
            try
            {
                return text.ToUpper();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string fieldValidation(String name, String userName, String password, String confirmPassword, int userType, String dateValidFrom, String dateValidTill)
        {
            try
            {
                Regex rgx = null;
                string errors = String.Empty;
                //Validation for Name
                rgx = new Regex(@"^[a-zA-Z]{5,30}$");
                if (string.IsNullOrEmpty(name))
                {
                    errors += "Name cannot be empty\n";
                }
                else if (!rgx.IsMatch(name))
                {
                    errors += "Name is either invalid or too short or too long\n";
                }
                //Validation for UserName 
                rgx = new Regex(@"^\w{2,20}$");
                if (string.IsNullOrEmpty(userName))
                {
                    errors += "User Name can not be empty\n";
                }
                else if (!rgx.IsMatch(userName))
                {
                    errors += "User Name is either invalid or too short or too long\n";
                }
                //Validation for Password and Confirm Password 
                rgx = new Regex(@"^[a-zA-Z0-9.!#$%&'*+-=?^_`{|}~\/]{2,20}$");
                if (string.IsNullOrEmpty(password))
                {
                    errors += "Password can not be empty\n";
                }
                else if (!rgx.IsMatch(password))
                {
                    errors += "Password is either invalid or too short or too long\n";
                }
                if (string.IsNullOrEmpty(confirmPassword))
                {
                    errors += "Confirm Password can not be empty\n";
                }
                else if (confirmPassword != password)
                {
                    errors += " Confirm Password did not match Password\n";
                }
                //Validation for UserType
                if (userType == -1)
                {
                    errors += "Select User Type\n";
                }
                //Validation for Date Valid From
                if (string.IsNullOrEmpty(dateValidFrom))
                {
                    errors += "Date Valid From can not be empty\n";
                }
                //Validation Date Valid To
                if (string.IsNullOrEmpty(dateValidTill))
                {
                    errors += "Date Valid Till can not be empty\n";
                }
                return errors;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string monthOrDayCheck(string monthOrDay)
        {
            try
            {
                if (monthOrDay.Length == 1)
                    monthOrDay = "0" + monthOrDay;
                return monthOrDay;
            }
            catch (Exception)
            {
                throw;
            }
        }
        /*******************************************************
      Warning, Error and Success Message
      * 
      * 
      * 
      * Develop By Joginder Singh
      * 02-09-2016
      *******************************************************/
        public static void MessageBoxError(string message)
        {
            try
            {
                MessageBox.Show(message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void MessageBoxWarning(string message)
        {
            try
            {
                MessageBox.Show(message, "Warning", MessageBoxButtons.RetryCancel, MessageBoxIcon.Warning);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void MessageBoxSuccess(string message)
        {
            try
            {
                MessageBox.Show(message, "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static void MessageBoxNone(string message)
        {
            MessageBox.Show(message, "", MessageBoxButtons.OK, MessageBoxIcon.None);
        }
        public static void MessageBoxInformation(string message)
        {
            MessageBox.Show(message, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        // Encrypt a byte array into a byte array using a key and an IV 
        //
        public static byte[] Encrypt(byte[] clearData, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a MemoryStream to accept the encrypted bytes 
                MemoryStream ms = new MemoryStream();
                // Create a symmetric algorithm. 
                // We are going to use Rijndael because it is strong and
                // available on all platforms. 
                // You can use other algorithms, to do so substitute the
                // next line with something like 
                //      TripleDES alg = TripleDES.Create(); 
                Rijndael alg = Rijndael.Create();
                // Now set the key and the IV. 
                // We need the IV (Initialization Vector) because
                // the algorithm is operating in its default 
                // mode called CBC (Cipher Block Chaining).
                // The IV is XORed with the first block (8 byte) 
                // of the data before it is encrypted, and then each
                // encrypted block is XORed with the 
                // following block of plaintext.
                // This is done to make encryption more secure. 
                // There is also a mode called ECB which does not need an IV,
                // but it is much less secure. 
                alg.Key = Key;
                alg.IV = IV;
                // Create a CryptoStream through which we are going to be
                // pumping our data. 
                // CryptoStreamMode.Write means that we are going to be
                // writing data to the stream and the output will be written
                // in the MemoryStream we have provided. 
                CryptoStream cs = new CryptoStream(ms,
                   alg.CreateEncryptor(), CryptoStreamMode.Write);
                // Write the data and make it do the encryption 
                cs.Write(clearData, 0, clearData.Length);
                // Close the crypto stream (or do FlushFinalBlock). 
                // This will tell it that we have done our encryption and
                // there is no more data coming in, 
                // and it is now a good time to apply the padding and
                // finalize the encryption process. 
                cs.Close();
                // Now get the encrypted data from the MemoryStream.
                // Some people make a mistake of using GetBuffer() here,
                // which is not the right way. 
                byte[] encryptedData = ms.ToArray();
                return encryptedData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Encrypt a string into a string using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 
        public static string Encrypt(string clearText, string Password)
        {
            try
            {
                // First we need to turn the input string into a byte array. 
                byte[] clearBytes =
                  System.Text.Encoding.Unicode.GetBytes(clearText);
                // Then, we need to turn the password into Key and IV 
                // We are using salt to make it harder to guess our key
                // using a dictionary attack - 
                // trying to guess a password by enumerating all possible words. 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                // Now get the key/IV and do the encryption using the
                // function that accepts byte arrays. 
                // Using PasswordDeriveBytes object we are first getting
                // 32 bytes for the Key 
                // (the default Rijndael key length is 256bit = 32bytes)
                // and then 16 bytes for the IV. 
                // IV should always be the block size, which is by default
                // 16 bytes (128 bit) for Rijndael. 
                // If you are using DES/TripleDES/RC2 the block size is
                // 8 bytes and so should be the IV size. 
                // You can also read KeySize/BlockSize properties off
                // the algorithm to find out the sizes. 
                byte[] encryptedData = Encrypt(clearBytes,
                         pdb.GetBytes(32), pdb.GetBytes(16));
                // Now we need to turn the resulting byte array into a string. 
                // A common mistake would be to use an Encoding class for that.
                //It does not work because not all byte values can be
                // represented by characters. 
                // We are going to be using Base64 encoding that is designed
                //exactly for what we are trying to do. 
                return Convert.ToBase64String(encryptedData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Encrypt bytes into bytes using a password 
        //    Uses Encrypt(byte[], byte[], byte[]) 
        public static byte[] Encrypt(byte[] clearData, string Password)
        {
            try
            {
                // We need to turn the password into Key and IV. 
                // We are using salt to make it harder to guess our key
                // using a dictionary attack - 
                // trying to guess a password by enumerating all possible words. 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                // Now get the key/IV and do the encryption using the function
                // that accepts byte arrays. 
                // Using PasswordDeriveBytes object we are first getting
                // 32 bytes for the Key 
                // (the default Rijndael key length is 256bit = 32bytes)
                // and then 16 bytes for the IV. 
                // IV should always be the block size, which is by default
                // 16 bytes (128 bit) for Rijndael. 
                // If you are using DES/TripleDES/RC2 the block size is 8
                // bytes and so should be the IV size. 
                // You can also read KeySize/BlockSize properties off the
                // algorithm to find out the sizes. 
                return Encrypt(clearData, pdb.GetBytes(32), pdb.GetBytes(16));
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Encrypt a file into another file using a password 
        //
        public static void Encrypt(string fileIn, string fileOut, string Password)
        {
            try
            {
                // First we are going to open the file streams 
                FileStream fsIn = new FileStream(fileIn,
                    FileMode.Open, FileAccess.Read);
                FileStream fsOut = new FileStream(fileOut,
                    FileMode.OpenOrCreate, FileAccess.Write);
                // Then we are going to derive a Key and an IV from the
                // Password and create an algorithm 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                Rijndael alg = Rijndael.Create();
                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);
                // Now create a crypto stream through which we are going
                // to be pumping data. 
                // Our fileOut is going to be receiving the encrypted bytes. 
                CryptoStream cs = new CryptoStream(fsOut,
                    alg.CreateEncryptor(), CryptoStreamMode.Write);
                // Now will will initialize a buffer and will be processing
                // the input file in chunks. 
                // This is done to avoid reading the whole file (which can
                // be huge) into memory. 
                int bufferLen = 4096;
                byte[] buffer = new byte[bufferLen];
                int bytesRead;
                do
                {
                    // read a chunk of data from the input file 
                    bytesRead = fsIn.Read(buffer, 0, bufferLen);
                    // encrypt it 
                    cs.Write(buffer, 0, bytesRead);
                } while (bytesRead != 0);
                // close everything 
                // this will also close the unrelying fsOut stream
                cs.Close();
                fsIn.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Decrypt a byte array into a byte array using a key and an IV 
        //
        public static byte[] Decrypt(byte[] cipherData, byte[] Key, byte[] IV)
        {
            try
            {
                // Create a MemoryStream that is going to accept the
                // decrypted bytes 
                MemoryStream ms = new MemoryStream();
                // Create a symmetric algorithm. 
                // We are going to use Rijndael because it is strong and
                // available on all platforms. 
                // You can use other algorithms, to do so substitute the next
                // line with something like 
                //     TripleDES alg = TripleDES.Create(); 
                Rijndael alg = Rijndael.Create();
                // Now set the key and the IV. 
                // We need the IV (Initialization Vector) because the algorithm
                // is operating in its default 
                // mode called CBC (Cipher Block Chaining). The IV is XORed with
                // the first block (8 byte) 
                // of the data after it is decrypted, and then each decrypted
                // block is XORed with the previous 
                // cipher block. This is done to make encryption more secure. 
                // There is also a mode called ECB which does not need an IV,
                // but it is much less secure. 
                alg.Key = Key;
                alg.IV = IV;
                // Create a CryptoStream through which we are going to be
                // pumping our data. 
                // CryptoStreamMode.Write means that we are going to be
                // writing data to the stream 
                // and the output will be written in the MemoryStream
                // we have provided. 
                CryptoStream cs = new CryptoStream(ms,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);
                // Write the data and make it do the decryption 
                cs.Write(cipherData, 0, cipherData.Length);
                // Close the crypto stream (or do FlushFinalBlock). 
                // This will tell it that we have done our decryption
                // and there is no more data coming in, 
                // and it is now a good time to remove the padding
                // and finalize the decryption process. 
                cs.Close();
                // Now get the decrypted data from the MemoryStream. 
                // Some people make a mistake of using GetBuffer() here,
                // which is not the right way. 
                byte[] decryptedData = ms.ToArray();
                return decryptedData;
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Decrypt a string into a string using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 
        public static string Decrypt(string cipherText, string Password)
        {
            try
            {
                // First we need to turn the input string into a byte array. 
                // We presume that Base64 encoding was used 
                byte[] cipherBytes = Convert.FromBase64String(cipherText);
                // Then, we need to turn the password into Key and IV 
                // We are using salt to make it harder to guess our key
                // using a dictionary attack - 
                // trying to guess a password by enumerating all possible words. 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 
            0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                // Now get the key/IV and do the decryption using
                // the function that accepts byte arrays. 
                // Using PasswordDeriveBytes object we are first
                // getting 32 bytes for the Key 
                // (the default Rijndael key length is 256bit = 32bytes)
                // and then 16 bytes for the IV. 
                // IV should always be the block size, which is by
                // default 16 bytes (128 bit) for Rijndael. 
                // If you are using DES/TripleDES/RC2 the block size is
                // 8 bytes and so should be the IV size. 
                // You can also read KeySize/BlockSize properties off
                // the algorithm to find out the sizes. 
                byte[] decryptedData = Decrypt(cipherBytes,
                    pdb.GetBytes(32), pdb.GetBytes(16));
                // Now we need to turn the resulting byte array into a string. 
                // A common mistake would be to use an Encoding class for that.
                // It does not work 
                // because not all byte values can be represented by characters. 
                // We are going to be using Base64 encoding that is 
                // designed exactly for what we are trying to do. 
                return System.Text.Encoding.Unicode.GetString(decryptedData);
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Decrypt bytes into bytes using a password 
        //    Uses Decrypt(byte[], byte[], byte[]) 
        public static byte[] Decrypt(byte[] cipherData, string Password)
        {
            try
            {
                // We need to turn the password into Key and IV. 
                // We are using salt to make it harder to guess our key
                // using a dictionary attack - 
                // trying to guess a password by enumerating all possible words. 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                // Now get the key/IV and do the Decryption using the 
                //function that accepts byte arrays. 
                // Using PasswordDeriveBytes object we are first getting
                // 32 bytes for the Key 
                // (the default Rijndael key length is 256bit = 32bytes)
                // and then 16 bytes for the IV. 
                // IV should always be the block size, which is by default
                // 16 bytes (128 bit) for Rijndael. 
                // If you are using DES/TripleDES/RC2 the block size is
                // 8 bytes and so should be the IV size. 
                // You can also read KeySize/BlockSize properties off the
                // algorithm to find out the sizes. 
                return Decrypt(cipherData, pdb.GetBytes(32), pdb.GetBytes(16));
            }
            catch (Exception)
            {
                throw;
            }
        }
        // Decrypt a file into another file using a password 
        //
        public static void Decrypt(string fileIn, string fileOut, string Password)
        {
            try
            {
                // First we are going to open the file streams 
                FileStream fsIn = new FileStream(fileIn,
                            FileMode.Open, FileAccess.Read);
                FileStream fsOut = new FileStream(fileOut,
                            FileMode.OpenOrCreate, FileAccess.Write);
                // Then we are going to derive a Key and an IV from
                // the Password and create an algorithm 
                PasswordDeriveBytes pdb = new PasswordDeriveBytes(Password,
                    new byte[] {0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 
            0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76});
                Rijndael alg = Rijndael.Create();
                alg.Key = pdb.GetBytes(32);
                alg.IV = pdb.GetBytes(16);
                // Now create a crypto stream through which we are going
                // to be pumping data. 
                // Our fileOut is going to be receiving the Decrypted bytes. 
                CryptoStream cs = new CryptoStream(fsOut,
                    alg.CreateDecryptor(), CryptoStreamMode.Write);
                // Now will will initialize a buffer and will be 
                // processing the input file in chunks. 
                // This is done to avoid reading the whole file (which can be
                // huge) into memory. 
                int bufferLen = 4096;
                byte[] buffer = new byte[bufferLen];
                int bytesRead;
                do
                {
                    // read a chunk of data from the input file 
                    bytesRead = fsIn.Read(buffer, 0, bufferLen);
                    // Decrypt it 
                    cs.Write(buffer, 0, bytesRead);
                } while (bytesRead != 0);
                // close everything 
                cs.Close(); // this will also close the unrelying fsOut stream 
                fsIn.Close();
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static byte[] ConvertBMPImageToByteArray(Image image)
        {
            try
            {
                byte[] imageBytes;
                using (MemoryStream ms = new MemoryStream())
                {
                    // Convert Image to byte[]
                    image.Save(ms, ImageFormat.Bmp);
                    imageBytes = ms.ToArray();
                }
                return imageBytes;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string MakeBatchNo(string BatchNo)
        {
            try
            {
                int length = 0;
                int TempValue = 0;
                int date = 0;
                if (Common.ValidateStringValue(BatchNo))
                {
                    //Get batchNo 1 when no data Available in table like empty table
                    if (BatchNo == "1")
                    {
                        length = Convert.ToInt32(BatchNo.Length);
                    }
                    else
                    {
                        // get the date of BatchNo.
                        date = Convert.ToInt32(BatchNo.Substring(3, 8));
                        //get today date/
                        int TempDate = Convert.ToInt32(DateTime.Now.Date.ToString("yyyyMMdd"));
                        if (date == TempDate)
                        {
                            //get the last four digit no..
                            BatchNo = BatchNo.Substring(BatchNo.Length - 4);
                            length = Convert.ToInt32(BatchNo).ToString().Length;
                            TempValue = Convert.ToInt32(BatchNo);
                            //increment one number in current value
                            TempValue += 1;
                            BatchNo = Convert.ToString(TempValue);
                        }
                        else
                        {
                            BatchNo = "1";
                            length = Convert.ToInt32(BatchNo.Length);
                        }
                    }
                    // Append the zero based on values
                    switch (length)
                    {
                        case 1:
                            BatchNo = "000" + BatchNo;
                            break;
                        case 2:
                            BatchNo = "00" + BatchNo;
                            break;
                        case 3:
                            BatchNo = "0" + BatchNo;
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
            return BatchNo;
        }
        /// Make A challan Number...
        /// </summary>
        /// <param name="NoofChallan"></param>
        /// <returns></returns>
        public static string MakeChallanNo(string NoofChallan)
        {
            try
            {
                NoofChallan = Convert.ToInt32(Convert.ToInt32(NoofChallan) + 1).ToString();
                int length = NoofChallan.Length;
                switch (length)
                {
                    case 1:
                        NoofChallan = "000" + NoofChallan;
                        break;
                    case 2:
                        NoofChallan = "00" + NoofChallan;
                        break;
                    case 3:
                        NoofChallan = "0" + NoofChallan;
                        break;
                    default:
                        break;
                }
                return NoofChallan;
            }
            catch (Exception)
            {
                throw;
            }
        }
        public static string ValidateDate(string date)
        {
            if (ValidateStringValue(date))
            {
                return date;
            }
            else
                return "00000000";
        }
        //RTO Credentials Page
        public static string AgentId = "Please Enter Agent Id";
        public static string RtoCode = "Please Enter RTO Code";
        public static string StateCode = "Please Enter State Code";
        public static string AgentPassword = "Please Enter Agent Password";
        public static string AgentConfirmPassword = "Please Enter Confirm Password";
        public static string PrinterSelect = "Select a Printer and then Print";
        public static string SomethingWrong = "Something wrong when trying to update in RC_Cash table after printe card!!!";
        public static string SomthingWrongChipWritting = "Something Wrong when trying to update in RC_Cash table... after chip writting...";
        public const string ChipSuccess = "0:-Successfull";
        public const string CardFeedError = "Card Feed Error";
        public static class myPrinters
        {
            [DllImport("winspool.drv", CharSet = CharSet.Auto, SetLastError = true)]
            public static extern bool SetDefaultPrinter(string Name);
        }
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);
            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }
            return destImage;
        }
        public static DataTable ReturnDataTableBasedOnStoreProcedure(string stroeProcedureName)
        {
            return DMLSql.MYInstance.GetRecords(stroeProcedureName, CommandType.StoredProcedure);
        }
        public static string FilePath(SaveFileDialog saveDialog)
        {
            string FileName = string.Empty;
            saveDialog.InitialDirectory = @"D:\";
            saveDialog.Title = "Save text Files";
            //saveFileDialog1.CheckFileExists = true;
            //saveFileDialog1.CheckPathExists = true;
            saveDialog.DefaultExt = "txt";
            saveDialog.Filter = "Excel files (*.xls)|*.xls";
            saveDialog.FilterIndex = 2;
            saveDialog.RestoreDirectory = true;
            string fileName = string.Empty;
            if (saveDialog.ShowDialog() == DialogResult.OK)
            {
                FileName = saveDialog.FileName;
            }
            else
            {
                FileName = "No Name fill";
            }
            return FileName;
        }
    }
}
