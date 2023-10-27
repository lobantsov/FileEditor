using System;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using Xceed.Wpf.Toolkit;
using RichTextBox = System.Windows.Controls.RichTextBox;

namespace FileEditor.Classes.ClassForComponents
{
    public class ParagraphSetings:IDisposable
    {
        public Paragraph paragraph;
        public RichTextBox richTextBoxR;
        public static FlowDocument FlowDocument;


        public ParagraphSetings()
        {
            InitializeRichTextBox();
        }

        private void InitializeRichTextBox()
        {
            richTextBoxR = null;
            richTextBoxR = new RichTextBox();
            richTextBoxR.Style = null;
            richTextBoxR.LostFocus += RichTextBox_OnLostFocus;
        }
        public void CreateParagraph(StringBuilder lines)
        {
            if (paragraph == null)
            {
                paragraph = new Paragraph();
                paragraph.MouseDown += Paragraph_MouseDown;
            }
            FillParagraph(lines);
            FlowDocument.Blocks.Add(paragraph);
        }

        public void FillParagraph(StringBuilder lines)
        {
            paragraph.Inlines.Clear();
            string[] linesArray = lines.ToString().Split(new[] { Environment.NewLine }, StringSplitOptions.None);

            foreach (string line in linesArray)
            {
                Run runElement = new Run();
                runElement.Text = line;
                paragraph.Inlines.Add(runElement);
            }
        }

        private void Paragraph_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var clickedParagraph = sender as Paragraph;
            Block tmp = null;
            foreach (object child in FlowDocument.Blocks)
            {
                if (child is BlockUIContainer)
                {
                    tmp = (Block)child;
                }
            }

            if (tmp != null)
            {
                FlowDocument.Blocks.Remove(tmp);
                InitializeRichTextBox();
            }

            FlowDocument.Blocks.Add(new BlockUIContainer(richTextBoxR));
            if (clickedParagraph != null)
            {
                richTextBoxR.Document = new FlowDocument(new Paragraph
                    (new Run(new TextRange(clickedParagraph.ContentStart, clickedParagraph.ContentEnd).Text)));
                richTextBoxR.Visibility = Visibility.Visible;
                clickedParagraph.Inlines.Clear();
            }
        }
        private void RichTextBox_OnLostFocus(object sender, RoutedEventArgs e)
        {
                FillParagraph(new StringBuilder(new TextRange(richTextBoxR.Document.ContentStart,
                    richTextBoxR.Document.ContentEnd).Text));
            richTextBoxR.Visibility = Visibility.Collapsed;
        }

        public void Dispose() { }
    }
}
