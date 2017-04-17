using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Windows.Forms;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace kampfpanzerin {
    public static class Utils {
        public static bool CopyFolderContents(string SourcePath, string DestinationPath) {
            SourcePath = SourcePath.EndsWith(@"\") ? SourcePath : SourcePath + @"\";
            DestinationPath = DestinationPath.EndsWith(@"\") ? DestinationPath : DestinationPath + @"\";

            try {
                if (Directory.Exists(SourcePath)) {
                    if (Directory.Exists(DestinationPath) == false) {
                        Directory.CreateDirectory(DestinationPath);
                    }

                    foreach (string files in Directory.GetFiles(SourcePath)) {
                        FileInfo fileInfo = new FileInfo(files);
                        fileInfo.CopyTo(string.Format(@"{0}\{1}", DestinationPath, fileInfo.Name), true);
                    }

                    foreach (string drs in Directory.GetDirectories(SourcePath)) {
                        DirectoryInfo directoryInfo = new DirectoryInfo(drs);
                        if (CopyFolderContents(drs, DestinationPath + directoryInfo.Name) == false) {
                            return false;
                        }
                    }
                }
                return true;
            } catch (Exception) {
                return false;
            }
        }

        public static void SaveJpeg(Bitmap bitmap, string savePath, int qualityPercent)
        {
            ImageCodecInfo[] codecs = ImageCodecInfo.GetImageDecoders();
            ImageCodecInfo jpgEncoder = codecs.Single(codec => codec.FormatID == ImageFormat.Jpeg.Guid);
            System.Drawing.Imaging.Encoder myEncoder = System.Drawing.Imaging.Encoder.Quality;
            EncoderParameters myEncoderParameters = new EncoderParameters(1);
            EncoderParameter myEncoderParameter = new EncoderParameter(myEncoder, qualityPercent);
            myEncoderParameters.Param[0] = myEncoderParameter;
            bitmap.Save(savePath, jpgEncoder, myEncoderParameters);
        }

        public static void LaunchAndLog(string filename, string args) {
            Process p = new Process();
            p.StartInfo.FileName = filename;
            p.StartInfo.Arguments = args;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.CreateNoWindow = true;
            p.OutputDataReceived += Kampfpanzerin.ProcessStreamHandler;
            p.Start();
            //activeExternalProcess = p;
            //stopButton.Visible = true;
            p.BeginOutputReadLine();

            while (p != null && !p.HasExited)
                Application.DoEvents();

            //activeExternalProcess = null;
            //stopButton.Visible = false;
        }

        public static float Clamp(float f) {
            if (f < 0)
                f = 0;
            if (f > 1.0f)
                f = 1.0f;
            return f;
        }

        public static Rectangle Rect(RectangleF rf) {
            Rectangle r = new Rectangle();
            r.X = (int)rf.X;
            r.Y = (int)rf.Y;
            r.Width = (int)rf.Width;
            r.Height = (int)rf.Height;
            return r;
        }

        public static RectangleF Rect(Rectangle r) {
            RectangleF rf = new RectangleF();
            rf.X = (float)r.X;
            rf.Y = (float)r.Y;
            rf.Width = (float)r.Width;
            rf.Height = (float)r.Height;
            return rf;
        }

        public static Point Point(PointF pf) {
            return new Point((int)pf.X, (int)pf.Y);
        }

        public static PointF Center(RectangleF r) {
            PointF center = r.Location;
            center.X += r.Width / 2;
            center.Y += r.Height / 2;
            return center;
        }
    }
}
