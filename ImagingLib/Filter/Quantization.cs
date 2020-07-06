using System;
using System.Drawing;
using System.Threading.Tasks;

namespace ImagingLib
{
    public static partial class ImageOperator
    {
        public unsafe static void Quantization(Bitmap src, Bitmap dst)
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
                            var val = (double)pLineSrc[ch];
                            var quan = Math.Floor(val / 64) * 64 + 32;
                            pLineDst[ch] = (byte)quan;
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
