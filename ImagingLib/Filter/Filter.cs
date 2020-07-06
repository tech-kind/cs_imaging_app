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
        /// ガウシアンフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="sigma"></param>
        /// <param name="kernel_size"></param>
        public unsafe static void GaunssianFilter(Bitmap src, Bitmap dst, double sigma, int kernel_size)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;
                
                var pad = (int)Math.Floor((double)kernel_size / 2);
                int _x = 0, _y = 0;
                double kernel_sum = 0;

                double[,] kernel = new double[kernel_size, kernel_size];

                for(int y = 0; y < kernel_size; y++)
                {
                    for(int x = 0; x < kernel_size; x++)
                    {
                        _y = y - pad;
                        _x = x - pad;
                        kernel[y, x] = 1 / (2 * Math.PI * sigma * sigma)
                            * Math.Exp(-1 * (_x * _x + _y * _y) / (2 * sigma * sigma));
                        kernel_sum += kernel[y, x];
                    }
                }

                for(int y = 0; y < kernel_size; y++)
                {
                    for(int x = 0; x < kernel_size; x++)
                    {
                        kernel[y, x] /= kernel_sum;
                    }
                }

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            double v = 0;
                            for (int dy = -pad; dy < pad + 1; dy++)
                            {
                                for(int dx = -pad; dx < pad + 1; dx++)
                                {
                                    if (((x + dx) < 0) || ((y + dy) < 0))
                                    {
                                        continue;
                                    }
                                    if (((x + dx) >= width) || ((y + dy) >= height))
                                    {
                                        continue;
                                    }
                                    v += lbSrc[y + dy, x + dx, ch] * kernel[dy + pad, dx + pad];
                                }
                            }
                            lbDst[y, x, ch] = (byte)v;
                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// メディアンフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="kernel_size"></param>
        public unsafe static void MedianFilter(Bitmap src, Bitmap dst, int kernel_size)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                var pad = (int)Math.Floor((double)kernel_size / 2);

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            int count = 0;
                            int[] vs = Enumerable.Repeat<int>(999, kernel_size * kernel_size).ToArray();

                            for (int dy = -pad; dy < pad + 1; dy++)
                            {
                                for (int dx = -pad; dx < pad + 1; dx++)
                                {
                                    if (((x + dx) < 0) || ((y + dy) < 0))
                                    {
                                        continue;
                                    }
                                    if (((x + dx) >= width) || ((y + dy) >= height))
                                    {
                                        continue;
                                    }
                                    vs[count++] = lbSrc[y + dy, x + dx, ch];
                                }
                            }

                            Array.Sort(vs);
                            lbDst[y, x, ch] = (byte)vs[(int)Math.Floor((decimal)count / 2) + 1];

                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// 平滑化フィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="kernel_size"></param>
        public unsafe static void MeanFilter(Bitmap src, Bitmap dst, int kernel_size)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                var pad = (int)Math.Floor((double)kernel_size / 2);

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            double v = 0;

                            for (int dy = -pad; dy < pad + 1; dy++)
                            {
                                for (int dx = -pad; dx < pad + 1; dx++)
                                {
                                    if (((x + dx) < 0) || ((y + dy) < 0))
                                    {
                                        continue;
                                    }
                                    if (((x + dx) >= width) || ((y + dy) >= height))
                                    {
                                        continue;
                                    }
                                    v += lbSrc[y + dy, x + dx, ch];
                                }
                            }

                            v /= (kernel_size * kernel_size);
                            lbDst[y, x, ch] = (byte)v;

                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// モーションフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="kernel_size"></param>
        public unsafe static void MotionFilter(Bitmap src, Bitmap dst, int kernel_size)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                var pad = (int)Math.Floor((double)kernel_size / 2);
                double[,] kernel = new double[kernel_size, kernel_size];

                for (int y = 0; y < kernel_size; y++)
                {
                    for (int x = 0; x < kernel_size; x++)
                    {
                        if (y == x)
                        {
                            kernel[y, x] = 1.0 / kernel_size;
                        }
                        else
                        {
                            kernel[y, x] = 0.0;
                        }
                    }
                }


                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            double v = 0;
                            for (int dy = -pad; dy < pad + 1; dy++)
                            {
                                for (int dx = -pad; dx < pad + 1; dx++)
                                {
                                    if (((x + dx) < 0) || ((y + dy) < 0))
                                    {
                                        continue;
                                    }
                                    if (((x + dx) >= width) || ((y + dy) >= height))
                                    {
                                        continue;
                                    }
                                    v += lbSrc[y + dy, x + dx, ch] * kernel[dy + pad, dx + pad];
                                }
                            }

                            lbDst[y, x, ch] = (byte)v;

                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// Max-Minフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="kernel_size"></param>
        public unsafe static void MaxMinFilter(Bitmap src, Bitmap dst, int kernel_size)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                var pad = (int)Math.Floor((double)kernel_size / 2);

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double vmax = 0;
                        double vmin = 999;
                        double v = 0;

                        for (int dy = -pad; dy < pad + 1; dy++)
                        {
                            for (int dx = -pad; dx < pad + 1; dx++)
                            {
                                if (((x + dx) < 0) || ((y + dy) < 0))
                                {
                                    continue;
                                }
                                if (((x + dx) >= width) || ((y + dy) >= height))
                                {
                                    continue;
                                }
                                v = lbSrc[y + dy, x + dx];
                                if(v > vmax)
                                {
                                    vmax = v;
                                }
                                if(v < vmin)
                                {
                                    vmin = v;
                                }
                            }
                        }
                        lbDst[y, x] = (byte)(vmax - vmin);
                    }
                }
                );
            }
        }

        /// <summary>
        /// 微分フィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="kernel_size"></param>
        /// <param name="horizontal"></param>
        public unsafe static void DiffFilter(Bitmap src, Bitmap dst, bool horizontal)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                // int kernel_size = 3;
                var pad = (int)Math.Floor((double)3 / 2);

                double[,] kernel = new double[3, 3] 
                { 
                    { 0, -1, 0}, 
                    { 0, 1, 0}, 
                    { 0, 0, 0}
                };

                if(horizontal)
                {
                    kernel[0, 1] = 0;
                    kernel[1, 0] = -1;
                }
                

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double v = 0;

                        for (int dy = -pad; dy < pad + 1; dy++)
                        {
                            for (int dx = -pad; dx < pad + 1; dx++)
                            {
                                if (((x + dx) < 0) || ((y + dy) < 0))
                                {
                                    continue;
                                }
                                if (((x + dx) >= width) || ((y + dy) >= height))
                                {
                                    continue;
                                }
                                v += lbSrc[y + dy, x + dx] * kernel[dy + pad, dx + pad];                                
                            }
                        }
                        v = Math.Max(v, 0);
                        v = Math.Min(v, 255);

                        lbDst[y, x] = (byte)v;
                    }
                }
                );
            }
        }

        /// <summary>
        /// Prewittフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="horizontal"></param>
        public unsafe static void PrewittFilter(Bitmap src, Bitmap dst, bool horizontal)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                // int kernel_size = 3;
                var pad = (int)Math.Floor((double)3 / 2);

                double[,] kernel = new double[3, 3]
                {
                    { -1, -1, -1},
                    { 0, 0, 0},
                    { 1, 1, 1}
                };

                if (horizontal)
                {
                    kernel[0, 1] = 0;
                    kernel[0, 2] = 1;
                    kernel[1, 0] = -1;
                    kernel[1, 2] = 1;
                    kernel[2, 0] = -1;
                    kernel[2, 1] = 0;
                }


                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double v = 0;

                        for (int dy = -pad; dy < pad + 1; dy++)
                        {
                            for (int dx = -pad; dx < pad + 1; dx++)
                            {
                                if (((x + dx) < 0) || ((y + dy) < 0))
                                {
                                    continue;
                                }
                                if (((x + dx) >= width) || ((y + dy) >= height))
                                {
                                    continue;
                                }
                                v += lbSrc[y + dy, x + dx] * kernel[dy + pad, dx + pad];
                            }
                        }
                        v = Math.Max(v, 0);
                        v = Math.Min(v, 255);

                        lbDst[y, x] = (byte)v;
                    }
                }
                );
            }
        }

        /// <summary>
        /// Sobelフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="horizontal"></param>
        public unsafe static void SobelFilter(Bitmap src, Bitmap dst, bool horizontal)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                // int kernel_size = 3;
                var pad = (int)Math.Floor((double)3 / 2);

                double[,] kernel = new double[3, 3]
                {
                    { 1, 2, 1},
                    { 0, 0, 0},
                    { -1, -2, -1}
                };

                if (horizontal)
                {
                    kernel[0, 1] = 0;
                    kernel[0, 2] = -1;                    
                    kernel[1, 0] = 2;
                    kernel[1, 2] = -2;
                    kernel[2, 0] = 1;
                    kernel[2, 1] = 0;
                }


                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double v = 0;

                        for (int dy = -pad; dy < pad + 1; dy++)
                        {
                            for (int dx = -pad; dx < pad + 1; dx++)
                            {
                                if (((x + dx) < 0) || ((y + dy) < 0))
                                {
                                    continue;
                                }
                                if (((x + dx) >= width) || ((y + dy) >= height))
                                {
                                    continue;
                                }
                                v += lbSrc[y + dy, x + dx] * kernel[dy + pad, dx + pad];
                            }
                        }
                        v = Math.Max(v, 0);
                        v = Math.Min(v, 255);

                        lbDst[y, x] = (byte)v;
                    }
                }
                );
            }
        }

        /// <summary>
        /// Laplacianフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public unsafe static void LaplacianFilter(Bitmap src, Bitmap dst)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                // int kernel_size = 3;
                var pad = (int)Math.Floor((double)3 / 2);

                double[,] kernel = new double[3, 3]
                {
                    { 0, 1, 0},
                    { 1, -4, 1},
                    { 0, 1, 0}
                };

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double v = 0;

                        for (int dy = -pad; dy < pad + 1; dy++)
                        {
                            for (int dx = -pad; dx < pad + 1; dx++)
                            {
                                if (((x + dx) < 0) || ((y + dy) < 0))
                                {
                                    continue;
                                }
                                if (((x + dx) >= width) || ((y + dy) >= height))
                                {
                                    continue;
                                }
                                v += lbSrc[y + dy, x + dx] * kernel[dy + pad, dx + pad];
                            }
                        }
                        v = Math.Max(v, 0);
                        v = Math.Min(v, 255);

                        lbDst[y, x] = (byte)v;
                    }
                }
                );
            }
        }

        /// <summary>
        /// Embossフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public unsafe static void EmbossFilter(Bitmap src, Bitmap dst)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                // int kernel_size = 3;
                var pad = (int)Math.Floor((double)3 / 2);

                double[,] kernel = new double[3, 3]
                {
                    { -2, -1, 0},
                    { -1, 1, 1},
                    { 0, 1, 2}
                };

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double v = 0;

                        for (int dy = -pad; dy < pad + 1; dy++)
                        {
                            for (int dx = -pad; dx < pad + 1; dx++)
                            {
                                if (((x + dx) < 0) || ((y + dy) < 0))
                                {
                                    continue;
                                }
                                if (((x + dx) >= width) || ((y + dy) >= height))
                                {
                                    continue;
                                }
                                v += lbSrc[y + dy, x + dx] * kernel[dy + pad, dx + pad];
                            }
                        }
                        v = Math.Max(v, 0);
                        v = Math.Min(v, 255);

                        lbDst[y, x] = (byte)v;
                    }
                }
                );
            }
        }

        /// <summary>
        /// Logフィルタ
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="kernel_size"></param>
        /// <param name="sigma"></param>
        public unsafe static void LogFilter(Bitmap src, Bitmap dst, int kernel_size, double sigma)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;
                
                var pad = (int)Math.Floor((double)kernel_size / 2);

                double[,] kernel = new double[kernel_size, kernel_size];
                double kernel_sum = 0;
                double _x, _y;

                for(int y = 0; y < kernel_size; y++)
                {
                    for(int x = 0; x < kernel_size; x++)
                    {
                        _y = y - pad;
                        _x = x - pad;
                        kernel[y, x] = (_x * _x + _y * _y - sigma * sigma)
                            / (2 * Math.PI * Math.Pow(sigma, 6))
                            * Math.Exp(-1 * (_x * _x + _y * _y) / (2 * sigma * sigma));
                        kernel_sum += kernel[y, x];
                    }
                }
                for (int y = 0; y < kernel_size; y++)
                {
                    for (int x = 0; x < kernel_size; x++)
                    {
                        kernel[y, x] /= kernel_sum;
                    }
                }

                Parallel.For(0, height, y =>
                {
                    for (int x = 0; x < width; x++)
                    {
                        double v = 0;

                        for (int dy = -pad; dy < pad + 1; dy++)
                        {
                            for (int dx = -pad; dx < pad + 1; dx++)
                            {
                                if (((x + dx) < 0) || ((y + dy) < 0))
                                {
                                    continue;
                                }
                                if (((x + dx) >= width) || ((y + dy) >= height))
                                {
                                    continue;
                                }
                                v += lbSrc[y + dy, x + dx] * kernel[dy + pad, dx + pad];
                            }
                        }
                        v = Math.Max(v, 0);
                        v = Math.Min(v, 255);

                        lbDst[y, x] = (byte)v;
                    }
                }
                );
            }

        }

    }
}
