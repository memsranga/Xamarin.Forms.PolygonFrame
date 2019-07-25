// Heavily ported from - https://github.com/sthewissen/Xamarin.Forms.PancakeView

using System;
using System.ComponentModel;
using Android.Content;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Xamarin.Forms.PolygonFrame;
using Xamarin.Forms.PolygonFrame.Droid;
using ACanvas = Android.Graphics.Canvas;

[assembly: ExportRenderer(typeof(PolygonFrame), typeof(PolygonFrameRenderer))]
namespace Xamarin.Forms.PolygonFrame.Droid
{
    public class PolygonFrameRenderer : VisualElementRenderer<ContentView>
    {
        private bool _disposed;

        public PolygonFrameRenderer(Context context) : base(context) { }

        public static void Init()
        {
            var ignore1 = typeof(PolygonFrameRenderer);
            var ignore2 = typeof(PolygonFrame);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<ContentView> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement == null || e.OldElement != null)
            {
                return;
            }

            var polygonFrame = e.NewElement as PolygonFrame;
            if (polygonFrame.Content == null)
            {
                polygonFrame.Content = new Grid();
            }

            UpdateBackground();

            if (polygonFrame.HasShadow)
            {
                Elevation = 10;
                TranslationZ = 10;

                if (polygonFrame.CornerRadius == 0)
                {
                    OutlineProvider = new PolygonOutlineProvider(polygonFrame);
                    ClipToOutline = true;
                }
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName ||
                e.PropertyName == PolygonFrame.CornerRadiusProperty.PropertyName)
            {
                UpdateBackground();
            }
        }

        private void UpdateBackground()
        {
            this.SetBackground(new PolygonDrawable(Element as PolygonFrame));
        }

        protected override bool DrawChild(ACanvas canvas, global::Android.Views.View child, long drawingTime)
        {
            if (Element == null)
            {
                return false;
            }

            var control = Element as PolygonFrame;

            SetClipChildren(true);
            using (var path = PolygonUitls.RegularPolygonPath(Width, Height, control.Sides, control.CornerRadius, 0, control.OffsetAngle))
            {
                canvas.Save();
                canvas.ClipPath(path);
            }

            var result = base.DrawChild(canvas, child, drawingTime);
            canvas.Restore();
            return result;
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing && !_disposed)
            {
                Background.Dispose();
                _disposed = true;
            }
        }
    }
}