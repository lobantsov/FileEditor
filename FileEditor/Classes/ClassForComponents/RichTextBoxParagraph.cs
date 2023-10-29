using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Media;
using Microsoft.Xaml.Behaviors.Layout;

namespace FileEditor.Classes.ClassForComponents
{
    public class RichTextBoxParagraph : IDisposable
    {
        private RichTextBox richTextBox;
        public static Canvas Canvas;
        private ResizeAborner objectiv;
        private Adorner a;

        public void CreateRichTextBox(System.Windows.Point location)
        {
            richTextBox = new RichTextBox();
            richTextBox.Width = 300;
            richTextBox.Height = 25;
            richTextBox.Style = null;
            richTextBox.PreviewMouseDown += RichTextBox_MouseDown;
            richTextBox.LostFocus += RichTextBox_LostFocus;
            Canvas.SetLeft(richTextBox, location.X);
            Canvas.SetTop(richTextBox, location.Y);
            Canvas.Children.Add(richTextBox);
        }

        private void RichTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            objectiv = new ResizeAborner(richTextBox);
            objectiv.AbornerVisibility(Visibility.Visible);
            AdornerLayer.GetAdornerLayer(Canvas).Add(objectiv);

        }

        private void RemoveAdornerAll()
        {
            objectiv = null;
            AdornerLayer adornerLayer = AdornerLayer.GetAdornerLayer(Canvas);

            Adorner[] adorners = adornerLayer.GetAdorners(Canvas);
            if (adorners != null)
            {
                foreach (Adorner adorner in adorners)
                {
                    adornerLayer.Remove(adorner);
                }
            }
        }

        private void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            AdornerLayer.GetAdornerLayer(Canvas).Remove(objectiv);
        }

        public RichTextBox GetRichTextBox()
        {
            return richTextBox;
        }

        public Adorner GetAdorner()
        {
            return objectiv;
        }

        public void Dispose()
        {
            richTextBox = null;
        }
    }
}
