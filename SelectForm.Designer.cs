namespace PictureList {
    partial class SelectForm {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing) {
            if (disposing && (components != null)) {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent() {
            System.Windows.Forms.ListViewItem listViewItem1 = new System.Windows.Forms.ListViewItem(new string[] { "Test" }, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SelectForm));
            listViewUnSelect = new System.Windows.Forms.ListView();
            property = new System.Windows.Forms.ColumnHeader();
            tagNae = new System.Windows.Forms.ColumnHeader();
            available = new System.Windows.Forms.ColumnHeader();
            listViewSelect = new System.Windows.Forms.ListView();
            groupBox1 = new System.Windows.Forms.GroupBox();
            btnDelete = new System.Windows.Forms.Button();
            btnAddLower = new System.Windows.Forms.Button();
            btnAddUpper = new System.Windows.Forms.Button();
            btnAllDelete = new System.Windows.Forms.Button();
            btnNoSave = new System.Windows.Forms.Button();
            btnExit = new System.Windows.Forms.Button();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            label3 = new System.Windows.Forms.Label();
            pictureBox1 = new System.Windows.Forms.PictureBox();
            ReaDEfaoultCSV = new System.Windows.Forms.Button();
            btnAllAdd = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // listViewUnSelect
            // 
            listViewUnSelect.BackColor = System.Drawing.SystemColors.Window;
            listViewUnSelect.CheckBoxes = true;
            listViewUnSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] { property, tagNae, available });
            listViewUnSelect.FullRowSelect = true;
            listViewUnSelect.GridLines = true;
            listViewItem1.StateImageIndex = 0;
            listViewUnSelect.Items.AddRange(new System.Windows.Forms.ListViewItem[] { listViewItem1 });
            listViewUnSelect.Location = new System.Drawing.Point(4, 59);
            listViewUnSelect.Margin = new System.Windows.Forms.Padding(4);
            listViewUnSelect.MultiSelect = false;
            listViewUnSelect.Name = "listViewUnSelect";
            listViewUnSelect.Size = new System.Drawing.Size(406, 634);
            listViewUnSelect.TabIndex = 0;
            listViewUnSelect.UseCompatibleStateImageBehavior = false;
            listViewUnSelect.View = System.Windows.Forms.View.Details;
            listViewUnSelect.ItemChecked += listViewUnSelect_ItemChecked;
            listViewUnSelect.SelectedIndexChanged += listViewUnSelect_SelectedIndexChanged;
            // 
            // property
            // 
            property.Text = "プロパティ";
            property.Width = 128;
            // 
            // tagNae
            // 
            tagNae.Text = "タグ名称";
            tagNae.Width = 128;
            // 
            // available
            // 
            available.Text = "有効";
            // 
            // listViewSelect
            // 
            listViewSelect.CheckBoxes = true;
            listViewSelect.FullRowSelect = true;
            listViewSelect.GridLines = true;
            listViewSelect.Location = new System.Drawing.Point(607, 59);
            listViewSelect.Margin = new System.Windows.Forms.Padding(4);
            listViewSelect.MultiSelect = false;
            listViewSelect.Name = "listViewSelect";
            listViewSelect.Size = new System.Drawing.Size(423, 634);
            listViewSelect.TabIndex = 1;
            listViewSelect.UseCompatibleStateImageBehavior = false;
            listViewSelect.View = System.Windows.Forms.View.Details;
            listViewSelect.ItemChecked += listViewSelect_ItemChecked;
            listViewSelect.SelectedIndexChanged += listViewSelect_SelectedIndexChanged;
            // 
            // groupBox1
            // 
            groupBox1.Controls.Add(btnDelete);
            groupBox1.Controls.Add(btnAddLower);
            groupBox1.Controls.Add(btnAddUpper);
            groupBox1.Location = new System.Drawing.Point(420, 70);
            groupBox1.Margin = new System.Windows.Forms.Padding(4);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(4);
            groupBox1.Size = new System.Drawing.Size(180, 191);
            groupBox1.TabIndex = 2;
            groupBox1.TabStop = false;
            groupBox1.Text = "選択項目を移動";
            // 
            // btnDelete
            // 
            btnDelete.Location = new System.Drawing.Point(42, 126);
            btnDelete.Margin = new System.Windows.Forms.Padding(4);
            btnDelete.Name = "btnDelete";
            btnDelete.Size = new System.Drawing.Size(93, 44);
            btnDelete.TabIndex = 2;
            btnDelete.Text = "< 削除";
            btnDelete.UseVisualStyleBackColor = true;
            btnDelete.Click += btnDelete_Click;
            // 
            // btnAddLower
            // 
            btnAddLower.Location = new System.Drawing.Point(9, 70);
            btnAddLower.Margin = new System.Windows.Forms.Padding(4);
            btnAddLower.Name = "btnAddLower";
            btnAddLower.Size = new System.Drawing.Size(159, 44);
            btnAddLower.TabIndex = 1;
            btnAddLower.Text = "> 選択項目の下に追加";
            btnAddLower.UseVisualStyleBackColor = true;
            btnAddLower.Click += btnAddLower_Click;
            // 
            // btnAddUpper
            // 
            btnAddUpper.Location = new System.Drawing.Point(9, 18);
            btnAddUpper.Margin = new System.Windows.Forms.Padding(4);
            btnAddUpper.Name = "btnAddUpper";
            btnAddUpper.Size = new System.Drawing.Size(159, 40);
            btnAddUpper.TabIndex = 0;
            btnAddUpper.Text = "> 選択項目の上に追加";
            btnAddUpper.UseVisualStyleBackColor = true;
            btnAddUpper.Click += btnAddUpper_Click;
            // 
            // btnAllDelete
            // 
            btnAllDelete.Location = new System.Drawing.Point(451, 269);
            btnAllDelete.Margin = new System.Windows.Forms.Padding(4);
            btnAllDelete.Name = "btnAllDelete";
            btnAllDelete.Size = new System.Drawing.Size(117, 40);
            btnAllDelete.TabIndex = 3;
            btnAllDelete.Text = "出力全削除";
            btnAllDelete.UseVisualStyleBackColor = true;
            btnAllDelete.Click += btnAllDelete_Click;
            // 
            // btnNoSave
            // 
            btnNoSave.Location = new System.Drawing.Point(429, 349);
            btnNoSave.Margin = new System.Windows.Forms.Padding(4);
            btnNoSave.Name = "btnNoSave";
            btnNoSave.Size = new System.Drawing.Size(166, 58);
            btnNoSave.TabIndex = 4;
            btnNoSave.Text = "変更を破棄して終了";
            btnNoSave.UseVisualStyleBackColor = true;
            btnNoSave.Click += btnNoSave_Click;
            // 
            // btnExit
            // 
            btnExit.Location = new System.Drawing.Point(442, 428);
            btnExit.Margin = new System.Windows.Forms.Padding(4);
            btnExit.Name = "btnExit";
            btnExit.Size = new System.Drawing.Size(145, 66);
            btnExit.TabIndex = 5;
            btnExit.Text = " 保存して終了";
            btnExit.UseVisualStyleBackColor = true;
            btnExit.Click += btnExit_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            label1.Location = new System.Drawing.Point(14, 22);
            label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(99, 14);
            label1.TabIndex = 6;
            label1.Text = "未選択タグリスト";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("MS UI Gothic", 10F);
            label2.Location = new System.Drawing.Point(918, 22);
            label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(85, 14);
            label2.TabIndex = 7;
            label2.Text = "出力タグリスト";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new System.Drawing.Font("MS UI Gothic", 12F);
            label3.Location = new System.Drawing.Point(390, 11);
            label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            label3.Name = "label3";
            label3.Size = new System.Drawing.Size(211, 16);
            label3.TabIndex = 8;
            label3.Text = "出力するExifタグを選んでください";
            // 
            // pictureBox1
            // 
            pictureBox1.Image = Properties.Resources.KUPマーク123x60;
            pictureBox1.Location = new System.Drawing.Point(443, 605);
            pictureBox1.Margin = new System.Windows.Forms.Padding(4);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new System.Drawing.Size(144, 75);
            pictureBox1.TabIndex = 9;
            pictureBox1.TabStop = false;
            // 
            // ReaDEfaoultCSV
            // 
            ReaDEfaoultCSV.Location = new System.Drawing.Point(451, 544);
            ReaDEfaoultCSV.Margin = new System.Windows.Forms.Padding(4);
            ReaDEfaoultCSV.Name = "ReaDEfaoultCSV";
            ReaDEfaoultCSV.Size = new System.Drawing.Size(115, 42);
            ReaDEfaoultCSV.TabIndex = 10;
            ReaDEfaoultCSV.Text = "デフォルトの EXIFデータを読み込む";
            ReaDEfaoultCSV.UseVisualStyleBackColor = true;
            ReaDEfaoultCSV.Click += ReaDEfaoultCSV_Click;
            // 
            // btnAllAdd
            // 
            btnAllAdd.Location = new System.Drawing.Point(454, 318);
            btnAllAdd.Name = "btnAllAdd";
            btnAllAdd.Size = new System.Drawing.Size(112, 24);
            btnAllAdd.TabIndex = 11;
            btnAllAdd.Text = "出力全追加";
            btnAllAdd.UseVisualStyleBackColor = true;
            btnAllAdd.Click += btnAllAdd_Click;
            // 
            // SelectForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(1031, 709);
            Controls.Add(btnAllAdd);
            Controls.Add(ReaDEfaoultCSV);
            Controls.Add(pictureBox1);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(btnExit);
            Controls.Add(btnNoSave);
            Controls.Add(btnAllDelete);
            Controls.Add(groupBox1);
            Controls.Add(listViewSelect);
            Controls.Add(listViewUnSelect);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            Margin = new System.Windows.Forms.Padding(4);
            Name = "SelectForm";
            Text = "SelectForm";
            Load += SelectForm_Load;
            groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.ListView listViewUnSelect;
        private System.Windows.Forms.ListView listViewSelect;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button btnDelete;
        private System.Windows.Forms.Button btnAddLower;
        private System.Windows.Forms.Button btnAddUpper;
        private System.Windows.Forms.Button btnAllDelete;
        private System.Windows.Forms.Button btnNoSave;
        private System.Windows.Forms.Button btnExit;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ColumnHeader property;
        private System.Windows.Forms.ColumnHeader tagNae;
        private System.Windows.Forms.ColumnHeader available;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button ReaDEfaoultCSV;
        private System.Windows.Forms.Button btnAllAdd;
    }
}