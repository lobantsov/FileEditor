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
                        }
                    }

                    doc.Save();
                }
            }
        }

        private void SetFontSetings(Block paragraph, Xceed.Document.NET.Paragraph docXParagraph)
        {
            foreach (Run run in ((Paragraph)paragraph).Inlines.OfType<Run>())
            {
                TextRange a = new TextRange(run.ContentStart, run.ContentEnd);
                docXParagraph.AppendLine(a.Text);
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
