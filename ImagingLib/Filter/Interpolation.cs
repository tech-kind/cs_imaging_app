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
        /// 最近傍補間
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        public unsafe static void NearestNeighbor(Bitmap src, ref Bitmap dst, double rx, double ry)
        {
            int resized_width = (int)(src.Width * rx);
            int resized_height = (int)(src.Height * ry);

            dst = CreateBitmap(resized_width, resized_height, src.PixelFormat);

            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                int width = lbSrc.Width;
                int height = lbSrc.Height;
                int channel = lbSrc.Channel;

                Parallel.For(0, resized_height, y =>
                {
                    int y_before = (int)Math.Round(y / ry);
                    y_before = Math.Min(y_before, height - 1);

                    for (int x = 0; x < resized_width; x++)
                    {
                        int x_before = (int)Math.Round(x / rx);
                        x_before = Math.Min(x_before, width - 1);

                        for (int ch = 0; ch < channel; ch++)
                        {
                            lbDst[y, x, ch] = lbSrc[y_before, x_before, ch];
                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// Bi-linear補間
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        public unsafe static void Bilinear(Bitmap src, ref Bitmap dst, double rx, double ry)
        {
            int resized_width = (int)(src.Width * rx);
            int resized_height = (int)(src.Height * ry);

            dst = CreateBitmap(resized_width, resized_height, src.PixelFormat);

            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                int width = lbSrc.Width;
                int height = lbSrc.Height;
                int channel = lbSrc.Channel;

                Parallel.For(0, resized_height, y =>
                {
                    int y_before = (int)Math.Floor(y / ry);
                    y_before = Math.Min(y_before, height - 1);
                    double dy = y / ry - y_before;

                    for (int x = 0; x < resized_width; x++)
                    {
                        int x_before = (int)Math.Floor(x / rx);
                        x_before = Math.Min(x_before, width - 1);
                        double dx = x / rx - x_before;

                        for (int ch = 0; ch < channel; ch++)
                        {
                            double val = (1 - dx) * (1 - dy) * lbSrc[y_before, x_before, ch]
                                + dx * (1 - dy) * lbSrc[y_before, x_before + 1, ch]
                                + (1 - dx) * dy * lbSrc[y_before + 1, x_before, ch]
                                + dx * dy * lbSrc[y_before + 1, x_before + 1, ch];
                            lbDst[y, x, ch] = (byte)val;
                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// Bi-cubic補間
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="rx"></param>
        /// <param name="ry"></param>
        public unsafe static void Bicubic(Bitmap src, ref Bitmap dst, double rx, double ry)
        {
            int resized_width = (int)(src.Width * rx);
            int resized_height = (int)(src.Height * ry);

            dst = CreateBitmap(resized_width, resized_height, src.PixelFormat);

            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                int width = lbSrc.Width;
                int height = lbSrc.Height;
                int channel = lbSrc.Channel;

                Parallel.For(0, resized_height, y =>
                {
                    double dy = y / ry;
                    int y_before = (int)Math.Floor(dy);

                    for (int x = 0; x < resized_width; x++)
                    {
                        double dx = x / rx;
                        int x_before = (int)Math.Floor(dx);

                        for (int ch = 0; ch < channel; ch++)
                        {
                            double w_sum = 0;
                            double val = 0;

                            for(int j = -1; j < 3; j++)
                            {
                                int _y = ValClip(y_before + j, 0, height - 1);
                                double wy = Weight(Math.Abs(dy - _y));

                                for(int i = -1; i < 3; i++)
                                {
                                    int _x = ValClip(x_before + i, 0, width - 1);
                                    double wx = Weight(Math.Abs(dx - _x));
                                    w_sum += wx * wy;
                                    val += lbSrc[_y, _x, ch] * wx * wy;
                                }
                            }
                            val /= w_sum;
                            val = ValClip((int)val, 0, 255);
                            
                            lbDst[y, x, ch] = (byte)val;
                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// Weightの計算
        /// </summary>
        /// <param name="t"></param>
        /// <returns></returns>
        private static double Weight(double t)
        {
            double a = -1;
            if(Math.Abs(t) <= 1)
            {
                return (a + 2) * Math.Pow(Math.Abs(t), 3) - (a + 3) * Math.Pow(Math.Abs(t), 2) + 1;
            }
            else if(Math.Abs(t) <= 2)
            {
                return a * Math.Pow(Math.Abs(t), 3) - 5 * a * Math.Pow(Math.Abs(t), 2) + 8 * a * Math.Abs(t) - 4 * a;
            }

            return 0;
        }

        /// <summary>
        /// 値を最小値から最大値の間でクリップする
        /// </summary>
        /// <param name="x"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        private static int ValClip(int x, int min, int max)
        {
            return Math.Min(Math.Max(x, min), max);
        }
    }
}
