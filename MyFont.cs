using System;
using System.Drawing.Text;
using System.Drawing;

namespace BattleCity
{
    public static class MyFont
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        private static extern IntPtr AddFontMemResourceEx(IntPtr pbFont, uint cbFont,
           IntPtr pdv, [System.Runtime.InteropServices.In] ref uint pcFonts);

        private static PrivateFontCollection fonts = new PrivateFontCollection();

        static MyFont()
        {
            byte[] fontData = Properties.Resources.BattleCityFont;
            IntPtr fontPtr = System.Runtime.InteropServices.Marshal.AllocCoTaskMem(fontData.Length);
            System.Runtime.InteropServices.Marshal.Copy(fontData, 0, fontPtr, fontData.Length);
            uint dummy = 0;
            fonts.AddMemoryFont(fontPtr, Properties.Resources.BattleCityFont.Length);
            AddFontMemResourceEx(fontPtr, (uint) Properties.Resources.BattleCityFont.Length, IntPtr.Zero, ref dummy);
            System.Runtime.InteropServices.Marshal.FreeCoTaskMem(fontPtr);
        }

        public static Font GetFont(int size)
        {
            return new Font(fonts.Families[0], size);
        }
    }
}