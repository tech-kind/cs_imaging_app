using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Media.Imaging;

namespace CsImagingApp.Model
{
    public enum EnumProcMode
    {
        ChannelCvt = 1,
        GrayScale,
        Threshold,
        ThreshOtsu,
        HsvCvt,
        Quantization,
        AveragePooling,
        MaxPooling,
        GaunssianFilter,
        MedianFilter,
        MeanFilter,
        MotionFilter,
        MaxMinFilter,
        DiffFilterVertical,
        DiffFilterHorizontal,
        PrewittFilterVertical,
        PrewittFilterHorizontal,
        SobelFilterVertical,
        SobelFilterHorizontal,
        LaplacianFilter,
        EmbossFilter,
        LogFilter,
        HistogramNormarization,
        HistogramTransform,
        HistogramEqualization,
        GammaCorrection,
        NearestNeighbor,
        Bilinear,
        Bicubic,
        AffineTrans,
        AffineResize,
        AffineRotation,
        AffineSkew,
        Fourier,
        LowPass,
        HighPass,
        BandPass,
    }

    public class MainWindowModel : IDisposable
    {
        /// <summary>
        /// 初期化
        /// </summary>
        public MainWindowModel()
        {
            FilePath = "";

            ProcMode = GetDictionaryFromQ1ToQ10();
            SelectMode = ProcMode.First().Key;
        }

        /// <summary>
        /// リソースの破棄
        /// </summary>
        public void Dispose()
        {
            if (bitmap != null) bitmap.Dispose();
        }

        /// <summary>
        /// 画像ファイルパス
        /// </summary>
        public string FilePath { get; set; }

        /// <summary>
        /// 元画像（表示用リソース）
        /// </summary>
        public BitmapSource Src { get; set; }

        /// <summary>
        /// 画像処理後画像（表示用リソース）
        /// </summary>
        public BitmapSource Proc { get; set; }

        /// <summary>
        /// 元画像
        /// </summary>
        public Bitmap bitmap { get; set; }

        /// <summary>
        /// 画像処理モード
        /// </summary>
        public Dictionary<EnumProcMode, string> ProcMode { get; set; }

        /// <summary>
        /// 選択中画像処理モード
        /// </summary>
        public EnumProcMode SelectMode { get; set; }

        /// <summary>
        /// 処理時間
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// Q1からQ10までの処理モードを取得
        /// </summary>
        /// <returns></returns>
        public Dictionary<EnumProcMode, string> GetDictionaryFromQ1ToQ10()
        {
            Dictionary<EnumProcMode, string> dic = new Dictionary<EnumProcMode, string>()
            {
                {EnumProcMode.ChannelCvt, "チャンネル変換" },
                {EnumProcMode.GrayScale, "グレースケール" },
                {EnumProcMode.Threshold, "二値化" },
                {EnumProcMode.ThreshOtsu, "大津の二値化" },
                {EnumProcMode.HsvCvt, "HSV変換" },
                {EnumProcMode.Quantization, "減色処理" },
                {EnumProcMode.AveragePooling, "平均プーリング" },
                {EnumProcMode.MaxPooling, "Maxプーリング" },
                {EnumProcMode.GaunssianFilter, "ガウシアンフィルタ" },
                {EnumProcMode.MedianFilter, "メディアンフィルタ" }
            };

            return dic;
        }

        /// <summary>
        /// Q11からQ20までの処理モードを取得
        /// </summary>
        /// <returns></returns>
        public Dictionary<EnumProcMode, string> GetDictionaryFromQ11ToQ20()
        {
            Dictionary<EnumProcMode, string> dic = new Dictionary<EnumProcMode, string>()
            {
                {EnumProcMode.MeanFilter, "平滑化フィルタ" },
                {EnumProcMode.MotionFilter, "モーションフィルタ" },
                {EnumProcMode.MaxMinFilter, "Max-Minフィルタ" },
                {EnumProcMode.DiffFilterVertical, "微分フィルタ（縦）" },
                {EnumProcMode.DiffFilterHorizontal, "微分フィルタ（横）" },
                {EnumProcMode.PrewittFilterVertical, "Prewittフィルタ（縦）" },
                {EnumProcMode.PrewittFilterHorizontal, "Prewittフィルタ（横）" },
                {EnumProcMode.SobelFilterVertical, "Sobelフィルタ（縦）" },
                {EnumProcMode.SobelFilterHorizontal, "Sobelフィルタ（横）" },
                {EnumProcMode.LaplacianFilter, "Laplacianフィルタ" },
                {EnumProcMode.EmbossFilter, "Embossフィルタ" },
                {EnumProcMode.LogFilter, "Logフィルタ" }
            };

            return dic;
        }

        /// <summary>
        /// Q21からQ30までの処理モードを取得
        /// </summary>
        /// <returns></returns>
        public Dictionary<EnumProcMode, string> GetDictionaryFromQ21ToQ30()
        {
            Dictionary<EnumProcMode, string> dic = new Dictionary<EnumProcMode, string>()
            {
                {EnumProcMode.HistogramNormarization, "ヒストグラム正規化" },
                {EnumProcMode.HistogramTransform, "ヒストグラム操作" },
                {EnumProcMode.HistogramEqualization, "ヒストグラム平坦化" },
                {EnumProcMode.GammaCorrection, "ガンマ補正" },
                {EnumProcMode.NearestNeighbor, "最近傍補間" },
                {EnumProcMode.Bilinear, "Bi-linear補間" },
                {EnumProcMode.Bicubic, "Bi-cubic補間" },
                {EnumProcMode.AffineTrans, "アフィン変換（平行移動）" },
                {EnumProcMode.AffineResize, "アフィン変換（拡大縮小）" },
                {EnumProcMode.AffineRotation, "アフィン変換（回転）" },
            };

            return dic;
        }

        /// <summary>
        /// Q31からQ40までの処理モードを取得
        /// </summary>
        /// <returns></returns>
        public Dictionary<EnumProcMode, string> GetDictionaryFromQ31ToQ40()
        {
            Dictionary<EnumProcMode, string> dic = new Dictionary<EnumProcMode, string>()
            {
                {EnumProcMode.AffineSkew, "アフィン変換（スキュー）" },
                {EnumProcMode.Fourier, "フーリエ変換" },
                {EnumProcMode.LowPass, "ローパスフィルタ" },
                {EnumProcMode.HighPass, "ハイパスフィルタ" },
                {EnumProcMode.BandPass, "バンドパスフィルタ" },
            };

            return dic;
        }
    }
}
