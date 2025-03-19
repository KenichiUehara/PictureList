//using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Drawing.Imaging;
// /*using ExifLib;*/          //未使用かも
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.ComponentModel.Design;
using System.Runtime.Versioning;


//using System.Drawing;
//using System.Drawing.Imaging;

//#pragma warning disable CA1416
namespace PictureList {
    public partial class Form1 : Form {
        private static List<Exiflist> exifLists = new();  //Exif項目のリスト
        List<Exiflist> ExifTagOutLists = new();          //出力するExif項目のリスト
        static string ExifListTitle;                                    //読み込んだExif CSVファイルの先頭行
        StringBuilder OutText = new();                    //出力テキスト
        StringBuilder OutTitleList = new();               //出力用のタイトル文字列
        public const string SettingCSVPath = "./OutputExifList/PictureListExif.csv"; //Exif CSVへのパス
        public const string SettingDefaultCSVPath = "./OutputExifList/DefaultPictureListExif.csv"; //同上デフォルト
        public const string SettingExifToolCSVPath = "./OutputExifList/ExifToolConv.csv"; //ExifToolConv.csvVへのパス
        public static bool IsReadCSV = false;                           //Exif CSVファイルを読み込んだかのフラグ
        List<string> ExifExtLists = new();                 //Exif調査用拡張子のリスト
        const string DefaultExifExt = "JPG,TIFF,TIF";                    //既定のEXIF拡張子
        private int FileSum, FileDone, DirSum, DirDone, BothSum, CountInterval, NextDispNum; //探索数カウント用
        const int ProgressBarDivide = 200;
        string mask, dmask, sepa, sepaHead, sepaTail = "\r\n";
        private bool IsMadeExifToolConv = false;
        static private bool IsExifAllSet = true;

        private int exifToolUse;
        public Dictionary<string, string> ExifToolVal = new();
        public static List<Exiflist> ExifLists { get => exifLists; set => exifLists = value; }

