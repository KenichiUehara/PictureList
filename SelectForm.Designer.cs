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
            System.Windows.Forms.ListViewItem listViewItem8 = new System.Windows.Forms.ListViewItem(new string[] {
            "Test"}, -1, System.Drawing.Color.Empty, System.Drawing.SystemColors.Window, null);
            this.listViewUnSelect = new System.Windows.Forms.ListView();
            this.property = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.tagNae = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.available = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.listViewSelect = new System.Windows.Forms.ListView();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.btnDelete = new System.Windows.Forms.Button();
            this.btnAddLower = new System.Windows.Forms.Button();
            this.btnAddUpper = new System.Windows.Forms.Button();
            this.btnAllDelete = new System.Windows.Forms.Button();
            this.btnNoSave = new System.Windows.Forms.Button();
            this.btnExit = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // listViewUnSelect
            // 
            this.listViewUnSelect.BackColor = System.Drawing.SystemColors.Window;
            this.listViewUnSelect.CheckBoxes = true;
            this.listViewUnSelect.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.property,
            this.tagNae,
            this.available});
            this.listViewUnSelect.FullRowSelect = true;
            this.listViewUnSelect.GridLines = true;
            this.listViewUnSelect.HideSelection = false;
            listViewItem8.StateImageIndex = 0;
            this.listViewUnSelect.Items.AddRange(new System.Windows.Forms.ListViewItem[] {
            listViewItem8});
            this.listViewUnSelect.Location = new System.Drawing.Point(3, 47);
            this.listViewUnSelect.MultiSelect = false;
            this.listViewUnSelect.Name = "listViewUnSelect";
            this.listViewUnSelect.Size = new System.Drawing.Size(349, 508);
            this.listViewUnSelect.TabIndex = 0;
            this.listViewUnSelect.UseCompatibleStateImageBehavior = false;
            this.listViewUnSelect.View = System.Windows.Forms.View.Details;
            this.listViewUnSelect.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewUnSelect_ItemChecked);
            this.listViewUnSelect.SelectedIndexChanged += new System.EventHandler(this.listViewUnSelect_SelectedIndexChanged);
            // 
            // property
            // 
            this.property.Text = "プロパティ";
            this.property.Width = 128;
            // 
            // tagNae
            // 
            this.tagNae.Text = "タグ名称";
            this.tagNae.Width = 128;
            // 
            // available
            // 
            this.available.Text = "有効";
            // 
            // listViewSelect
            // 
            this.listViewSelect.CheckBoxes = true;
            this.listViewSelect.FullRowSelect = true;
            this.listViewSelect.GridLines = true;
            this.listViewSelect.HideSelection = false;
            this.listViewSelect.Location = new System.Drawing.Point(520, 47);
            this.listViewSelect.MultiSelect = false;
            this.listViewSelect.Name = "listViewSelect";
            this.listViewSelect.Size = new System.Drawing.Size(363, 508);
            this.listViewSelect.TabIndex = 1;
            this.listViewSelect.UseCompatibleStateImageBehavior = false;
            this.listViewSelect.View = System.Windows.Forms.View.Details;
            this.listViewSelect.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewSelect_ItemChecked);
            this.listViewSelect.SelectedIndexChanged += new System.EventHandler(this.listViewSelect_SelectedIndexChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnDelete);
            this.groupBox1.Controls.Add(this.btnAddLower);
            this.groupBox1.Controls.Add(this.btnAddUpper);
            this.groupBox1.Location = new System.Drawing.Point(360, 56);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(154, 153);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "選択項目を移動";
            // 
            // btnDelete
            // 
            this.btnDelete.Location = new System.Drawing.Point(36, 101);
            this.btnDelete.Name = "btnDelete";
            this.btnDelete.Size = new System.Drawing.Size(80, 35);
            this.btnDelete.TabIndex = 2;
            this.btnDelete.Text = "< 削除";
            this.btnDelete.UseVisualStyleBackColor = true;
            this.btnDelete.Click += new System.EventHandler(this.btnDelete_Click);
            // 
            // btnAddLower
            // 
            this.btnAddLower.Location = new System.Drawing.Point(8, 56);
            this.btnAddLower.Name = "btnAddLower";
            this.btnAddLower.Size = new System.Drawing.Size(136, 35);
            this.btnAddLower.TabIndex = 1;
            this.btnAddLower.Text = "> 選択項目の下に追加";
            this.btnAddLower.UseVisualStyleBackColor = true;
            this.btnAddLower.Click += new System.EventHandler(this.btnAddLower_Click);
            // 
            // btnAddUpper
            // 
            this.btnAddUpper.Location = new System.Drawing.Point(8, 14);
            this.btnAddUpper.Name = "btnAddUpper";
            this.btnAddUpper.Size = new System.Drawing.Size(136, 32);
            this.btnAddUpper.TabIndex = 0;
            this.btnAddUpper.Text = "> 選択項目の上に追加";
            this.btnAddUpper.UseVisualStyleBackColor = true;
            this.btnAddUpper.Click += new System.EventHandler(this.btnAddUpper_Click);
            // 
            // btnAllDelete
            // 
            this.btnAllDelete.Location = new System.Drawing.Point(387, 231);
            this.btnAllDelete.Name = "btnAllDelete";
            this.btnAllDelete.Size = new System.Drawing.Size(100, 32);
            this.btnAllDelete.TabIndex = 3;
            this.btnAllDelete.Text = "出力全削除";
            this.btnAllDelete.UseVisualStyleBackColor = true;
            this.btnAllDelete.Click += new System.EventHandler(this.btnAllDelete_Click);
            // 
            // btnNoSave
            // 
            this.btnNoSave.Location = new System.Drawing.Point(368, 283);
            this.btnNoSave.Name = "btnNoSave";
            this.btnNoSave.Size = new System.Drawing.Size(142, 46);
            this.btnNoSave.TabIndex = 4;
            this.btnNoSave.Text = "変更を破棄して終了";
            this.btnNoSave.UseVisualStyleBackColor = true;
            this.btnNoSave.Click += new System.EventHandler(this.btnNoSave_Click);
            // 
            // btnExit
            // 
            this.btnExit.Location = new System.Drawing.Point(380, 346);
            this.btnExit.Name = "btnExit";
            this.btnExit.Size = new System.Drawing.Size(124, 53);
            this.btnExit.TabIndex = 5;
            this.btnExit.Text = "保存して終了";
            this.btnExit.UseVisualStyleBackColor = true;
            this.btnExit.Click += new System.EventHandler(this.btnExit_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label1.Location = new System.Drawing.Point(12, 18);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(64, 14);
            this.label1.TabIndex = 6;
            this.label1.Text = "候補リスト";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("MS UI Gothic", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label2.Location = new System.Drawing.Point(794, 18);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(64, 14);
            this.label2.TabIndex = 7;
            this.label2.Text = "出力リスト";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("MS UI Gothic", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(128)));
            this.label3.Location = new System.Drawing.Point(345, 9);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(196, 16);
            this.label3.TabIndex = 8;
            this.label3.Text = "出力する属性を選んでください";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = global::PictureList.Properties.Resources.KUPマーク123x60;
            this.pictureBox1.Location = new System.Drawing.Point(380, 484);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(123, 60);
            this.pictureBox1.TabIndex = 9;
            this.pictureBox1.TabStop = false;
            // 
            // SelectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 567);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnExit);
            this.Controls.Add(this.btnNoSave);
            this.Controls.Add(this.btnAllDelete);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.listViewSelect);
            this.Controls.Add(this.listViewUnSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SelectForm";
            this.Text = "SelectForm";
            this.Load += new System.EventHandler(this.SelectForm_Load);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

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
    }
}