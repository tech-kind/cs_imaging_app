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
        /// アフィン変換
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        public unsafe static void Affine(Bitmap src, ref Bitmap dst, double a,
            double b, double c, double d, double tx, double ty)
        {
            int resized_width = (int)(src.Width * a);
            int resized_height = (int)(src.Height * d);

            dst = CreateBitmap(resized_width, resized_height, src.PixelFormat);

            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                int width = lbSrc.Width;
                int height = lbSrc.Height;
                int channel = lbSrc.Channel;
                double det = a * d - b * c;

                Parallel.For(0, resized_height, y =>
                {
                    for (int x = 0; x < resized_width; x++)
                    {
                        int x_before = (int)((d * x - b * y) / det - tx);

                        if((x_before < 0) || (x_before >= width))
                        {
                            continue;
                        }

                        int y_before = (int)((-c * x + a * y) / det - ty);

                        if((y_before < 0) || (y_before >= height))
                        {
                            continue;
                        }

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
        /// アフィン変換（回転を考慮）
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="theta"></param>
        public unsafe static void Affine(Bitmap src, ref Bitmap dst, double a,
            double b, double c, double d, double tx, double ty, double theta)
        {
            double det = a * d - b * c;
            int width = src.Width;
            int height = src.Height;

            if(theta != 0)
            {
                double rad = theta / 180.0 * Math.PI;
                a = Math.Cos(rad);
                b = -Math.Sin(rad);
                c = Math.Sin(rad);
                d = Math.Cos(rad);
                tx = 0;
                ty = 0;

                det = a * d - b * c;

                double cx = width / 2;
                double cy = height / 2;
                double new_cx = (d * cx - b * cy) / det;
                double new_cy = (-c * cx + a * cy) / det;
                tx = new_cx - cx;
                ty = new_cy - cy;

            }

            int resized_width = (int)(width * a);
            int resized_height = (int)(height * d);

            if(theta != 0)
            {
                resized_width = (int)width;
                resized_height = (int)height;
            }

            dst = CreateBitmap(resized_width, resized_height, src.PixelFormat);            


            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                int channel = lbSrc.Channel;                

                Parallel.For(0, resized_height, y =>
                {
                    for (int x = 0; x < resized_width; x++)
                    {
                        int x_before = (int)((d * x - b * y) / det - tx);

                        if ((x_before < 0) || (x_before >= width))
                        {
                            continue;
                        }

                        int y_before = (int)((-c * x + a * y) / det - ty);

                        if ((y_before < 0) || (y_before >= height))
                        {
                            continue;
                        }

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
        /// アフィン変換（スキューを考慮）
        /// </summary>
        /// <param name="src"></param>
        /// <param name="dst"></param>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <param name="c"></param>
        /// <param name="d"></param>
        /// <param name="tx"></param>
        /// <param name="ty"></param>
        /// <param name="theta"></param>
        /// <param name="dx"></param>
        /// <param name="dy"></param>
        public unsafe static void Affine(Bitmap src, ref Bitmap dst, double a,
            double b, double c, double d, double tx, double ty, double theta, double dx, double dy)
        {
            double det = a * d - b * c;
            int width = src.Width;
            int height = src.Height;
            
            if (dx != 0)
            {
                b = dx / height;
            }
            if (dy != 0)
            {
                c = dy / width;
            }

            if (theta != 0)
            {
                double rad = theta / 180.0 * Math.PI;
                a = Math.Cos(rad);
                b = -Math.Sin(rad);
                c = Math.Sin(rad);
                d = Math.Cos(rad);
                tx = 0;
                ty = 0;

                det = a * d - b * c;

                double cx = width / 2;
                double cy = height / 2;
                double new_cx = (d * cx - b * cy) / det;
                double new_cy = (-c * cx + a * cy) / det;
                tx = new_cx - cx;
                ty = new_cy - cy;

            }

            int resized_width = (int)(width * a + dx);
            int resized_height = (int)(height * d + dy);

            if (theta != 0)
            {
                resized_width = (int)(width + dx);
                resized_height = (int)(height + dy);
            }

            dst = CreateBitmap(resized_width, resized_height, src.PixelFormat);


            using (var lbSrc = new LockBitmap(src))
            using (var lbDst = new LockBitmap(dst))
            {
                int channel = lbSrc.Channel;

                Parallel.For(0, resized_height, y =>
                {
                    for (int x = 0; x < resized_width; x++)
                    {
                        int x_before = (int)((d * x - b * y) / det - tx);

                        if ((x_before < 0) || (x_before >= width))
                        {
                            continue;
                        }

                        int y_before = (int)((-c * x + a * y) / det - ty);

                        if ((y_before < 0) || (y_before >= height))
                        {
                            continue;
                        }

                        for (int ch = 0; ch < channel; ch++)
                        {
                            lbDst[y, x, ch] = lbSrc[y_before, x_before, ch];
                        }
                    }
                }
                );
            }

        }
    }
}
