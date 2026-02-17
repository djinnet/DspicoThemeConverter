using OkieDan.Forms.DarkModeCore;

namespace DspicoThemeForms
{
    partial class Main
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            lblSource = new Label();
            txtSourcePath = new TextBox();
            btnBrowseSource = new Button();
            lblOutput = new Label();
            txtOutputPath = new TextBox();
            btnBrowseOutput = new Button();
            lblThemeType = new Label();
            cmbThemeType = new FlatComboBox();
            btnConvert = new Button();
            tabPreview = new FlatTabControl();
            tabTop = new TabPage();
            picTop = new PictureBox();
            tabBottom = new TabPage();
            picBottom = new PictureBox();
            tabGrid = new TabPage();
            picGrid = new PictureBox();
            tabGridSelected = new TabPage();
            picGridSelected = new PictureBox();
            tabBanner = new TabPage();
            picBanner = new PictureBox();
            tabBannerSelected = new TabPage();
            picBannerSelected = new PictureBox();
            tabScrim = new TabPage();
            picScrim = new PictureBox();
            txtLog = new TextBox();
            groupBoxTheme = new GroupBox();
            chkDarkTheme = new CheckBox();
            label1 = new Label();
            lblThemeName = new Label();
            txtThemeName = new TextBox();
            lblThemeDesc = new Label();
            txtThemeDesc = new TextBox();
            lblThemeAuthor = new Label();
            txtThemeAuthor = new TextBox();
            lblThemeOrigin = new Label();
            txtThemeOrigin = new TextBox();
            chkAllowedOverwrite = new CheckBox();
            chkDarkMode = new CheckBox();
            tabPreview.SuspendLayout();
            tabTop.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picTop).BeginInit();
            tabBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBottom).BeginInit();
            tabGrid.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picGrid).BeginInit();
            tabGridSelected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picGridSelected).BeginInit();
            tabBanner.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBanner).BeginInit();
            tabBannerSelected.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picBannerSelected).BeginInit();
            tabScrim.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)picScrim).BeginInit();
            groupBoxTheme.SuspendLayout();
            SuspendLayout();
            // 
            // lblSource
            // 
            lblSource.AutoSize = true;
            lblSource.Location = new Point(12, 15);
            lblSource.Name = "lblSource";
            lblSource.Size = new Size(114, 15);
            lblSource.TabIndex = 0;
            lblSource.Text = "Source theme folder";
            // 
            // txtSourcePath
            // 
            txtSourcePath.Location = new Point(12, 33);
            txtSourcePath.Name = "txtSourcePath";
            txtSourcePath.ReadOnly = true;
            txtSourcePath.Size = new Size(852, 23);
            txtSourcePath.TabIndex = 1;
            // 
            // btnBrowseSource
            // 
            btnBrowseSource.Location = new Point(870, 33);
            btnBrowseSource.Name = "btnBrowseSource";
            btnBrowseSource.Size = new Size(75, 25);
            btnBrowseSource.TabIndex = 2;
            btnBrowseSource.Text = "Browse";
            // 
            // lblOutput
            // 
            lblOutput.AutoSize = true;
            lblOutput.Location = new Point(12, 65);
            lblOutput.Name = "lblOutput";
            lblOutput.Size = new Size(116, 15);
            lblOutput.TabIndex = 3;
            lblOutput.Text = "Output theme folder";
            // 
            // txtOutputPath
            // 
            txtOutputPath.Location = new Point(12, 83);
            txtOutputPath.Name = "txtOutputPath";
            txtOutputPath.ReadOnly = true;
            txtOutputPath.Size = new Size(852, 23);
            txtOutputPath.TabIndex = 4;
            // 
            // btnBrowseOutput
            // 
            btnBrowseOutput.Location = new Point(870, 81);
            btnBrowseOutput.Name = "btnBrowseOutput";
            btnBrowseOutput.Size = new Size(75, 25);
            btnBrowseOutput.TabIndex = 5;
            btnBrowseOutput.Text = "Browse";
            // 
            // lblThemeType
            // 
            lblThemeType.AutoSize = true;
            lblThemeType.Location = new Point(12, 120);
            lblThemeType.Name = "lblThemeType";
            lblThemeType.Size = new Size(134, 15);
            lblThemeType.TabIndex = 6;
            lblThemeType.Text = "Detected / override type";
            // 
            // cmbThemeType
            // 
            cmbThemeType.DropDownStyle = ComboBoxStyle.DropDownList;
            cmbThemeType.FormattingEnabled = true;
            cmbThemeType.Items.AddRange(new object[] { "Auto-detect", "YSMenu", "Moonshell", "AKMenu", "TwiLightMenu", "DSpico" });
            cmbThemeType.Location = new Point(12, 138);
            cmbThemeType.Name = "cmbThemeType";
            cmbThemeType.Size = new Size(200, 23);
            cmbThemeType.TabIndex = 7;
            // 
            // btnConvert
            // 
            btnConvert.Location = new Point(870, 120);
            btnConvert.Name = "btnConvert";
            btnConvert.Size = new Size(75, 30);
            btnConvert.TabIndex = 8;
            btnConvert.Text = "Convert";
            // 
            // tabPreview
            // 
            tabPreview.Appearance = TabAppearance.Buttons;
            tabPreview.BorderColor = SystemColors.ControlDark;
            tabPreview.Controls.Add(tabTop);
            tabPreview.Controls.Add(tabBottom);
            tabPreview.Controls.Add(tabGrid);
            tabPreview.Controls.Add(tabGridSelected);
            tabPreview.Controls.Add(tabBanner);
            tabPreview.Controls.Add(tabBannerSelected);
            tabPreview.Controls.Add(tabScrim);
            tabPreview.DrawMode = TabDrawMode.OwnerDrawFixed;
            tabPreview.ItemSize = new Size(100, 24);
            tabPreview.LineColor = SystemColors.Highlight;
            tabPreview.Location = new Point(12, 180);
            tabPreview.Name = "tabPreview";
            tabPreview.SelectedForeColor = SystemColors.ControlText;
            tabPreview.SelectedIndex = 0;
            tabPreview.SelectTabColor = SystemColors.ControlLight;
            tabPreview.ShowTabCloseButton = false;
            tabPreview.Size = new Size(725, 260);
            tabPreview.SizeMode = TabSizeMode.Fixed;
            tabPreview.TabCloseColor = SystemColors.ControlText;
            tabPreview.TabColor = SystemColors.ControlLight;
            tabPreview.TabIndex = 9;
            // 
            // tabTop
            // 
            tabTop.BackColor = SystemColors.ControlLight;
            tabTop.Controls.Add(picTop);
            tabTop.Location = new Point(4, 28);
            tabTop.Name = "tabTop";
            tabTop.Size = new Size(717, 228);
            tabTop.TabIndex = 0;
            tabTop.Text = "Top screen";
            // 
            // picTop
            // 
            picTop.Dock = DockStyle.Fill;
            picTop.Location = new Point(0, 0);
            picTop.Name = "picTop";
            picTop.Size = new Size(717, 228);
            picTop.TabIndex = 0;
            picTop.TabStop = false;
            picTop.Paint += Picturebox_Paint;
            // 
            // tabBottom
            // 
            tabBottom.BackColor = SystemColors.ControlLight;
            tabBottom.Controls.Add(picBottom);
            tabBottom.Location = new Point(4, 28);
            tabBottom.Name = "tabBottom";
            tabBottom.Size = new Size(717, 228);
            tabBottom.TabIndex = 1;
            tabBottom.Text = "Bottom screen";
            // 
            // picBottom
            // 
            picBottom.Dock = DockStyle.Fill;
            picBottom.Location = new Point(0, 0);
            picBottom.Name = "picBottom";
            picBottom.Size = new Size(717, 228);
            picBottom.TabIndex = 0;
            picBottom.TabStop = false;
            picBottom.Paint += Picturebox_Paint;
            // 
            // tabGrid
            // 
            tabGrid.BackColor = SystemColors.ControlLight;
            tabGrid.Controls.Add(picGrid);
            tabGrid.Location = new Point(4, 28);
            tabGrid.Name = "tabGrid";
            tabGrid.Size = new Size(717, 228);
            tabGrid.TabIndex = 2;
            tabGrid.Text = "Grid cell";
            // 
            // picGrid
            // 
            picGrid.Dock = DockStyle.Fill;
            picGrid.Location = new Point(0, 0);
            picGrid.Name = "picGrid";
            picGrid.Size = new Size(717, 228);
            picGrid.TabIndex = 0;
            picGrid.TabStop = false;
            picGrid.Paint += Picturebox_Paint;
            // 
            // tabGridSelected
            // 
            tabGridSelected.BackColor = SystemColors.ControlLight;
            tabGridSelected.Controls.Add(picGridSelected);
            tabGridSelected.Location = new Point(4, 28);
            tabGridSelected.Name = "tabGridSelected";
            tabGridSelected.Size = new Size(717, 228);
            tabGridSelected.TabIndex = 5;
            tabGridSelected.Text = "Grid cell Select";
            // 
            // picGridSelected
            // 
            picGridSelected.Dock = DockStyle.Fill;
            picGridSelected.Location = new Point(0, 0);
            picGridSelected.Name = "picGridSelected";
            picGridSelected.Size = new Size(717, 228);
            picGridSelected.TabIndex = 0;
            picGridSelected.TabStop = false;
            picGridSelected.Paint += Picturebox_Paint;
            // 
            // tabBanner
            // 
            tabBanner.BackColor = SystemColors.ControlLight;
            tabBanner.Controls.Add(picBanner);
            tabBanner.Location = new Point(4, 28);
            tabBanner.Name = "tabBanner";
            tabBanner.Size = new Size(717, 228);
            tabBanner.TabIndex = 3;
            tabBanner.Text = "Banner cell";
            // 
            // picBanner
            // 
            picBanner.Dock = DockStyle.Fill;
            picBanner.Location = new Point(0, 0);
            picBanner.Name = "picBanner";
            picBanner.Size = new Size(717, 228);
            picBanner.TabIndex = 0;
            picBanner.TabStop = false;
            picBanner.Paint += Picturebox_Paint;
            // 
            // tabBannerSelected
            // 
            tabBannerSelected.BackColor = SystemColors.ControlLight;
            tabBannerSelected.Controls.Add(picBannerSelected);
            tabBannerSelected.Font = new Font("Segoe UI", 9F);
            tabBannerSelected.Location = new Point(4, 28);
            tabBannerSelected.Name = "tabBannerSelected";
            tabBannerSelected.Size = new Size(717, 228);
            tabBannerSelected.TabIndex = 6;
            tabBannerSelected.Text = "Banner cell Select";
            // 
            // picBannerSelected
            // 
            picBannerSelected.Dock = DockStyle.Fill;
            picBannerSelected.Location = new Point(0, 0);
            picBannerSelected.Name = "picBannerSelected";
            picBannerSelected.Size = new Size(717, 228);
            picBannerSelected.TabIndex = 0;
            picBannerSelected.TabStop = false;
            picBannerSelected.Paint += Picturebox_Paint;
            // 
            // tabScrim
            // 
            tabScrim.BackColor = SystemColors.ControlLight;
            tabScrim.Controls.Add(picScrim);
            tabScrim.Location = new Point(4, 28);
            tabScrim.Name = "tabScrim";
            tabScrim.Size = new Size(717, 228);
            tabScrim.TabIndex = 4;
            tabScrim.Text = "Scrim";
            // 
            // picScrim
            // 
            picScrim.Dock = DockStyle.Fill;
            picScrim.Location = new Point(0, 0);
            picScrim.Name = "picScrim";
            picScrim.Size = new Size(717, 228);
            picScrim.TabIndex = 0;
            picScrim.TabStop = false;
            picScrim.Paint += Picturebox_Paint;
            // 
            // txtLog
            // 
            txtLog.Location = new Point(12, 450);
            txtLog.Multiline = true;
            txtLog.Name = "txtLog";
            txtLog.ReadOnly = true;
            txtLog.ScrollBars = ScrollBars.Vertical;
            txtLog.Size = new Size(933, 120);
            txtLog.TabIndex = 10;
            // 
            // groupBoxTheme
            // 
            groupBoxTheme.Controls.Add(chkDarkTheme);
            groupBoxTheme.Controls.Add(label1);
            groupBoxTheme.Controls.Add(lblThemeName);
            groupBoxTheme.Controls.Add(txtThemeName);
            groupBoxTheme.Controls.Add(lblThemeDesc);
            groupBoxTheme.Controls.Add(txtThemeDesc);
            groupBoxTheme.Controls.Add(lblThemeAuthor);
            groupBoxTheme.Controls.Add(txtThemeAuthor);
            groupBoxTheme.Controls.Add(lblThemeOrigin);
            groupBoxTheme.Controls.Add(txtThemeOrigin);
            groupBoxTheme.Location = new Point(743, 184);
            groupBoxTheme.Name = "groupBoxTheme";
            groupBoxTheme.Size = new Size(200, 256);
            groupBoxTheme.TabIndex = 11;
            groupBoxTheme.TabStop = false;
            groupBoxTheme.Text = "Theme metadata";
            // 
            // chkDarkTheme
            // 
            chkDarkTheme.AutoSize = true;
            chkDarkTheme.Enabled = false;
            chkDarkTheme.Location = new Point(6, 213);
            chkDarkTheme.Name = "chkDarkTheme";
            chkDarkTheme.Size = new Size(156, 19);
            chkDarkTheme.TabIndex = 9;
            chkDarkTheme.Text = "Is the theme dark mode?";
            chkDarkTheme.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(6, 195);
            label1.Name = "label1";
            label1.Size = new Size(71, 15);
            label1.TabIndex = 8;
            label1.Text = "Dark Theme";
            // 
            // lblThemeName
            // 
            lblThemeName.AutoSize = true;
            lblThemeName.Location = new Point(6, 19);
            lblThemeName.Name = "lblThemeName";
            lblThemeName.Size = new Size(110, 15);
            lblThemeName.TabIndex = 0;
            lblThemeName.Text = "Name of the theme";
            // 
            // txtThemeName
            // 
            txtThemeName.Location = new Point(6, 37);
            txtThemeName.Name = "txtThemeName";
            txtThemeName.ReadOnly = true;
            txtThemeName.Size = new Size(188, 23);
            txtThemeName.TabIndex = 1;
            // 
            // lblThemeDesc
            // 
            lblThemeDesc.AutoSize = true;
            lblThemeDesc.Location = new Point(6, 63);
            lblThemeDesc.Name = "lblThemeDesc";
            lblThemeDesc.Size = new Size(138, 15);
            lblThemeDesc.TabIndex = 2;
            lblThemeDesc.Text = "Description of the theme";
            // 
            // txtThemeDesc
            // 
            txtThemeDesc.Location = new Point(6, 81);
            txtThemeDesc.Name = "txtThemeDesc";
            txtThemeDesc.ReadOnly = true;
            txtThemeDesc.Size = new Size(188, 23);
            txtThemeDesc.TabIndex = 3;
            // 
            // lblThemeAuthor
            // 
            lblThemeAuthor.AutoSize = true;
            lblThemeAuthor.Location = new Point(6, 107);
            lblThemeAuthor.Name = "lblThemeAuthor";
            lblThemeAuthor.Size = new Size(115, 15);
            lblThemeAuthor.TabIndex = 4;
            lblThemeAuthor.Text = "Author of the theme";
            // 
            // txtThemeAuthor
            // 
            txtThemeAuthor.Location = new Point(6, 125);
            txtThemeAuthor.Name = "txtThemeAuthor";
            txtThemeAuthor.ReadOnly = true;
            txtThemeAuthor.Size = new Size(188, 23);
            txtThemeAuthor.TabIndex = 5;
            // 
            // lblThemeOrigin
            // 
            lblThemeOrigin.AutoSize = true;
            lblThemeOrigin.Location = new Point(6, 151);
            lblThemeOrigin.Name = "lblThemeOrigin";
            lblThemeOrigin.Size = new Size(111, 15);
            lblThemeOrigin.TabIndex = 6;
            lblThemeOrigin.Text = "Origin of the theme";
            // 
            // txtThemeOrigin
            // 
            txtThemeOrigin.Location = new Point(6, 169);
            txtThemeOrigin.Name = "txtThemeOrigin";
            txtThemeOrigin.ReadOnly = true;
            txtThemeOrigin.Size = new Size(188, 23);
            txtThemeOrigin.TabIndex = 7;
            // 
            // chkAllowedOverwrite
            // 
            chkAllowedOverwrite.AutoSize = true;
            chkAllowedOverwrite.Location = new Point(743, 159);
            chkAllowedOverwrite.Name = "chkAllowedOverwrite";
            chkAllowedOverwrite.Size = new Size(214, 19);
            chkAllowedOverwrite.TabIndex = 12;
            chkAllowedOverwrite.Text = "Allowed overwrite Theme metadata";
            chkAllowedOverwrite.UseVisualStyleBackColor = true;
            chkAllowedOverwrite.CheckedChanged += ChkAllowedOverwrite_CheckedStateChanged;
            // 
            // chkDarkMode
            // 
            chkDarkMode.AutoSize = true;
            chkDarkMode.Location = new Point(870, 8);
            chkDarkMode.Name = "chkDarkMode";
            chkDarkMode.Size = new Size(84, 19);
            chkDarkMode.TabIndex = 13;
            chkDarkMode.Text = "Dark mode";
            chkDarkMode.UseVisualStyleBackColor = true;
            chkDarkMode.CheckedChanged += chkDarkMode_CheckedChanged;
            // 
            // Main
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(954, 585);
            Controls.Add(chkDarkMode);
            Controls.Add(chkAllowedOverwrite);
            Controls.Add(lblSource);
            Controls.Add(txtSourcePath);
            Controls.Add(btnBrowseSource);
            Controls.Add(lblOutput);
            Controls.Add(txtOutputPath);
            Controls.Add(btnBrowseOutput);
            Controls.Add(lblThemeType);
            Controls.Add(cmbThemeType);
            Controls.Add(btnConvert);
            Controls.Add(tabPreview);
            Controls.Add(txtLog);
            Controls.Add(groupBoxTheme);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            Name = "Main";
            Text = "DSpico Theme Converter";
            tabPreview.ResumeLayout(false);
            tabTop.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picTop).EndInit();
            tabBottom.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBottom).EndInit();
            tabGrid.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picGrid).EndInit();
            tabGridSelected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picGridSelected).EndInit();
            tabBanner.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBanner).EndInit();
            tabBannerSelected.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picBannerSelected).EndInit();
            tabScrim.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)picScrim).EndInit();
            groupBoxTheme.ResumeLayout(false);
            groupBoxTheme.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.Label lblSource;
        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Button btnBrowseSource;

        private System.Windows.Forms.Label lblOutput;
        private System.Windows.Forms.TextBox txtOutputPath;
        private System.Windows.Forms.Button btnBrowseOutput;

        private System.Windows.Forms.Label lblThemeType;
        private FlatComboBox cmbThemeType;

        private System.Windows.Forms.Button btnConvert;

        private FlatTabControl tabPreview;
        private System.Windows.Forms.TabPage tabTop;
        private System.Windows.Forms.TabPage tabBottom;
        private System.Windows.Forms.TabPage tabGrid;
        private System.Windows.Forms.TabPage tabBanner;

        private System.Windows.Forms.PictureBox picTop;
        private System.Windows.Forms.PictureBox picBottom;
        private System.Windows.Forms.PictureBox picGrid;
        private System.Windows.Forms.PictureBox picBanner;

        private System.Windows.Forms.TextBox txtLog;
        private TabPage tabScrim;
        private PictureBox picScrim;
        private TabPage tabGridSelected;
        private PictureBox picGridSelected;
        private TabPage tabBannerSelected;
        private PictureBox picBannerSelected;
        private GroupBox groupBoxTheme;
        private Label lblThemeDesc;
        private Label lblThemeName;
        private TextBox txtThemeName;
        private Label lblThemeAuthor;
        private TextBox txtThemeDesc;
        private TextBox txtThemeAuthor;
        private Label lblThemeOrigin;
        private CheckBox chkDarkTheme;
        private Label label1;
        private TextBox txtThemeOrigin;
        private CheckBox chkAllowedOverwrite;
        private CheckBox chkDarkMode;
    }
}
