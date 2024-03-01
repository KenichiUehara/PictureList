using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Resources;
using System.Windows.Shapes;


namespace PictureList {
    public partial class Form1 : Form {
        public List<Exiflist> ExifLists = new List<Exiflist>();
        List<Exiflist> ExifTagOutLists = new List<Exiflist>();
        string ExifListTitle;
        StringBuilder OutText = new StringBuilder();
        StringBuilder OutTitleList = new StringBuilder();
        const string SettingCSVPath = "./OutputExifList/PictureListExif.csv";
        private int FileSum, FileDone, CountInterval,NextDispNum; //探索数カウント用
        const int ProgressBarDivide = 200;
        string mask, dmask, sepa, sepaHead, sepaTail = "\r\n";
        //出力するもののリスト
        internal struct IdxOrder {
            public int Idx;
            public int Order;
            public IdxOrder(int idx, int order) {
                Idx = idx;
                Order = order;
            }
        }

        private enum OutputType {
            DirFiles = 0, Files, Dirs, Image, Exif
        }
        public Form1() {
            InitializeComponent();
        }
        
        public class Exiflist {
            public string Property { get; set; }
            public string TagName { get; set; }
            public string Type { get; set; }
            public bool Availeble { get; set; }
            public int Order { get; set; }
            
            /// <summary>
            /// コンストラクト
            /// </summary>
            /// <param name="property"></param>
            /// <param name="tagName"></param>
            /// <param name="type"></param>
            /// <param name="available"></param>
            /// <param name="order">リストに表示する順番。文字列で受取、数値に変換、数字以外は0にする</param>
            public Exiflist(string property, string tagName, string type, string available, string order) {
                Property = property;
                TagName = tagName;
                Type = type;
                if (available.ToLower() == "true") { Availeble = true; } else { Availeble = false; }
                SetOrder(order);
            }
            public Exiflist() {

            }
            public void SetOrder(string str) {
                if (str == "") return;
                try {
                    Order = int.Parse(str);
                } catch (ArgumentNullException) {
                    Order = 0;
                } catch (FormatException) {
                    Order = 0;
                }
            }

        }

        /// <summary>
        /// Exifの出力項目をビルドに含まれるcsvファイルのコンテンツから得る
        /// </summary>
        public void ReadExifListFromContent() {
            string strs;
            ExifLists.Clear();
            string path = SettingCSVPath;
            using (var sr = new StreamReader(path, System.Text.Encoding.GetEncoding("shift_jis"))) {
                ExifListTitle = sr.ReadLine();
                while (!sr.EndOfStream) {
                    strs = sr.ReadLine();
                    string[] str = strs.Split(',');
                    if (str.Length > 4) {
                        Exiflist exiflist = new Exiflist(str[0], str[1], str[2], str[3], str[4]);
                        ExifLists.Add(exiflist);
                    }
                }
            }
            ////出力するリストの作成
            ////まずは順番の値の持つ出力リストのindexと順番の値のリストを作成
            //var IdxOrders = new List<IdxOrder>();
            //for (int i = 0; i < ExifLists.Count; i++) {
            //    int order = ExifLists[i].Order;
            //    if (order != 0) {
            //        var tmpIOrder = new IdxOrder(i, order);
            //        IdxOrders.Add(tmpIOrder);
            //    }
            //}
            ////順番でソートされた出力リストのindexのリスト
            //var sortedIxdOrders = IdxOrders.OrderBy(x => x.Order);
            ////ExifListsの順番の値を次で1から順番の値にする
            //int num = 1;
            //foreach (var item in sortedIxdOrders) {
            //    ExifLists[item.Idx].Order = num++;
            //    ExifTagOutLists.Add(ExifLists[item.Idx]);
            //}
            ////WriteExifListToContent();
        }

