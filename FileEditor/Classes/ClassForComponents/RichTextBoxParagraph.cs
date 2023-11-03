using System;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using MailKit;
using Microsoft.Xaml.Behaviors.Layout;
using Point = System.Windows.Point;

namespace FileEditor.Classes.ClassForComponents
{
    public class RichTextBoxParagraph : IDisposable
    {
        public RichTextBox richTextBox;
        public static Canvas Canvas;
        private AdornerReSizeLocation objectiv;
        public static List<RichTextBoxParagraph> TextBoxParagraphs;
        public static ComboBox ComboBox;
        private bool IsAdded;

        public void CreateRichTextBox(System.Windows.Point location)
        {
            richTextBox = new RichTextBox();
            richTextBox.Width = 300;
            richTextBox.Height = 25;
            richTextBox.Style = null;
            richTextBox.FontFamily = new System.Windows.Media.FontFamily("Times New Roman");
            richTextBox.FontSize = 12;
            richTextBox.PreviewMouseDown += RichTextBox_MouseDown;
            richTextBox.LostFocus += RichTextBox_LostFocus;
            richTextBox.PreviewKeyDown += RichTextBox_PreviewKeyDown;
            Canvas.SetLeft(richTextBox, location.X);
            Canvas.SetTop(richTextBox, location.Y);

            ValidateLocatoin();

            Canvas.Children.Add(richTextBox);
            objectiv = new AdornerReSizeLocation(richTextBox, Canvas);
            TextBoxParagraphs.Add(this);
            IsAdded = false;
        }

        private void ValidateLocatoin()
        {
            double canvasLeft = 0;
            double canvasTop = 0;
            double canvasWidth = Canvas.ActualWidth; 
            double canvasHeight = Canvas.ActualHeight; 

            if (Canvas.GetLeft(richTextBox) < canvasLeft)
            {
                Canvas.SetLeft(richTextBox, canvasLeft);
            }

            if (Canvas.GetTop(richTextBox) < canvasTop)
            {
                Canvas.SetTop(richTextBox, canvasTop);
            }

            if (Canvas.GetLeft(richTextBox)+ richTextBox.Width > canvasWidth)
            {
                Canvas.SetLeft(richTextBox, canvasWidth-richTextBox.Width);
            }

            if (Canvas.GetTop(richTextBox)+richTextBox.Height > canvasHeight)
            {
                Canvas.SetTop(richTextBox, canvasHeight-richTextBox.Height-20);
            }
        }

        private void RichTextBox_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (!IsAdded)
            {
                AdornerLayer.GetAdornerLayer(Canvas).Add(objectiv);
                IsAdded = true;
            }
            foreach (var VARIABLE in TextBoxParagraphs)
            {
                VARIABLE.GetAborner().AbornerVisibility(Visibility.Collapsed);
            }

            ComboBox.SelectedItem = Convert.ToInt32(richTextBox.FontSize);
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

        public AdornerReSizeLocation GetAborner()
        {
            return objectiv;
        }

        private void RichTextBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Delete)
            {
                AdornerLayer.GetAdornerLayer(Canvas).Remove(objectiv);
                TextBoxParagraphs.Remove(this);
                Canvas.Children.Remove(richTextBox);
                richTextBox.LostFocus -= RichTextBox_LostFocus;
                richTextBox = null;
                objectiv = null;
                this.Dispose();
            }

            else if (e.Key == Key.Enter)
            {

            }
            else
            {

            }
        }

        public void Dispose()
        {

        }
    }
}
