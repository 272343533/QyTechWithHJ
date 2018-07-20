using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
namespace SunMvcExpress.Core.FileIO
{
    public class FileOperate
    {
        public static  void CombineToFile(String infileName, String outfileName)
        {
            int b;
            int n = infileName.Length;
            FileStream fileIn = new FileStream(infileName, FileMode.Open);
 
            using (FileStream fileOut = new FileStream(outfileName, FileMode.Create))
            {
                try
                {
                    while ((b = fileIn.ReadByte()) != -1)
                        fileOut.WriteByte((byte)b);
                }
                catch (System.Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
                finally
                {
                    fileIn.Close();
                }

            }
        }
    }
}
