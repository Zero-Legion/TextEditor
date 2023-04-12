
namespace TextEditor
{
    using System;
    using System.Windows.Forms;

   
    public partial class MainWindow : Form
    {
        
        private bool textChanged;

        
        private string fileName;

       
        public MainWindow()
        {
            this.InitializeComponent();

            this.textChanged = false;
            this.fileName = "Untilted";
            this.Text = "TextEditor" + " - " + this.fileName;

            this.OpenFileAtStartUp();
        }

      
        private void OpenFileAtStartUp()
        {
            string[] args = Environment.GetCommandLineArgs();

            if (args != null && args.Length == 2)
            {
                this.fileName = args[1];
                this.OpenFile();
            }
        }

        
        private void MainWindow_KeyDown(object sender, KeyEventArgs k)
        {
            if (k.Control && k.Shift && k.KeyCode == Keys.S)
            {
                this.SaveAsToolStripMenuItem_Click(this, new EventArgs());
            }
            else if (k.Control)
            {
                if (k.KeyCode == Keys.N)
                {
                    this.NewToolStripMenuItem_Click(this, new EventArgs());
                }
                else if (k.KeyCode == Keys.O)
                {
                    this.OpenToolStripMenuItem_Click(this, new EventArgs());
                }
                else if (k.KeyCode == Keys.S)
                {
                    this.SaveToolStripMenuItem_Click(this, new EventArgs());
                }
                else if (k.KeyCode == Keys.F)
                {
                    this.FontSettingsToolStripMenuItem_Click(this, new EventArgs());
                }
               
                else if (k.KeyCode == Keys.X)
                {
                    this.Close();
                }
            }
        }

        
        private void TextHasChanged(object sender, EventArgs e)
        {
            this.textChanged = true;
        }

        
        private string GetFileName(string path)
        {
            string[] pathArray = path.Split('\\');
            return pathArray[pathArray.Length - 1];
        }

       
        private void OpenToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult askIfSave = DialogResult.None;

            if (this.textChanged == true)
            {
                askIfSave = MessageBox.Show("Vrei sa salvezi ?", "Windows Editor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }

            if (askIfSave == DialogResult.Yes)
            {
                this.SaveFile(false);
            }

            if (askIfSave != DialogResult.Cancel)
            {
                if (this.openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.fileName = this.openFileDialog1.FileName;

                    this.OpenFile();
                }
            }
        }
        private System.Drawing.Printing.PrintDocument printDocument = new System.Drawing.Printing.PrintDocument();

       
        private void OpenFile()
        {
            try
            {
                if (this.CheckIfFileIsRtf(this.fileName) == true)
                {
                    this.richTextBox1.LoadFile(this.fileName, RichTextBoxStreamType.RichText);

                    // Sets the font of the richtextbox to the font of the .rtf file.
                    this.richTextBox1.HideSelection = true;
                    this.richTextBox1.SelectAll();
                    this.richTextBox1.Font = this.richTextBox1.SelectionFont;
                    this.richTextBox1.Select(this.richTextBox1.Text.Length, this.richTextBox1.Text.Length);
                    this.richTextBox1.HideSelection = false;
                    this.fontDialog1.Font = this.richTextBox1.Font;
                }
                else
                {
                    this.richTextBox1.LoadFile(this.fileName, RichTextBoxStreamType.PlainText);
                }

                this.textChanged = false;
                this.Text = "TextEditor" + " - " + this.GetFileName(this.fileName);
                this.openFileDialog1.FileName = string.Empty;
            }
            catch
            {
                MessageBox.Show("A intervenit o eroare, nu s a putut deschide fisierul!", "TextEditor", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
            }
        }

       
        private void NewToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DialogResult askIfSave = DialogResult.None;

            if (this.textChanged == true)
            {
                askIfSave = MessageBox.Show("Vrei sa salvezi schimbarile ?", "TextEditor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }

            if (askIfSave == DialogResult.Yes)
            {
                this.SaveFile(false);
            }

            if (askIfSave != DialogResult.Cancel)
            {
                this.fileName = "Untitled";
                this.Text = "TextEditor" + " - " + this.fileName;
                this.richTextBox1.Text = string.Empty;
                this.textChanged = false;
            }
        }

       
        private void SaveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveFile(false);
        }

         private void SaveFile(bool saveAs)
        {
            DialogResult askIfSave = DialogResult.None;

            if (this.fileName == "Untitled" || saveAs == true)
            {
                if (this.saveFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    this.fileName = this.saveFileDialog1.FileName;
                    this.Text = "TextEditor" + " - " + this.GetFileName(this.fileName);
                }
                else
                {
                    askIfSave = DialogResult.Cancel;
                }
            }

            if (askIfSave != DialogResult.Cancel)
            {
                try
                {
                    if (this.CheckIfFileIsRtf(this.fileName) == true)
                    {
                        this.richTextBox1.SaveFile(this.fileName, RichTextBoxStreamType.RichText);
                    }
                    else
                    {
                        this.richTextBox1.SaveFile(this.fileName, RichTextBoxStreamType.PlainText);
                    }

                    this.textChanged = false;
                }
                catch
                {
                    MessageBox.Show("A intervenit o eroare, nu am reusit sa salvez", "TextEditor", MessageBoxButtons.OK, MessageBoxIcon.Error, MessageBoxDefaultButton.Button1);
                }
            }

            this.saveFileDialog1.FileName = string.Empty;
        }

   
        private bool CheckIfFileIsRtf(string path)
        {
            string[] pathArray = path.Split('.');

            if (pathArray[pathArray.Length - 1] == "rtf")
            {
                return true;
            }
            else
            {
                return false;
            }
        }

  
        private void SaveAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.SaveFile(true);
        }

   
        private void MainWindow_Closing(object sender, FormClosingEventArgs e)
        {
            DialogResult askIfSave = DialogResult.None;

            if (this.textChanged == true)
            {
                askIfSave = MessageBox.Show("Vrei sa salvezi modificarile ?", "TextEditor", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question, MessageBoxDefaultButton.Button1);
            }

            if (askIfSave == DialogResult.Yes)
            {
                this.SaveFile(false);
            }
            else if (askIfSave == DialogResult.Cancel)
            {
                e.Cancel = true;
            }
        }

      
        private void FontSettingsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.fontDialog1.ShowDialog() == DialogResult.OK)
            {
                this.richTextBox1.Font = this.fontDialog1.Font;
            }
        }

    
        private void printToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if (printDialog.ShowDialog() == DialogResult.OK)
            {
                // do something with print
            }
        }

        private void fontDialog1_Apply(object sender, EventArgs e)
        {

        }

        private void colorToolStripMenuItem_Click(object sender, EventArgs e)
        {
            colorDialog1.ShowDialog();

            if (colorDialog1.ShowDialog() == DialogResult.OK)
            {
                richTextBox1.ForeColor = colorDialog1.Color;
            }
        }

        private void inchideAplicatieToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void setarePaginaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            PageSetupDialog pageSetupDialog = new PageSetupDialog();

            pageSetupDialog.Document = printDocument;

            if (pageSetupDialog.ShowDialog() == DialogResult.OK)
            {
                printDocument.DefaultPageSettings = pageSetupDialog.PageSettings;
                printDocument.PrinterSettings = pageSetupDialog.PrinterSettings;
            }
        }

        private void printPrevizualizareToolStripMenuItem_Click(object sender, EventArgs e)
        {
            printPreviewDialog1.Document = printDocument;
            printPreviewDialog1.ShowDialog();
        }
    }
}
