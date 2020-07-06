using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CsImagingApp.Model;

namespace CsImagingApp.Factory
{
    public class ImageProcFactory : AbstractFactory
    {
        /// <summary>
        /// 画像処理モードに合わせてクラスを生成
        /// </summary>
        /// <param name="mode"></param>
        /// <returns></returns>
        public override Product Create(EnumProcMode mode)
        {
            Product product = null;
            switch(mode)
            {
                case EnumProcMode.ChannelCvt:
                    product = new ImageProcChannelCvt();
                    break;
                case EnumProcMode.GrayScale:
                    product = new ImageProcGrayScale();
                    break;
                case EnumProcMode.Threshold:
                    product = new ImageProcThreshold();
                    break;
                case EnumProcMode.ThreshOtsu:
                    product = new ImageProcThreshOtsu();
                    break;
                case EnumProcMode.HsvCvt:
                    product = new ImageProcHsvCvt();
                    break;
                case EnumProcMode.Quantization:
                    product = new ImageProcQuantization();
                    break;
                case EnumProcMode.AveragePooling:
                    product = new ImageProcAveragePooling();
                    break;
                case EnumProcMode.MaxPooling:
                    product = new ImageProcMaxPooling();
                    break;
                case EnumProcMode.GaunssianFilter:
                    product = new ImageProcGaussianFilter();
                    break;
                case EnumProcMode.MedianFilter:
                    product = new ImageProcMedianFilter();
                    break;
                case EnumProcMode.MeanFilter:
                    product = new ImageProcMeanFilter();
                    break;
                case EnumProcMode.MotionFilter:
                    product = new ImageProcMotionFilter();
                    break;
                case EnumProcMode.MaxMinFilter:
                    product = new ImageProcMaxMinFilter();
                    break;
                case EnumProcMode.DiffFilterVertical:
                    product = new ImageProcDiffFilterVertical();
                    break;
                case EnumProcMode.DiffFilterHorizontal:
                    product = new ImageProcDiffFilterHorizontal();
                    break;
                case EnumProcMode.PrewittFilterVertical:
                    product = new ImageProcPrewittFilterVertical();
                    break;
                case EnumProcMode.PrewittFilterHorizontal:
                    product = new ImageProcPrewittFilterHorizontal();
                    break;
                case EnumProcMode.SobelFilterVertical:
                    product = new ImageProcSobelFilterVertical();
                    break;
                case EnumProcMode.SobelFilterHorizontal:
                    product = new ImageProcSobelFilterHorizontal();
                    break;
                case EnumProcMode.LaplacianFilter:
                    product = new ImageProcLaplacianFilter();
                    break;
                case EnumProcMode.EmbossFilter:
                    product = new ImageProcEmbossFilter();
                    break;
                case EnumProcMode.LogFilter:
                    product = new ImageProcLogFilter();
                    break;
                case EnumProcMode.HistogramNormarization:
                    product = new ImageProcHistogramNormalization();
                    break;
                case EnumProcMode.HistogramTransform:
                    product = new ImageProcHistogramTransform();
                    break;
                case EnumProcMode.HistogramEqualization:
                    product = new ImageProcHistogramEqualization();
                    break;
                case EnumProcMode.GammaCorrection:
                    product = new ImageProcGammaCorrection();
                    break;
                case EnumProcMode.NearestNeighbor:
                    product = new ImageProcNearestNeighbor();
                    break;
                case EnumProcMode.Bilinear:
                    product = new ImageProcBilinear();
                    break;
                case EnumProcMode.Bicubic:
                    product = new ImageProcBicubic();
                    break;
                case EnumProcMode.AffineTrans:
                    product = new ImageProcAffineTrans();
                    break;
                case EnumProcMode.AffineResize:
                    product = new ImageProcAffineResize();
                    break;
                case EnumProcMode.AffineRotation:
                    product = new ImageProcAffineRotation();
                    break;
                case EnumProcMode.AffineSkew:
                    product = new ImageProcAffineSkew();
                    break;
                case EnumProcMode.Fourier:
                    product = new ImageProcFourier();
                    break;
                case EnumProcMode.LowPass:
                    product = new ImageProcLowPass();
                    break;
                case EnumProcMode.HighPass:
                    product = new ImageProcHighPass();
                    break;
                case EnumProcMode.BandPass:
                    product = new ImageProcBandPass();
                    break;
                default:
                    break;
            }
            return product;
        }
    }
}
