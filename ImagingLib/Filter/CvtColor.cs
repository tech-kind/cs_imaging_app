using System.Drawing;
using System.Threading.Tasks;

namespace ImagingLib
{
    public static partial class ImageOperator
    {
        // enum ColorConversionCodes相当の定数
        public const int COLOR_BGR2BGRA = 0; //!< add alpha channel to RGB or BGR image
        public const int COLOR_RGB2RGBA = COLOR_BGR2BGRA;

        public const int COLOR_BGRA2BGR = 1; //!< remove alpha channel from RGB or BGR image
        public const int COLOR_RGBA2RGB = COLOR_BGRA2BGR;

        public const int COLOR_BGR2RGBA = 2; //!< convert between RGB and BGR color spaces (with or without alpha channel)
        public const int COLOR_RGB2BGRA = COLOR_BGR2RGBA;

        public const int COLOR_RGBA2BGR = 3;
        public const int COLOR_BGRA2RGB = COLOR_RGBA2BGR;

        public const int COLOR_BGR2RGB = 4;
        public const int COLOR_RGB2BGR = COLOR_BGR2RGB;

        public const int COLOR_BGRA2RGBA = 5;
        public const int COLOR_RGBA2BGRA = COLOR_BGRA2RGBA;

        public const int COLOR_BGR2GRAY = 6; //!< convert between RGB/BGR and grayscale; @ref color_convert_rgb_gray "color conversions"
        public const int COLOR_RGB2GRAY = 7;
        public const int COLOR_GRAY2BGR = 8;
        public const int COLOR_GRAY2RGB = COLOR_GRAY2BGR;
        public const int COLOR_GRAY2BGRA = 9;
        public const int COLOR_GRAY2RGBA = COLOR_GRAY2BGRA;
        public const int COLOR_BGRA2GRAY = 10;
        public const int COLOR_RGBA2GRAY = 11;

        /// <summary>
        /// カラー変換
        /// </summary>
        /// <param name="src">変換前の画像データ</param>
        /// <param name="code">カラー変換コード　ImagingDotNet.COLOR_BGR2GRAY など、名前部分はOpenCVと同じ</param>
        /// <returns>変換後の画像データ</returns>
        public static Bitmap CvtColor(Bitmap src, int code)
        {
            if (src == null) return null;

            Bitmap dst = null;

            switch (code)
            {
                case COLOR_BGR2GRAY:
                    dst = CreateBitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format8bppIndexed);
                    break;

                case COLOR_BGR2RGB: // COLOR_RGB2BGR も同じ
                    dst = CreateBitmap(src.Width, src.Height, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                    break;

                default:
                    break;
            }

            CvtColor(src, dst, code);

            return dst;
        }

        /// <summary>
        /// カラー変換
        /// </summary>
        /// <param name="src">変換前の画像データ</param>
        /// <param name="dst">変換後の画像データ</param>
        /// <param name="code">カラー変換コード　ImagingDotNet.COLOR_BGR2GRAY など、名前部分はOpenCVと同じ</param>
        public static void CvtColor(Bitmap src, Bitmap dst, int code)
        {
            if ((src == null) || (dst == null)) return;

            switch (code)
            {
                case COLOR_BGR2GRAY:
                    cvt_COLOR_BGR2GRAY(src, dst);
                    break;

                case COLOR_BGR2RGB:
                    cvt_COLOR_BGR2RGB(src, dst);
                    break;


                default:
                    break;
            }
        }

        /// <summary>
        /// カラー→モノクロ変換
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        private unsafe static void cvt_COLOR_BGR2GRAY(Bitmap src, Bitmap dst)
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
                        // 輝度値(モノクロ)の設定
                        // gray = ((0.299 * R + 0.587 * G + 0.114 * B) * 256) / 256
                        pLineDst[0] = (byte)((77 * pLineSrc[2] + 150 * pLineSrc[1] + 29 * pLineSrc[0]) >> 8);

                        // 次の画素へ
                        pLineSrc += channel;
                        pLineDst++;
                    }
                }
                );
            }
        }

        /// <summary>
        /// チャンネル入れ替え
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        private unsafe static void cvt_COLOR_BGR2RGB(Bitmap src, Bitmap dst)
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
                        // チャンネル入れ替え
                        pLineDst[0] = pLineSrc[2];
                        pLineDst[1] = pLineSrc[1];
                        pLineDst[2] = pLineSrc[0];

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
