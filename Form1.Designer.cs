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
            this.button1 = new System.Windows.Forms.Button();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.folderBrowserDialog1 = new System.Windows.Forms.FolderBrowserDialog();
            this.btnFolderSelect = new System.Windows.Forms.Button();
            this.lblSearchPath = new System.Windows.Forms.Label();
            this.btnCopy2Clip = new System.Windows.Forms.Button();
            this.chkSubDir = new System.Windows.Forms.CheckBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label5 = new System.Windows.Forms.Label();
            this.rbtnOnlyExif = new System.Windows.Forms.RadioButton();
            this.rbtnOnlyImage = new System.Windows.Forms.RadioButton();
            this.rbtnIncldDir = new System.Windows.Forms.RadioButton();
            this.rbtnOnlyFiles = new System.Windows.Forms.RadioButton();
            this.rbtnOnlyDir = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.txtMask = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.lblRows = new System.Windows.Forms.Label();
            this.btnFind = new System.Windows.Forms.Button();
            this.btnOut2File = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.rbtnTab = new System.Windows.Forms.RadioButton();
            this.rbtnCSV = new System.Windows.Forms.RadioButton();
            this.chkDirMask = new System.Windows.Forms.CheckBox();
            this.lblOutFile = new System.Windows.Forms.Label();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.txtComment = new System.Windows.Forms.TextBox();
            this.btnExit = new System.Windows.Forms.Button();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.btnSetExif = new System.Windows.Forms.Button();
            this.chkExif = new System.Windows.Forms.CheckBox();
            this.chkAttribute = new System.Windows.Forms.CheckBox();
            this.chkSize = new System.Windows.Forms.CheckBox();
            this.chkLastAccessTime = new System.Windows.Forms.CheckBox();
            this.chkCreationTime = new System.Windows.Forms.CheckBox();
            this.chkLastWriteTime = new System.Windows.Forms.CheckBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.progressBar1 = new System.Windows.Forms.ProgressBar();
            this.lblQty = new System.Windows.Forms.Label();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(445, 506);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(99, 45);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(14, 474);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(322, 19);
            this.textBox1.TabIndex = 1;
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(12, 499);
            this.textBox2.Multiline = true;
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new System.Drawing.Size(242, 52);
            this.textBox2.TabIndex = 2;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(259, 521);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(92, 30);
            this.button2.TabIndex = 3;
            this.button2.Text = "button2";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(357, 517);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(82, 28);
            this.button3.TabIndex = 4;
            this.button3.Text = "button3";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.button3_Click);
            // 
            // btnFolderSelect
            // 
            this.btnFolderSelect.Location = new System.Drawing.Point(5, 1);
            this.btnFolderSelect.Name = "btnFolderSelect";
            this.btnFolderSelect.Size = new System.Drawing.Size(94, 24);
            this.btnFolderSelect.TabIndex = 0;
            this.btnFolderSelect.Text = "フォルダーの選択";
            this.btnFolderSelect.UseVisualStyleBackColor = true;
            this.btnFolderSelect.Click += new System.EventHandler(this.btnFolderSelect_Click);
            // 
            // lblSearchPath
            // 
            this.lblSearchPath.AutoSize = true;
            this.lblSearchPath.Location = new System.Drawing.Point(105, 7);
            this.lblSearchPath.Name = "lblSearchPath";
            this.lblSearchPath.Size = new System.Drawing.Size(126, 12);
            this.lblSearchPath.TabIndex = 1;
            this.lblSearchPath.Text = "フォルダ名を選んでください";
            // 
            // btnCopy2Clip
            // 
            this.btnCopy2Clip.Location = new System.Drawing.Point(5, 103);
            this.btnCopy2Clip.Name = "btnCopy2Clip";
            this.btnCopy2Clip.Size = new System.Drawing.Size(110, 23);
            this.btnCopy2Clip.TabIndex = 4;
            this.btnCopy2Clip.Text = "クリップボードにコピー";
            this.btnCopy2Clip.UseVisualStyleBackColor = true;
            this.btnCopy2Clip.Click += new System.EventHandler(this.btnCopy2Clip_Click);
            // 
            // chkSubDir
            // 
            this.chkSubDir.AutoSize = true;
            this.chkSubDir.Checked = true;
            this.chkSubDir.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSubDir.Location = new System.Drawing.Point(102, 30);
            this.chkSubDir.Name = "chkSubDir";
            this.chkSubDir.Size = new System.Drawing.Size(133, 16);
            this.chkSubDir.TabIndex = 5;
            this.chkSubDir.Text = "サブディレクトリーを含む";
            this.chkSubDir.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.Control;
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.rbtnOnlyExif);
            this.groupBox1.Controls.Add(this.rbtnOnlyImage);
            this.groupBox1.Controls.Add(this.rbtnIncldDir);
            this.groupBox1.Controls.Add(this.rbtnOnlyFiles);
            this.groupBox1.Controls.Add(this.rbtnOnlyDir);
            this.groupBox1.Location = new System.Drawing.Point(276, 25);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(152, 119);
            this.groupBox1.TabIndex = 6;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "出力対象";
            // 
            // label5
            // 
            this.label5.BackColor = System.Drawing.SystemColors.ScrollBar;
            this.label5.Location = new System.Drawing.Point(12, 75);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(133, 3);
            this.label5.TabIndex = 5;
            // 
            // rbtnOnlyExif
            // 
            this.rbtnOnlyExif.AutoSize = true;
            this.rbtnOnlyExif.Location = new System.Drawing.Point(18, 100);
            this.rbtnOnlyExif.Name = "rbtnOnlyExif";
            this.rbtnOnlyExif.Size = new System.Drawing.Size(128, 16);
            this.rbtnOnlyExif.TabIndex = 4;
            this.rbtnOnlyExif.TabStop = true;
            this.rbtnOnlyExif.Text = "Exifを持つファイルのみ";
            this.rbtnOnlyExif.UseVisualStyleBackColor = true;
            // 
            // rbtnOnlyImage
            // 
            this.rbtnOnlyImage.AutoSize = true;
            this.rbtnOnlyImage.Location = new System.Drawing.Point(8, 80);
            this.rbtnOnlyImage.Name = "rbtnOnlyImage";
            this.rbtnOnlyImage.Size = new System.Drawing.Size(102, 16);
            this.rbtnOnlyImage.TabIndex = 3;
            this.rbtnOnlyImage.TabStop = true;
            this.rbtnOnlyImage.Text = "画像ファイルのみ";
            this.rbtnOnlyImage.UseVisualStyleBackColor = true;
            // 
            // rbtnIncldDir
            // 
            this.rbtnIncldDir.AutoSize = true;
            this.rbtnIncldDir.Checked = true;
            this.rbtnIncldDir.Location = new System.Drawing.Point(8, 14);
            this.rbtnIncldDir.Name = "rbtnIncldDir";
            this.rbtnIncldDir.Size = new System.Drawing.Size(118, 16);
            this.rbtnIncldDir.TabIndex = 0;
            this.rbtnIncldDir.TabStop = true;
            this.rbtnIncldDir.Text = "ディレクトリ＋ファイル";
            this.rbtnIncldDir.UseVisualStyleBackColor = true;
            // 
            // rbtnOnlyFiles
            // 
            this.rbtnOnlyFiles.AutoSize = true;
            this.rbtnOnlyFiles.Location = new System.Drawing.Point(8, 34);
            this.rbtnOnlyFiles.Name = "rbtnOnlyFiles";
            this.rbtnOnlyFiles.Size = new System.Drawing.Size(78, 16);
            this.rbtnOnlyFiles.TabIndex = 1;
            this.rbtnOnlyFiles.Text = "ファイルのみ";
            this.rbtnOnlyFiles.UseVisualStyleBackColor = true;
            // 
            // rbtnOnlyDir
            // 
            this.rbtnOnlyDir.AutoSize = true;
            this.rbtnOnlyDir.Location = new System.Drawing.Point(8, 54);
            this.rbtnOnlyDir.Name = "rbtnOnlyDir";
            this.rbtnOnlyDir.Size = new System.Drawing.Size(93, 16);
            this.rbtnOnlyDir.TabIndex = 2;
            this.rbtnOnlyDir.Text = "ディレクトリのみ";
            this.rbtnOnlyDir.UseVisualStyleBackColor = true;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(104, 50);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(65, 12);
            this.label2.TabIndex = 7;
            this.label2.Text = "ファイルマスク";
            // 
            // txtMask
            // 
            this.txtMask.Location = new System.Drawing.Point(175, 47);
            this.txtMask.Name = "txtMask";
            this.txtMask.Size = new System.Drawing.Size(93, 19);
            this.txtMask.TabIndex = 8;
            this.txtMask.Text = "*.*";
            this.txtMask.TextChanged += new System.EventHandler(this.txtMask_TextChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(12, 73);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 12);
            this.label3.TabIndex = 9;
            this.label3.Text = "探索数";
            // 
            // lblRows
            // 
            this.lblRows.AutoSize = true;
            this.lblRows.Location = new System.Drawing.Point(69, 72);
            this.lblRows.Name = "lblRows";
            this.lblRows.Size = new System.Drawing.Size(11, 12);
            this.lblRows.TabIndex = 10;
            this.lblRows.Text = "0";
            // 
            // btnFind
            // 
            this.btnFind.Location = new System.Drawing.Point(5, 36);
            this.btnFind.Name = "btnFind";
            this.btnFind.Size = new System.Drawing.Size(75, 23);
            this.btnFind.TabIndex = 11;
            this.btnFind.Text = "探索開始";
            this.btnFind.UseVisualStyleBackColor = true;
            this.btnFind.Click += new System.EventHandler(this.btnFind_Click);
            // 
            // btnOut2File
            // 
            this.btnOut2File.Location = new System.Drawing.Point(5, 132);
            this.btnOut2File.Name = "btnOut2File";
            this.btnOut2File.Size = new System.Drawing.Size(131, 23);
            this.btnOut2File.TabIndex = 12;
            this.btnOut2File.Text = "結果をファイルに出力";
            this.btnOut2File.UseVisualStyleBackColor = true;
            this.btnOut2File.Click += new System.EventHandler(this.btnOut2File_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.rbtnTab);
            this.groupBox2.Controls.Add(this.rbtnCSV);
            this.groupBox2.Location = new System.Drawing.Point(276, 150);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(152, 56);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "出力テキスト形式";
            // 
            // rbtnTab
            // 
            this.rbtnTab.AutoSize = true;
            this.rbtnTab.Checked = true;
            this.rbtnTab.Location = new System.Drawing.Point(11, 15);
            this.rbtnTab.Name = "rbtnTab";
            this.rbtnTab.Size = new System.Drawing.Size(72, 16);
            this.rbtnTab.TabIndex = 1;
            this.rbtnTab.TabStop = true;
            this.rbtnTab.Text = "タブ区切り";
            this.rbtnTab.UseVisualStyleBackColor = true;
            this.rbtnTab.CheckedChanged += new System.EventHandler(this.rbtnTab_CheckedChanged);
            // 
            // rbtnCSV
            // 
            this.rbtnCSV.AutoSize = true;
            this.rbtnCSV.Location = new System.Drawing.Point(11, 37);
            this.rbtnCSV.Name = "rbtnCSV";
            this.rbtnCSV.Size = new System.Drawing.Size(139, 16);
            this.rbtnCSV.TabIndex = 0;
            this.rbtnCSV.Text = "CSV形式(カンマ区切り）";
            this.rbtnCSV.UseVisualStyleBackColor = true;
            this.rbtnCSV.CheckedChanged += new System.EventHandler(this.rbtnCSV_CheckedChanged);
            // 
            // chkDirMask
            // 
            this.chkDirMask.AutoSize = true;
            this.chkDirMask.Location = new System.Drawing.Point(142, 72);
            this.chkDirMask.Name = "chkDirMask";
            this.chkDirMask.Size = new System.Drawing.Size(127, 16);
            this.chkDirMask.TabIndex = 14;
            this.chkDirMask.Text = "ディレクトリもマスクする";
            this.chkDirMask.UseVisualStyleBackColor = true;
            this.chkDirMask.CheckedChanged += new System.EventHandler(this.chkDirMask_CheckedChanged);
            // 
            // lblOutFile
            // 
            this.lblOutFile.AutoSize = true;
            this.lblOutFile.Location = new System.Drawing.Point(142, 136);
            this.lblOutFile.Name = "lblOutFile";
            this.lblOutFile.Size = new System.Drawing.Size(75, 12);
            this.lblOutFile.TabIndex = 15;
            this.lblOutFile.Text = "出力ファイル名";
            // 
            // txtComment
            // 
            this.txtComment.Location = new System.Drawing.Point(5, 307);
            this.txtComment.Multiline = true;
            this.txtComment.Name = "txtComment";
            this.txtComment.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.txtComment.Size = new System.Drawing.Size(411, 149);
            this.txtComment.TabIndex = 16;
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(452, 299);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(115, 34);
            this.btnExit.TabIndex = 17;
            this.btnExit.Text = "設定を保存して終了";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.btnSetExif);
            this.groupBox3.Controls.Add(this.chkExif);
            this.groupBox3.Controls.Add(this.chkAttribute);
            this.groupBox3.Controls.Add(this.chkSize);
            this.groupBox3.Controls.Add(this.chkLastAccessTime);
            this.groupBox3.Controls.Add(this.chkCreationTime);
            this.groupBox3.Controls.Add(this.chkLastWriteTime);
            this.groupBox3.Controls.Add(this.label4);
            this.groupBox3.Controls.Add(this.label1);
            this.groupBox3.Location = new System.Drawing.Point(444, 28);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(128, 251);
            this.groupBox3.TabIndex = 18;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "出力項目の選択";
            // 
            // btnSetExif
            // 
            this.btnSetExif.Location = new System.Drawing.Point(12, 213);
            this.btnSetExif.Name = "btnSetExif";
            this.btnSetExif.Size = new System.Drawing.Size(104, 25);
            this.btnSetExif.TabIndex = 8;
            this.btnSetExif.Text = "Exif項目の選択";
            this.btnSetExif.UseVisualStyleBackColor = true;
            // 
            // chkExif
            // 
            this.chkExif.AutoSize = true;
            this.chkExif.Checked = true;
            this.chkExif.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkExif.Location = new System.Drawing.Point(6, 191);
            this.chkExif.Name = "chkExif";
            this.chkExif.Size = new System.Drawing.Size(106, 16);
            this.chkExif.TabIndex = 7;
            this.chkExif.Text = "Exifデータの表示";
            this.chkExif.UseVisualStyleBackColor = true;
            // 
            // chkAttribute
            // 
            this.chkAttribute.AutoSize = true;
            this.chkAttribute.Checked = true;
            this.chkAttribute.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAttribute.Location = new System.Drawing.Point(6, 146);
            this.chkAttribute.Name = "chkAttribute";
            this.chkAttribute.Size = new System.Drawing.Size(82, 16);
            this.chkAttribute.TabIndex = 6;
            this.chkAttribute.Text = "ファイル属性";
            this.chkAttribute.UseVisualStyleBackColor = true;
            // 
            // chkSize
            // 
            this.chkSize.AutoSize = true;
            this.chkSize.Checked = true;
            this.chkSize.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSize.Location = new System.Drawing.Point(6, 124);
            this.chkSize.Name = "chkSize";
            this.chkSize.Size = new System.Drawing.Size(87, 16);
            this.chkSize.TabIndex = 5;
            this.chkSize.Text = "ファイルサイズ";
            this.chkSize.UseVisualStyleBackColor = true;
            // 
            // chkLastAccessTime
            // 
            this.chkLastAccessTime.AutoSize = true;
            this.chkLastAccessTime.Location = new System.Drawing.Point(6, 102);
            this.chkLastAccessTime.Name = "chkLastAccessTime";
            this.chkLastAccessTime.Size = new System.Drawing.Size(108, 16);
            this.chkLastAccessTime.TabIndex = 4;
            this.chkLastAccessTime.Text = "最終アクセス日時";
            this.chkLastAccessTime.UseVisualStyleBackColor = true;
            // 
            // chkCreationTime
            // 
            this.chkCreationTime.AutoSize = true;
            this.chkCreationTime.Checked = true;
            this.chkCreationTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkCreationTime.Location = new System.Drawing.Point(6, 80);
            this.chkCreationTime.Name = "chkCreationTime";
            this.chkCreationTime.Size = new System.Drawing.Size(72, 16);
            this.chkCreationTime.TabIndex = 3;
            this.chkCreationTime.Text = "作成日時";
            this.chkCreationTime.UseVisualStyleBackColor = true;
            // 
            // chkLastWriteTime
            // 
            this.chkLastWriteTime.AutoSize = true;
            this.chkLastWriteTime.Checked = true;
            this.chkLastWriteTime.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkLastWriteTime.Location = new System.Drawing.Point(6, 58);
            this.chkLastWriteTime.Name = "chkLastWriteTime";
            this.chkLastWriteTime.Size = new System.Drawing.Size(72, 16);
            this.chkLastWriteTime.TabIndex = 2;
            this.chkLastWriteTime.Text = "更新日時";
            this.chkLastWriteTime.UseVisualStyleBackColor = true;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 40);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(64, 12);
            this.label4.TabIndex = 1;
            this.label4.Text = "レ ファイル名";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(65, 12);
            this.label1.TabIndex = 0;
            this.label1.Text = "レ フォルダ名";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(12, 283);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(85, 12);
            this.label6.TabIndex = 19;
            this.label6.Text = "記事、出力項目";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 194);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(75, 12);
            this.label7.TabIndex = 20;
            this.label7.Text = "出力ファイル数";
            // 
            // progressBar1
            // 
            this.progressBar1.Location = new System.Drawing.Point(12, 230);
            this.progressBar1.Name = "progressBar1";
            this.progressBar1.Size = new System.Drawing.Size(389, 35);
            this.progressBar1.TabIndex = 21;
            // 
            // lblQty
            // 
            this.lblQty.AutoSize = true;
            this.lblQty.Location = new System.Drawing.Point(97, 195);
            this.lblQty.Name = "lblQty";
            this.lblQty.Size = new System.Drawing.Size(11, 12);
            this.lblQty.TabIndex = 22;
            this.lblQty.Text = "0";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(584, 557);
            this.Controls.Add(this.lblQty);
            this.Controls.Add(this.progressBar1);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.groupBox3);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.txtComment);
            this.Controls.Add(this.lblOutFile);
            this.Controls.Add(this.chkDirMask);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.btnOut2File);
            this.Controls.Add(this.btnFind);
            this.Controls.Add(this.lblRows);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.txtMask);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.chkSubDir);
            this.Controls.Add(this.btnCopy2Clip);
            this.Controls.Add(this.lblSearchPath);
            this.Controls.Add(this.btnFolderSelect);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.button1);
            this.MinimumSize = new System.Drawing.Size(504, 472);
            this.Name = "Form1";
            this.Text = "FileList(Exif)";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
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
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label1;
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
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ProgressBar progressBar1;
        private System.Windows.Forms.Label lblQty;
    }
}