        /// <summary>
        /// ExifListsをExiflist.Orderが1以上のものをソートしたExifListの出力用のExifTagOutListsを作る
        /// </summary>
        private void makeExifTagOutLists() {
            ExifTagOutLists.Clear();
            //出力するリストの作成
            //まずは順番の値の持つ出力リストのindexと順番の値のリストを作成
            var IdxOrders = new List<IdxOrder>();
            for (int i = 0; i < ExifLists.Count; i++) {
                int order = ExifLists[i].Order;
                if (order != 0) {
                    var tmpIOrder = new IdxOrder(i, order);
                    IdxOrders.Add(tmpIOrder);
                }
            }
            //順番でソートされた出力リストのindexのリスト
            var sortedIxdOrders = IdxOrders.OrderBy(x => x.Order);
            //ExifListsの順番の値を次で1から順番の値にする
            int num = 1;
            foreach (var item in sortedIxdOrders) {
                ExifLists[item.Idx].Order = num++;
                ExifTagOutLists.Add(ExifLists[item.Idx]);
            }
        }

        /// <summary>
        /// ExifItemListsを出力する
        /// </summary>
        private void WriteExifListToContent() {
            //以下の2行は開発時に不用意に属性候補リストを変更しないため
            DialogResult result =MessageBox.Show("表示属性の変更を保存するなら ハイ", "確認",MessageBoxButtons.YesNo,MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return; //ハイでなければ保存しない
            string path = SettingCSVPath;
            using (StreamWriter sw = new StreamWriter(path, false, System.Text.Encoding.GetEncoding("shift_jis"))) {
                sw.WriteLine(ExifListTitle);
                foreach (var exifItem in ExifLists) {
                    string property = exifItem.Property;
                    string tagName = exifItem.TagName;
                    string type = exifItem.Type;
                    string available = exifItem.Availeble.ToString();
                    string order = "";
                    if (exifItem.Order != 0)
                        order = exifItem.Order.ToString();
                    sw.WriteLine(property + "," + tagName + "," + type + "," + available + "," + order);
                }
            }
        }

        //
        //以下はFilesからの改良部分
        //


        /// <summary>
        /// 探索中は各実行ボタンを押せないようにする
        /// </summary>
        /// <param name="busy">trueなら実行中</param>
        private void InSaerching(bool busy) {
            btnFind.Enabled = !busy;
            btnFolderSelect.Enabled = !busy;
            btnOut2File.Enabled = !busy;
            btnCopy2Clip.Enabled = !busy;
            rbtnTab.Enabled = !busy;
            rbtnCSV.Enabled = !busy;
        }

        /// <summary>
        /// ファイル探索のメインルーチン
        /// </summary>
        /// <param name="Dir">探索の基準になるディレクトリ</param>
        private void GetDirsFilesList(string Dir) {
            InSaerching(true);
            //Exif項目を読む必要がある場合はExif項目を読み込む Imageの場合はその判定に必要
            if (chkExif.Checked || rbtnOnlyImage.Checked || rbtnOnlyExif.Checked) {
                ReadExifListFromContent();
            }
            //フォルダのリストをフォルダのマスク、サブディレクトリも含めるかに従って作成する
            List<string> dirLists = new List<string>();  //フォルダのリスト
            var serchOption = SearchOption.TopDirectoryOnly;
            if (chkSubDir.Checked)
                serchOption = SearchOption.AllDirectories;
            txtComment.Text = "";
            try {
                // LINQ
                var dirs = from dir in Directory.EnumerateDirectories(Dir, dmask, serchOption)
                           select dir;
                dirLists = dirs.ToList(); //ディレクトリのリスト
                dirLists.Insert(0, Dir);  //探索開始のDirを含める
                //FileSum:ファイルの総数
                FileSum = Directory.GetFiles(Dir, mask, serchOption).Length;
                lblRows.Text = FileSum.ToString("#,0");
                lblRows.Refresh();
            } catch (UnauthorizedAccessException) {
                txtComment.Text = "必要なアクセス許可がありません\n";
            } catch (PathTooLongException) {
                txtComment.Text = "パス名として長すぎます\n";
            } catch (DirectoryNotFoundException) {
                txtComment.Text = "マップされていない参照など無効です\n";
            }
            //ここからファイル情報を取り出す
            //まずタイトルの行の作成
            makeTextTitle();
            OutText.Clear();
            OutText.Append(OutTitleList);
            //ファイル情報を取り出す時間が掛かる処理の進捗表示の初期設定       
            progressBar1.Minimum = 0;
            progressBar1.Maximum = FileSum;
            FileDone = 0;
            CountInterval = (int)(FileSum / ProgressBarDivide);
            if (CountInterval < 1) CountInterval = 1;
            lblQty.Text = String.Format("{0:#,#} / {1:#,#}", FileDone, FileSum);
            lblQty.Refresh();
            NextDispNum = CountInterval;


            //ファイルの一般的な情報を個々に取り出す 
            foreach (var dirList in dirLists) { //DirListsのDir名毎の回す
                System.IO.DirectoryInfo dirInfo = new System.IO.DirectoryInfo(dirList);
                //出力にディレクトリが含まれるならディレクトリ情報を出力に追加する
                makeDirData(dirInfo, dirList);
                //個々のファイルの情報を追加する
                if (rbtnOnlyDir.Checked) break; //ディレクトリのみのときはここで抜ける
                System.IO.FileInfo[] fInfoList = dirInfo.GetFiles(mask);
                foreach (System.IO.FileInfo finfo in fInfoList) {
                    OutText.Append(sepaHead + finfo.DirectoryName);
                    OutText.Append(sepa + finfo.Name);
                    if (chkLastWriteTime.Checked)
                        OutText.Append(sepa + finfo.LastWriteTime.ToString());
                    if (chkCreationTime.Checked)
                        OutText.Append(sepa + finfo.CreationTime.ToString());
                    if (chkLastAccessTime.Checked)
                        OutText.Append(sepa + finfo.LastAccessTime.ToString());
                    if (chkSize.Checked)
                        OutText.Append(sepa + finfo.Length.ToString());
                    if (chkAttribute.Checked)
                        OutText.Append(sepa + finfo.Attributes.ToString());
                    //Exif属性を表示する場合
                    //Exif属性取得には時間が掛かると思われるためここでExif属性が不要な場合は先に進まないようにしたい
                    if (chkExif.Checked) {
                        var exifDate = new ExifPicture(finfo.FullName);
                        makeExifData(exifDate);
                    }
                    OutText.Append(sepaTail);
                    FileDone++;
                    if (NextDispNum >= FileSum)
                        DisplayDidFiles();
                    while ((NextDispNum += CountInterval) < FileSum)
                        ;

                }



                //OutText.Append(sepaTail);
            }

            //フォルダ情報も必用なら最初にファルダ情報を出力する


            InSaerching(false);

        }

        /// <summary>
        /// ディレクトリ情報を出す設定ならその情報を追加する
        /// </summary>
        /// <param name="dirInfo">そのディレクトリのDirectoryInfo</param>
        /// <param name="pathName">DirectoryInfoはバージョンにより相対パスの場合もあるので絶対パスを指定して持ってくる</param>
        private void makeDirData(System.IO.DirectoryInfo dirInfo, string pathName) {
            if (rbtnIncldDir.Checked || rbtnOnlyDir.Checked) {
                OutText.Append(sepaHead + pathName);
                OutText.Append(sepa + dirInfo.Name);
                if (chkLastWriteTime.Checked)
                    OutText.Append(sepa + dirInfo.LastWriteTime.ToString());
                if (chkCreationTime.Checked)
                    OutText.Append(sepa + dirInfo.CreationTime.ToString());
                if (chkLastAccessTime.Checked)
                    OutText.Append(sepa + dirInfo.LastAccessTime.ToString());
                if (chkSize.Checked)
                    OutText.Append(sepa + "0");
                if (chkAttribute.Checked)
                    OutText.Append(sepa + dirInfo.Attributes.ToString());
                OutText.Append(sepaTail);
            }
        }

        private void DisplayDidFiles() {
            lblQty.Text = String.Format("{0:#,#} / {1:#,#}", FileDone, FileSum);
            lblQty.Refresh();
            progressBar1.Value = FileDone;
        }

        /// <summary>
        /// 引数で指定したクラスのデータからExifTagOutListsを順番に調べてそれと一致する情報をOutTextに追加していく
        /// </summary>
        /// <param name="exifData">調べるファイルのExif情報</param>
        private void makeExifData(ExifPicture exifData) {
            foreach (var item in ExifTagOutLists) {
                string type = item.Type.ToLower();
                //ExifTagOutListのProperty名と一致するExifPicturesクラスのプロパティを探す
                PropertyInfo propertyInfo = exifData.GetType().GetProperty(item.Property);
                if (type == "string") {
                    //string tmp = (string)propertyInfo.GetValue(exifData);
                    //OutText.Append(sepa + tmp);
                    OutText.Append(sepa + (string)propertyInfo.GetValue(exifData));
                } else if (type == "long")
                    OutText.Append(sepa + ((long)propertyInfo.GetValue(exifData)).ToString());
                else if (type == "double")
                    OutText.Append(sepa + ((double)propertyInfo.GetValue(exifData)).ToString());
                else if (type == "DateTime")
                    OutText.Append(sepa + ((DateTime)propertyInfo.GetValue(exifData)).ToString());
                else if (type == "bool")
                    OutText.Append(sepa + ((bool)propertyInfo.GetValue(exifData)).ToString());
                else
                    OutText.Append(sepa + "不明な型によるエラー");
            }
        }

        private void button1_Click(object sender, EventArgs e) {
            string fPath;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK) {
                fPath = openFileDialog1.FileName;
                textBox1.Text = fPath;
                var exifF = new ExifPicture(fPath);

                textBox2.Text = exifF.Name;

                //ExifPicture exifP = new ExifPicture(fPath);

            }
            //    ///string imgFile = "D:\\users\\kup\\Photo\\2016\\Work\\1607-12\\2016-12-20\\CIMG1768.JPG";
            //    string imgFile = "D:\\users\\kup\\Photo\\2023\\SG03\\2023-01-01\\20230101_070005.JPG";
            //    //読み込む
            //    System.Drawing.Bitmap bmp = new System.Drawing.Bitmap(imgFile);
            //    //Exif情報を列挙する
            //    foreach (System.Drawing.Imaging.PropertyItem item in bmp.PropertyItems) {
            //        //データの型を判断
            //        if (item.Type == 2) {
            //            //ASCII文字の場合は、文字列に変換する
            //            string val = System.Text.Encoding.ASCII.GetString(item.Value);
            //            val = val.Trim(new char[] { '\0' });
            //            //表示する
            //            Console.WriteLine("{0:X}:{1}:{2}", item.Id, item.Type, val);
            //        } else {
            //            //表示する
            //            Console.WriteLine("{0:X}:{1}:{2}", item.Id, item.Type, item.Len);
            //        }
            //    }
            //    bmp.Dispose();
            //}
        }

