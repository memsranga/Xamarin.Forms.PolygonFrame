// Heavily ported from - https://github.com/sthewissen/Xamarin.Forms.PancakeView

using Android.Graphics;
using Android.Views;

namespace Xamarin.Forms.PolygonFrame.Droid
{
    public class PolygonOutlineProvider : ViewOutlineProvider
    {
        private readonly PolygonFrame _hexView;

        public PolygonOutlineProvider(PolygonFrame hexView)
        {
            _hexView = hexView;
        }

        public override void GetOutline(global::Android.Views.View view, Outline outline)
        {
            // scale based on height
            var cornerRadius = (view.Width / _hexView.Width) * _hexView.CornerRadius;
            var offsetAngle = _hexView.OffsetAngle;
            var hexPath = PolygonUitls.RegularPolygonPath(view.Width, view.Height, _hexView.Sides, cornerRadius, 0, offsetAngle);
            if (hexPath.IsConvex)
            {
                outline.SetConvexPath(hexPath);
            }
        }
    }
}
