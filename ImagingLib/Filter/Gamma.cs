using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImagingLib
{
    public static partial class ImageOperator
    {
        /// <summary>
        /// ガンマ補正
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="gamma_c"></param>
        /// <param name="gamma_g"></param>
        public unsafe static void GammaCorrection(Bitmap src, Bitmap dst, double gamma_c, double gamma_g)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var pSrc = (byte*)lbSrc.Scan0;
                var pDst = (byte*)lbDst.Scan0;

                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;
                
                Parallel.For(0, height, y =>
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;
                    byte* pLineDst = pDst + y * lbDst.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            double val = pLineSrc[ch] / 255.0;
                            pLineDst[ch] = (byte)(Math.Pow(val / gamma_c, 1 / gamma_g) * 255.0);
                        }
                        // 次の画素へ
                        pLineSrc += channel;
                        pLineDst += channel;
                    }
                }
                );
            }
        }
    }
}
