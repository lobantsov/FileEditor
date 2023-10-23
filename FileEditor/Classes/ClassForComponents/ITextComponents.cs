using System.Windows.Controls;
using System.Windows.Documents;

namespace FileEditor.Classes.ClassForComponents
{
    interface ITextComponents
    {
       public Paragraph Paragraph();
       public Table Table();
       public List List();
    }
}
