namespace ExcelToA5er
{
    partial class MainForm
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
            this.buttonToBrowseXlsx = new Button();
            this.textBoxXlsxFilePath = new TextBox();
            this.label1 = new Label();
            this.tableLayoutPanel1 = new TableLayoutPanel();
            this.tableLayoutPanel2 = new TableLayoutPanel();
            this.button1 = new Button();
            this.textBox1 = new TextBox();
            this.tableLayoutPanel3 = new TableLayoutPanel();
            this.buttonToConvert = new Button();
            this.textBoxOutputFilePath = new TextBox();
            this.listBoxTableInfo = new ListBox();
            this.tableLayoutPanel5 = new TableLayoutPanel();
            this.checkBoxToSplitOutputFile = new CheckBox();
            this.tableLayoutPanel4 = new TableLayoutPanel();
            this.checkBoxToUseTableNames = new CheckBox();
            this.buttonToBrowseTableNames = new Button();
            this.textBoxTableNamesFilePath = new TextBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            this.tableLayoutPanel3.SuspendLayout();
            this.tableLayoutPanel5.SuspendLayout();
            this.tableLayoutPanel4.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonToBrowseXlsx
            // 
            this.buttonToBrowseXlsx.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.buttonToBrowseXlsx.Location = new Point(536, 3);
            this.buttonToBrowseXlsx.Name = "buttonToBrowseXlsx";
            this.buttonToBrowseXlsx.Size = new Size(75, 23);
            this.buttonToBrowseXlsx.TabIndex = 0;
            this.buttonToBrowseXlsx.Text = "参照";
            this.buttonToBrowseXlsx.UseVisualStyleBackColor = true;
            this.buttonToBrowseXlsx.Click += this.ButtonToBrowseXlsx_Click;
            // 
            // textBoxXlsxFilePath
            // 
            this.textBoxXlsxFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.textBoxXlsxFilePath.Location = new Point(3, 3);
            this.textBoxXlsxFilePath.Name = "textBoxXlsxFilePath";
            this.textBoxXlsxFilePath.ReadOnly = true;
            this.textBoxXlsxFilePath.Size = new Size(507, 23);
            this.textBoxXlsxFilePath.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.Dock = DockStyle.Top;
            this.label1.Location = new Point(5, 5);
            this.label1.Name = "label1";
            this.label1.Size = new Size(614, 23);
            this.label1.TabIndex = 2;
            this.label1.Text = "テーブル定義書からA5ER図へ変換するツールです。";
            this.label1.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.ColumnCount = 3;
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel1.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel1.Controls.Add(this.buttonToBrowseXlsx, 2, 0);
            this.tableLayoutPanel1.Controls.Add(this.textBoxXlsxFilePath, 0, 0);
            this.tableLayoutPanel1.Dock = DockStyle.Top;
            this.tableLayoutPanel1.Location = new Point(5, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 1;
            this.tableLayoutPanel1.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel1.Size = new Size(614, 29);
            this.tableLayoutPanel1.TabIndex = 4;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 3;
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel2.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.button1, 2, 0);
            this.tableLayoutPanel2.Dock = DockStyle.Top;
            this.tableLayoutPanel2.Location = new Point(0, 0);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 1;
            this.tableLayoutPanel2.Size = new Size(200, 100);
            this.tableLayoutPanel2.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.button1.Location = new Point(122, 3);
            this.button1.Name = "button1";
            this.button1.Size = new Size(75, 94);
            this.button1.TabIndex = 0;
            this.button1.Text = "参照";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // textBox1
            // 
            this.textBox1.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.textBox1.Location = new Point(3, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new Size(93, 23);
            this.textBox1.TabIndex = 1;
            // 
            // tableLayoutPanel3
            // 
            this.tableLayoutPanel3.ColumnCount = 3;
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel3.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel3.Controls.Add(this.buttonToConvert, 2, 0);
            this.tableLayoutPanel3.Controls.Add(this.textBoxOutputFilePath, 0, 0);
            this.tableLayoutPanel3.Dock = DockStyle.Bottom;
            this.tableLayoutPanel3.Location = new Point(5, 327);
            this.tableLayoutPanel3.Name = "tableLayoutPanel3";
            this.tableLayoutPanel3.RowCount = 1;
            this.tableLayoutPanel3.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel3.Size = new Size(614, 29);
            this.tableLayoutPanel3.TabIndex = 5;
            // 
            // buttonToConvert
            // 
            this.buttonToConvert.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.buttonToConvert.Location = new Point(536, 3);
            this.buttonToConvert.Name = "buttonToConvert";
            this.buttonToConvert.Size = new Size(75, 23);
            this.buttonToConvert.TabIndex = 0;
            this.buttonToConvert.Text = "変換";
            this.buttonToConvert.UseVisualStyleBackColor = true;
            this.buttonToConvert.Click += this.ButtonToConvert_Click;
            // 
            // textBoxOutputFilePath
            // 
            this.textBoxOutputFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.textBoxOutputFilePath.Location = new Point(3, 3);
            this.textBoxOutputFilePath.Name = "textBoxOutputFilePath";
            this.textBoxOutputFilePath.ReadOnly = true;
            this.textBoxOutputFilePath.Size = new Size(507, 23);
            this.textBoxOutputFilePath.TabIndex = 1;
            // 
            // listBoxTableInfo
            // 
            this.listBoxTableInfo.Dock = DockStyle.Fill;
            this.listBoxTableInfo.FormattingEnabled = true;
            this.listBoxTableInfo.ItemHeight = 15;
            this.listBoxTableInfo.Location = new Point(5, 57);
            this.listBoxTableInfo.Name = "listBoxTableInfo";
            this.listBoxTableInfo.SelectionMode = SelectionMode.MultiExtended;
            this.listBoxTableInfo.Size = new Size(614, 212);
            this.listBoxTableInfo.TabIndex = 6;
            this.listBoxTableInfo.SelectedIndexChanged += this.ListBoxTableInfo_SelectedIndexChanged;
            // 
            // tableLayoutPanel5
            // 
            this.tableLayoutPanel5.ColumnCount = 3;
            this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel5.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel5.Controls.Add(this.checkBoxToSplitOutputFile, 0, 0);
            this.tableLayoutPanel5.Dock = DockStyle.Bottom;
            this.tableLayoutPanel5.Location = new Point(5, 298);
            this.tableLayoutPanel5.Name = "tableLayoutPanel5";
            this.tableLayoutPanel5.RowCount = 1;
            this.tableLayoutPanel5.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel5.Size = new Size(614, 29);
            this.tableLayoutPanel5.TabIndex = 8;
            // 
            // checkBoxToSplitOutputFile
            // 
            this.checkBoxToSplitOutputFile.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.checkBoxToSplitOutputFile.Location = new Point(3, 3);
            this.checkBoxToSplitOutputFile.Name = "checkBoxToSplitOutputFile";
            this.checkBoxToSplitOutputFile.Size = new Size(588, 23);
            this.checkBoxToSplitOutputFile.TabIndex = 0;
            this.checkBoxToSplitOutputFile.Text = "変換結果を物理テーブル名毎に分割";
            this.checkBoxToSplitOutputFile.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel4
            // 
            this.tableLayoutPanel4.ColumnCount = 4;
            this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle(SizeType.Absolute, 20F));
            this.tableLayoutPanel4.ColumnStyles.Add(new ColumnStyle());
            this.tableLayoutPanel4.Controls.Add(this.checkBoxToUseTableNames, 0, 0);
            this.tableLayoutPanel4.Controls.Add(this.buttonToBrowseTableNames, 3, 0);
            this.tableLayoutPanel4.Controls.Add(this.textBoxTableNamesFilePath, 1, 0);
            this.tableLayoutPanel4.Dock = DockStyle.Bottom;
            this.tableLayoutPanel4.Location = new Point(5, 269);
            this.tableLayoutPanel4.Name = "tableLayoutPanel4";
            this.tableLayoutPanel4.RowCount = 1;
            this.tableLayoutPanel4.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            this.tableLayoutPanel4.Size = new Size(614, 29);
            this.tableLayoutPanel4.TabIndex = 9;
            // 
            // checkBoxToUseTableNames
            // 
            this.checkBoxToUseTableNames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.checkBoxToUseTableNames.Location = new Point(3, 3);
            this.checkBoxToUseTableNames.Name = "checkBoxToUseTableNames";
            this.checkBoxToUseTableNames.Size = new Size(184, 23);
            this.checkBoxToUseTableNames.TabIndex = 2;
            this.checkBoxToUseTableNames.Text = "物理テーブル名リストを使用";
            this.checkBoxToUseTableNames.UseVisualStyleBackColor = true;
            // 
            // buttonToBrowseTableNames
            // 
            this.buttonToBrowseTableNames.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.buttonToBrowseTableNames.Location = new Point(536, 3);
            this.buttonToBrowseTableNames.Name = "buttonToBrowseTableNames";
            this.buttonToBrowseTableNames.Size = new Size(75, 23);
            this.buttonToBrowseTableNames.TabIndex = 0;
            this.buttonToBrowseTableNames.Text = "参照";
            this.buttonToBrowseTableNames.UseVisualStyleBackColor = true;
            this.buttonToBrowseTableNames.Click += this.ButtonToBrowseTableNames_Click;
            // 
            // textBoxTableNamesFilePath
            // 
            this.textBoxTableNamesFilePath.Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right;
            this.textBoxTableNamesFilePath.Location = new Point(193, 3);
            this.textBoxTableNamesFilePath.Name = "textBoxTableNamesFilePath";
            this.textBoxTableNamesFilePath.ReadOnly = true;
            this.textBoxTableNamesFilePath.Size = new Size(317, 23);
            this.textBoxTableNamesFilePath.TabIndex = 1;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new SizeF(7F, 15F);
            this.AutoScaleMode = AutoScaleMode.Font;
            this.ClientSize = new Size(624, 361);
            this.Controls.Add(this.listBoxTableInfo);
            this.Controls.Add(this.tableLayoutPanel4);
            this.Controls.Add(this.tableLayoutPanel5);
            this.Controls.Add(this.tableLayoutPanel3);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Controls.Add(this.label1);
            this.Name = "MainForm";
            this.Padding = new Padding(5);
            this.Text = "ExcelToA5er";
            this.Load += this.MainForm_Load;
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel3.ResumeLayout(false);
            this.tableLayoutPanel3.PerformLayout();
            this.tableLayoutPanel5.ResumeLayout(false);
            this.tableLayoutPanel4.ResumeLayout(false);
            this.tableLayoutPanel4.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private Button buttonToBrowseXlsx;
        private TextBox textBoxXlsxFilePath;
        private Label label1;
        private TableLayoutPanel tableLayoutPanel1;
        private TableLayoutPanel tableLayoutPanel2;
        private Button button1;
        private TextBox textBox1;
        private TableLayoutPanel tableLayoutPanel3;
        private Button buttonToConvert;
        private TextBox textBoxOutputFilePath;
        private ListBox listBoxTableInfo;
        private TableLayoutPanel tableLayoutPanel5;
        private CheckBox checkBoxToSplitOutputFile;
        private TableLayoutPanel tableLayoutPanel4;
        private CheckBox checkBoxToUseTableNames;
        private Button buttonToBrowseTableNames;
        private TextBox textBoxTableNamesFilePath;
    }
}
