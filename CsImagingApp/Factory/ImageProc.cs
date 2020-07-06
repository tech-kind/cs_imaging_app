using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagingLib;

namespace CsImagingApp.Factory
{
    public class ImageProcChannelCvt : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            // カラーでない時は何もしない
            if (src.GetBitCount() < 24) return null;
            Bitmap dst = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2RGB);

            return dst;
        }
    }

    public class ImageProcGrayScale : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            // モノクロなら何もしない
            if (src.GetBitCount() == 8) return null;
            Bitmap dst = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);

            return dst;
        }
    }

    public class ImageProcThreshold : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.Threshold(tmp, dst, 128, 255, ImageOperator.THRESH_BINARY);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcThreshOtsu : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.Threshold(tmp, dst, 128, 255, ImageOperator.THRESH_OTSU);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcHsvCvt : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            // モノクロなら何もしない
            if (src.GetBitCount() == 8) return null;
            Bitmap dst = ImageOperator.CvtHSV(src);

            return dst;
        }
    }

    public class ImageProcQuantization : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.Quantization(src, dst);

            return dst;
        }
    }

    public class ImageProcAveragePooling : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.AveragePooling(src, dst);

            return dst;
        }
    }

    public class ImageProcMaxPooling : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.MaxPooling(src, dst);

            return dst;
        }
    }

    public class ImageProcGaussianFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.GaunssianFilter(src, dst, 1.3, 3);

            return dst;
        }
    }

    public class ImageProcMedianFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.MedianFilter(src, dst, 3);

            return dst;
        }
    }

    public class ImageProcMeanFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.MeanFilter(src, dst, 3);

            return dst;
        }
    }

    public class ImageProcMotionFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.MotionFilter(src, dst, 3);

            return dst;
        }
    }

    public class ImageProcMaxMinFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.MaxMinFilter(tmp, dst, 3);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcDiffFilterVertical : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.DiffFilter(tmp, dst, false);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcDiffFilterHorizontal : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.DiffFilter(tmp, dst, true);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcPrewittFilterVertical : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.PrewittFilter(tmp, dst, false);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcPrewittFilterHorizontal : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.PrewittFilter(tmp, dst, true);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcSobelFilterVertical : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.SobelFilter(tmp, dst, false);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcSobelFilterHorizontal : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.SobelFilter(tmp, dst, true);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcLaplacianFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.LaplacianFilter(tmp, dst);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcEmbossFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.EmbossFilter(tmp, dst);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcLogFilter : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            ImageOperator.LogFilter(tmp, dst, 5, 3);

            tmp.Dispose();
            return dst;
        }
    }

    public class ImageProcHistogramNormalization : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.HistogramNormalization(src, dst, 0, 255);

            return dst;
        }
    }

    public class ImageProcHistogramTransform : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.HistogramTransform(src, dst, 128, 52);

            return dst;
        }
    }

    public class ImageProcHistogramEqualization : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.HistogramEqualization(src, dst);

            return dst;
        }
    }

    public class ImageProcGammaCorrection : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            ImageOperator.GammaCorrection(src, dst, 1, 2.2);

            return dst;
        }
    }

    public class ImageProcNearestNeighbor : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = null;
            ImageOperator.NearestNeighbor(src, ref dst, 1.5, 1.5);

            return dst;
        }
    }

    public class ImageProcBilinear : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = null;
            ImageOperator.Bilinear(src, ref dst, 1.5, 1.5);

            return dst;
        }
    }

    public class ImageProcBicubic : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = null;
            ImageOperator.Bicubic(src, ref dst, 1.5, 1.5);

            return dst;
        }
    }

    public class ImageProcAffineTrans : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = null;
            ImageOperator.Affine(src, ref dst, 1, 0, 0, 1, 30, -30);

            return dst;
        }
    }

    public class ImageProcAffineResize : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = null;
            ImageOperator.Affine(src, ref dst, 1.3, 0, 0, 0.8, 0, 0);

            return dst;
        }
    }

    public class ImageProcAffineRotation : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = null;
            ImageOperator.Affine(src, ref dst, 1, 0, 0, 1, 0, 0, -30);

            return dst;
        }
    }

    public class ImageProcAffineSkew : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = null;
            ImageOperator.Affine(src, ref dst, 1, 0, 0, 1, 0, 0, 0, 30, 30);

            return dst;
        }
    }

    public class ImageProcFourier : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap dst = (Bitmap)src.Clone();
            FourierClass fourier = ImageOperator.Dft(src);
            ImageOperator.IDft(dst, fourier);

            return dst;
        }
    }

    public class ImageProcLowPass : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            FourierClass fourier = ImageOperator.Dft(tmp);
            ImageOperator.Lpf(ref fourier, 0.5);
            ImageOperator.IDft(dst, fourier);

            return dst;
        }
    }

    public class ImageProcHighPass : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            FourierClass fourier = ImageOperator.Dft(tmp);
            ImageOperator.Hpf(ref fourier, 0.1);
            ImageOperator.IDft(dst, fourier);

            return dst;
        }
    }

    public class ImageProcBandPass : Product
    {
        public override Bitmap Action(Bitmap src)
        {
            Bitmap tmp = null;
            Bitmap dst = null;
            if (src.GetBitCount() >= 24)
            {
                tmp = ImageOperator.CvtColor(src, ImageOperator.COLOR_BGR2GRAY);
                dst = (Bitmap)tmp.Clone();
            }
            else
            {
                tmp = (Bitmap)src.Clone();
                dst = (Bitmap)src.Clone();
            }
            FourierClass fourier = ImageOperator.Dft(tmp);
            ImageOperator.Bpf(ref fourier, 0.1, 0.5);
            ImageOperator.IDft(dst, fourier);

            return dst;
        }
    }
}
