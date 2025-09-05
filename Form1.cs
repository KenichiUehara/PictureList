//using Microsoft.Win32;
using Microsoft.VisualBasic.FileIO;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.Design;
using System.Data;
// /*using ExifLib;*/          //未使用かも
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.Versioning;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using System.Windows.Resources;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace PictureList {
    public partial class Form1 : Form {
        private static List<Exiflist> exifLists = new();  //Exif項目のリスト
        public static List<Exiflist> ExifTagOutLists = new(); //出力するExif項目のリスト          
        public static string ExifListTitle;                                    //読み込んだExif CSVファイルの先頭行
        StringBuilder OutText = new();                    //出力テキスト
        StringBuilder OutTitleList = new();               //出力用のタイトル文字列

        public const string SettingCSVPath = "./OutputExifList/PictureListExif.csv"; //Exif CSVへのパス
        public const string SettingDefaultCSVPath = "./OutputExifList/DefaultPictureListExif.csv"; //同上デフォルト
        public static bool IsReadCSV = false;                           //Exif CSVファイルを読み込んだかのフラグ
        Dictionary<string, string> ExifExtDic = new();                 //Exif調査用拡張子のリスト
        // ExifAPIを適用できるファイルの拡張子のリスト
        static public readonly List<string> APIEXTs = new() { "jpg", "jpeg", "tiff", "tif" };
        // ExifToolで調査できる拡張子のリスト
        static public List<string> ExifToolEXTs = new();
        //ExifTool関係
        public const string ExifToolValueCSVPath = "./OutputExifList/ExifToolValueTranslate.csv"; //ExifTool変換CSVファイルへのパス

        public class ExifTransDic : Dictionary<string, string> {
            public ExifTransDic() : base() { }

            //public static implicit operator string(ExifTransDic v) {
            //    throw new NotImplementedException();
            //}
        }

        // Change the field `ExifToolListsDic` to be static since it is being accessed in a static context.
        private static Dictionary<string, Dictionary<string, ExifTransDic>> ExifToolListsDic = new();
        // private static bool IsReadExifToolConv = false; // ExifTool変換CSVファイルを読み込んだかのフラグ

        // プログレスバーなどのための変数
        private int FileSum, FileDone, DirSum, DirDone, BothSum, CountInterval, NextDispNum; //探索数カウント用
        const int ProgressBarDivide = 200;
        string mask, dmask, sepa, sepaHead, sepaTail = "\r\n";
        private bool IsMadeExifToolConv = false;
        //static private bool IsExifAllSet = true;

        private int exifToolUse;
        public static List<Exiflist> ExifLists { get => exifLists; set => exifLists = value; }

        //ExifToolConvList ExifToolのExifPictureの項目への対照ディクショナリ用
        class ExifToolItems {
            // public int qty;
            public string[] exifToolItem = new string[4];
        };

        static Dictionary<string, ExifToolItems> ExifConvList = new Dictionary<string, ExifToolItems>();
        //private Dictionary<string, string> exifOutLists = new Dictionary<string, string>();

        /// <summary>
        /// ExifToolListsを pathのcsvファイルから作成する
        /// ExifToolで使用するExifTool名の選択肢のCIPA向気の翻訳の対応表である
        /// </summary>
        /// <param name="path"></param>
        static public void ReadExifToolConv(string path) {
            ExifToolListsDic.Clear();
            bool IsFistLine = true; // 最初の行は項目名なので無視する
            string TagID, ToolName, CIPAName;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            //using var sr = new StreamReader(path, System.Text.Encoding.GetEncoding("Shift_Jis"));
            TextFieldParser parser = new TextFieldParser(path, Encoding.GetEncoding("Shift_JIS")); //ファイルとエンコードを指定する
            parser.TextFieldType = FieldType.Delimited;    // 区切子での処理を指定
            parser.SetDelimiters(",");                     // ","区切り 
            parser.HasFieldsEnclosedInQuotes = true;       // 値がダブルコーテーションで囲まれている
            parser.TrimWhiteSpace = true;                  // 値をトリミングする
            while (!parser.EndOfData) {
                string[] str = parser.ReadFields(); // 行を読み込む
                if (str.Length < 3) {
                    MessageBox.Show(path + "\r\n" + string.Join(",", str) + "\r\nの内容が異常です", "エラー", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    continue; // 異常な行はスキップ
                } else {
                    if (!IsFistLine) { // 最初の行は項目名なので無視する
                        TagID = str[0];
                        ToolName = str[1];
                        CIPAName = str[2];
                        if (!ExifToolListsDic.ContainsKey(TagID)) { // TagIDが存在しない場合
                            var exifToolDicWrapper = new Dictionary<string, ExifTransDic> {
                                                                    { ToolName, new ExifTransDic { [ToolName] = CIPAName } }
                                                                };
                            ExifToolListsDic.Add(TagID, exifToolDicWrapper);
                        } else {
                            // TagIDが既に存在する場合は、ToolNameとCIPANameを追加
                            if (!ExifToolListsDic[TagID].ContainsKey(ToolName)) {
                                ExifToolListsDic[TagID].Add(ToolName, new ExifTransDic { [ToolName] = CIPAName });
                            }
                        }
                    } else
                        IsFistLine = false;
                }
            }
            //IsReadExifToolConv = true; // ExifTool変換CSVファイルを読み込んだ
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
            public string EValue { get; set; }
            public int Order { get; set; }
            public string TagID { get; set; } // TagIDを追加 

            /// <summary>
            /// PictureListExif.csvから読み込むコンストラクト
            /// </summary>
            /// <param name="property">Exif名</param>
            /// <param name="tagName">Exifの用途名</param>
            /// <param name="type">データ型を表す文字列</param>
            /// <param name="EValue">ExifTagOutListsで値を保持するために用意</param>
            /// <param name="order">リストに表示する順番。文字列で受取、数値に変換、数字以外は0にする</param>
            public Exiflist(string property, string tagName, string type, string order, string tagID) {
                Property = property;
                TagName = tagName;
                Type = type;
                EValue = "";
                SetOrder(order);
                TagID = tagID;
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
                        exiflist.TagID = FormTo0xffff(str[4]); // 0xXXXXの形式を0xffffの形式に変換する
                        ExifLists.Add(exiflist);
                    }
                }
            }
            if (ExifLists.Count == 0) {     // ExifListsがトラブルで空のときにタイトル行を追加してエラーを回避
                ExifListTitle = "PictureListExif.csv,などが破損しました,デフォルト,読み直してください,";
            }
            IsReadCSV = true;
        }

        // Change the method `FormTo0xffff` to be static since it is being called in a static context.
        /// <summary>
        /// フォーマット "0xXXXX" の16進文字列を、標準フォーマット "0xffff" 小文字の１６進数に変換する。
        /// 元の形式が16進数でない場合は元の値を返す
        /// </summary>
        /// <param name="str">変換候補の文字列</param>
        /// <returns>strが0xまたは0Xで始まる4桁の16進数の場合のみ0xffffけいしきの0xで始まる4桁の16進数に
        /// 変換されそれ以外は元のstrを返す</returns>
        private static string FormTo0xffff(string str) {
            if ((str.Length == 6) && (str.Substring(0, 1) == "0") && str.Substring(1, 1).ToUpper() == "X") {
                if (Int32.TryParse(str.Substring(2), System.Globalization.NumberStyles.HexNumber, null, out int result)) {
                    return ("0x" + result.ToString("x4"));
                }
            }
            return str;
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
            if (!IsReadCSV)
                return;
            DialogResult result = MessageBox.Show("Exif表示項目の変更を保存するなら はい", "確認", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (result != DialogResult.Yes) return; //はいでなければ保存しない
            string path = SettingCSVPath;
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using StreamWriter sw = new(path, false, System.Text.Encoding.GetEncoding("shift_jis"));
            sw.WriteLine(ExifListTitle);
            foreach (var exifItem in ExifLists) {
                string property = exifItem.Property;
                string tagName = exifItem.TagName;
                string type = exifItem.Type;
                //string eValue = "";
                string order = "";
                string tagID = exifItem.TagID; // TagIDを追加
                if (exifItem.Order != 0)
                    order = exifItem.Order.ToString();
                sw.WriteLine(property + "," + tagName + "," + type + "," + order + "," + tagID);
            }
        }

        public Dictionary<string, int> OrgExiftTagOutDic { get; set; } = new(); //出力用のExif項目のディクショナリの大本
        public Dictionary<string, int> ExifTagOutDic { get; set; } = new();    //出力用のExif項目のディクショナリ
        // public Dictionary<string, string> ExifOutLists { get; private set; } //OrgExiftOutDicの探索時の変更される辞書

        // OrgExifTagOutDic を作成する。これには合成関数用のTagID（そのValueは -1)も含まれる
        void SetOrgExifTagOutDic() {
            OrgExiftTagOutDic.Clear();
            for (int i = 0; i < ExifTagOutLists.Count; i++) {
                OrgExiftTagOutDic.Add(ExifTagOutLists[i].TagID, ExifTagOutLists[i].Order);
            }
            //合成関数使用時に必要なExifIDを追加
            if (OrgExiftTagOutDic.ContainsKey("G-WidthHeight")) { //WidthHeightは0xA0002 PixelXDimensionと０ｘA0003 PixelYDimensionを使用
                if (!OrgExiftTagOutDic.ContainsKey("0xa0002"))
                    OrgExiftTagOutDic.Add("0xA0002", -1);
                if (!OrgExiftTagOutDic.ContainsKey("0xa0003"))
                    OrgExiftTagOutDic.Add("0xA0003", -1);
            }
            if (OrgExiftTagOutDic.ContainsKey("G-ShutterSpeed")) { //G-ShutterSpeedは 0x829A ExposureTime を使用
                if (!OrgExiftTagOutDic.ContainsKey("0x829a"))
                    OrgExiftTagOutDic.Add("0x829a", -1);
            }
            if (OrgExiftTagOutDic.ContainsKey("G-Resolution")) { //Resolutionは0x011A XResolution,0x011B YResolution を使用
                if (!OrgExiftTagOutDic.ContainsKey("0x011a"))
                    OrgExiftTagOutDic.Add("0x011a", -1);
                if (!OrgExiftTagOutDic.ContainsKey("0x011b"))
                    OrgExiftTagOutDic.Add("0x011b", -1);
                if (!OrgExiftTagOutDic.ContainsKey("0x0128"))
                    OrgExiftTagOutDic.Add("0x0128", -1);
            }
            if (OrgExiftTagOutDic.ContainsKey("G-GPSLocation")) { //G-GPSLocationは0x0001 GPSLatitudeRef,0x0002 GPSLatitude,0x0003
                                                                  //GPSLongitudeRef,0x0004 GPSLongitude を使用
                if (!OrgExiftTagOutDic.ContainsKey("0x0001"))
                    OrgExiftTagOutDic.Add("0x0001", -1);
                if (!OrgExiftTagOutDic.ContainsKey("0x0002"))
                    OrgExiftTagOutDic.Add("0x0002", -1);
                if (!OrgExiftTagOutDic.ContainsKey("0x0003"))
                    OrgExiftTagOutDic.Add("0x0003", -1);
                if (!OrgExiftTagOutDic.ContainsKey("0x0004"))
                    OrgExiftTagOutDic.Add("0x0004", -1);
            }
            if (OrgExiftTagOutDic.ContainsKey("G-GPSHeight")) { //G-GPSHeight は 0x0005　GPSAltitudeRef,0x0006 GPSAltitude を使用
                if (!OrgExiftTagOutDic.ContainsKey("0x0005"))
                    OrgExiftTagOutDic.Add("0x0005", -1);
                if (!OrgExiftTagOutDic.ContainsKey("0x0006"))
                    OrgExiftTagOutDic.Add("0x0006", -1);
            }
        }


        //以下はFilesからの改良部分

        /// <summary>
        /// 探索中は各実行ボタンを押せないようにする
        /// </summary>
        /// <param name="busy">trueなら実行中</param>
        private void InSaerching(bool busy) {
            foreach (Control c in this.Controls) {
                if (c is Button || c is GroupBox)
                    c.Enabled = !busy;
            }
            if (!chkExif.Checked)   // Exifを出力しない場合はExifの拡張子グループを無効にする
                groupBoxExifExt.Enabled = false;
        }

        /// <summary>
        /// ファイル探索のメインルーチン
        /// </summary>
        /// <param name="Dir">探索の基準になるディレクトリ</param>
        private void GetDirsFilesList(string Dir) {

# if DEBUG
            // 実行時間の計測
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
#endif
            InSaerching(true);
            if (chkExif.Checked)    //Exifを出力する場合は対象の拡張子リストを得る
                GetExifExtDic();
            //Exif項目を読む必要がある場合はExif項目を読み込む Imageの場合はその判定に必要
            if ((chkExif.Checked || rbtnOnlyImage.Checked || rbtnOnlyExif.Checked) && !IsReadCSV) {
                ReadExifListFromContent(SettingCSVPath);
            }
            //フォルダのリストをフォルダのマスク、サブディレクトリも含めるかに従って作成する
            List<string> dirLists = new();  //フォルダのリスト
            var serchOption = System.IO.SearchOption.TopDirectoryOnly;    //SearchOptionはSystem.IO名前空間の列挙型
            if (chkSubDir.Checked)
                serchOption = System.IO.SearchOption.AllDirectories;
            txtComment.Text = "";
            try {
                // LINQ Directory.EnumerateDirectoriesは高速に処理できるので採用
                if (chkSubDir.Checked) {
                    var dirs = from dir in Directory.EnumerateDirectories(Dir, dmask, serchOption)
                               select dir;
                    dirLists = dirs.ToList(); //ディレクトリのリスト
                }

            } catch (UnauthorizedAccessException) {
                txtComment.Text = "必要なアクセス許可がありません\n";
            } catch (PathTooLongException) {
                txtComment.Text = "パス名として長すぎます\n";
            } catch (DirectoryNotFoundException) {
                txtComment.Text = "マップされていない参照など無効です\n";
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

            //ここからファイル情報を取り出す
            //まずExif出力用のリストとタイトルの行の作成
            makeExifTagOutLists();
            SetOrgExifTagOutDic();  //ExifTagOutDicの大本となるディクショナリをセット
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

                    //出力にFileInfo項目を追加する。
                    foreach (System.IO.FileInfo finfo in fInfoList) {
                        string CExt = finfo.Extension.ToLower().Replace(".", "");
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
                        if(chkExt.Checked)
                            OutText.Append(sepa + CExt);


                        //Exif属性を表示する
                        //Exif属性取得には時間が掛かると思われるためここでExif属性が不要な場合は先に進まないようにするい
                        if (chkExif.Checked && ( rbtnAllExt.Checked || ExifExtDic.ContainsKey(CExt) || CExt=="gif" || CExt=="bmp" || CExt=="png")) {
                            //ExifTagOutListsのExif値を初期化
                            foreach (Exiflist eList in ExifTagOutLists) {
                                eList.EValue = ""; //初期値は空文字列
                            }
                            //ExifTagOutDicをOrgExiftTagOutDicからハードコピー
                            ExifTagOutDic.Clear();
                            foreach (var exiflis in OrgExiftTagOutDic) {
                                ExifTagOutDic.Add(exiflis.Key, exiflis.Value);
                            }
                            //拡張子がgif,bmpの時はBitmapで画像情報を取得する
                            if (CExt == "gif" || CExt == "bmp" || CExt=="png") {
                                ChkGIFBMP(finfo.FullName);
                            } else { 
                                //ExifAPIクラスでExif情報を取得。拡張子がjpg, jpeg, tiff, tifの時のみ
                                if (APIEXTs.Contains(CExt) && exifToolUse != 2) {
                                    if (ExifTagOutDic.Count > 0) {
                                        // ExifAPIを使用してExif情報を取得
                                        ExifAPI exifData = new(finfo.FullName, ExifTagOutLists, ExifTagOutDic);
                                    }
                                }
                                //ExixToolを調べる必要のある時だけExifToolで調べる
                                if (/* IsExifAllSet && */ exifToolUse != 0 && ExifTagOutDic.Count > 0) {
                                    //
                                    if (!IsMadeExifToolConv) {  // ExifToolConvが読み込まれていなければ読み込む
                                        ReadExifToolConv(ExifToolValueCSVPath);
                                        IsMadeExifToolConv = true;
                                    }
                                    var exifToolData = new ExifToolMy(finfo.FullName, ExifTagOutLists, ExifTagOutDic, ExifToolListsDic, CExt);  //ExifToolのExifの読込
                                }
                            }
                            makeExifData(); // ExifTagOutListsの値をOutTextに追加する
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
# if DEBUG
            stopwatch.Stop();
            Debug.Print($"処理数: {FileDone + DirDone}  時間: {stopwatch.ElapsedMilliseconds / 1000.0}Sec");
#endif
        }

        // Exif検索対象の拡張子の辞書を作成する
        private void GetExifExtDic() {
            ExifExtDic.Clear();
            if (rbtnExtSelect.Checked) {
                //if (chkMainExifExt.Checked) {
                var exts = ".JPG,.JPEG,TIF,TIFF".Split(',');
                foreach (var ext in exts) {
                    string sExt = ext.ToLower().Trim();
                    if (sExt[0] == '.')
                        sExt = sExt.Substring(1); //先頭のドットを削除
                    if (!ExifExtDic.ContainsKey(sExt)) {
                        ExifExtDic.Add(sExt, "0");
                    }
                }
                //}
                if (chkMainExifExt.Checked) {
                    exts = chkMainExifExt.Text.Split(',');
                    foreach (var ext in exts) {
                        string sExt = ext.ToLower().Trim();
                        if (sExt[0] == '.')
                            sExt = sExt.Substring(1); //先頭のドットを削除
                        if (!ExifExtDic.ContainsKey(sExt)) {
                            ExifExtDic.Add(sExt, "1");
                        }
                    }
                }
                if (chkSubExifExt.Checked) {
                    exts = chkSubExifExt.Text.Split(',');
                    foreach (var ext in exts) {
                        string sExt = ext.ToLower().Trim();
                        if (sExt[0] == '.')
                            sExt = sExt.Substring(1); //先頭のドットを削除
                        if (!ExifExtDic.ContainsKey(sExt)) {
                            ExifExtDic.Add(sExt, "2");
                        }
                    }
                }
                if (chkExifTextExt.Checked) {
                    exts = txtAdditionalExifExtLists.Text.Split(',');
                    foreach (var ext in exts) {
                        string sExt = ext.ToLower().Trim();
                        if (sExt[0] == '.')
                            sExt = sExt.Substring(1); //先頭のドットを削除
                        if (!ExifExtDic.ContainsKey(sExt)) {
                            ExifExtDic.Add(sExt, "3");
                        }
                    }
                }
            }
        }

        
        /// <summary>
        /// ディレクトリ情報を出す設定ならその情報を追加する
        /// </summary>
        /// <param name="dirInfo">そのディレクトリのDirectoryInfo</param>
        /// <param name="pathName">DirectoryInfoはバージョンにより相対パスの場合もあるので絶対パスを指定して持ってくる</param>
        private void makeDirData(System.IO.DirectoryInfo orgDirInfo, string pathName) {
            DirectoryInfo[] dirInfos = orgDirInfo.GetDirectories(dmask, System.IO.SearchOption.TopDirectoryOnly);

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
                if(chkExt.Checked)

                OutText.Append(sepaTail);
                DirDone++;
            }
        }

        /// <summary>
        /// fullpathで示すBtmapを返そうとする
        /// </summary>
        /// <param name="fullpath"></param>
        /// <returns></returns>
        static public Bitmap MakeBMP(string fullpath) {
            System.Drawing.Bitmap bmp = null;

            try {
                bmp = new System.Drawing.Bitmap(fullpath);
                //bmp = new System.Drawing.Bitmap(fullPath);
                //bmp= Image.FromFile(fullPath);
            } catch (Exception exception) {
                string error = fullpath + " : " + exception.Message;
                // Console.WriteLine(error);
#if DEBUG
                System.Diagnostics.Debug.Print(error);
#endif
                //System.Windows.Forms.MessageBox.Show(error);
                //DateTimeOriginal = ""; //以前はなぜか error;
            }
            return bmp;
        }

        //fullpathで渡されるgif,bmp,pngのファイルに対してBitmap情報の値をExif値の代わりに代入する
        private void ChkGIFBMP(string fullpath) {
            Bitmap bmp = MakeBMP(fullpath);
            if (bmp != null) {
                if (ExifTagOutDic.ContainsKey("G-WidthHeight")) {
                    int idx = ExifTagOutDic["G-WidthHeight"];
                    if (idx > 0) {
                        ExifTagOutLists[idx - 1].EValue = bmp.Width.ToString() + " x " + bmp.Height.ToString();
                        ExifTagOutDic.Remove("G-WidthHeight");
                    }
                }
                if (ExifTagOutDic.ContainsKey("G-Resolution")) {
                    int idx = ExifTagOutDic["G-Resolution"];
                    if (idx > 0) {
                        ExifTagOutLists[idx - 1].EValue = bmp.HorizontalResolution.ToString("0.##") + " x " + bmp.VerticalResolution.ToString("0.##") + " " + bmp.PixelFormat.ToString();
                        ExifTagOutDic.Remove("G-Resolution");
                    }
                }
                if(ExifTagOutDic.ContainsKey("0x0100")) { //ImageWidth
                    int idx = ExifTagOutDic["0x0100"];
                    if (idx > 0) {
                        ExifTagOutLists[idx - 1].EValue = bmp.Width.ToString();
                        ExifTagOutDic.Remove("0x0100");
                    }
                }
                if (ExifTagOutDic.ContainsKey("0x0101")) {
                    int idx = ExifTagOutDic["0x0101"];
                    if (idx > 0) {
                        ExifTagOutLists[idx - 1].EValue = bmp.Height.ToString();
                        ExifTagOutDic.Remove("0x0101");
                    }
                }
                if (ExifTagOutDic.ContainsKey("0x011a")) { //ImageWidth
                    int idx = ExifTagOutDic["0x011a"];
                    if (idx > 0) {
                        ExifTagOutLists[idx - 1].EValue = bmp.HorizontalResolution.ToString();
                        ExifTagOutDic.Remove("0x011a");
                    }
                }
                if (ExifTagOutDic.ContainsKey("0x011b")) {
                    int idx = ExifTagOutDic["0x011b"];
                    if (idx > 0) {
                        ExifTagOutLists[idx - 1].EValue = bmp.VerticalResolution.ToString();
                        ExifTagOutDic.Remove("0x011b");
                    }
                }
                bmp.Dispose();
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

        /// <summary>
        /// 引数で指定したExifPictureクラスのタグデータからExifTagOutListsに一致するタグプロパティを順番に調べてそれと一致する情報をOutTextに追加していく
        /// </summary>
        /// <param name="exifData">調べるファイルのExif情報</param>
        private void makeExifData() {
            foreach (var exifOutList in ExifTagOutLists) {
                OutText.Append(sepa + exifOutList.EValue);
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
            //chkLastWriteTime.Checked = Properties.Settings.Default.IsLastWriteTime;　ダブっていたので削除
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

            rbtnTab.Checked = Properties.Settings.Default.IsTab;
            chkLastWriteTime.Checked = Properties.Settings.Default.IsLastWriteTime;
            chkCreationTime.Checked = Properties.Settings.Default.IsCreationTime;
            chkLastAccessTime.Checked = Properties.Settings.Default.IsLastAccessTime;
            chkSize.Checked = Properties.Settings.Default.IsSize;
            chkAttribute.Checked = Properties.Settings.Default.IsAttribute;
            chkExif.Checked = Properties.Settings.Default.IsExif;
            chkExt.Checked = Properties.Settings.Default.IsExt;

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
                    rbExiToolIfNeed.Checked = true;
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
            // GetExifExt();
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

        //private void txtAdditionalExifExtLists_TextChanged(object sender, EventArgs e) {
        //    GetExifExt();
        //}

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
            Properties.Settings.Default.IsIncludeSubDir = chkSubDir.Checked;
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
            Properties.Settings.Default.IsExt = chkExt.Checked;
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
            btnCopy2Clip.Enabled = false;
            MessageBox.Show("クリップボードにExcel用などに出力内容をコピーしました", "情報", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clipboard.SetDataObject(OutText.ToString());
            btnCopy2Clip.Enabled = true;
        }

        // SelctFormでEXIF項目を変更する
        private void btnSetExif_Click(object sender, EventArgs e) {
            if (!IsReadCSV)
                ReadExifListFromContent(SettingCSVPath);
            SelectForm tgtForm = new() {
                AllLists = ExifLists,
                //exifListTitle = ExifListTitle　exifListTitelはForm1のExifListTitleを参照するようにしたので不要
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
                OutTitleList.Append(sepa + "更新日時");
            if (chkCreationTime.Checked)
                OutTitleList.Append(sepa + "作成日時");
            if (chkLastAccessTime.Checked)
                OutTitleList.Append(sepa + "アクセス日時");
            if (chkSize.Checked)
                OutTitleList.Append(sepa + "サイズ");
            if (chkAttribute.Checked)
                OutTitleList.Append(sepa + "属性");
            if (chkExt.Checked)
                OutTitleList.Append(sepa + "拡張子");
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
            ExifToolEXTs.Clear();
            ExifToolEXTs.AddRange(APIEXTs);
            if (chkMainExifExt.Checked) {
                //表示の都合で入る改行を取り除いて大文字にして追加する（以下同じ
                ExifToolEXTs.AddRange(chkMainExifExt.Text.Replace("\r", "").Replace("\n", "").ToUpper().Split(','));
            }
            if (chkSubExifExt.Checked) {
                ExifToolEXTs.AddRange(chkSubExifExt.Text.Replace("\r", "").Replace("\n", "").ToUpper().Split(','));
            }
            if (chkExifTextExt.Checked) {
                ExifToolEXTs.AddRange(chkExifTextExt.Text.Replace("\r", "").Replace("\n", "").ToUpper().Split(','));
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

        
    }
}
