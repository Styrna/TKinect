using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;
using GMap.NET.WindowsPresentation;
using System.Diagnostics;

namespace Gui.CustomMarkers
{
   /// <summary>
   /// Interaction logic for CustomMarkerDemo.xaml
   /// </summary>
   public partial class CustomMarkerDemo
   {
      Popup Popup;
      Label Label;
      GMapMarker Marker;

      public CustomMarkerDemo(GMapMarker marker, string title)
      {
         this.InitializeComponent();
         this.Marker = marker;

         Popup = new Popup();
         Label = new Label();

         this.Unloaded += new RoutedEventHandler(CustomMarkerDemo_Unloaded);
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

      void CustomMarkerDemo_Unloaded(object sender, RoutedEventArgs e)
      {
         this.Unloaded -= new RoutedEventHandler(CustomMarkerDemo_Unloaded);
         this.Loaded -= new RoutedEventHandler(CustomMarkerDemo_Loaded);

         Marker.Shape = null;
         icon.Source = null;
         icon = null;
         Popup = null;
         Label = null;         
      }

   }
}