using System;
using System.Collections.Generic;
using Microsoft.Win32;
using Xceed.Words.NET;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Linq;
using Block = System.Windows.Documents.Block;
using Run = System.Windows.Documents.Run;
using Paragraph = System.Windows.Documents.Paragraph;
using List = System.Windows.Documents.List;
using System.Windows;
using Xceed.Document.NET;
using System.Globalization;
using System.Windows.Media;
using System.Windows.Shapes;
using FormattedText = System.Windows.Media.FormattedText;

namespace FileEditor.Classes.SaveDoc
{
    public class SaveDocument
    {
        private SaveFileDialog _fileDialog;

        public SaveDocument()
        {
            _fileDialog = new SaveFileDialog();
            _fileDialog.Filter = "Word Documents|*.docx";
            _fileDialog.DefaultExt = "docx";
            _fileDialog.AddExtension = true;
        }

        public void SaveFile(List<RichTextBox> listOfRichTextBoxes)
        {
            if (_fileDialog.ShowDialog() == true)
            {
                using (DocX doc = DocX.Create(_fileDialog.FileName))
                {
                    foreach (var richText in listOfRichTextBoxes)
                    {
                        foreach (Block paragraph in richText.Document.Blocks.OfType<Block>())
                        {
                            var docXParagraph = doc.InsertParagraph();

                            if (paragraph.GetType() == typeof(System.Windows.Documents.Paragraph))
                            {
                                SetFontSetings(paragraph, docXParagraph);
                            }
                            else if (paragraph.GetType() == typeof(List))
                            {
                                foreach (var listItem in ((List)paragraph).ListItems)
                                {
                                    foreach (Block listItemBlock in listItem.Blocks)
                                    {
                                        if (listItemBlock is Paragraph)
                                        {
                                            SetFontSetings(listItemBlock,docXParagraph);
                                        }
                                    }
                                }
                            }
                            AddSpaces(docXParagraph, GetSpaceCount(richText),0);
                        }
                    }

                    doc.Save();
                }
            }
        }

        private int GetSpaceCount(RichTextBox richTextBox)
        {
            char a = ' ';
            TextRange text = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd);
            FormattedText formattedText = new FormattedText(
                a.ToString(),
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(richTextBox.FontFamily, richTextBox.FontStyle, richTextBox.FontWeight, richTextBox.FontStretch),
                richTextBox.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                TextFormattingMode.Display
            );
            return Convert.ToInt32(Canvas.GetLeft(richTextBox) / formattedText.WidthIncludingTrailingWhitespace);
        }

        private void AddSpaces(Xceed.Document.NET.Paragraph paragraph, int numberOfSpaces, int index)
        {
            string newText = new string(' ', numberOfSpaces);
            paragraph.InsertText(index,newText);
            paragraph.AppendLine();
        }

        private void SetFontSetings(Block paragraph, Xceed.Document.NET.Paragraph docXParagraph)
        {
            foreach (Run run in ((Paragraph)paragraph).Inlines.OfType<Run>())
            {
                TextRange a = new TextRange(run.ContentStart, run.ContentEnd);
                docXParagraph.Append(a.Text);
                var fontFamily = run.FontFamily;
                var fontSize = run.FontSize;
                var foreground = run.Foreground;
                var fontWeight = run.FontWeight;
                var fontStyle = run.FontStyle;
                var textDecorations = run.TextDecorations;

                docXParagraph.Font(fontFamily.ToString())
                    .FontSize(fontSize)
                    .Color(System.Drawing.ColorTranslator.FromHtml(foreground.ToString()));

                if (fontWeight == FontWeights.Bold)
                {
                    docXParagraph.Bold();
                }

                if (fontStyle == FontStyles.Italic)
                {
                    docXParagraph.Italic();
                }
                if (textDecorations == TextDecorations.Underline)
                {
                    docXParagraph.UnderlineColor(System.Drawing.Color.Black);
                }
            }
        }
    }
}
