using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureList {
    public partial class SelectForm : Form {
        private List<Form1.Exiflist> allLists;
        private List<Form1.Exiflist> iniLists;

        private int LastUnSelectIdx = -1;
        private int LastSelectIdx = -1;
        //各ListViewのBackColorの設定
        private Color unSelectedBackColor = Color.FromArgb(210, 210, 120);

        public List<Form1.Exiflist> AllLists {
            set { allLists = value; }
            get { return allLists; }
        }
        public List<Form1.Exiflist> selectLists = new List<Form1.Exiflist>();
        public bool IsChangeOk;

        public SelectForm() {
            InitializeComponent();
        }

        /// <summary>
        /// Formを開いた時の最初の動作で各リストビューの基本のプロパティの設定等
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SelectForm_Load(object sender, EventArgs e) {
            iniLists = new List<Form1.Exiflist>(allLists);

            listViewUnSelect.View = View.Details;
            listViewSelect.View = View.Details;
            listViewUnSelect.BackColor = unSelectedBackColor;
            listViewSelect.BackColor = unSelectedBackColor;
            listViewUnSelect.MultiSelect = false;
            listViewSelect.MultiSelect = false;

            ReWriteViews();
        }

        /// <summary>
        /// 未選択の項目をListViewUnSelectに表示する 
        /// </summary>
        private void SetUnSelectView(string[] Titles) {
            listViewUnSelect.Clear();
            //Titlesの値は{プロパティ,タグ名称,型,可否,表示順}
            listViewUnSelect.Columns.Add(Titles[0], 140);
            listViewUnSelect.Columns.Add(Titles[1], 165);
            //Titles[2]=型は使用しない
            listViewUnSelect.Columns.Add(Titles[4], 80);

            for (int i = 0; i < allLists.Count; i++) {
                if (allLists[i].Order == 0) {
                    ListViewItem UnSelctViewItem = new ListViewItem();
                    UnSelctViewItem.Text = allLists[i].Property;
                    UnSelctViewItem.SubItems.Add(allLists[i].TagName);
                    UnSelctViewItem.SubItems.Add(allLists[i].TagID);
                    listViewUnSelect.Items.Add(UnSelctViewItem);
                }
            }
            LastUnSelectIdx = -1;
        }

        /// <summary>
        /// ListViewSelectに表示する
        /// </summary>
        private void SetSelectView(string[] Titles) {
            //まずは順番のついた出力するリストの作成
            //まずは順番の値の持つ出力リストのindexと順番の値のリストを作成
            var IdxOrders = new List<Form1.IdxOrder>();
            for (int i = 0; i < allLists.Count; i++) {
                int order = allLists[i].Order;
                if (order != 0) {
                    var tmpIOrder = new Form1.IdxOrder(i, order);
                    IdxOrders.Add(tmpIOrder);
                }
            }
            //順番でソートされた出力リストのindexのリスト
            var sortedIxdOrders = IdxOrders.OrderBy(x => x.Order);
            //allListsの順番の値を次で1から順番の値にし、SelectListView用のattrbListsを作成
            selectLists.Clear();
            int num = 1;
            foreach (var item in sortedIxdOrders) {
                allLists[item.Idx].Order = num++;
                selectLists.Add(allLists[item.Idx]);
            }
            //ヘッダ行の作成　プロパティ,タグ名称,型,可否,表示順
            //Form1.Exiflist Title = new Form1.Exiflist();
            listViewSelect.Clear();
            //Title = allLists[0];
            listViewSelect.Columns.Add(Titles[0], 136);
            listViewSelect.Columns.Add(Titles[1], 150);
            //listViewSelect.Columns.Add(Titles[2], 64);　型は表示しない
            listViewSelect.Columns.Add(Titles[4], 85);
            listViewSelect.Columns.Add(Titles[3], 48);

            for (int i = 0; i < selectLists.Count; i++) {
                ListViewItem SelectViewItem = new ListViewItem();
                SelectViewItem.Text = selectLists[i].Property;
                SelectViewItem.SubItems.Add(selectLists[i].TagName);
                SelectViewItem.SubItems.Add(selectLists[i].TagID);
                SelectViewItem.SubItems.Add(selectLists[i].Order.ToString());
                listViewSelect.Items.Add(SelectViewItem);
            }
            LastSelectIdx = -1;
        }

        /// <summary>
        /// listViewUbSelectの選択が変更されたときに呼ばれ選択された項目のBackColorを変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewUnSelect_SelectedIndexChanged(object sender, EventArgs e) {
            if (listViewUnSelect.SelectedItems.Count > 0) {
                LastUnSelectIdx = listViewUnSelect.SelectedItems[0].Index;
                listViewUnSelect.Items[LastUnSelectIdx].Checked = true;
            } else {
                listViewUnSelect.Items[LastUnSelectIdx].Checked = false;
            }
        }
        /// <summary>
        /// listViewSelectの選択が変更されたときに呼ばれ選択された項目のBackColorを変更する
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listViewSelect_SelectedIndexChanged(object sender, EventArgs e) {
            if (listViewSelect.SelectedItems.Count > 0) {
                LastSelectIdx = listViewSelect.SelectedItems[0].Index;
                listViewSelect.Items[LastSelectIdx].Checked = true;
            } else {
                listViewSelect.Items[LastSelectIdx].Checked = false;
            }
        }

        /// <summary>
        /// 選択項目を出力リストの指定位置の上に追加 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddUpper_Click(object sender, EventArgs e) {
            //Disp("Move Above");
            MoveToSelect(true);
        }
        /// <summary>
        /// 選択項目を出力リストの指定位置の下に追加 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddLower_Click(object sender, EventArgs e) {
            //Disp("ModeDown");
            //何も選択されていないときは最後の出力項目を選択する 2024/09/06
            if (LastSelectIdx < 0) {
                LastSelectIdx = listViewSelect.Items.Count - 1;
            }
            MoveToSelect(false);
        }
        /// <summary>
        /// 出力リストから削除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelete_Click(object sender, EventArgs e) {
            //Disp("Dele");
            if (LastSelectIdx < 0) { //削除項目が選ばれていないときは警告を表示して終了
                MessageBox.Show("移動元を選んでください", "注意",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            string SelectName = listViewSelect.Items[LastSelectIdx].Text;
            int SelectIdx = allLists.FindIndex(n => n.Property == SelectName);
            int DecreceNum = allLists[SelectIdx].Order;
            allLists[SelectIdx].Order = 0;
            foreach (var item in allLists) {
                int num = item.Order;
                if (num > DecreceNum) {
                    item.Order = --num;
                }
            }
            ReWriteViews();
        }
        /// <summary>
        /// 選択項目を出力リストに追加 の共通部
        /// </summary>
        /// <param name="isAbove"></param>
        private void MoveToSelect(bool isAbove) {
            //Disp("Move in");
            string UnSelectName = ""; //未選択項目の名前
            int UnSelectIdx = -1;       //  未選択項目のallListでのインデックス
            if (LastUnSelectIdx >= 0) {
                UnSelectName = listViewUnSelect.Items[LastUnSelectIdx].Text;
                UnSelectIdx = allLists.FindIndex(n => n.Property == UnSelectName);
            }
            //出力項目が無い場合は非選択項目だけの出力にする
            if (listViewSelect.Items.Count == 0 && LastUnSelectIdx >= 0) {
                allLists[UnSelectIdx].Order = 1;
                ReWriteViews();
                return;
            }
            //通常の処理
            //非選択も選択も選ばれていない場合は注意を表示して戻る
            if (LastUnSelectIdx < 0 || LastSelectIdx < 0) {
                MessageBox.Show("移動元と移動先を選んでください", "注意",
                    MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }                        
            int SelectOrder = LastSelectIdx + 1;    //候補で選択したもの新しいOrder
            int ChangingOrder = SelectOrder;    // alllistでインクリメントしなければいけない一番小さいOrder
                                                // 結局 SelectOrederもCahngingOredrも同じ値
            if (!isAbove) {
                SelectOrder++;
                ChangingOrder++;
            }
            foreach (Form1.Exiflist item in allLists) {
                int nowOrder = item.Order;
                if (nowOrder >= ChangingOrder) {
                    item.Order = ++nowOrder;
                }
            }
            allLists[UnSelectIdx].Order = SelectOrder;

            ReWriteViews();
        }
        /// <summary>
        /// 両方のリストビューの再描画
        /// </summary>
        private void ReWriteViews() {
            string[] exifTitles = Form1.ExifListTitle.Split(',');
            SetUnSelectView(exifTitles);
            SetSelectView(exifTitles);
        }

        private void btnNoSave_Click(object sender, EventArgs e) {
            IsChangeOk = false;
            allLists = iniLists;
            this.Close();
        }

        private void btnExit_Click(object sender, EventArgs e) {
            IsChangeOk = true;
            this.Close();
        }

        private void btnAllDelete_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("出力を削除しますが良いですか?", "確認", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);
            if (result == DialogResult.Cancel) {
                return;
            }

            foreach (Form1.Exiflist item in allLists) {
                item.Order = 0;
            }

            ReWriteViews();
        }        

        private void Disp(string str) {
            string LName = "-";
            if (LastUnSelectIdx >= 0) {
                LName = listViewUnSelect.Items[LastUnSelectIdx].Text;
            }
            string RName = "-";
            if (LastSelectIdx >= 0)
                RName = listViewSelect.Items[LastSelectIdx].Text;
        }

        private void listViewUnSelect_ItemCheck(object sender, ItemCheckEventArgs e) {

        }

        private void listViewUnSelect_ItemChecked(object sender, ItemCheckedEventArgs e) {
            //Console.WriteLine("候補_ItemChecked {0}", LastUnSelectIdx);
            if (LastUnSelectIdx < 0 && listViewUnSelect.CheckedItems.Count > 0) {
                foreach (ListViewItem item in listViewUnSelect.CheckedItems) {
                    item.Checked = false;
                }
            }
        }       

        private void listViewSelect_ItemChecked(object sender, ItemCheckedEventArgs e) {
            Console.WriteLine("出力_ItemChecked {0}", LastUnSelectIdx);
            if (LastSelectIdx < 0 && listViewSelect.CheckedItems.Count > 0) {
                foreach (ListViewItem item in listViewSelect.CheckedItems) {
                    item.Checked = false;
                }
            }
        }

        private void ReaDEfaoultCSV_Click(object sender, EventArgs e) {
            Form1.ReadExifListFromContent(Form1.SettingDefaultCSVPath);
            ReWriteViews();
        }

        /// <summary>
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllAdd_Click(object sender, EventArgs e) {
            DialogResult result = MessageBox.Show("出力をそのまま全追加しますが良いですか?", "確認", MessageBoxButtons.OKCancel,
                MessageBoxIcon.Warning);
            if (result == DialogResult.Cancel) {
                return;
            }
            int i = 1;

            foreach (Form1.Exiflist item in allLists) {
                item.Order = i++;              
            }
            ReWriteViews();
        }
    }
}

