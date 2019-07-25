// Heavily ported from - https://github.com/sthewissen/Xamarin.Forms.PancakeView

using System;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using CoreAnimation;
using CoreGraphics;
using UIKit;
using Xamarin.Forms;
using Xamarin.Forms.PolygonFrame.iOS;
using Xamarin.Forms.Platform.iOS;
using Xamarin.Forms.PolygonFrame;

[assembly: ExportRenderer(typeof(PolygonFrame), typeof(PolygonFrameRenderer))]
namespace Xamarin.Forms.PolygonFrame.iOS
{
    public class PolygonFrameRenderer : ViewRenderer<PolygonFrame, UIView>
    {
        private UIView _actualView;
        private UIView _wrapperView;

        private UIColor _colorToRender;
        private CGSize _previousSize;

        public static new void Init()
        {
            var ignore1 = typeof(PolygonFrameRenderer);
            var ignore2 = typeof(PolygonFrame);
        }

        protected override void OnElementChanged(ElementChangedEventArgs<PolygonFrame> e)
        {
            base.OnElementChanged(e);

            if (e.NewElement != null)
            {
                _actualView = new UIView();
                _wrapperView = new UIView();

                foreach (var item in NativeView.Subviews)
                {
                    _actualView.AddSubview(item);
                }

                _wrapperView.AddSubview(_actualView);

                SetNativeControl(_wrapperView);
                SetBackgroundColor(Element.BackgroundColor);
                SetCornerRadius();
                SetOffsetAngle();
            }
        }

        private void SetOffsetAngle()
        {
            if (Element == null)
            {
                return;
            }

            SetNeedsDisplay();
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);

            if (e.PropertyName == VisualElement.BackgroundColorProperty.PropertyName)
            {
                SetBackgroundColor(Element.BackgroundColor);
                return;
            }

            if (e.PropertyName == PolygonFrame.CornerRadiusProperty.PropertyName || e.PropertyName == PolygonFrame.OffsetAngleProperty.PropertyName)
            {
                SetCornerRadius();
                return;
            }

            if (e.PropertyName == VisualElement.IsVisibleProperty.PropertyName && Element.IsVisible)
            {
                SetNeedsDisplay();
            }
        }

        protected override void SetBackgroundColor(Color color)
        {
            if (Element == null)
                return;

            var elementColor = Element.BackgroundColor;

            _colorToRender = !elementColor.IsDefault ? elementColor.ToUIColor() : color.ToUIColor();

            SetNeedsDisplay();
        }

        private void SetCornerRadius()
        {
            if (Element == null)
            {
                return;
            }

            SetNeedsDisplay();
        }

        private void DrawShadow()
        {
            var hexView = Element as PolygonFrame;

            if (hexView.HasShadow)
            {
                _wrapperView.Layer.CornerRadius = hexView.CornerRadius;
                _wrapperView.Layer.ShadowRadius = 10;
                _wrapperView.Layer.ShadowColor = UIColor.Black.CGColor;
                _wrapperView.Layer.ShadowOpacity = 0.4f;
                _wrapperView.Layer.ShadowOffset = new SizeF();
                _wrapperView.Layer.ShadowPath = RegularHexagonPath(Bounds, hexView.Sides, hexView.CornerRadius, 0, hexView.OffsetAngle).CGPath;

                _actualView.Layer.CornerRadius = hexView.CornerRadius;
                _actualView.ClipsToBounds = true;
            }
            else
            {
                _wrapperView.Layer.ShadowOpacity = 0;
            }

            _wrapperView.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            _wrapperView.Layer.ShouldRasterize = true;
            _actualView.Layer.RasterizationScale = UIScreen.MainScreen.Scale;
            _actualView.Layer.ShouldRasterize = true;
        }

        private void DrawBackground()
        {
            var hexView = Element as PolygonFrame;
            var hexPath = RegularHexagonPath(Bounds, hexView.Sides, hexView.CornerRadius, 0, hexView.OffsetAngle);

            var maskLayer = new CAShapeLayer
            {
                Frame = Bounds,
                Path = hexPath.CGPath
            };

            _actualView.Layer.Mask = maskLayer;
            _actualView.Layer.MasksToBounds = true;

            var shapeLayer = new CAShapeLayer
            {
                Frame = Bounds,
                Path = hexPath.CGPath,
                MasksToBounds = true,
                FillColor = _colorToRender.CGColor
            };

            if (_actualView.Layer.Sublayers == null || (_actualView.Layer.Sublayers != null && !_actualView.Layer.Sublayers.Any(x => x.GetType() == typeof(CAShapeLayer))))
            {
                _actualView.Layer.InsertSublayer(shapeLayer, 0);
            }
            else
            {
                var gradLayer = _actualView.Layer.Sublayers.FirstOrDefault(x => x.GetType() == typeof(CAShapeLayer));
                gradLayer?.RemoveFromSuperLayer();
                _actualView.Layer.InsertSublayer(shapeLayer, 0);
            }
        }

        public override void Draw(CGRect rect)
        {
            _actualView.Frame = Bounds;
            _wrapperView.Frame = Bounds;

            DrawBackground();
            DrawShadow();

            base.Draw(rect);

            _previousSize = Bounds.Size;
        }

        public override void LayoutSubviews()
        {
            if (_previousSize != Bounds.Size)
            {
                SetNeedsDisplay();
            }

            base.LayoutSubviews();
        }

        private static UIBezierPath RegularHexagonPath(CGRect rect, int sides, double cornerRadius = 0.0, double lineWidth = 0.0, double rotationOffset = 0.0)
        {
            var offsetRadians = rotationOffset * Math.PI / 180;
            var path = new UIBezierPath();
            var theta = 2 * Math.PI / sides;

            var width = (-cornerRadius + Math.Min(rect.Size.Width, rect.Size.Height)) / 2;
            var center = new CGPoint(rect.Width / 2, rect.Height / 2);

            var radius = width - lineWidth + cornerRadius - (Math.Cos(theta) * cornerRadius) / 2;

            var angle = offsetRadians;
            var corner = new CGPoint(center.X + (radius - cornerRadius) * Math.Cos(angle), center.Y + (radius - cornerRadius) * Math.Sin(angle));
            path.MoveTo(new CGPoint(corner.X + cornerRadius * Math.Cos(angle + theta), corner.Y + cornerRadius * Math.Sin(angle + theta)));

            for (var i = 0; i < sides; i++)
            {
                angle += theta;
                corner = new CGPoint(center.X + (radius - cornerRadius) * Math.Cos(angle), center.Y + (radius - cornerRadius) * Math.Sin(angle));
                var tip = new CGPoint(center.X + radius * Math.Cos(angle), center.Y + radius * Math.Sin(angle));
                var start = new CGPoint(corner.X + cornerRadius * Math.Cos(angle - theta), corner.Y + cornerRadius * Math.Sin(angle - theta));
                var end = new CGPoint(corner.X + cornerRadius * Math.Cos(angle + theta), corner.Y + cornerRadius * Math.Sin(angle + theta));

                path.AddLineTo(start);
                path.AddQuadCurveToPoint(end, tip);
            }
            path.ClosePath();
            return path;
        }
    }
}