        private void Form1_Load(object sender, EventArgs e) {
            //Settings.settingsに保存したデータの読み込み
            lblSearchPath.Text = Properties.Settings.Default.StartDir;
            chkSubDir.Checked = Properties.Settings.Default.IsIncludeSubDir;
            txtMask.Text = Properties.Settings.Default.MaskString;
            chkDirMask.Checked = Properties.Settings.Default.IsMaskDir;
            saveFileDialog1.InitialDirectory = Properties.Settings.Default.OutputDir;
            SetOutPutFileType();
            chkLastWriteTime.Checked = Properties.Settings.Default.IsLastWriteTime;
            OutputType Otype = (OutputType)Properties.Settings.Default.OutPutType;
            switch (Otype) {
                case OutputType.DirFiles:
                    rbtnIncldDir.Checked = true; break;
                case OutputType.Files:
                    rbtnOnlyFiles.Checked = true; break;
                case OutputType.Dirs:
                    rbtnOnlyDir.Checked = true; break;
                case OutputType.Image:
                    rbtnOnlyImage.Checked = true; break;
                case OutputType.Exif:
                    rbtnOnlyExif.Checked = true; break;
            }
            if (Properties.Settings.Default.IsTab)
                rbtnTab.Checked = true;
            else
                rbtnCSV.Checked = true;
            if (Properties.Settings.Default.IsLastWriteTime) chkLastWriteTime.Checked = true;
            else chkLastWriteTime.Checked = false;
            if (Properties.Settings.Default.IsCreationTime) chkCreationTime.Checked = true;
            else chkCreationTime.Checked = false;
            if (Properties.Settings.Default.IsLastAccessTime) chkLastAccessTime.Checked = true;
            else chkLastAccessTime.Checked = false;
            if (Properties.Settings.Default.IsSize) chkSize.Checked = true;
            else chkSize.Checked = false;
            if (Properties.Settings.Default.IsAttribute) chkAttribute.Checked = true;
            else chkAttribute.Checked = false;
            if (Properties.Settings.Default.IsExif) chkExif.Checked = true;
            else chkExif.Checked = false;

            mask = txtMask.Text;
            if (chkDirMask.Checked) {
                dmask = mask;
            } else {
                dmask = "*.*";
            }

            SetOutPutFileType();
        }

