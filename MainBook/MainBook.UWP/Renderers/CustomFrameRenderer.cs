using System;
using System.ComponentModel;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using MainBook.CustomControls;
using MainBook.UWP.Renderers;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(CustomFrame), typeof(CustomFrameRenderer))]
namespace MainBook.UWP.Renderers
{
    public delegate void ChangedEventHandler(object sender, EventArgs e);

    public class CustomFrameRenderer : ViewRenderer<CustomFrame, Border>
    {
        
        public CustomFrame CustomFrame { get; set; }

        public CustomFrameRenderer()
        {
            base.AutoPackage = false;
        }

        protected override void OnElementChanged(ElementChangedEventArgs<CustomFrame> e)
        {
            base.OnElementChanged(e);

            CustomFrame = (CustomFrame)e.NewElement;
            if (CustomFrame != null)
            {
                var border = new Border { Background = new SolidColorBrush(ToMediaColor(CustomFrame.InlineColor)) };

                base.SetNativeControl(border);
                this.PackChild();

                this.UpdateBorder(CustomFrame.BorderWidth, CustomFrame.BorderRadius);
            }
        }

        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (CustomFrame != null)
            {
                if (e.PropertyName == "Content")
                {
                    this.PackChild();
                    return;
                }
                if (e.PropertyName == CustomFrame.OutlineColorProperty.PropertyName ||
                    e.PropertyName == CustomFrame.BorderWidthProperty.PropertyName)
                {
                    this.UpdateBorder(CustomFrame.BorderWidth, CustomFrame.BorderRadius);
                }
            }
        }
        private void PackChild()
        {
            if (base.Element.Content == null)
            {
                return;
            }
            Platform.SetRenderer(base.Element.Content, Platform.CreateRenderer(base.Element.Content));
            UIElement containerElement = Platform.GetRenderer(base.Element.Content).ContainerElement;
            base.Control.Child = containerElement;
        }

        private void UpdateBorder(int borderWidth, int borderRadius)
        {
            base.Control.CornerRadius = new CornerRadius(borderRadius);
            if (base.Element.OutlineColor == Color.Default)
            {
                Xamarin.Forms.Color borderColor = new Xamarin.Forms.Color(0, 0, 0, 0);
                base.Control.BorderBrush = ToBrush(borderColor);
                return;
            }
            base.Control.BorderBrush = ToBrush(Element.OutlineColor);
            base.Control.BorderThickness = new Windows.UI.Xaml.Thickness(borderWidth);
        }

        public static Brush ToBrush(Xamarin.Forms.Color color)
        {
            return new SolidColorBrush(ToMediaColor(color));
        }

        public static Windows.UI.Color ToMediaColor(Xamarin.Forms.Color color)
        {
            return Windows.UI.Color.FromArgb(((byte)(color.A * 255)), (byte)(color.R * 255), (byte)(color.G * 255), (byte)(color.B * 255));
        }
    }
}
