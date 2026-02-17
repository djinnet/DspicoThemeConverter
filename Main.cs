using DspicoThemeForms.Core.DspicoExporter;
using DspicoThemeForms.Core.ThemeImporters;
using DspicoThemeForms.Core.ThemeNormalizationLayer;
using OkieDan.Forms.DarkModeCore;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace DspicoThemeForms
{
    public partial class Main : Form
    {
        private NormalizedTheme? _currentTheme;
        private DarkModeCS dm;
        private bool _isDarkMode;

        public Main(IDarkModeFactory darkModeFactory)
        {
            InitializeComponent();
            dm = darkModeFactory.Create(this);
            dm.ColorMode = DarkModeCS.DisplayMode.DarkMode;
            _isDarkMode = dm.isDarkMode();
            chkDarkMode.Checked = _isDarkMode;
            InitializeUIState();
            WireEvents();
        }

        private void InitializeUIState()
        {
            cmbThemeType.SelectedIndex = 0;
            btnConvert.Enabled = false;
        }

        private void WireEvents()
        {
            btnBrowseSource.Click += BtnBrowseSource_Click;
            btnBrowseOutput.Click += BtnBrowseOutput_Click;
            btnConvert.Click += BtnConvert_ClickAsync;
        }

        private async void BtnConvert_ClickAsync(object? sender, EventArgs e)
        {
            btnConvert.Enabled = false;
            AppendLog("Starting conversion...");

            try
            {
                if (_currentTheme == null)
                {
                    throw new Exception("No theme loaded. Please select a valid source theme folder.");
                }

                //the ptexconv.exe is included in the tools folder and should be in the same directory as the executable, so we can just reference it by name
                string toolsDir = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Tools");
                string ptexConvPath = Path.Combine(toolsDir, "ptexconv.exe");
                await Task.Run(() =>
                {
                    DSpicoThemeExporter exporter = new(
                        txtOutputPath.Text,
                        ptexConvPath,
                        AppendLogSafe
                    );

                    exporter.Export(theme: _currentTheme);
                });

                AppendLog("Conversion completed successfully");
                MessageBox.Show("Theme converted successfully!", "Done", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                AppendLog(ex.ToString());
                MessageBox.Show(ex.Message, "Conversion failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            finally
            {
                btnConvert.Enabled = true;
            }
        }

        private void BtnBrowseOutput_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog
            {
                Description = "Select output folder"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            txtOutputPath.Text = dlg.SelectedPath;
            UpdateConvertButtonState();
        }

        private void BtnBrowseSource_Click(object? sender, EventArgs e)
        {
            using var dlg = new FolderBrowserDialog
            {
                Description = "Select source theme folder"
            };

            if (dlg.ShowDialog() != DialogResult.OK)
                return;

            txtSourcePath.Text = dlg.SelectedPath;
            DetectThemeAndLoadPreview(dlg.SelectedPath);
        }

        private void DetectThemeAndLoadPreview(string path)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(path) || !Directory.Exists(path))
                {
                    throw new Exception("Selected path is not a valid directory.");
                }

                AppendLog("Detecting theme type...");

                IThemeImporter? importer = ThemeImporterFactory.Create(path, cmbThemeType.SelectedItem?.ToString()) ?? throw new Exception("No suitable theme importer found for the selected folder.");
                
                AppendLog($"Found theme type: {importer.Name}");
                _currentTheme = importer.Import(path);

                if (_currentTheme == null)
                {
                    throw new Exception("Failed to import theme. The importer returned null.");
                }

                AppendLog($"Loaded theme: {_currentTheme.Name}");
                ShowPreview(_currentTheme);
                ShowPreviewTheme(_currentTheme);
                UpdateConvertButtonState();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Theme load failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                AppendLog(ex.ToString());
                txtSourcePath.Text = string.Empty;
            }
        }

        private void ShowPreviewTheme(NormalizedTheme theme)
        {
            txtThemeName.Text = theme.Name;
            txtThemeAuthor.Text = theme.Author ?? "Unknown";
            txtThemeDesc.Text = theme.Description ?? "Unknown";
            txtThemeOrigin.Text = theme.OriginTheme ?? "Unknown";
            chkDarkTheme.Checked = theme.DarkTheme;
        }

        private void ShowPreview(NormalizedTheme theme)
        {
            picTop.Image = theme.TopBackground;
            picBottom.Image = theme.BottomBackground;
            picGrid.Image = theme.GridCell;
            picBanner.Image = theme.BannerListCell;
            picScrim.Image = theme.Scrim;
            picBannerSelected.Image = theme.BannerListCellSelected;
            picGridSelected.Image = theme.GridCellSelected;

            picTop.Invalidate();
            picBottom.Invalidate();
            picGrid.Invalidate();
            picBanner.Invalidate();
            picScrim.Invalidate();
            picBannerSelected.Invalidate();
            picGridSelected.Invalidate();
        }

        private void UpdateConvertButtonState()
        {
            btnConvert.Enabled = _currentTheme != null && Directory.Exists(txtOutputPath.Text);
        }

        private void AppendLog(string text)
        {
            txtLog.AppendText($"[{DateTime.Now:HH:mm:ss}] {text}{Environment.NewLine}");
        }

        private void AppendLogSafe(string text)
        {
            if (InvokeRequired)
            {
                Invoke(new Action<string>(AppendLog), text);
                return;
            }

            AppendLog(text);
        }

        private void Picturebox_Paint(object sender, PaintEventArgs e)
        {
            if (sender is not PictureBox pb || pb.Image == null)
                return;

            e.Graphics.Clear(pb.BackColor);

            e.Graphics.InterpolationMode = InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = PixelOffsetMode.Half;
            e.Graphics.SmoothingMode = SmoothingMode.None;

            var img = pb.Image;

            // Calculate integer scaling
            float scaleX = (float)pb.ClientSize.Width / img.Width;
            float scaleY = (float)pb.ClientSize.Height / img.Height;
            float scale = Math.Min(scaleX, scaleY);

            int drawWidth = (int)(img.Width * scale);
            int drawHeight = (int)(img.Height * scale);

            int offsetX = (pb.ClientSize.Width - drawWidth) / 2;
            int offsetY = (pb.ClientSize.Height - drawHeight) / 2;

            e.Graphics.DrawImage(
                img,
                new Rectangle(offsetX, offsetY, drawWidth, drawHeight),
                new Rectangle(0, 0, img.Width, img.Height),
                GraphicsUnit.Pixel
            );
        }

        private void ChkAllowedOverwrite_CheckedStateChanged(object sender, EventArgs e)
        {
            if (_currentTheme == null)
            {
                return;
            }

            UpdateAllowedOverwrite(chkAllowedOverwrite.Checked);
        }

        //have an function to enable or disable the theme metadata overwrite based on the checkbox, and update the current theme's AllowedOverwriteData property accordingly. This will allow users to choose whether they want the converted DSpico theme to use the original theme's name, description, author, and primary color when possible.
        private void UpdateAllowedOverwrite(bool allowed)
        {
            if (_currentTheme != null)
            {
                _currentTheme.AllowedOverwriteData = allowed;
            }

            txtThemeAuthor.ReadOnly = !allowed;
            txtThemeDesc.ReadOnly = !allowed;
            txtThemeName.ReadOnly = !allowed;
            chkDarkTheme.Enabled = allowed;

            txtThemeAuthor.Invalidate();
            txtThemeDesc.Invalidate();
            txtThemeName.Invalidate();
            chkDarkTheme.Invalidate();
        }

        private void chkDarkMode_CheckedChanged(object sender, EventArgs e)
        {
            dm.ColorMode = chkDarkMode.Checked ? DarkModeCS.DisplayMode.DarkMode : DarkModeCS.DisplayMode.ClearMode;
            _isDarkMode = chkDarkMode.Checked;

            dm.ApplyTheme(_isDarkMode);
        }
    }
}
