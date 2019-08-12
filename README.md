# Not maintained
As the features in the repo have been merged in to the awesome [PancakeView](https://github.com/sthewissen/Xamarin.Forms.PancakeView), it will no longer be maintained. All the future development will be done directly on PancakeView

# Xamarin.Forms.PolygonFrame
Regular Polygonal Frame for Xamarin Forms with rounded corners and shadows

![](https://img.shields.io/nuget/v/Xamarin.Forms.PolygonFrame?style=for-the-badge)

## How to use it?

The project is up on NuGet at the following URL:

https://www.nuget.org/packages/Xamarin.Forms.PolygonFrame

Adds the nuget to all the projects and just use!

```xaml
<ContentPage xmlns="http://xamarin.com/schemas/2014/forms" xmlns:x="http://schemas.microsoft.com/winfx/2009/xaml"  xmlns:polygon="clr-namespace:Xamarin.Forms.PolygonFrame;assembly=Xamarin.Forms.PolygonFrame">
   ...
   <polygon:PolygonFrame BackgroundColor="DodgerBlue" CornerRadius="30" HasShadow="True" HeightRequest="200">
      <Label Text="X" TextColor="White" FontSize="80" FontAttributes="Bold" TextColor="White" HorizontalOptions="Center" VerticalTextAlignment="Center" />
   </polygon:PolygonFrame>
   ...
</ContentPage>
```

### What can I do with it?

| Property | What it does | Extra info |
| ------ | ------ | ------ |
| `CornerRadius` | A `float` representing the edge radius  | Polygon side length will be calculated based on the radius |
| `HasShadow` | Whether or not to draw a shadow beneath the control. | Works perfectly for iOS. On Android, works only when corner radius is not set |
| `Sides` | A `int` representing number of sides of the polygon | Default is `6` |
| `OffsetAngle` | A `float` representing starting angle of the polygon | In `degrees` |


## Sample
Android                   |  iOS
:-------------------------:|:-------------------------:
<img src="https://github.com/shanranm/Xamarin.Forms.PolygonFrame/blob/master/images/droid-screenshot.jpeg" width="400px" />  |  <img src="https://github.com/shanranm/Xamarin.Forms.PolygonFrame/blob/master/images/ios-screenshot.png" width="400px" />

## License

This project is shamelessly copied from Steven Thewissen [PancakeView](https://github.com/sthewissen/Xamarin.Forms.PancakeView) :)
