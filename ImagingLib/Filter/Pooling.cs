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
        /// 平均プーリング
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public unsafe static void AveragePooling(Bitmap src, Bitmap dst)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                int r = 8;
                int div = height / r;
                Parallel.For(0, div, y =>
                {
                    int col = y * r;                    
                    for (int x = 0; x < width; x += r)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            double v = 0;
                            for (int dy = 0; dy < r; dy++)
                            {
                                for (int dx = 0; dx < r; dx++)
                                {
                                    v += lbSrc[col + dy, x + dx, ch];
                                }
                            }
                            v /= (r * r);
                            for (int dy = 0; dy < r; dy++)
                            {
                                for (int dx = 0; dx < r; dx++)
                                {
                                    lbDst[col + dy, x + dx, ch] = (byte)v;
                                }
                            }
                        }
                    }
                });
            }
        }

        /// <summary>
        /// Maxプーリング
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public unsafe static void MaxPooling(Bitmap src, Bitmap dst)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                int r = 8;
                int div = height / r;
                Parallel.For(0, div, y =>
                {
                    int col = y * r;                    
                    for (int x = 0; x < width; x += r)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            double v = 0;
                            for (int dy = 0; dy < r; dy++)
                            {
                                for (int dx = 0; dx < r; dx++)
                                {
                                    v = Math.Max(lbSrc[col + dy, x + dx, ch], v);
                                }
                            }

                            for (int dy = 0; dy < r; dy++)
                            {
                                for (int dx = 0; dx < r; dx++)
                                {
                                    lbDst[col + dy, x + dx, ch] = (byte)v;
                                }
                            }
                        }
                    }
                });
            }
        }
    }
}
