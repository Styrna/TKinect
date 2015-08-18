using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;

namespace Gui.CustomMarkers
{
   /// <summary>
   /// Interaction logic for CustomMarkerDemo.xaml
   /// </summary>
   public partial class CustomMarkerRed
   {
      Popup Popup;
      Label Label;
      GMapMarker Marker;

      public CustomMarkerRed(GMapMarker marker, string title)
      {
         this.InitializeComponent();

         this.Marker = marker;

         Popup = new Popup();
         Label = new Label();

         this.Loaded += new RoutedEventHandler(CustomMarkerDemo_Loaded);

         Popup.Placement = PlacementMode.Mouse;
         {
            Label.Background = Brushes.Blue;
            Label.Foreground = Brushes.White;
            Label.BorderBrush = Brushes.WhiteSmoke;
            Label.BorderThickness = new Thickness(2);
            Label.Padding = new Thickness(5);
            Label.FontSize = 22;
            Label.Content = title;
         }
         Popup.Child = Label;
      }

      void CustomMarkerDemo_Loaded(object sender, RoutedEventArgs e)
      {
         if(icon.Source.CanFreeze)
         {
            icon.Source.Freeze();
         }
      }
   }
}