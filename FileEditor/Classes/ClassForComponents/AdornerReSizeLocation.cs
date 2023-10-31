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
using System.Windows.Input;
using System.Windows.Media;
using Point = System.Windows.Point;
using Rectangle = System.Windows.Shapes.Rectangle;
using Size = System.Windows.Size;

namespace FileEditor.Classes.ClassForComponents
{
    public class AdornerReSizeLocation : Adorner
    {
        private VisualCollection VisualCollection;
        private Thumb ThumbLeftTop;
        private Thumb ThumbRightBottom;
        private Thumb ThumbResize;
        private Rectangle rectangle;
        public AdornerReSizeLocation(UIElement adornerUiElement) : base(adornerUiElement)
        {
            VisualCollection = new VisualCollection(this);
            ThumbLeftTop = new Thumb() { Background = Brushes.Coral, Height = 10, Width = 10 };
            ThumbRightBottom = new Thumb() {Background = Brushes.Coral, Height = 10, Width = 10};
            ThumbResize = new Thumb() {Background = Brushes.Coral, Height = 10, Width = 10};

            #region Events

            ThumbLeftTop.DragDelta += ThumbLeftTop_DragDelta;
            ThumbRightBottom.DragDelta += ThumbRightBottom_DragDelta;
            ThumbResize.DragDelta += ThumbResize_DragDelta;

            ThumbLeftTop.MouseEnter += ThumbLeftTop_MouseEnter;
            ThumbRightBottom.MouseEnter += ThumbLeftTop_MouseEnter;
            ThumbResize.MouseEnter += ThumbResize_MouseEnter;

            ThumbLeftTop.MouseLeave += ThumbLeftTop_MouseLeave;
            ThumbRightBottom.MouseLeave += ThumbLeftTop_MouseLeave;
            ThumbResize.MouseLeave += ThumbLeftTop_MouseLeave;

            #endregion

            rectangle = new Rectangle() { Stroke = Brushes.Coral, StrokeThickness = 2, StrokeDashArray = { 3, 2 } };

            VisualCollection.Add(ThumbLeftTop);
            VisualCollection.Add(ThumbRightBottom);
            VisualCollection.Add(ThumbResize);
            VisualCollection.Add(rectangle);

            AbornerVisibility(Visibility.Collapsed);
        }

        private void ThumbLeftTop_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = null;
        }

        private void ThumbResize_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.Hand;
        }
        private void ThumbLeftTop_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            Mouse.OverrideCursor = Cursors.SizeNWSE;
        }

        private void ThumbResize_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var element = (FrameworkElement)AdornedElement;

            Canvas.SetLeft(element,Canvas.GetLeft(element)+e.HorizontalChange);
            Canvas.SetTop(element,Canvas.GetTop(element)+e.VerticalChange);
        }

        private void ThumbRightBottom_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var element = (FrameworkElement)AdornedElement;

            //element.Height = element.Height + e.VerticalChange < 0 ? 0 : element.Height + e.VerticalChange;
            element.Width = element.Width + e.HorizontalChange < 0 ? 0 : element.Width + e.HorizontalChange;
        }

        private void ThumbLeftTop_DragDelta(object sender, DragDeltaEventArgs e)
        {
            var element = (FrameworkElement)AdornedElement;
            if(element.Width>0)
            Canvas.SetLeft(element,Canvas.GetLeft(element)+e.HorizontalChange);
            //if(element.Height>0)
            //Canvas.SetTop(element,Canvas.GetTop(element)+e.VerticalChange);

            Point j = new Point(element.Width, element.Width);

            //element.Height = element.Height - e.VerticalChange < 0 ? 0 : element.Height - e.VerticalChange;
            element.Width = element.Width - e.HorizontalChange < 0 ? 0 : element.Width - e.HorizontalChange;
        }

        protected override Visual GetVisualChild(int index)
        {
            return VisualCollection[index];
        }

        protected override int VisualChildrenCount => VisualCollection.Count;

        protected override Size ArrangeOverride(Size finalSize)
        {
            ThumbLeftTop.Arrange(new Rect(-10, -10, 10, 10));
            ThumbRightBottom.Arrange(new Rect(AdornedElement.DesiredSize.Width, AdornedElement.DesiredSize.Height, 10, 10));
            ThumbResize.Arrange(new Rect(AdornedElement.DesiredSize.Width / 2, AdornedElement.DesiredSize.Height + 10, 10, 10));
            rectangle.Arrange(new Rect(-5, -5, AdornedElement.DesiredSize.Width + 10, AdornedElement.DesiredSize.Height + 10));
            return base.ArrangeOverride(finalSize);
        }

        public void AbornerVisibility(Visibility visibleMode)
        {
            ThumbResize.Visibility = visibleMode;
            ThumbRightBottom.Visibility = visibleMode;
            ThumbLeftTop.Visibility = visibleMode;
            rectangle.Visibility = visibleMode;
        }

        public Visibility GetAdornerdVisibility()
        {
            return ThumbResize.Visibility;
        }
    }
}
