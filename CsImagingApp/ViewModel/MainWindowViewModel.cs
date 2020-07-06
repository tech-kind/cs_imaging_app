using System;
using System.Collections.Generic;
using System.Windows.Input;
using CsImagingApp.Model;
using ImagingLib;
using System.Drawing;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.WindowsAPICodePack.Dialogs;
using System.Diagnostics;
using CsImagingApp.Factory;
using System.Threading.Tasks;

namespace CsImagingApp.ViewModel
{
    public class MainWindowViewModel : ViewModelBase
    {
        [System.Runtime.InteropServices.DllImport("gdi32.dll")]
        public static extern bool DeleteObject(IntPtr hObject);

        public MainWindowModel _model = new MainWindowModel();

        /// <summary>
        /// 元画像のパス
        /// </summary>
        public string FilePath
        {
            get { return _model.FilePath; }
            set
            {
                if (_model.FilePath != value)
                {
                    _model.FilePath = value;
                    OnPropertyChanged("FilePath");
                }
            }
        }

        /// <summary>
        /// 実行時間
        /// </summary>
        public string ExecTime
        {
            get { return _model.Time; }
            set
            {
                if (_model.Time != value)
                {
                    _model.Time = value;
                    OnPropertyChanged("ExecTime");
                }
            }
        }

        /// <summary>
        /// 画像処理モード
        /// </summary>
        public Dictionary<EnumProcMode, string> ProcMode
        {
            get { return _model.ProcMode; }
            set
            {
                if (_model.ProcMode != value)
                {
                    _model.ProcMode = value;
                    OnPropertyChanged("ProcMode");
                }
            }
        }

        /// <summary>
        /// 選択中画像処理モード
        /// </summary>
        public EnumProcMode SelectMode
        {
            get { return _model.SelectMode; }
            set
            {
                if (_model.SelectMode != value)
                {
                    _model.SelectMode = value;
                    OnPropertyChanged("SelectMode");
                }
            }
        }

        /// <summary>
        /// 元画像
        /// </summary>
        public BitmapSource SrcImage
        {
            get
            {
                return _model.Src;
            }
            set
            {
                if (_model.Src != value)
                {
                    _model.Src = value;
                    OnPropertyChanged("SrcImage");
                }
            }
        }

        /// <summary>
        /// 加工画像
        /// </summary>
        public BitmapSource ProcImage
        {
            get
            {
                return _model.Proc;
            }
            set
            {
                if (_model.Proc != value)
                {
                    _model.Proc = value;
                    OnPropertyChanged("ProcImage");
                }
            }
        }

        /// <summary>
        /// 画像読み込み
        /// </summary>    
        public ICommand ReadImage => new DelegateCommand(obj =>
        {
            var dialog = new CommonOpenFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("画像ファイル", "*.bmp;*.jpg;*.png"));

            if (dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                FilePath = dialog.FileName;
                _model.bitmap = ImageOperator.CreateBitmap(dialog.FileName);
                // 表示
                IntPtr hbitmap = _model.bitmap.GetHbitmap();
                SrcImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                DeleteObject(hbitmap);
            }
        });

        /// <summary>
        /// 画像処理実行
        /// </summary>
        public ICommand ProcStart => new DelegateCommand(obj =>
        {
            if (_model.bitmap == null) return;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            Bitmap dst = null;
            AbstractFactory factory = new ImageProcFactory();
            Product product = factory.Create(SelectMode);
            dst = product.Action(_model.bitmap);
            sw.Stop();

            if (dst != null)
            {
                // 表示
                IntPtr hbitmap = dst.GetHbitmap();
                ProcImage = System.Windows.Interop.Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
                ExecTime = sw.ElapsedMilliseconds.ToString() + "ms";
                DeleteObject(hbitmap);
                dst.Dispose();
            }
        });

        /// <summary>
        /// 画像処理実行
        /// </summary>
        public ICommand SaveImage => new DelegateCommand(obj =>
        {
            var dialog = new CommonSaveFileDialog();
            dialog.Filters.Add(new CommonFileDialogFilter("JPGファイル", "*.jpg"));
            dialog.Filters.Add(new CommonFileDialogFilter("PNGファイル", "*.png"));
            dialog.Filters.Add(new CommonFileDialogFilter("BMPファイル", "*.bmp"));

            if(dialog.ShowDialog() == CommonFileDialogResult.Ok)
            {
                var bitmap = new Bitmap(
                    _model.Proc.PixelWidth,
                    _model.Proc.PixelHeight,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb
                );
                var bitmapData = bitmap.LockBits(
                    new Rectangle(System.Drawing.Point.Empty, bitmap.Size),
                    System.Drawing.Imaging.ImageLockMode.WriteOnly,
                    System.Drawing.Imaging.PixelFormat.Format32bppPArgb
                );
                _model.Proc.CopyPixels(
                    Int32Rect.Empty,
                    bitmapData.Scan0,
                    bitmapData.Height * bitmapData.Stride,
                    bitmapData.Stride
                );
                bitmap.UnlockBits(bitmapData);
                bitmap.Save(dialog.FileName);
                bitmap.Dispose();
            }
        });

        /// <summary>
        /// 画像処理モード入れ替え（Q1からQ10）
        /// </summary>
        public ICommand ChangeFromQ1ToQ10 => new DelegateCommand(obj =>
        {
            ProcMode.Clear();
            ProcMode = _model.GetDictionaryFromQ1ToQ10();
            SelectMode = EnumProcMode.ChannelCvt;
        });

        /// <summary>
        /// 画像処理モード入れ替え（Q11からQ20）
        /// </summary>
        public ICommand ChangeFromQ11ToQ20 => new DelegateCommand(obj =>
        {
            ProcMode.Clear();
            ProcMode = _model.GetDictionaryFromQ11ToQ20();
            SelectMode = EnumProcMode.MeanFilter;
        });

        /// <summary>
        /// 画像処理モード入れ替え（Q21からQ30）
        /// </summary>
        public ICommand ChangeFromQ21ToQ30 => new DelegateCommand(obj =>
        {
            ProcMode.Clear();
            ProcMode = _model.GetDictionaryFromQ21ToQ30();
            SelectMode = EnumProcMode.HistogramNormarization;
        });

        /// <summary>
        /// 画像処理モード入れ替え（Q21からQ30）
        /// </summary>
        public ICommand ChangeFromQ31ToQ40 => new DelegateCommand(obj =>
        {
            ProcMode.Clear();
            ProcMode = _model.GetDictionaryFromQ31ToQ40();
            SelectMode = EnumProcMode.AffineSkew;
        });
    }
}
