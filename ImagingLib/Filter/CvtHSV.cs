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
        /// HSV変換
        /// </summary>
        /// <param name="src"></param>
        /// <returns></returns>
        public static Bitmap CvtHSV(Bitmap src)
        {
            if (src == null) return null;

            Bitmap dst = null;

            dst = CreateBitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
            cvt_BGR2HSV(src, dst);

            return dst;
        }

        /// <summary>
        /// BGRをHSVに変換する
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        private unsafe static void cvt_BGR2HSV(Bitmap src, Bitmap dst)
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
                        // RGB取得
                        var r = pLineSrc[2] / 255.0;
                        var g = pLineSrc[1] / 255.0;
                        var b = pLineSrc[0] / 255.0;

                        double h, s, v;
                        double _r, _g, _b;
                        GetHSV(r, g, b, out h, out s, out v);
                        var _h = (h + 180) % 360;
                        GetRGB(_h, s, v, out _r, out _g, out _b);

                        pLineDst[0] = (byte)(_b * 255);
                        pLineDst[1] = (byte)(_g * 255);
                        pLineDst[2] = (byte)(_r * 255);

                        // 次の画素へ
                        pLineSrc += channel;
                        pLineDst += channel;
                    }
                }
                );
            }
        }

        /// <summary>
        /// HSV値取得
        /// </summary>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        private static void GetHSV(double r, double g, double b,
            out double h, out double s, out double v)
        {
            var max = Math.Max(r, Math.Max(g, b));
            var min = Math.Min(r, Math.Min(g, b));

            h = 0;
            s = 0;
            v = 0;

            if (min == max)
            {
                h = 0;
            }
            else if (min == b)
            {
                h = 60 * (g - r) / (max - min) + 60;
            }
            else if (min == r)
            {
                h = 60 * (b - g) / (max - min) + 180;
            }
            else if (min == g)
            {
                h = 60 * (r - b) / (max - min) + 300;
            }

            s = max - min;

            v = max;
        }

        /// <summary>
        /// RGB値取得
        /// </summary>
        /// <param name="h"></param>
        /// <param name="s"></param>
        /// <param name="v"></param>
        /// <param name="r"></param>
        /// <param name="g"></param>
        /// <param name="b"></param>
        private static void GetRGB(double h, double s, double v,
            out double r, out double g, out double b)
        {
            var c = s;
            var _h = h / 60;
            var _x = c * (1 - Math.Abs(_h % 2 - 1));

            r = g = b = v - c;

            if (_h < 1)
            {
                r += c;
                g += _x;
            }
            else if (_h < 2)
            {
                r += _x;
                g += c;
            }
            else if (_h < 3)
            {
                g += c;
                b += _x;
            }
            else if (_h < 4)
            {
                g += _x;
                b += c;
            }
            else if (_h < 5)
            {
                r += _x;
                b += c;
            }
            else if (_h < 6)
            {
                r += c;
                b += _x;
            }
        }
    }
}
