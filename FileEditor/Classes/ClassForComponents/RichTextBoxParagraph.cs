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
        public ResizeAborner objectiv;
        public static List<RichTextBoxParagraph> TextBoxParagraphs;

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
            objectiv = new ResizeAborner(richTextBox);
        }

        private void RichTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!TextBoxParagraphs.Contains(this))
            {
                AdornerLayer.GetAdornerLayer(Canvas).Add(objectiv);
                TextBoxParagraphs.Add(this);
            }
            objectiv.AbornerVisibility(Visibility.Visible);
        }

        public static void RemoveAdornerAll(RichTextBox rich)
        {
            if (rich != null)
            {
                var a = AdornerLayer.GetAdornerLayer(Canvas).GetAdorners(rich);
                if (a != null)
                {
                    foreach (var VARIABLE in a)
                    {
                        AdornerLayer.GetAdornerLayer(Canvas).Remove(VARIABLE);

                    }
                }
            }
        }

        public void RichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            objectiv.AbornerVisibility(Visibility.Collapsed);
        }

        public void SetTagRichTextBox(int i)
        {
            richTextBox.Tag = i;
        }

        public void Dispose()
        {

        }
    }
}
