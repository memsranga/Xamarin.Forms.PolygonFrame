// Heavily ported from - https://github.com/sthewissen/Xamarin.Forms.PancakeView

namespace Xamarin.Forms.PolygonFrame
{
    public class PolygonFrame : ContentView
    {
        #region properties

        public static readonly BindableProperty SidesProperty = BindableProperty.Create(nameof(Sides), typeof(int), typeof(PolygonFrame), 6);
        public static readonly BindableProperty CornerRadiusProperty = BindableProperty.Create(nameof(CornerRadius), typeof(float), typeof(PolygonFrame), default(float));
        public static readonly BindableProperty HasShadowProperty = BindableProperty.Create(nameof(HasShadow), typeof(bool), typeof(PolygonFrame), default(bool));
        public static readonly BindableProperty OffsetAngleProperty = BindableProperty.Create(nameof(OffsetAngle), typeof(float), typeof(PolygonFrame), default(float));

        public float OffsetAngle
        {
            get { return (float)GetValue(OffsetAngleProperty); }
            set { SetValue(OffsetAngleProperty, value); }
        }

        public int Sides
        {
            get { return (int)GetValue(SidesProperty); }
            set { SetValue(SidesProperty, value); }
        }

        public float CornerRadius
        {
            get { return (float)GetValue(CornerRadiusProperty); }
            set { SetValue(CornerRadiusProperty, value); }
        }

        public bool HasShadow
        {
            get { return (bool)GetValue(HasShadowProperty); }
            set { SetValue(HasShadowProperty, value); }
        }
        #endregion
    }
}
