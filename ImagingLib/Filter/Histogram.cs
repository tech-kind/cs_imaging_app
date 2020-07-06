using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ImagingLib
{
    public static partial class ImageOperator
    {
        /// <summary>
        /// ヒストグラムの取得
        /// </summary>
        /// <param name="src">入力画像</param>
        /// <param name="dst">出力画像</param>
        /// <param name="histogram">ヒストグラム</param>
        public static int[,] Histogram(this Bitmap src)
        {
            var channel = src.GetBitCount() / 8;
            var hist = new int[channel, 256];

            Histogram(src, hist);

            return hist;
        }

        /// <summary>
        /// ヒストグラムの取得
        /// </summary>
        /// <param name="src">入力画像</param>
        /// <param name="dst">出力画像</param>
        /// <param name="histogram">ヒストグラム</param>
        public unsafe static void Histogram(this Bitmap src, int[,] histogram)
        {
            using (var lbSrc = new LockBitmap(src))
            {
                var pSrc = (byte*)lbSrc.Scan0;

                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                // ヒストグラムの値の初期化
                for (int ch = 0; ch < channel; ch++)
                {
                    for (int i = 0; i < 256; i++)
                    {
                        histogram[ch, i] = 0;
                    }
                }

                for (int y = 0; y < height; y++)
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            // 輝度値のカウント
                            histogram[ch, pLineSrc[ch]]++;
                        }

                        // 次の画素へ
                        pLineSrc += channel;
                    }
                }
            }
        }

        /// <summary>
        /// ヒストグラム正規化
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        public unsafe static void HistogramNormalization(Bitmap src, Bitmap dst, int a, int b)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var pSrc = (byte*)lbSrc.Scan0;
                var pDst = (byte*)lbDst.Scan0;

                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                int c = 255;
                int d = 0;
                int tmp;
                for (int y = 0; y < height; y++)
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;                    

                    for (int x = 0; x < width; x++)
                    {
                        for (int _c = 0; _c < channel; _c++)
                        {
                            tmp = pLineSrc[_c];
                            c = Math.Min(c, tmp);
                            d = Math.Max(d, tmp);
                        }
                        // 次の画素へ
                        pLineSrc += channel;
                    }
                }

                Parallel.For(0, height, y =>
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;
                    byte* pLineDst = pDst + y * lbDst.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            int val = pLineSrc[ch];

                            if(val < a)
                            {
                                pLineDst[ch] = (byte)a;
                            }
                            else if(val <= b)
                            {
                                pLineDst[ch] = (byte)((b - a) / (d - c) * (val - c) + a);
                            }
                            else
                            {
                                pLineDst[ch] = (byte)b;
                            }
                        }
                        // 次の画素へ
                        pLineSrc += channel;
                        pLineDst += channel;
                    }
                }
                );
            }
        }

        /// <summary>
        /// ヒストグラム操作
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="m0"></param>
        /// <param name="s0"></param>
        public unsafe static void HistogramTransform(Bitmap src, Bitmap dst, int m0, int s0)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var pSrc = (byte*)lbSrc.Scan0;
                var pDst = (byte*)lbDst.Scan0;

                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                double m, s;
                double sum = 0;
                double squared_sum = 0;
                double tmp;
                for (int y = 0; y < height; y++)
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        for (int _c = 0; _c < channel; _c++)
                        {
                            tmp = pLineSrc[_c];
                            sum += tmp;
                            squared_sum += (tmp * tmp);
                        }
                        // 次の画素へ
                        pLineSrc += channel;
                    }
                }

                // get standard deviation
                m = sum / (height * width * channel);
                s = Math.Sqrt(squared_sum / (height * width * channel) - m * m);

                Parallel.For(0, height, y =>
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;
                    byte* pLineDst = pDst + y * lbDst.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            int val = pLineSrc[ch];
                            double output = s0 / s * (val - m) + m0;
                            if (output < 0) output = 0;
                            if (output > 255) output = 255;
                            pLineDst[ch] = (byte)(output);
                        }
                        // 次の画素へ
                        pLineSrc += channel;
                        pLineDst += channel;
                    }
                }
                );
            }
        }

        /// <summary>
        /// ヒストグラム平坦化
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        public unsafe static void HistogramEqualization(Bitmap src, Bitmap dst)
        {
            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                var pSrc = (byte*)lbSrc.Scan0;
                var pDst = (byte*)lbDst.Scan0;

                var width = lbSrc.Width;
                var height = lbSrc.Height;
                var channel = lbSrc.Channel;

                double zmax = 255;
                double[] hist = Enumerable.Repeat<double>(0, 256).ToArray();
                double s = width * height * channel;

                int tmp;
                for (int y = 0; y < height; y++)
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        for (int _c = 0; _c < channel; _c++)
                        {
                            tmp = pLineSrc[_c];
                            hist[tmp]++;
                        }
                        // 次の画素へ
                        pLineSrc += channel;
                    }
                }

                Parallel.For(0, height, y =>
                {
                    // 行の先頭ポインタ
                    byte* pLineSrc = pSrc + y * lbSrc.Stride;
                    byte* pLineDst = pDst + y * lbDst.Stride;

                    for (int x = 0; x < width; x++)
                    {
                        for (int ch = 0; ch < channel; ch++)
                        {
                            int val = pLineSrc[ch];
                            double hist_sum = 0;

                            for(int l = 0; l < val; l++)
                            {
                                hist_sum += hist[l];
                            }

                            pLineDst[ch] = (byte)(zmax / s * hist_sum);
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
