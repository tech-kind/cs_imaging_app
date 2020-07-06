using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace ImagingLib
{
    public class FourierClass
    {
        public Complex[,,] coef { get; set; }

        public FourierClass(int width, int height, int channel)
        {
            coef = new Complex[height, width, channel];
        }
    }

    public static partial class ImageOperator
    {
        /// <summary>
        /// フーリエ変換
        /// </summary>
        /// <param name="src"></param>
        /// <param name="fourier"></param>
        public unsafe static FourierClass Dft(Bitmap src)
        {
            FourierClass fourier = null;

            using (var lbSrc = new LockBitmap(src))
            {
                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                fourier = new FourierClass(width, height, channel);

                // for (int ch = 0; ch < channel; ch++)
                Parallel.For(0, channel, ch =>
                {
                    for (int l = 1; l <= height; l++)
                    {
                        for (int k = 1; k <= width; k++)
                        {
                            Complex val = new Complex(0, 0);
                            for (int y = 0; y < height; y++)
                            {
                                for (int x = 0; x < width; x++)
                                {
                                    double I = lbSrc[y, x, ch];
                                    double theta = -2 * Math.PI * ((double)k * (double)x
                                        / (double)width + (double)l * (double)y / (double)height);
                                    val += new Complex(Math.Cos(theta), Math.Sin(theta)) * I;
                                }
                            }
                            val /= Math.Sqrt(height * width);
                            fourier.coef[l - 1, k - 1, ch] = val;
                        }
                    }
                }
                );
            }
            return fourier;
        }

        /// <summary>
        /// 逆フーリエ変換
        /// </summary>
        /// <param name="dst"></param>
        /// <param name="fourier"></param>
        public unsafe static void IDft(Bitmap dst, FourierClass fourier)
        {
            using (var lbDst = new LockBitmap(dst))
            {
                int width = lbDst.Width;
                int height = lbDst.Height;
                int channel = lbDst.Channel;

                // for (int ch = 0; ch < channel; ch++)
                Parallel.For(0, channel, ch =>
                {

                    for (int y = 1; y <= height; y++)
                    {
                        for (int x = 1; x <= width; x++)
                        {
                            Complex val = new Complex(0, 0);
                            for (int l = 0; l < height; l++)
                            {
                                for (int k = 0; k < width; k++)
                                {
                                    Complex G = fourier.coef[l, k, ch];
                                    double theta = -2 * Math.PI * ((double)k * (double)x
                                        / (double)width + (double)l * (double)y / (double)height);
                                    val += new Complex(Math.Cos(theta), Math.Sin(theta)) * G;
                                }
                            }
                            double g = Math.Sqrt(val.Real * val.Real + val.Imaginary * val.Imaginary) / Math.Sqrt(width * height);
                            g = Math.Min(Math.Max(g, 0), 255);
                            lbDst[height - y, width - x, ch] = (byte)g;
                        }
                    }
                }
                );
            }
        }

        /// <summary>
        /// ローパスフィルタ
        /// </summary>
        /// <param name="fourier"></param>
        /// <param name="pass_r"></param>
        public unsafe static void Lpf(ref FourierClass fourier, double pass_r)
        {
            int height = fourier.coef.GetLength(0);
            int width = fourier.coef.GetLength(1);

            double r = height / 2;
            int filter_d = (int)(r * pass_r);

            for(int j = 0; j < height / 2; j++)
            {
                for(int i = 0; i < width / 2; i++)
                {
                    if(Math.Sqrt(i * i + j * j) >= filter_d)
                    {
                        fourier.coef[j, i, 0] = 0;
                        fourier.coef[j, width - i - 1, 0] = 0;
                        fourier.coef[height - j - 1, i, 0] = 0;
                        fourier.coef[height - j - 1, width - i - 1, 0] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// ハイパスフィルタ
        /// </summary>
        /// <param name="fourier"></param>
        /// <param name="pass_r"></param>
        public unsafe static void Hpf(ref FourierClass fourier, double pass_r)
        {
            int height = fourier.coef.GetLength(0);
            int width = fourier.coef.GetLength(1);

            double r = height / 2;
            int filter_d = (int)(r * pass_r);

            for (int j = 0; j < height / 2; j++)
            {
                for (int i = 0; i < width / 2; i++)
                {
                    if (Math.Sqrt(i * i + j * j) <= filter_d)
                    {
                        fourier.coef[j, i, 0] = 0;
                        fourier.coef[j, width - i - 1, 0] = 0;
                        fourier.coef[height - j - 1, i, 0] = 0;
                        fourier.coef[height - j - 1, width - i - 1, 0] = 0;
                    }
                }
            }
        }

        /// <summary>
        /// バンドパスフィルタ
        /// </summary>
        /// <param name="fourier"></param>
        /// <param name="pass_lower"></param>
        /// <param name="pass_upper"></param>
        public unsafe static void Bpf(ref FourierClass fourier, double pass_lower, double pass_upper)
        {
            int height = fourier.coef.GetLength(0);
            int width = fourier.coef.GetLength(1);

            double r = height / 2;
            int filter_lower = (int)(r * pass_lower);
            int filter_upper = (int)(r * pass_upper);

            for (int j = 0; j < height / 2; j++)
            {
                for (int i = 0; i < width / 2; i++)
                {
                    if ((Math.Sqrt(i * i + j * j) < filter_lower) || 
                        (Math.Sqrt(i * i + j * j) > filter_upper))
                    {
                        fourier.coef[j, i, 0] = 0;
                        fourier.coef[j, width - i - 1, 0] = 0;
                        fourier.coef[height - j - 1, i, 0] = 0;
                        fourier.coef[height - j - 1, width - i - 1, 0] = 0;
                    }
                }
            }
        }
    }
}
