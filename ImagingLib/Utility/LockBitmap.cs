using System;
using System.Drawing;
using System.Drawing.Imaging;

namespace ImagingLib
{
    public static partial class ImageOperator
    {
        /// <summary>
        /// Bitmapのロック～アンロックを行うクラス
        /// </summary>
        public class LockBitmap : IDisposable
        {
            /// <summary>
            /// Bitmapのロック～アンロックを行います
            /// </summary>
            /// <param name="bmp">ロック、アンロックを行うBitmapクラスオブジェクト</param>
            public LockBitmap(Bitmap bmp) : this(bmp, new Rectangle(0, 0, bmp.Width, bmp.Height))
            {
            }

            /// <summary>
            /// 範囲を指定してBitmapのロック～アンロックを行います
            /// </summary>
            /// <param name="bmp">ロック、アンロックを行うBitmapクラスオブジェクト</param>
            /// <param name="rect">ロック範囲をRectangleで指定します。</param>
            public LockBitmap(Bitmap bmp, Rectangle rect)
            {
                // プロパティの代入
                _bitmap = bmp;
                this.Width = bmp.Width;
                this.Height = bmp.Height;
                this.PixelFormat = bmp.PixelFormat;
                this.BitCount = Bitmap.GetPixelFormatSize(this.PixelFormat);
                this.Channel = this.BitCount / 8;

                this.Rectangle = rect;

                // Bitmapをロック
                _bitmapData = bmp.LockBits(
                        this.Rectangle,
                        System.Drawing.Imaging.ImageLockMode.ReadWrite,
                        this.PixelFormat
                    );

                this.Stride = Math.Abs(_bitmapData.Stride);

                this.Scan0 = _bitmapData.Scan0;
            }

            /// <summary>
            /// Dispose
            /// </summary>
            public void Dispose()
            {
                if ((_bitmap == null) || (_bitmapData == null)) return;

                // アンロック
                _bitmap.UnlockBits(_bitmapData);
                _bitmapData = null;
                _bitmap = null;
            }

            /// <summary>
            /// ファイナライザ
            /// </summary>
            ~LockBitmap()
            {
                Dispose();
            }

            private Bitmap _bitmap;
            /// <summary>
            /// ロックしているBitmapクラスオブジェクトを取得します。
            /// </summary>
            public Bitmap Bitmap
            {
                get
                {
                    return _bitmap;
                }
            }

            private BitmapData _bitmapData;
            /// <summary>
            /// ロック中のBitmapDataクラスオブジェクトを取得します。
            /// </summary>
            public BitmapData BitmapData
            {
                get
                {
                    return _bitmapData;
                }
            }

            /// <summary>
            /// 画像の幅を取得します。
            /// </summary>
            public int Width { get; }
            /// <summary>
            /// 画像の高さを取得します。
            /// </summary>
            public int Height { get; }
            /// <summary>
            /// 画像のビット数を取得します。
            /// </summary>
            public int BitCount { get; }
            /// <summary>
            /// 画像のPixelFormatを取得します。
            /// </summary>
            public PixelFormat PixelFormat { get; }
            /// <summary>
            /// 画像のチャンネル数を取得します。
            /// </summary>
            public int Channel { get; }
            /// <summary>
            /// 画像１行あたりのバイト数を取得します。
            /// </summary>
            public int Stride { get; }
            /// <summary>
            /// 画像データの先頭ポインタを取得します。
            /// </summary>
            public IntPtr Scan0 { get; }
            /// <summary>
            /// ロックした領域を取得します。
            /// </summary>
            public Rectangle Rectangle { get; }

            /// <summary>
            /// 輝度値を取得設定するインデクサ
            /// </summary>
            /// <param name="y">画像のY座標</param>
            /// <param name="x">画像のX座標</param>
            /// <returns></returns>
            public byte this[int y, int x]
            {
                set
                {
                    unsafe
                    {
                        var ptr = (byte*)this.Scan0;
                        ptr[x + y * this.Stride] = value;
                    }
                }
                get
                {
                    unsafe
                    {
                        var ptr = (byte*)this.Scan0;
                        return ptr[x + y * this.Stride];
                    }
                }
            }

            /// <summary>
            /// 輝度値を取得設定するインデクサ
            /// </summary>
            /// <param name="y">画像のY座標</param>
            /// <param name="x">画像のX座標</param>
            /// <param name="ch">画像のチャンネル番号 0:B, 1:G, 2:R</param>
            /// <returns></returns>
            public byte this[int y, int x, int ch]
            {
                set
                {
                    unsafe
                    {
                        var ptr = (byte*)this.Scan0;
                        ptr[x * this.Channel + ch + y * this.Stride] = value;
                    }
                }
                get
                {
                    unsafe
                    {
                        var ptr = (byte*)this.Scan0;
                        return ptr[x * this.Channel + ch + y * this.Stride];
                    }
                }
            }

        }
    }
}
