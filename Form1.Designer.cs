namespace PictureList {
    partial class Form1 {
        /// <summary>
        /// 必要なデザイナー変数です。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 使用中のリソースをすべてクリーンアップします。
        /// </summary>
        /// <param name="disposing">マネージド リソースを破棄する場合は true を指定し、その他の場合は false を指定します。</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows フォーム デザイナーで生成されたコード

        /// <summary>
        /// デザイナー サポートに必要なメソッドです。このメソッドの内容を
        /// コード エディターで変更しないでください。
        /// </summary>
        private void InitializeComponent() {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            btnFolderSelect = new System.Windows.Forms.Button();
            lblSearchPath = new System.Windows.Forms.Label();
            btnCopy2Clip = new System.Windows.Forms.Button();
            chkSubDir = new System.Windows.Forms.CheckBox();
            groupBox1 = new System.Windows.Forms.GroupBox();
            label5 = new System.Windows.Forms.Label();
            rbtnOnlyExif = new System.Windows.Forms.RadioButton();
            rbtnOnlyImage = new System.Windows.Forms.RadioButton();
            rbtnIncldDir = new System.Windows.Forms.RadioButton();
            rbtnOnlyFiles = new System.Windows.Forms.RadioButton();
            rbtnOnlyDir = new System.Windows.Forms.RadioButton();
            label2 = new System.Windows.Forms.Label();
            txtMask = new System.Windows.Forms.TextBox();
            label3 = new System.Windows.Forms.Label();
            lblRows = new System.Windows.Forms.Label();
            btnFind = new System.Windows.Forms.Button();
            btnOut2File = new System.Windows.Forms.Button();
            groupBox2 = new System.Windows.Forms.GroupBox();
            rbtnTab = new System.Windows.Forms.RadioButton();
            rbtnCSV = new System.Windows.Forms.RadioButton();
            chkDirMask = new System.Windows.Forms.CheckBox();
            lblOutFile = new System.Windows.Forms.Label();
            saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            txtComment = new System.Windows.Forms.TextBox();
            btnExit = new System.Windows.Forms.Button();
            groupBox3 = new System.Windows.Forms.GroupBox();
            chkExt = new System.Windows.Forms.CheckBox();
            btnSetExif = new System.Windows.Forms.Button();
            chkExif = new System.Windows.Forms.CheckBox();
            chkAttribute = new System.Windows.Forms.CheckBox();
            chkSize = new System.Windows.Forms.CheckBox();
            chkLastAccessTime = new System.Windows.Forms.CheckBox();
            chkCreationTime = new System.Windows.Forms.CheckBox();
            chkLastWriteTime = new System.Windows.Forms.CheckBox();
            label6 = new System.Windows.Forms.Label();
            lblQty = new System.Windows.Forms.Label();
            progressBar1 = new System.Windows.Forms.ProgressBar();
            lblPrcsdQty = new System.Windows.Forms.Label();
            groupBoxExifExt = new System.Windows.Forms.GroupBox();
            label9 = new System.Windows.Forms.Label();
            label4 = new System.Windows.Forms.Label();
            txtAdditionalExifExtLists = new System.Windows.Forms.TextBox();
            chkExifTextExt = new System.Windows.Forms.CheckBox();
            chkSubExifExt = new System.Windows.Forms.CheckBox();
            rbtnExtSelect = new System.Windows.Forms.RadioButton();
            chkMainExifExt = new System.Windows.Forms.CheckBox();
            label1 = new System.Windows.Forms.Label();
            rbtnAllExt = new System.Windows.Forms.RadioButton();
            groupBox4 = new System.Windows.Forms.GroupBox();
            label8 = new System.Windows.Forms.Label();
            rbExifToolAllways = new System.Windows.Forms.RadioButton();
            rbExiToolIfNeed = new System.Windows.Forms.RadioButton();
            rbExifToolDonotUse = new System.Windows.Forms.RadioButton();
            lblEstTime = new System.Windows.Forms.Label();
            lblRmningTime = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            groupBox1.SuspendLayout();
            groupBox2.SuspendLayout();
            groupBox3.SuspendLayout();
            groupBoxExifExt.SuspendLayout();
            groupBox4.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // btnFolderSelect
            // 
            btnFolderSelect.BackColor = System.Drawing.SystemColors.Control;
            btnFolderSelect.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Bold);
            btnFolderSelect.Location = new System.Drawing.Point(6, 1);
            btnFolderSelect.Margin = new System.Windows.Forms.Padding(4);
            btnFolderSelect.Name = "btnFolderSelect";
            btnFolderSelect.Size = new System.Drawing.Size(124, 38);
            btnFolderSelect.TabIndex = 0;
            btnFolderSelect.Text = "フォルダーの選択";
            btnFolderSelect.UseVisualStyleBackColor = false;
            btnFolderSelect.Click += btnFolderSelect_Click;
            // 
            // lblSearchPath
            // 
            lblSearchPath.AutoEllipsis = true;
            lblSearchPath.BackColor = System.Drawing.SystemColors.Control;
            lblSearchPath.Location = new System.Drawing.Point(136, 4);
            lblSearchPath.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblSearchPath.Name = "lblSearchPath";
            lblSearchPath.Size = new System.Drawing.Size(374, 40);
            lblSearchPath.TabIndex = 1;
            lblSearchPath.Text = "フォルダ名を選んでください";
            // 
            // btnCopy2Clip
            // 
            btnCopy2Clip.BackColor = System.Drawing.SystemColors.Control;
            btnCopy2Clip.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Bold);
            btnCopy2Clip.Location = new System.Drawing.Point(9, 132);
            btnCopy2Clip.Margin = new System.Windows.Forms.Padding(4);
            btnCopy2Clip.Name = "btnCopy2Clip";
            btnCopy2Clip.Size = new System.Drawing.Size(147, 42);
            btnCopy2Clip.TabIndex = 4;
            btnCopy2Clip.Text = "クリップボードにコピー";
            btnCopy2Clip.UseVisualStyleBackColor = false;
            btnCopy2Clip.Click += btnCopy2Clip_Click;
            // 
            // chkSubDir
            // 
            chkSubDir.AutoSize = true;
            chkSubDir.Checked = true;
            chkSubDir.CheckState = System.Windows.Forms.CheckState.Checked;
            chkSubDir.Location = new System.Drawing.Point(139, 48);
            chkSubDir.Margin = new System.Windows.Forms.Padding(4);
            chkSubDir.Name = "chkSubDir";
            chkSubDir.Size = new System.Drawing.Size(133, 19);
            chkSubDir.TabIndex = 5;
            chkSubDir.Text = "サブディレクトリーを含む";
            chkSubDir.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            groupBox1.BackColor = System.Drawing.SystemColors.Control;
            groupBox1.Controls.Add(label5);
            groupBox1.Controls.Add(rbtnOnlyExif);
            groupBox1.Controls.Add(rbtnOnlyImage);
            groupBox1.Controls.Add(rbtnIncldDir);
            groupBox1.Controls.Add(rbtnOnlyFiles);
            groupBox1.Controls.Add(rbtnOnlyDir);
            groupBox1.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            groupBox1.Location = new System.Drawing.Point(321, 45);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(177, 129);
            groupBox1.TabIndex = 6;
            groupBox1.TabStop = false;
            groupBox1.Text = "出力対象";
            // 
            // label5
            // 
            label5.BackColor = System.Drawing.SystemColors.ScrollBar;
            label5.Location = new System.Drawing.Point(14, 81);
            label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label5.Name = "label5";
            label5.Size = new System.Drawing.Size(155, 4);
            label5.TabIndex = 5;
            // 
            // rbtnOnlyExif
            // 
            rbtnOnlyExif.AutoSize = true;
            rbtnOnlyExif.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnOnlyExif.Location = new System.Drawing.Point(21, 109);
            rbtnOnlyExif.Margin = new System.Windows.Forms.Padding(4);
            rbtnOnlyExif.Name = "rbtnOnlyExif";
            rbtnOnlyExif.Size = new System.Drawing.Size(129, 19);
            rbtnOnlyExif.TabIndex = 4;
            rbtnOnlyExif.TabStop = true;
            rbtnOnlyExif.Text = "Exifを持つファイルのみ";
            rbtnOnlyExif.UseVisualStyleBackColor = true;
            // 
            // rbtnOnlyImage
            // 
            rbtnOnlyImage.AutoSize = true;
            rbtnOnlyImage.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnOnlyImage.Location = new System.Drawing.Point(9, 88);
            rbtnOnlyImage.Margin = new System.Windows.Forms.Padding(4);
            rbtnOnlyImage.Name = "rbtnOnlyImage";
            rbtnOnlyImage.Size = new System.Drawing.Size(104, 19);
            rbtnOnlyImage.TabIndex = 3;
            rbtnOnlyImage.TabStop = true;
            rbtnOnlyImage.Text = "画像ファイルのみ";
            rbtnOnlyImage.UseVisualStyleBackColor = true;
            // 
            // rbtnIncldDir
            // 
            rbtnIncldDir.AutoSize = true;
            rbtnIncldDir.Checked = true;
            rbtnIncldDir.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnIncldDir.Location = new System.Drawing.Point(9, 18);
            rbtnIncldDir.Margin = new System.Windows.Forms.Padding(4);
            rbtnIncldDir.Name = "rbtnIncldDir";
            rbtnIncldDir.Size = new System.Drawing.Size(121, 19);
            rbtnIncldDir.TabIndex = 0;
            rbtnIncldDir.TabStop = true;
            rbtnIncldDir.Text = "ディレクトリ＋ファイル";
            rbtnIncldDir.UseVisualStyleBackColor = true;
            // 
            // rbtnOnlyFiles
            // 
            rbtnOnlyFiles.AutoSize = true;
            rbtnOnlyFiles.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnOnlyFiles.Location = new System.Drawing.Point(9, 39);
            rbtnOnlyFiles.Margin = new System.Windows.Forms.Padding(4);
            rbtnOnlyFiles.Name = "rbtnOnlyFiles";
            rbtnOnlyFiles.Size = new System.Drawing.Size(80, 19);
            rbtnOnlyFiles.TabIndex = 1;
            rbtnOnlyFiles.Text = "ファイルのみ";
            rbtnOnlyFiles.UseVisualStyleBackColor = true;
            // 
            // rbtnOnlyDir
            // 
            rbtnOnlyDir.AutoSize = true;
            rbtnOnlyDir.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnOnlyDir.Location = new System.Drawing.Point(9, 60);
            rbtnOnlyDir.Margin = new System.Windows.Forms.Padding(4);
            rbtnOnlyDir.Name = "rbtnOnlyDir";
            rbtnOnlyDir.Size = new System.Drawing.Size(96, 19);
            rbtnOnlyDir.TabIndex = 2;
            rbtnOnlyDir.Text = "ディレクトリのみ";
            rbtnOnlyDir.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new System.Drawing.Point(136, 79);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(68, 15);
            label2.TabIndex = 7;
            label2.Text = "ファイルマスク";
            // 
            // txtMask
            // 
            txtMask.Location = new System.Drawing.Point(219, 75);
            txtMask.Margin = new System.Windows.Forms.Padding(4);
            txtMask.Name = "txtMask";
            txtMask.Size = new System.Drawing.Size(93, 23);
            txtMask.TabIndex = 8;
            txtMask.Text = "*.*";
            txtMask.TextChanged += txtMask_TextChanged;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new System.Drawing.Point(14, 111);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(43, 15);
            label3.TabIndex = 9;
            label3.Text = "探索数";
            // 
            // lblRows
            // 
            lblRows.AutoSize = true;
            lblRows.Location = new System.Drawing.Point(65, 111);
            lblRows.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblRows.Name = "lblRows";
            lblRows.Size = new System.Drawing.Size(13, 15);
            lblRows.TabIndex = 10;
            lblRows.Text = "0";
            // 
            // btnFind
            // 
            btnFind.BackColor = System.Drawing.SystemColors.Control;
            btnFind.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Bold);
            btnFind.Location = new System.Drawing.Point(6, 44);
            btnFind.Margin = new System.Windows.Forms.Padding(4);
            btnFind.Name = "btnFind";
            btnFind.Size = new System.Drawing.Size(124, 55);
            btnFind.TabIndex = 11;
            btnFind.Text = "探索開始";
            btnFind.UseVisualStyleBackColor = false;
            btnFind.Click += btnFind_Click;
            // 
            // btnOut2File
            // 
            btnOut2File.BackColor = System.Drawing.SystemColors.Control;
            btnOut2File.Location = new System.Drawing.Point(161, 133);
            btnOut2File.Margin = new System.Windows.Forms.Padding(4);
            btnOut2File.Name = "btnOut2File";
            btnOut2File.Size = new System.Drawing.Size(153, 29);
            btnOut2File.TabIndex = 12;
            btnOut2File.Text = "結果をファイルに出力";
            btnOut2File.UseVisualStyleBackColor = false;
            btnOut2File.Click += btnOut2File_Click;
            // 
            // groupBox2
            // 
            groupBox2.Controls.Add(rbtnTab);
            groupBox2.Controls.Add(rbtnCSV);
            groupBox2.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            groupBox2.Location = new System.Drawing.Point(323, 179);
            groupBox2.Margin = new System.Windows.Forms.Padding(4);
            groupBox2.Name = "groupBox2";
            groupBox2.Padding = new System.Windows.Forms.Padding(4);
            groupBox2.Size = new System.Drawing.Size(177, 70);
            groupBox2.TabIndex = 13;
            groupBox2.TabStop = false;
            groupBox2.Text = "出力テキスト形式";
            // 
            // rbtnTab
            // 
            rbtnTab.AutoSize = true;
            rbtnTab.Checked = true;
            rbtnTab.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnTab.Location = new System.Drawing.Point(13, 19);
            rbtnTab.Margin = new System.Windows.Forms.Padding(4);
            rbtnTab.Name = "rbtnTab";
            rbtnTab.Size = new System.Drawing.Size(75, 19);
            rbtnTab.TabIndex = 1;
            rbtnTab.TabStop = true;
            rbtnTab.Text = "タブ区切り";
            rbtnTab.UseVisualStyleBackColor = true;
            rbtnTab.CheckedChanged += rbtnTab_CheckedChanged;
            // 
            // rbtnCSV
            // 
            rbtnCSV.AutoSize = true;
            rbtnCSV.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnCSV.Location = new System.Drawing.Point(13, 46);
            rbtnCSV.Margin = new System.Windows.Forms.Padding(4);
            rbtnCSV.Name = "rbtnCSV";
            rbtnCSV.Size = new System.Drawing.Size(144, 19);
            rbtnCSV.TabIndex = 0;
            rbtnCSV.Text = "CSV形式(カンマ区切り）";
            rbtnCSV.UseVisualStyleBackColor = true;
            rbtnCSV.CheckedChanged += rbtnCSV_CheckedChanged;
            // 
            // chkDirMask
            // 
            chkDirMask.AutoSize = true;
            chkDirMask.Location = new System.Drawing.Point(181, 105);
            chkDirMask.Margin = new System.Windows.Forms.Padding(4);
            chkDirMask.Name = "chkDirMask";
            chkDirMask.Size = new System.Drawing.Size(131, 19);
            chkDirMask.TabIndex = 14;
            chkDirMask.Text = "ディレクトリもマスクする";
            chkDirMask.UseVisualStyleBackColor = true;
            chkDirMask.CheckedChanged += chkDirMask_CheckedChanged;
            // 
            // lblOutFile
            // 
            lblOutFile.AutoEllipsis = true;
            lblOutFile.Location = new System.Drawing.Point(6, 182);
            lblOutFile.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblOutFile.Name = "lblOutFile";
            lblOutFile.Size = new System.Drawing.Size(301, 39);
            lblOutFile.TabIndex = 15;
            lblOutFile.Text = "出力ファイル名";
            // 
            // txtComment
            // 
            txtComment.Location = new System.Drawing.Point(9, 240);
            txtComment.Margin = new System.Windows.Forms.Padding(4);
            txtComment.Multiline = true;
            txtComment.Name = "txtComment";
            txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtComment.Size = new System.Drawing.Size(305, 273);
            txtComment.TabIndex = 16;
            txtComment.Text = "ファイル情報出力プログラム\r\n\r\n写真のExif情報をExcelに出力する\r\nExif情報が不要の場合は高速のファイルリスト作成アプリ\r\n\r\n使い方\r\n1.フォルダーを選択\r\n2.出力項目の選択\r\n3.探索開始\r\n4.クリップボードにコピー\r\n5.ExcelにCtr+v などでコピー\r\n\r\nExcelでは区切位置の指定が、コンマやタブだけになっていないとデータがずれます";
            // 
            // btnExit
            // 
            btnExit.Location = new System.Drawing.Point(537, 604);
            btnExit.Margin = new System.Windows.Forms.Padding(4);
            btnExit.Name = "btnExit";
            btnExit.Size = new System.Drawing.Size(128, 39);
            btnExit.TabIndex = 17;
            btnExit.Text = "設定を保存して終了";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // groupBox3
            // 
            groupBox3.Controls.Add(chkExt);
            groupBox3.Controls.Add(btnSetExif);
            groupBox3.Controls.Add(chkExif);
            groupBox3.Controls.Add(chkAttribute);
            groupBox3.Controls.Add(chkSize);
            groupBox3.Controls.Add(chkLastAccessTime);
            groupBox3.Controls.Add(chkCreationTime);
            groupBox3.Controls.Add(chkLastWriteTime);
            groupBox3.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            groupBox3.Location = new System.Drawing.Point(518, 10);
            groupBox3.Margin = new System.Windows.Forms.Padding(4);
            groupBox3.Name = "groupBox3";
            groupBox3.Padding = new System.Windows.Forms.Padding(4);
            groupBox3.Size = new System.Drawing.Size(149, 239);
            groupBox3.TabIndex = 18;
            groupBox3.TabStop = false;
            groupBox3.Text = "出力項目の選択";
            // 
            // chkExt
            // 
            chkExt.AutoSize = true;
            chkExt.Location = new System.Drawing.Point(6, 140);
            chkExt.Name = "chkExt";
            chkExt.Size = new System.Drawing.Size(70, 23);
            chkExt.TabIndex = 9;
            chkExt.Text = "拡張子";
            chkExt.UseVisualStyleBackColor = true;
            // 
            // btnSetExif
            // 
            btnSetExif.BackColor = System.Drawing.SystemColors.Control;
            btnSetExif.Font = new System.Drawing.Font("MS UI Gothic", 9F, System.Drawing.FontStyle.Bold);
            btnSetExif.Location = new System.Drawing.Point(26, 198);
            btnSetExif.Margin = new System.Windows.Forms.Padding(4);
            btnSetExif.Name = "btnSetExif";
            btnSetExif.Size = new System.Drawing.Size(114, 32);
            btnSetExif.TabIndex = 8;
            btnSetExif.Text = "Exif項目の選択";
            btnSetExif.UseVisualStyleBackColor = false;
            btnSetExif.Click += btnSetExif_Click;
            // 
            // chkExif
            // 
            chkExif.AutoSize = true;
            chkExif.Checked = true;
            chkExif.CheckState = System.Windows.Forms.CheckState.Checked;
            chkExif.Font = new System.Drawing.Font("Yu Gothic UI", 11F);
            chkExif.Location = new System.Drawing.Point(7, 175);
            chkExif.Margin = new System.Windows.Forms.Padding(4);
            chkExif.Name = "chkExif";
            chkExif.Size = new System.Drawing.Size(85, 24);
            chkExif.TabIndex = 7;
            chkExif.Text = "Exifデータ";
            chkExif.UseVisualStyleBackColor = true;
            chkExif.CheckedChanged += chkExif_CheckedChanged;
            // 
            // chkAttribute
            // 
            chkAttribute.AutoSize = true;
            chkAttribute.Checked = true;
            chkAttribute.CheckState = System.Windows.Forms.CheckState.Checked;
            chkAttribute.Location = new System.Drawing.Point(7, 116);
            chkAttribute.Margin = new System.Windows.Forms.Padding(4);
            chkAttribute.Name = "chkAttribute";
            chkAttribute.Size = new System.Drawing.Size(97, 23);
            chkAttribute.TabIndex = 6;
            chkAttribute.Text = "ファイル属性";
            chkAttribute.UseVisualStyleBackColor = true;
            // 
            // chkSize
            // 
            chkSize.AutoSize = true;
            chkSize.Checked = true;
            chkSize.CheckState = System.Windows.Forms.CheckState.Checked;
            chkSize.Location = new System.Drawing.Point(7, 92);
            chkSize.Margin = new System.Windows.Forms.Padding(4);
            chkSize.Name = "chkSize";
            chkSize.Size = new System.Drawing.Size(100, 23);
            chkSize.TabIndex = 5;
            chkSize.Text = "ファイルサイズ";
            chkSize.UseVisualStyleBackColor = true;
            // 
            // chkLastAccessTime
            // 
            chkLastAccessTime.AutoSize = true;
            chkLastAccessTime.Location = new System.Drawing.Point(7, 68);
            chkLastAccessTime.Margin = new System.Windows.Forms.Padding(4);
            chkLastAccessTime.Name = "chkLastAccessTime";
            chkLastAccessTime.Size = new System.Drawing.Size(128, 23);
            chkLastAccessTime.TabIndex = 4;
            chkLastAccessTime.Text = "最終アクセス日時";
            chkLastAccessTime.UseVisualStyleBackColor = true;
            // 
            // chkCreationTime
            // 
            chkCreationTime.AutoSize = true;
            chkCreationTime.Checked = true;
            chkCreationTime.CheckState = System.Windows.Forms.CheckState.Checked;
            chkCreationTime.Location = new System.Drawing.Point(7, 44);
            chkCreationTime.Margin = new System.Windows.Forms.Padding(4);
            chkCreationTime.Name = "chkCreationTime";
            chkCreationTime.Size = new System.Drawing.Size(84, 23);
            chkCreationTime.TabIndex = 3;
            chkCreationTime.Text = "作成日時";
            chkCreationTime.UseVisualStyleBackColor = true;
            // 
            // chkLastWriteTime
            // 
            chkLastWriteTime.AutoSize = true;
            chkLastWriteTime.Checked = true;
            chkLastWriteTime.CheckState = System.Windows.Forms.CheckState.Checked;
            chkLastWriteTime.Location = new System.Drawing.Point(7, 20);
            chkLastWriteTime.Margin = new System.Windows.Forms.Padding(4);
            chkLastWriteTime.Name = "chkLastWriteTime";
            chkLastWriteTime.Size = new System.Drawing.Size(84, 23);
            chkLastWriteTime.TabIndex = 2;
            chkLastWriteTime.Text = "更新日時";
            chkLastWriteTime.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new System.Drawing.Point(6, 221);
            label6.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label6.Name = "label6";
            label6.Size = new System.Drawing.Size(87, 15);
            label6.TabIndex = 19;
            label6.Text = "記事、出力項目";
            // 
            // lblQty
            // 
            lblQty.AutoSize = true;
            lblQty.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            lblQty.Location = new System.Drawing.Point(9, 517);
            lblQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblQty.Name = "lblQty";
            lblQty.Size = new System.Drawing.Size(92, 19);
            lblQty.TabIndex = 20;
            lblQty.Text = "出力ファイル数";
            // 
            // progressBar1
            // 
            progressBar1.Location = new System.Drawing.Point(6, 609);
            progressBar1.Margin = new System.Windows.Forms.Padding(4);
            progressBar1.Minimum = 100;
            progressBar1.Name = "progressBar1";
            progressBar1.Size = new System.Drawing.Size(454, 34);
            progressBar1.TabIndex = 21;
            progressBar1.Value = 100;
            // 
            // lblPrcsdQty
            // 
            lblPrcsdQty.AutoSize = true;
            lblPrcsdQty.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            lblPrcsdQty.Location = new System.Drawing.Point(30, 536);
            lblPrcsdQty.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            lblPrcsdQty.Name = "lblPrcsdQty";
            lblPrcsdQty.Size = new System.Drawing.Size(19, 21);
            lblPrcsdQty.TabIndex = 22;
            lblPrcsdQty.Text = "0";
            // 
            // groupBoxExifExt
            // 
            groupBoxExifExt.Controls.Add(label9);
            groupBoxExifExt.Controls.Add(label4);
            groupBoxExifExt.Controls.Add(txtAdditionalExifExtLists);
            groupBoxExifExt.Controls.Add(chkExifTextExt);
            groupBoxExifExt.Controls.Add(chkSubExifExt);
            groupBoxExifExt.Controls.Add(rbtnExtSelect);
            groupBoxExifExt.Controls.Add(chkMainExifExt);
            groupBoxExifExt.Controls.Add(label1);
            groupBoxExifExt.Controls.Add(rbtnAllExt);
            groupBoxExifExt.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            groupBoxExifExt.Location = new System.Drawing.Point(320, 369);
            groupBoxExifExt.Margin = new System.Windows.Forms.Padding(4);
            groupBoxExifExt.Name = "groupBoxExifExt";
            groupBoxExifExt.Padding = new System.Windows.Forms.Padding(4);
            groupBoxExifExt.Size = new System.Drawing.Size(346, 232);
            groupBoxExifExt.TabIndex = 23;
            groupBoxExifExt.TabStop = false;
            groupBoxExifExt.Text = "ExifToolで調べる拡張子";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label9.Location = new System.Drawing.Point(203, 129);
            label9.Name = "label9";
            label9.Size = new System.Drawing.Size(121, 30);
            label9.TabIndex = 6;
            label9.Text = "Exifを持つ可能性のある\r\n特殊な静止画";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label4.Location = new System.Drawing.Point(238, 110);
            label4.Name = "label4";
            label4.Size = new System.Drawing.Size(86, 15);
            label4.TabIndex = 5;
            label4.Text = "RAWファイルなど";
            // 
            // txtAdditionalExifExtLists
            // 
            txtAdditionalExifExtLists.Location = new System.Drawing.Point(40, 189);
            txtAdditionalExifExtLists.Margin = new System.Windows.Forms.Padding(4);
            txtAdditionalExifExtLists.Multiline = true;
            txtAdditionalExifExtLists.Name = "txtAdditionalExifExtLists";
            txtAdditionalExifExtLists.Size = new System.Drawing.Size(247, 38);
            txtAdditionalExifExtLists.TabIndex = 4;
            txtAdditionalExifExtLists.Text = "avif,exif,hdp,mp4,mov,3gp,mjpg,mjpeg";
            // 
            // chkExifTextExt
            // 
            chkExifTextExt.AutoSize = true;
            chkExifTextExt.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            chkExifTextExt.Location = new System.Drawing.Point(20, 162);
            chkExifTextExt.Margin = new System.Windows.Forms.Padding(4);
            chkExifTextExt.Name = "chkExifTextExt";
            chkExifTextExt.Size = new System.Drawing.Size(222, 19);
            chkExifTextExt.TabIndex = 3;
            chkExifTextExt.Text = "その他必要に応じてカンマで区切って入力";
            chkExifTextExt.UseVisualStyleBackColor = true;
            // 
            // chkSubExifExt
            // 
            chkSubExifExt.AutoSize = true;
            chkSubExifExt.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            chkSubExifExt.Location = new System.Drawing.Point(22, 125);
            chkSubExifExt.Margin = new System.Windows.Forms.Padding(4);
            chkSubExifExt.Name = "chkSubExifExt";
            chkSubExifExt.Size = new System.Drawing.Size(153, 19);
            chkSubExifExt.TabIndex = 2;
            chkSubExifExt.Text = "heic,heif,webp,j6i,jpegxr";
            chkSubExifExt.UseVisualStyleBackColor = true;
            // 
            // rbtnExtSelect
            // 
            rbtnExtSelect.AutoSize = true;
            rbtnExtSelect.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnExtSelect.Location = new System.Drawing.Point(6, 88);
            rbtnExtSelect.Margin = new System.Windows.Forms.Padding(4);
            rbtnExtSelect.Name = "rbtnExtSelect";
            rbtnExtSelect.Size = new System.Drawing.Size(210, 19);
            rbtnExtSelect.TabIndex = 2;
            rbtnExtSelect.TabStop = true;
            rbtnExtSelect.Text = "拡張子選択(既定: JPG,JPEG,TIF,TIFF)";
            rbtnExtSelect.UseVisualStyleBackColor = true;
            // 
            // chkMainExifExt
            // 
            chkMainExifExt.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            chkMainExifExt.Location = new System.Drawing.Point(22, 106);
            chkMainExifExt.Margin = new System.Windows.Forms.Padding(4);
            chkMainExifExt.Name = "chkMainExifExt";
            chkMainExifExt.Size = new System.Drawing.Size(216, 19);
            chkMainExifExt.TabIndex = 1;
            chkMainExifExt.Text = "dng,raw,cr2,cr3,nef,arw,rw2,orf,pef,x3f";
            chkMainExifExt.UseVisualStyleBackColor = true;
            // 
            // label1
            // 
            label1.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label1.Location = new System.Drawing.Point(6, 20);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(331, 46);
            label1.TabIndex = 1;
            label1.Text = "EXIF探索はファイルを読み込むので時間がかかるのでExifを持つファイルのみ指定したほうが速度が速くなる\r\nなお、GIFやBMP形式はEXIFを持たない";
            // 
            // rbtnAllExt
            // 
            rbtnAllExt.AutoSize = true;
            rbtnAllExt.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbtnAllExt.Location = new System.Drawing.Point(6, 70);
            rbtnAllExt.Margin = new System.Windows.Forms.Padding(4);
            rbtnAllExt.Name = "rbtnAllExt";
            rbtnAllExt.Size = new System.Drawing.Size(247, 19);
            rbtnAllExt.TabIndex = 0;
            rbtnAllExt.TabStop = true;
            rbtnAllExt.Text = "全拡張子（Exifを持たないものでも調べる））";
            rbtnAllExt.UseVisualStyleBackColor = true;
            // 
            // groupBox4
            // 
            groupBox4.Controls.Add(label8);
            groupBox4.Controls.Add(rbExifToolAllways);
            groupBox4.Controls.Add(rbExiToolIfNeed);
            groupBox4.Controls.Add(rbExifToolDonotUse);
            groupBox4.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            groupBox4.Location = new System.Drawing.Point(321, 251);
            groupBox4.Name = "groupBox4";
            groupBox4.Size = new System.Drawing.Size(346, 111);
            groupBox4.TabIndex = 1;
            groupBox4.TabStop = false;
            groupBox4.Text = "Exif調査方法";
            // 
            // label8
            // 
            label8.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            label8.Location = new System.Drawing.Point(8, 19);
            label8.Name = "label8";
            label8.Size = new System.Drawing.Size(331, 60);
            label8.TabIndex = 3;
            label8.Text = "ExifはWindowsAPIが比較的早いがJpegのほぼすべてとTIFF形式の一部にしか対応していない\r\nExifToolはRAW形式をはじめ多くのファイル形式に対応しているが処理に時間がかかる";
            // 
            // rbExifToolAllways
            // 
            rbExifToolAllways.AutoSize = true;
            rbExifToolAllways.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbExifToolAllways.Location = new System.Drawing.Point(250, 82);
            rbExifToolAllways.Name = "rbExifToolAllways";
            rbExifToolAllways.Size = new System.Drawing.Size(87, 19);
            rbExifToolAllways.TabIndex = 2;
            rbExifToolAllways.TabStop = true;
            rbExifToolAllways.Text = "ExifToolのみ";
            rbExifToolAllways.UseVisualStyleBackColor = true;
            rbExifToolAllways.CheckedChanged += rbExifToolAllways_CheckedChanged;
            // 
            // rbExiToolIfNeed
            // 
            rbExiToolIfNeed.AutoSize = true;
            rbExiToolIfNeed.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbExiToolIfNeed.Location = new System.Drawing.Point(132, 82);
            rbExiToolIfNeed.Name = "rbExiToolIfNeed";
            rbExiToolIfNeed.Size = new System.Drawing.Size(99, 19);
            rbExiToolIfNeed.TabIndex = 1;
            rbExiToolIfNeed.TabStop = true;
            rbExiToolIfNeed.Text = "ExifToolも使用";
            rbExiToolIfNeed.UseVisualStyleBackColor = true;
            rbExiToolIfNeed.CheckedChanged += rbExiToolIfNeed_CheckedChanged;
            // 
            // rbExifToolDonotUse
            // 
            rbExifToolDonotUse.AutoSize = true;
            rbExifToolDonotUse.Font = new System.Drawing.Font("Yu Gothic UI", 9F);
            rbExifToolDonotUse.Location = new System.Drawing.Point(6, 82);
            rbExifToolDonotUse.Name = "rbExifToolDonotUse";
            rbExifToolDonotUse.Size = new System.Drawing.Size(113, 19);
            rbExifToolDonotUse.TabIndex = 0;
            rbExifToolDonotUse.TabStop = true;
            rbExifToolDonotUse.Text = "WindowsAPIのみ";
            rbExifToolDonotUse.UseVisualStyleBackColor = true;
            rbExifToolDonotUse.CheckedChanged += rbExifToolDonotUse_CheckedChanged;
            // 
            // lblEstTime
            // 
            lblEstTime.AutoSize = true;
            lblEstTime.Font = new System.Drawing.Font("Yu Gothic UI", 10F);
            lblEstTime.Location = new System.Drawing.Point(9, 565);
            lblEstTime.Name = "lblEstTime";
            lblEstTime.Size = new System.Drawing.Size(89, 19);
            lblEstTime.TabIndex = 24;
            lblEstTime.Text = "残り予想時間";
            // 
            // lblRmningTime
            // 
            lblRmningTime.AutoSize = true;
            lblRmningTime.Font = new System.Drawing.Font("Yu Gothic UI", 12F);
            lblRmningTime.Location = new System.Drawing.Point(30, 584);
            lblRmningTime.Name = "lblRmningTime";
            lblRmningTime.Size = new System.Drawing.Size(34, 21);
            lblRmningTime.TabIndex = 27;
            lblRmningTime.Text = "----";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.KUPマーク123x60;
            pictureBox1.Location = new System.Drawing.Point(467, 603);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(63, 45);
            pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 28;
            pictureBox1.TabStop = false;
            // 
            // Form1
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            BackColor = System.Drawing.SystemColors.Control;
            ClientSize = new System.Drawing.Size(673, 647);
            Controls.Add(pictureBox1);
            Controls.Add(lblRmningTime);
            Controls.Add(lblEstTime);
            Controls.Add(groupBox4);
            Controls.Add(groupBoxExifExt);
            Controls.Add(lblPrcsdQty);
            Controls.Add(progressBar1);
            Controls.Add(lblQty);
            Controls.Add(label6);
            Controls.Add(groupBox3);
            Controls.Add(btnExit);
            Controls.Add(txtComment);
            Controls.Add(lblOutFile);
            Controls.Add(chkDirMask);
            Controls.Add(groupBox2);
            Controls.Add(btnOut2File);
            Controls.Add(btnFind);
            Controls.Add(lblRows);
            Controls.Add(label3);
            Controls.Add(txtMask);
            Controls.Add(label2);
            Controls.Add(groupBox1);
            Controls.Add(chkSubDir);
            Controls.Add(btnCopy2Clip);
            Controls.Add(lblSearchPath);
            Controls.Add(btnFolderSelect);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            MinimumSize = new System.Drawing.Size(585, 580);
            Name = "Form1";
            Text = "FileList(Exif)";
            Load += Form1_Load;
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            groupBox2.ResumeLayout(false);
            groupBox2.PerformLayout();
            groupBox3.ResumeLayout(false);
            groupBox3.PerformLayout();
            groupBoxExifExt.ResumeLayout(false);
            groupBoxExifExt.PerformLayout();
            groupBox4.ResumeLayout(false);
            groupBox4.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog1;
        private System.Windows.Forms.Button btnFolderSelect;
        private System.Windows.Forms.Label lblSearchPath;
        private System.Windows.Forms.Button btnCopy2Clip;
        private System.Windows.Forms.CheckBox chkSubDir;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox txtMask;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblRows;
        private System.Windows.Forms.Button btnFind;
        private System.Windows.Forms.Button btnOut2File;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton rbtnTab;
        private System.Windows.Forms.RadioButton rbtnCSV;
        private System.Windows.Forms.CheckBox chkDirMask;
        private System.Windows.Forms.Label lblOutFile;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.TextBox txtComment;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.CheckBox chkLastAccessTime;
        private System.Windows.Forms.CheckBox chkCreationTime;
        private System.Windows.Forms.CheckBox chkLastWriteTime;
        private System.Windows.Forms.Button btnSetExif;
        private System.Windows.Forms.CheckBox chkExif;
        private System.Windows.Forms.CheckBox chkAttribute;
        private System.Windows.Forms.CheckBox chkSize;
        private System.Windows.Forms.RadioButton rbtnOnlyImage;
        private System.Windows.Forms.RadioButton rbtnIncldDir;
        private System.Windows.Forms.RadioButton rbtnOnlyFiles;
        private System.Windows.Forms.RadioButton rbtnOnlyDir;
        private System.Windows.Forms.RadioButton rbtnOnlyExif;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label lblQty;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblPrcsdQty;
        private System.Windows.Forms.GroupBox groupBoxExifExt;
        private System.Windows.Forms.RadioButton rbtnAllExt;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.RadioButton rbtnExtSelect;
        private System.Windows.Forms.CheckBox chkExifTextExt;
        private System.Windows.Forms.CheckBox chkSubExifExt;
        private System.Windows.Forms.CheckBox chkMainExifExt;
        private System.Windows.Forms.TextBox txtAdditionalExifExtLists;
        private System.Windows.Forms.GroupBox groupBox4;
        private System.Windows.Forms.RadioButton rbExifToolDonotUse;
        private System.Windows.Forms.RadioButton rbExifToolAllways;
        private System.Windows.Forms.RadioButton rbExiToolIfNeed;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.CheckBox chkExt;
        private System.Windows.Forms.Label lblEstTime;
        private System.Windows.Forms.Label lblRmningTime;
        private System.Windows.Forms.PictureBox pictureBox1;
    }
}