        //ExifToolConvList ExifToolのExifPictureの項目への対照ディクショナリ用
        class ExifToolItems {
            public int qty;
            public string[] exifToolItem = new string[4];
        };
        static Dictionary<string, ExifToolItems> ExifConvList = new();
        /// <summary>
        /// ExifConvListを pathのcsvファイルから作成する
        /// </summary>
        /// <param name="path"></param>
        static public void ReadExifToolConv(string path) {
            string strs;
            ExifConvList.Clear();
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using var sr = new StreamReader(path, System.Text.Encoding.GetEncoding("Shift_Jis"));
            while (!sr.EndOfStream) {
                strs = sr.ReadLine();
                string[] str = strs.Split(',');
                if (str.Length < 3) {
                    MessageBox.Show(path + "\r\n" + strs + "\r\nの内容が異常です", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    var exifOutItems = new ExifToolItems();
                    exifOutItems.qty = int.Parse(str[1]);
                    for (int i = 2; i < str.Length; i++) {
                        exifOutItems.exifToolItem[i - 2] = str[i];
                    }
                    ExifConvList.Add(str[0], exifOutItems);
                }
            }
        }

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

        /// <summary>
        /// PictureListExif.csv を格納するためにクラス
        /// </summary>
        public class Exiflist {
            public string Property { get; set; }
            public string TagName { get; set; }
            public string Type { get; set; }
            public bool Availeble { get; set; }
            public int Order { get; set; }

            /// <summary>
            /// PictureListExif.csvから読み込むコンストラクト
            /// </summary>
            /// <param name="property">Exif名</param>
            /// <param name="tagName">Exifの用途名</param>
            /// <param name="type">データ型を表す文字列</param>
            /// <param name="available">有効：TRUE,無効:FALSE コンストラクタでBooleanに変換</param>
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
        static public void ReadExifListFromContent(string path) {
            string strs;

            ExifLists.Clear();
            //string path = SettingCSVPath;
            //using (var sr = new StreamReader(path, System.Text.Encoding.GetEncoding("shift_jis"))) {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (var sr = new StreamReader(path, System.Text.Encoding.GetEncoding("Shift_Jis"))) {
                ExifListTitle = sr.ReadLine();
                while (!sr.EndOfStream) {
                    strs = sr.ReadLine();
                    string[] str = strs.Split(',');
                    if (str.Length > 4) {
                        Exiflist exiflist = new(str[0], str[1], str[2], str[3], str[4]);
                        ExifLists.Add(exiflist);
                    }
                }
            }
            IsReadCSV = true;
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
        private static void WriteExifListToContent() {
            //以下の2行は開発時に不用意に属性候補リストを変更しないため
            DialogResult result = MessageBox.Show("表示属性の変更を保存するなら ハイ", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return; //ハイでなければ保存しない
            string path = SettingCSVPath;
            using StreamWriter sw = new(path, false, System.Text.Encoding.GetEncoding("shift_jis"));
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


        // Exifの出力値のリストExifOutLists 
        Dictionary<string, string> ExifOutLists = new();

        ///<summary>
        /// ExifPictureからExifTagOutListsに従ってExif項目の出力用項目名と値のリストを作る
        /// 値はすべて文字列に変換。値が見つからなければ ""とする
        /// </summary>
        /// <param name="exifPicture"></param>
        private void GetExifList(ExifPicture exifPicture) {
            ExifOutLists.Clear();
            IsExifAllSet = true;
            foreach (var item in ExifTagOutLists) {
                //string type = item.Type.ToLower();

                //string tmpstr = exifPicture.FullPath + " , " + item.Property;
                //Debug.Print(tmpstr);

                bool isOk = false;
                object obj = new();
                PropertyInfo propertyInfo = exifPicture.GetType().GetProperty(item.Property);
                if (propertyInfo != null) {
                    obj = propertyInfo.GetValue(exifPicture);
                    if (obj != null)
                        isOk = true;
                }
                if (isOk) {
                    ExifOutLists.Add(item.Property, obj.ToString());
                } else {
                    ExifOutLists.Add(item.Property, "");
                    IsExifAllSet = false;

                }
            }
        }




        //以下はFilesからの改良部分


        /// <summary>
        /// 探索中は各実行ボタンを押せないようにする
        /// </summary>
        /// <param name="busy">trueなら実行中</param>
        private void InSaerching(bool busy) {
            //if(busy==false) return;
            btnFind.Enabled = !busy;
            btnFolderSelect.Enabled = !busy;
            btnOut2File.Enabled = !busy;
            btnCopy2Clip.Enabled = !busy;
            rbtnTab.Enabled = !busy;
            rbtnCSV.Enabled = !busy;

            //this.Refresh();

            //btnFind.BackColor = System.Drawing.SystemColors.Control;

            //btnFind.Refresh();
            //btnFolderSelect.Refresh(); 
            //btnOut2File.Refresh();
            //btnCopy2Clip.Refresh();
            //rbtnCSV.Refresh();
            //rbtnTab.Refresh();
            //rbtnCSV.Refresh();  
        }

        /// <summary>
        /// ファイル探索のメインルーチン
        /// </summary>
        /// <param name="Dir">探索の基準になるディレクトリ</param>
        private void GetDirsFilesList(string Dir) {

            // 実行時間の計測
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            //

            InSaerching(true);
            //Exif項目を読む必要がある場合はExif項目を読み込む Imageの場合はその判定に必要
            if ((chkExif.Checked || rbtnOnlyImage.Checked || rbtnOnlyExif.Checked) && !IsReadCSV) {
                ReadExifListFromContent(SettingCSVPath);
            }
            //フォルダのリストをフォルダのマスク、サブディレクトリも含めるかに従って作成する
            List<string> dirLists = new();  //フォルダのリスト
            var serchOption = SearchOption.TopDirectoryOnly;
            if (chkSubDir.Checked)
                serchOption = SearchOption.AllDirectories;
            txtComment.Text = "";
            try {
                // LINQ Directory.EnumerateDirectoriesは高速に処理できるので採用
                if (chkSubDir.Checked) {
                    var dirs = from dir in Directory.EnumerateDirectories(Dir, dmask, serchOption)
                               select dir;
                    dirLists = dirs.ToList(); //ディレクトリのリスト
                }
                dirLists.Insert(0, Dir);  //探索開始のDirを含める
                //FileSum:ファイルの総数, DirSim:フォルダの総数, BothSum 両者の合計
                FileSum = Directory.GetFiles(Dir, mask, serchOption).Length;
                DirSum = Directory.GetDirectories(Dir, dmask, serchOption).Length;
                if (rbtnOnlyFiles.Checked) {
                    BothSum = FileSum;
                } else if (rbtnOnlyDir.Checked) {
                    BothSum = DirSum;
                } else {
                    BothSum = FileSum + DirSum;
                }
                lblRows.Text = BothSum.ToString("#,0");
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
            makeExifTagOutLists();
            makeTextTitle();
            OutText.Clear();
            OutText.Append(OutTitleList);
            //ファイル情報を取り出す時間が掛かる処理の進捗表示の初期設定       
            progressBar1.Minimum = 0;
            progressBar1.Maximum = BothSum;
            FileDone = 0; DirDone = 0;
            CountInterval = (int)(BothSum / ProgressBarDivide);
            if (CountInterval < 1) CountInterval = 1;
            lblQty.Text = String.Format("{0:#,0} / {1:#,0}", (FileDone + DirDone), BothSum);
            lblQty.Refresh();
            NextDispNum = CountInterval;

            //ファイルの情報をフォルダ単位で取り出す 
            foreach (var dirList in dirLists) { //DirListsのDir名毎に回す
                System.IO.DirectoryInfo dirInfo = new(dirList);

                //出力にディレクトリが含まれるならディレクトリ情報を出力に追加する
                if (rbtnIncldDir.Checked || rbtnOnlyDir.Checked) {
                    makeDirData(dirInfo, dirList);
                }
                //ファイルの情報を出力する場合はファイル情報を出力する
                if (!rbtnOnlyDir.Checked) {
                    System.IO.FileInfo[] fInfoList = dirInfo.GetFiles(mask);
                    if (chkExif.Checked)
                        GetExifExt();
                    //出力に項目を追加する。
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
                        if (chkExif.Checked && (rbtnAllExt.Checked || ExifExtLists.Contains(finfo.Extension.ToUpper().Replace(".", "")))) {
                            //ExifPictureクラスでExif情報を取得
                            ExifPicture exifData = new(finfo.FullName);
                            GetExifList(exifData);  //出力するExifのリスト ExifOutListsを得る
                            //ExixToolを調べる必要のある時だけExifToolで調べる
                            if (!IsExifAllSet && exifToolUse != 0) {
                                var exifToolData = new ExifToolMy(finfo.FullName);  //ExifToolのExifの読込
                                if (!IsMadeExifToolConv) {  // ExifToolConvが読み込まれていなければ読み込む
                                    ReadExifToolConv(SettingExifToolCSVPath);
                                    IsMadeExifToolConv = true;
                                }
                                //ExifOutListsの値を順に調べて値が無ければExifToolであるか調べる
                                GetExifOutValue(exifToolData);
                            }
                            makeExifData(exifData); // この関数はExifOutListsを使用するものに変える
                        }
                        OutText.Append(sepaTail);
                        FileDone++;
                        if (FileDone + DirDone > NextDispNum) {
                            DisplayDidFiles();
                            NextDispNum += CountInterval;
                        }
                    }
                }
                if (FileDone + DirDone > NextDispNum) {
                    DisplayDidFiles();
                    NextDispNum += CountInterval;
                }
            }
            InSaerching(false);

            // 実行時間の表示
            stopwatch.Stop();
            Debug.Print($"処理数: { FileDone + DirDone}  時間: { stopwatch.ElapsedMilliseconds/1000.0}Sec");
            //

        }


        /// <summary>
        /// ExifOutListsの値をExifConvListsの変換表に従ってExifToolの値で更新する
        /// </summary>
        /// <param name="exifToolMy"></param>
        /// <returns></returns>
        private bool GetExifOutValue(ExifToolMy exifToolMy) {
            bool rtnBool = false;
            foreach (var exifOutList in ExifOutLists) {
                if (exifToolUse == 2 || exifOutList.Value == "") {
                    string exifName = exifOutList.Key;
                    string nValue = "";
                    rtnBool = false;
                    if (ExifConvList.TryGetValue(exifName, out ExifToolItems value)) {
                        for (int i = 0; i < value.qty; i++) {
                            //rtnBool = false;
                            if (exifToolMy.ExifToolLists.ContainsKey(value.exifToolItem[i])) {
                                nValue = exifToolMy.ExifToolLists[value.exifToolItem[i]];
                                if (nValue != "") {
                                    rtnBool = true;
                                    break;
                                }
                            }
                        }
                        if (rtnBool)
                            ExifOutLists[exifName] = nValue;

                        if (nValue != "")
                            ExifOutLists[exifName] = nValue;
                    }
                }
            }
            return rtnBool;
        }

        /// <summary>
        /// ディレクトリ情報を出す設定ならその情報を追加する
        /// </summary>
        /// <param name="dirInfo">そのディレクトリのDirectoryInfo</param>
        /// <param name="pathName">DirectoryInfoはバージョンにより相対パスの場合もあるので絶対パスを指定して持ってくる</param>
        private void makeDirData(System.IO.DirectoryInfo orgDirInfo, string pathName) {
            DirectoryInfo[] dirInfos = orgDirInfo.GetDirectories(dmask, SearchOption.TopDirectoryOnly);

            foreach (DirectoryInfo dirInfo in dirInfos) {
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
                DirDone++;
            }
        }

        private void DisplayDidFiles() {
            int BothDone = FileDone + DirDone;
            lblQty.Text = String.Format("{0:#,#} / {1:#,#}", BothDone, BothSum);
            lblQty.Refresh();
            if (BothDone > progressBar1.Maximum)
                BothDone = progressBar1.Maximum;
            progressBar1.Value = BothDone;
        }

        // この関数はExifOutListsを使用するものに変える
        /// <summary>
        /// 引数で指定したExifPictureクラスのタグデータからExifTagOutListsに一致するタグプロパティを順番に調べてそれと一致する情報をOutTextに追加していく
        /// </summary>
        /// <param name="exifData">調べるファイルのExif情報</param>
        private void makeExifData(ExifPicture exifData) {
            foreach (var exifOutList in ExifOutLists) {
                OutText.Append(sepa + exifOutList.Value);
            }
            //foreach (var item in ExifTagOutLists) {
            //    string type = item.Type.ToLower();
            //    //ExifTagOutListのProperty名と一致するExifPicturesクラスのプロパティを探す
            //    PropertyInfo propertyInfo = exifData.GetType().GetProperty(item.Property);
            //    if (propertyInfo != null) {
            //        if (type == "string") {
            //            //string tmp = (string)propertyInfo.GetValue(exifData);
            //            //OutText.Append(sepa + tmp);
            //            OutText.Append(sepa + (string)propertyInfo.GetValue(exifData));
            //        } else if (type == "long")
            //            OutText.Append(sepa + ((long)propertyInfo.GetValue(exifData)).ToString());
            //        else if (type == "double")
            //            OutText.Append(sepa + ((double)propertyInfo.GetValue(exifData)).ToString());
            //        else if (type == "DateTime")
            //            OutText.Append(sepa + ((DateTime)propertyInfo.GetValue(exifData)).ToString());
            //        else if (type == "bool")
            //            OutText.Append(sepa + ((bool)propertyInfo.GetValue(exifData)).ToString());
            //        else
            //            OutText.Append(sepa + "不明な型によるエラー");
            //    } else {
            //        OutText.Append(sepa + item.Property + " というタグ名は存在しない");
            //    }
            //}
        }

        //exiftool コマンドライン版を使ってRawファイルなどのExifデータを取り込むルーチン
        private void button1_Click(object sender, EventArgs e) {
            //Rawファイルを読み込むテスト
            //BitmapのコンストラクタははファイルパスからではRAWファイルに対応していない
            string fPath;
            DialogResult dr = openFileDialog1.ShowDialog();
            if (dr == DialogResult.OK) {
                fPath = openFileDialog1.FileName;
                //var exiftoolcls = new ExifToolMy(fPath);
                ////ProcessStartInfo processStartInfo = new ProcessStartInfo("cmd.exe","/c dir");
                //ProcessStartInfo processStartInfo = new ProcessStartInfo("exiftool", fPath);
                ////ウィンドーを表示しない
                //processStartInfo.CreateNoWindow = true;
                //processStartInfo.UseShellExecute = false;
                ////標準主力、標準エラー出力を取得できるようにする
                //processStartInfo.RedirectStandardOutput = true;
                //processStartInfo.RedirectStandardError = true;


                //Process process=Process.Start(processStartInfo);

                //string standardOutput = process.StandardOutput.ReadToEnd();
                //string standardError = process.StandardError.ReadToEnd();

                //int exitCode = process.ExitCode;
                //process.Close();

                //MessageBox.Show(standardOutput);
            }

        }


        private void Form1_Load(object sender, EventArgs e) {
            //Settings.settingsに保存したデータの読み込み
            lblSearchPath.Text = Properties.Settings.Default.StartDir;
            chkSubDir.Checked = Properties.Settings.Default.IsIncludeSubDir;
            txtMask.Text = Properties.Settings.Default.MaskString;
            chkDirMask.Checked = Properties.Settings.Default.IsMaskDir;
            saveFileDialog1.InitialDirectory = Properties.Settings.Default.OutputDir;
            //SetOutPutFileType();
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

            //if (Properties.Settings.Default.IsTab)
            //    rbtnTab.Checked = true;
            //else
            //    rbtnCSV.Checked = true;
            //if (Properties.Settings.Default.IsLastWriteTime) chkLastWriteTime.Checked = true;
            //else chkLastWriteTime.Checked = false;
            //if (Properties.Settings.Default.IsCreationTime) chkCreationTime.Checked = true;
            //else chkCreationTime.Checked = false;
            //if (Properties.Settings.Default.IsLastAccessTime) chkLastAccessTime.Checked = true;
            //else chkLastAccessTime.Checked = false;
            //if (Properties.Settings.Default.IsSize) chkSize.Checked = true;
            //else chkSize.Checked = false;
            //if (Properties.Settings.Default.IsAttribute) chkAttribute.Checked = true;
            //else chkAttribute.Checked = false;
            //if (Properties.Settings.Default.IsExif) chkExif.Checked = true;
            //else chkExif.Checked = false;

            rbtnTab.Checked = Properties.Settings.Default.IsTab;
            chkLastWriteTime.Checked = Properties.Settings.Default.IsLastWriteTime;
            chkCreationTime.Checked = Properties.Settings.Default.IsCreationTime;
            chkLastAccessTime.Checked = Properties.Settings.Default.IsLastAccessTime;
            chkSize.Checked = Properties.Settings.Default.IsSize;
            chkAttribute.Checked = Properties.Settings.Default.IsAttribute;
            chkExif.Checked = Properties.Settings.Default.IsExif;

            rbtnAllExt.Checked = Properties.Settings.Default.ExifExtAllExt;
            rbtnExtSelect.Checked = !rbtnAllExt.Checked;
            chkMainExifExt.Checked = Properties.Settings.Default.IsChkExifMainExt;
            chkSubExifExt.Checked = Properties.Settings.Default.IsChkExifSubExt;
            chkExifTextExt.Checked = Properties.Settings.Default.IsChkExifTextExt;
            txtAdditionalExifExtLists.Text = Properties.Settings.Default.ExifTextExt;
            exifToolUse = Properties.Settings.Default.ExifToolUse;
            switch (exifToolUse) {
                case 0:
                    rbExifToolDonotUse.Checked = true;
                    break;
                case 1:
                    rbExifToolDonotUse.Checked = true;
                    break;
                case 2:
                default:
                    rbExifToolAllways.Checked = true;
                    break;
            }

            mask = txtMask.Text;
            if (chkDirMask.Checked) {
                dmask = mask;
            } else {
                dmask = "*.*";
            }

            SetOutPutFileType();
            GetExifExt();
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

        private void chkExif_CheckedChanged(object sender, EventArgs e) {
            groupBoxExifExt.Enabled = chkExif.Checked;
        }

        //ディレクトリもマスクするボタンのチェック変更
        private void chkDirMask_CheckedChanged(object sender, EventArgs e) {
            if (chkDirMask.Checked) {
                dmask = mask;
            } else {
                dmask = "*.*";
            }
        }

        private void txtAdditionalExifExtLists_TextChanged(object sender, EventArgs e) {
            GetExifExt();
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
            Properties.Settings.Default.ExifExtAllExt = rbtnAllExt.Checked;
            Properties.Settings.Default.IsChkExifMainExt = chkMainExifExt.Checked;
            Properties.Settings.Default.IsChkExifSubExt = chkSubExifExt.Checked;
            Properties.Settings.Default.IsChkExifTextExt = chkExifTextExt.Checked;
            Properties.Settings.Default.ExifTextExt = txtAdditionalExifExtLists.Text;
            Properties.Settings.Default.ExifToolUse = exifToolUse;

            Properties.Settings.Default.Save();

            WriteExifListToContent();
            this.Close();
        }

        private void btnCopy2Clip_Click(object sender, EventArgs e) {
            Clipboard.SetDataObject(OutText.ToString());
        }

        // SelctFormでEXIF項目を変更する
        private void btnSetExif_Click(object sender, EventArgs e) {
            if (!IsReadCSV)
                ReadExifListFromContent(SettingCSVPath);
            SelectForm tgtForm = new() {
                AllLists = ExifLists,
                exifListTitle = ExifListTitle
            };
            tgtForm.ShowDialog();
            //ExifLists = tgtForm.AllLists;
            //WriteExifListToContent();
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
            //StringBuilder bd = new StringBuilder();
            if (saveFileDialog1.ShowDialog() == System.Windows.Forms.DialogResult.OK) {
                lblOutFile.Text = saveFileDialog1.FileName;
                //getOutStr(ref bd);
                //System.IO.File.WriteAllText(saveFileDialog1.FileName, bd.ToString(), Encoding.Default);
                System.IO.File.WriteAllText(saveFileDialog1.FileName, OutText.ToString(), Encoding.Default);
            }
        }

        //Exifを調べるファイルの拡張子のリスト  ExifExtLists を作成する
        private void GetExifExt() {
            ExifExtLists.Clear();
            ExifExtLists.AddRange(DefaultExifExt.ToUpper().Split(','));
            string str;
            if (chkMainExifExt.Checked) {
                str = chkMainExifExt.Text;
                //表示の都合で入る改行を取り除く
                str = str.Replace("\r", "").Replace("\n", "");
                ExifExtLists.AddRange(str.ToUpper().Split(','));
            }
            if (chkSubExifExt.Checked) {
                str = chkSubExifExt.Text;
                str = str.Replace("\r", "").Replace("\n", "");
                ExifExtLists.AddRange(str.ToUpper().Split(','));
            }
            if (chkExifTextExt.Checked) {
                str = txtAdditionalExifExtLists.Text;
                str = str.Replace("\r", "").Replace("\n", "");
                ExifExtLists.AddRange(str.ToUpperInvariant().Split(','));
            }
        }

        private void rbExifToolDonotUse_CheckedChanged(object sender, EventArgs e) {
            if (rbExifToolDonotUse.Checked) exifToolUse = 0;
        }

        private void rbExiToolIfNeed_CheckedChanged(object sender, EventArgs e) {
            if (rbExiToolIfNeed.Checked) exifToolUse = 1;
        }

        private void rbExifToolAllways_CheckedChanged(object sender, EventArgs e) {
            if (rbExifToolAllways.Checked) exifToolUse = 2;
        }

        //Test用
        private void button2_Click(object sender, EventArgs e) {
            //if (IsMadeExifToolConv == false) {
            //    ReadExifToolConv(SettingExifToolCSVPath);
            //    IsMadeExifToolConv = true;
            //}
            string fullPath = @"D:\Temp\Test\JPG\20240623_193051723.JPG";
            Image newImage = Image.FromFile(fullPath);

            foreach (PropertyItem propertyItem in newImage.PropertyItems) {
                //Console.WriteLine($"ID: {propertyItem.Id}, Type: {propertyItem.Type}, Length: {propertyItem.Len}");
                Debug.Print($"ID: {propertyItem.Id}, Type: {propertyItem.Type}, Length: {propertyItem.Len}");
            }
            //// System.Drawing.Bitmap bmp = null;
            //System.Drawing.Image bmp = null;
            //try {
            //    //bmp = new System.Drawing.Bitmap(fullPath);
            //    Image newImage = Image.FromFile(fullPath);
            //    bmp = Image.FromFile(fullPath);

            //    //bmp = new System.Drawing.Image.FromFile         //(fullPath);
            //    //bmp= Image.FromFile(fullPath);
            //} catch (Exception exception) {
            //    string error = fullPath + " : " + exception.Message;
            //    // Console.WriteLine(error);
            //    System.Diagnostics.Debug.Print(error);
            //    //System.Windows.Forms.MessageBox.Show(error);                
            //}
            //Debug.Print( bmp.PropertyItems.ToString());




        }
    }
}
