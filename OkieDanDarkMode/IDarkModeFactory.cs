using OkieDan.Forms.DarkModeCore;
using System.Windows.Forms;
public interface IDarkModeFactory
{
    DarkModeCS Create(Form form);
}