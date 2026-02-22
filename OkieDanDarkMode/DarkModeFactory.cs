using OkieDan.Forms.DarkModeCore;

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