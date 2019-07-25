// Heavily ported from - https://github.com/sthewissen/Xamarin.Forms.PancakeView

using System;
using Android.Graphics;
using Android.Graphics.Drawables;
using Xamarin.Forms.Platform.Android;
using ACanvas = Android.Graphics.Canvas;

namespace Xamarin.Forms.PolygonFrame.Droid
{
    public class PolygonDrawable : Drawable
    {
        private readonly PolygonFrame _polygonFrame;
        private Bitmap _normalBitmap;
        private bool _isDisposed;

        public override int Opacity => 0;

        public PolygonDrawable(PolygonFrame polygonFrame)
        {
            _polygonFrame = polygonFrame;
            _polygonFrame.PropertyChanged += PolygonFramePropertyChanged;
        }

        private void PolygonFramePropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == PolygonFrame.CornerRadiusProperty.PropertyName)
            {
                if (_normalBitmap == null)
                {
                    return;
                }

                using (var canvas = new ACanvas(_normalBitmap))
                {
                    int width = Bounds.Width();
                    int height = Bounds.Height();
                    canvas.DrawColor(global::Android.Graphics.Color.Black, PorterDuff.Mode.Clear);
                    DrawCanvas(canvas, width, height, false);
                }

                InvalidateSelf();
            }
        }

        public override void Draw(ACanvas canvas)
        {
            int width = Bounds.Width();
            int height = Bounds.Height();

            if (width <= 0 || height <= 0)
            {
                if (_normalBitmap != null)
                {
                    _normalBitmap.Dispose();
                    _normalBitmap = null;
                }
                return;
            }

            if (_normalBitmap == null || _normalBitmap.Height != height || _normalBitmap.Width != width)
            {
                if (_normalBitmap != null)
                {
                    _normalBitmap.Dispose();
                    _normalBitmap = null;
                }

                _normalBitmap = CreateBitmap(false, width, height);
            }

            var bitmap = _normalBitmap;

            using (var paint = new Paint { AntiAlias = true })
            {
                canvas.DrawBitmap(bitmap, 0, 0, paint);
            }
        }

        public override void SetAlpha(int alpha) { }

        public override void SetColorFilter(ColorFilter colorFilter) { }

        protected override bool OnStateChange(int[] state)
        {
            return false;
        }

        private Bitmap CreateBitmap(bool pressed, int width, int height)
        {
            Bitmap bitmap;

            using (Bitmap.Config config = Bitmap.Config.Argb8888)
            {
                bitmap = Bitmap.CreateBitmap(width, height, config);
            }

            using (var canvas = new ACanvas(bitmap))
            {
                DrawCanvas(canvas, width, height, pressed);
            }

            return bitmap;
        }

        private void DrawBackground(ACanvas canvas, int width, int height, CornerRadius cornerRadius, bool pressed)
        {
            using (var paint = new Paint { AntiAlias = true })
            using (var path = PolygonUitls.RegularPolygonPath(width, height, _polygonFrame.Sides, _polygonFrame.CornerRadius, 0, _polygonFrame.OffsetAngle))
            using (Paint.Style style = Paint.Style.Fill)
            {
                global::Android.Graphics.Color color = _polygonFrame.BackgroundColor.ToAndroid();
                paint.SetStyle(style);
                paint.Color = color;

                canvas.DrawPath(path, paint);
            }
        }

        private void DrawCanvas(ACanvas canvas, int width, int height, bool pressed)
        {
            DrawBackground(canvas, width, height, _polygonFrame.CornerRadius, pressed);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && !_isDisposed)
            {
                if (_normalBitmap != null)
                {
                    _normalBitmap.Dispose();
                    _normalBitmap = null;
                }

                _isDisposed = true;
            }

            base.Dispose(disposing);
        }
    }
}