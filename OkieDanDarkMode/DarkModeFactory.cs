using OkieDan.Forms.DarkModeCore;
using System.Windows.Forms;

public class DarkModeFactory : IDarkModeFactory
{
    public DarkModeCS Create(Form form)
    {
        return new DarkModeCS(form)
        {
            ColorMode = DarkModeCS.DisplayMode.SystemDefault
        };
    }
}