using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows;
using System.Windows.Controls;

namespace FileEditor.Classes.ClassForComponents
{
    public class ResizeRichTextBox
    {
        string _text = string.Empty;
        private double GetCharacterWidth(RichTextBox richTextBox, string line)
        {
            FormattedText formattedText = new FormattedText(
                line,
                CultureInfo.CurrentCulture,
                FlowDirection.LeftToRight,
                new Typeface(richTextBox.FontFamily, richTextBox.FontStyle, richTextBox.FontWeight, richTextBox.FontStretch),
                richTextBox.FontSize,
                Brushes.Black,
                new NumberSubstitution(),
                TextFormattingMode.Display
            );

            return formattedText.Width;
        }

        public void CheckAndResizeRichTextBox(RichTextBox richTextBox, string character)
        {
            char[] richtext = new TextRange(richTextBox.Document.ContentStart, richTextBox.Document.ContentEnd).Text.ToCharArray();
            richtext = richtext.Where(c => c != '\r' && c != '\n').ToArray();
            char lastSymb = ExtracteChar(richtext);
            double characterWidth = GetCharacterWidth(richTextBox,lastSymb.ToString());
            double currentLineWidth = GetCharacterWidth(richTextBox, new string(richtext));

            if (currentLineWidth + characterWidth > richTextBox.ActualWidth-40)
            {
                richTextBox.Width += characterWidth;
            }
        }

        private char ExtracteChar(char[] line )
        {
            //char[] OriginalChars = FirstLine.ToCharArray().Where(c => c != '\r' && c != '\n').ToArray();
            char[] ModifiedChars = line.Where(c => c != '\r' && c != '\n').ToArray();

            //int maxLength = (OriginalChars.Length > ModifiedChars.Length) ? OriginalChars.Length : ModifiedChars.Length;
            if(ModifiedChars.Length > 0 )
            return ModifiedChars[ModifiedChars.Length-1];
            return '0';
        }
    }
}
