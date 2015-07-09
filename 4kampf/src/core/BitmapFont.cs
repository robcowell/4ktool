using System;
using Tao.OpenGl;
using Tao.Platform.Windows;

namespace kampfpanzerin
{
	public class BitmapFont
	{
		private static int fontbase;
        
		public BitmapFont(string fontname, IntPtr hDC, int fontSize) {
			IntPtr font, oldfont;
			fontbase = Gl.glGenLists(96);
			font = Gdi.CreateFont(-fontSize, 6, 0, 0, Gdi.FW_BOLD, false, false, false, Gdi.ANSI_CHARSET, Gdi.OUT_TT_PRECIS, Gdi.CLIP_DEFAULT_PRECIS, Gdi.ANTIALIASED_QUALITY, Gdi.FF_DONTCARE | Gdi.DEFAULT_PITCH, fontname);
			oldfont = Gdi.SelectObject(hDC, font);
			Wgl.wglUseFontBitmapsA(hDC, 32, 96, fontbase);
			Gdi.SelectObject(hDC, oldfont);
			Gdi.DeleteObject(font);
		}

		public void Print(string text){
            if (text == null || text.Length == 0) 
                return;

			Gl.glRasterPos2f(0, 0);
			Gl.glPushAttrib(Gl.GL_LIST_BIT);
			Gl.glListBase(fontbase - 32);
			byte[] textbytes = new byte[text.Length];  // Chuffing conversion required
			for (int i=0; i<text.Length; i++) 
                textbytes[i] = (byte)text[i];
			Gl.glCallLists(text.Length, Gl.GL_UNSIGNED_BYTE, textbytes);
			Gl.glPopAttrib();
		}
	}
}