        //探索フォルダーを得る
        private void btnFolderSelect_Click(object sender, EventArgs e) {
            folderBrowserDialog1.SelectedPath = lblSearchPath.Text;
            if (folderBrowserDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                lblSearchPath.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        //ファイルマスクテキストの変更時
        private void txtMask_TextChanged(object sender, EventArgs e) {
            mask = txtMask.Text;
            if (chkDirMask.Checked) {
                dmask = mask;
            } else {
                dmask = "*.*";
            }
        }
        //ディレクトリもマスクするボタンのチェック変更
        private void chkDirMask_CheckedChanged(object sender, EventArgs e) {
            if (chkDirMask.Checked) {
                dmask = mask;
            } else {
                dmask = "*.*";
            }
        }

        private void btnFind_Click(object sender, EventArgs e) {
            if (System.IO.Directory.Exists(lblSearchPath.Text)) {
                GetDirsFilesList(lblSearchPath.Text);
            } else {
                MessageBox.Show("ディレクトリ " + lblSearchPath.Text + "\nは見つかりません");
            }
        }

        /// <summary>
        /// 各設定を保存して終了
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, EventArgs e) {
            //設定値の保存
            Properties.Settings.Default.StartDir = lblSearchPath.Text;
            Properties.Settings.Default.IsIncludeSubDir = chkDirMask.Checked;
            Properties.Settings.Default.MaskString = txtMask.Text;
            Properties.Settings.Default.IsMaskDir = chkDirMask.Checked;
            Properties.Settings.Default.OutputDir = saveFileDialog1.InitialDirectory;
            int oT = 0;
            if (rbtnIncldDir.Checked) oT = (int)OutputType.DirFiles;
            else if (rbtnOnlyFiles.Checked) oT = (int)OutputType.Files;
            else if (rbtnOnlyDir.Checked) oT = (int)OutputType.Dirs;
            else if (rbtnOnlyImage.Checked) oT = (int)OutputType.Image;
            else if (rbtnOnlyExif.Checked) oT = (int)OutputType.Exif;
            Properties.Settings.Default.OutPutType = oT;
            Properties.Settings.Default.IsTab = rbtnTab.Checked;
            Properties.Settings.Default.IsLastWriteTime = chkLastWriteTime.Checked;
            Properties.Settings.Default.IsCreationTime = chkCreationTime.Checked;
            Properties.Settings.Default.IsLastAccessTime = chkLastAccessTime.Checked;
            Properties.Settings.Default.IsSize = chkSize.Checked;
            Properties.Settings.Default.IsAttribute = chkAttribute.Checked;
            Properties.Settings.Default.IsExif = chkExif.Checked;

            Properties.Settings.Default.Save();
            this.Close();
        }

        private void btnCopy2Clip_Click(object sender, EventArgs e) {
            Clipboard.SetDataObject(OutText.ToString());
        }

        // SelctFormでEXIF項目を変更する
        private void btnSetExif_Click(object sender, EventArgs e) {
            ReadExifListFromContent();
            SelectForm tgtForm = new SelectForm();
            tgtForm.AllLists = ExifLists;
            tgtForm.exifListTitle = ExifListTitle;
            tgtForm.ShowDialog();
        }

        private void rbtnTab_CheckedChanged(object sender, EventArgs e) {
            SetOutPutFileType();
        }
        //出力テキストファイルがTabとcsvファイルの違いによる出力ファイル呼出し時のファイルタイプの変更
        private void SetOutPutFileType() {
            if (rbtnTab.Checked) {
                sepa = "\t"; sepaHead = ""; sepaTail = "\r\n";
                saveFileDialog1.DefaultExt = ".txt";
                saveFileDialog1.Filter = "テキストファイル(*.txt)|*.txt|すべて(*.*)|*.*";
            } else {
                sepa = "\",\""; sepaHead = "\""; sepaTail = "\"\r\n";
                saveFileDialog1.DefaultExt = ".csv";
                saveFileDialog1.Filter = "テキストファイル(*.csv)|*.csv|すべて(*.*)|*.*";
            }
        }
        private void makeTextTitle() {
            OutTitleList.Clear();
            OutTitleList.Append(sepaHead + "フォルダ名" + sepa + "ファイル名");
            if (chkLastWriteTime.Checked)
                OutTitleList.Append(sepa + "最終更新日");
            if (chkCreationTime.Checked)
                OutTitleList.Append(sepa + "作成日");
            if (chkLastAccessTime.Checked)
                OutTitleList.Append(sepa + "最終アクセス日");
            if (chkSize.Checked)
                OutTitleList.Append(sepa + "ファイルサイズ");
            if (chkAttribute.Checked)
                OutTitleList.Append(sepa + "属性");
            if (chkExif.Checked) {
                foreach (var exiflist in ExifTagOutLists) {
                    OutTitleList.Append(sepa + exiflist.TagName);
                }
            }
            OutTitleList.Append(sepaTail);
            txtComment.Text = OutTitleList.ToString();
            txtComment.Refresh();
        }

        private void rbtnCSV_CheckedChanged(object sender, EventArgs e) {
            SetOutPutFileType();
        }

        private void btnOut2File_Click(object sender, EventArgs e) {
            StringBuilder bd = new StringBuilder();
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                lblOutFile.Text = saveFileDialog1.FileName;
                //getOutStr(ref bd);
                //System.IO.File.WriteAllText(saveFileDialog1.FileName, bd.ToString(), Encoding.Default);
            }
        }

        private void button2_Click(object sender, EventArgs e) {
            ReadExifListFromContent();
        }

        private void button3_Click(object sender, EventArgs e) {
            string path = "./OutputExifList/PictureListExif.csv";
            using (StreamReader sr = new StreamReader(path, System.Text.Encoding.GetEncoding("shift_jis"))) {
                string line;
                while ((line = sr.ReadLine()) != null) {
                    Console.WriteLine(line);
                }
            }
        }
    }
}
