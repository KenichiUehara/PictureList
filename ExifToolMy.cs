using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.TextFormatting;
using Windows.ApplicationModel.Contacts;
using Windows.Devices.WiFiDirect;
using static PictureList.Form1;

namespace PictureList {
    internal class ExifToolMy {

        //ExfToolのEXIFグループのTagIDと値を格納する辞書
        public Dictionary<string, string> ExifToolValueDic = new();
        //　Exif出力用のリストを取り合えず内部変数として使用
        private List<Exiflist> exifTagOutLists = new List<Exiflist>();
        // Exif出力用のリストのインデックスをTagIDで指定するための辞書
        private Dictionary<string, int> exifTagOutDic = new Dictionary<string, int>();
        // private Dictionary<string, Dictionary<string, ExifTransDic>> _exifToolDic = new();
        private Dictionary<string, Dictionary<string, ExifTransDic>> exifToolListsDic = new();
        private string cExt;
        //ExifToolMyで使用するExifToolの出力読み込み時の拡張子の優先グループの辞書
        public static Dictionary<string, string> PriorityExifGroup = new Dictionary<string, string> {
            {"arw","EXIF:SubIFD" },
            {"dng", "EXIF:SubIFD" },
            {"nef","EXIF:SubIFD1" },
        };

        public ExifToolMy(string fullpath, List<Exiflist> exifTagOutLists, Dictionary<string, int> exifTagOutDic, Dictionary<string, Dictionary<string, ExifTransDic>> exifToolListsDic, string CExt) {
            this.exifTagOutLists = exifTagOutLists;
            this.exifTagOutDic = exifTagOutDic;
            this.exifToolListsDic = exifToolListsDic;
            this.cExt = CExt;
            this.exifToolMy(fullpath);
        }

        private void exifToolMy(string fullpath) {
            string stdout;
            ExifToolValueDic.Clear();
            stdout = GetRawExif(fullpath);          //ExifToolでデータを取得
            if (string.IsNullOrEmpty(stdout)) {
#if DEBUG
                Debug.Print("ExifToolの出力が空です。ファイルパス: " + fullpath);
#endif
                return; // 出力が空の場合は何もしない
            }
#if DEBUG
            Debug.Print($"ExifToolMy内 {fullpath}");
#endif
            GetExifToolLDic(stdout); // EXIFツールデータを辞書に変換

            //未所得の辞書リストExifTagOutDicのTagIDが値がExifToolVakueDicのキーに含まれていたらその値を所得する
            foreach (var eDic in exifTagOutDic) {   //  値未取得の辞書リストを順に調べる
                if (eDic.Value > 0) {            // 値が0より大きければ出力すべき項目へのインデックスを値として持つTagIDをキーとするもの
                    if (ExifToolValueDic.ContainsKey(eDic.Key)) {    // TagIDがExifToolの全取得値ExifToolValueDicの中に見つかった
                        var foundTagID = eDic.Key; // ExifToolValueDicのキーを取得
                        var foundTagValue = ExifToolValueDic[eDic.Key]; // 同じく値を取得
                        exifTagOutLists[eDic.Value - 1].EValue = foundTagValue; // ExifToolValueDicの値を出力用リストに設定
                        // CIPA風への訳があるときの処理
                        if (exifToolListsDic.ContainsKey(foundTagID)) { // 翻訳辞書ExifToolListsDicにToolTagIDが存在する場合
                            Dictionary<string, PictureList.Form1.ExifTransDic> ToolDicLists = exifToolListsDic[foundTagID];
                            // CIPA風の訳がある場合はそれを使用
                            if (ToolDicLists.ContainsKey(foundTagValue)) { // CIPA風の訳がある場合はそれを採用
                                string TransVal = ToolDicLists[foundTagValue][foundTagValue];
                                exifTagOutLists[eDic.Value - 1].EValue = TransVal; // 辞書から値を取得してリストに設定
                                foundTagValue = TransVal; // 変換後の値を更新
                            } else {
                                // CIPA風の訳が無ければエラーログを作成
                            }
                        }

                        // 変換式があればそれを実行。なければそのまま値を使用
                        string fStr = foundTagValue;    // 変換前の値
                        string tStr = fStr;             // tStrは変換後の値を入れるためのもので変換が無ければ =fStr
                        switch (eDic.Key) {
                            case "0x829a": tStr = GetExpodureDecimal(fStr); break;   // 分数表示を少数に
                            case "0x829d":
                            case "0x9205": tStr = "F " + fStr; break; // 先頭に"F " を付ける
                            case "0x8827": tStr = GetSensitivity(fStr); break; // 先頭に"撮影感度 " を付ける
                            case "0x9101": tStr = fStr.Replace(" ", ""); break; // 空白を取り除く
                            case "0x9201": tStr = GetShutterSpeedToApex(fStr); break; // シャッター速度をAPEX値に変換
                            case "0x9202": tStr = GetFNumberToApex(fStr); break; // F値をAPEX値に変換
                            case "0x920a": tStr = GetFocusNumber(fStr); break; // 焦点距離の表示を小数点以下が0なら表示しない形式に変換
                            case "0xa000": tStr = GetFlashPixVersion(fStr); break; // FlashPixバージョンを変換
                            case "0xa404": if (tStr == "undef") tStr = "未定義"; break; // undefなら未定義
                            case "0xa431": tStr = "S/N " + fStr; break; // "S/N " を付ける
                            case "0xa435": tStr = "S/N " + fStr; break; // "S/N " を付ける
                            case "0x0002":      //0x0002,0x0004,0x0014,0x0016　緯度経度の変換
                            case "0x0004":
                            case "0x0014":
                            case "0x0016":
                                if (!GetGPSCoordinates(fStr, out tStr)) {
                                    tStr = "GPS情報不完全"; // GPS情報が不完全な場合はそのまま
                                }
                                break;
                            case "0x001d": tStr = fStr.Replace(":", "/"); break; // ExifToolでは日付の区切りが ":" なので修正
                        }
                        if (fStr != tStr) {
                            exifTagOutLists[eDic.Value - 1].EValue = tStr;
                        }
                    }
                }
            }
            // 合成関数を処理
            string fgStr = "";    // 変換前の値
            string tgStr = fgStr;
            // G-WidthHeigh 0x0100,101は圧縮画像用なので0xa002の値が無いときに使用
            if (exifTagOutDic.ContainsKey("G-WidthHeight")) {
                string sWidth = GetExifLastValue("0xa002");
                string sHeight = GetExifLastValue("0xa003");
                if(string.IsNullOrEmpty(sWidth) || string.IsNullOrEmpty(sHeight)) {
                    sWidth = GetExifLastValue("0x0100");
                    sHeight = GetExifLastValue("0x0101");
                }
                if (!string.IsNullOrEmpty(sWidth) && !string.IsNullOrEmpty(sHeight)) {
                    exifTagOutLists[exifTagOutDic["G-WidthHeight"] - 1].EValue = $"{sWidth} x {sHeight}";
                }
            }
            // G-ShutterSpeed
            if (exifTagOutDic.ContainsKey("G-ShutterSpeed")) {
                if (ExifToolValueDic.ContainsKey("0x829a")) {
                    exifTagOutLists[exifTagOutDic["G-ShutterSpeed"] - 1].EValue = ExifToolValueDic["0x829a"] + " 秒";
                }
            }
            // G-Resolution
            if (exifTagOutDic.ContainsKey("G-Resolution")) {
                string Xreso = GetExifLastValue("0x011a");
                string Yreso = GetExifLastValue("0x011b");
                string ResoUnit = GetExifLastValue("0x0128");
                if (!string.IsNullOrEmpty(Xreso) && !string.IsNullOrEmpty(Yreso) && !string.IsNullOrEmpty(ResoUnit)) {
                    string str = Xreso + " x " + Yreso + " /" + ResoUnit;
                    exifTagOutLists[exifTagOutDic["G-Resolution"] - 1].EValue = str;
                }
            }
            // G-GPSLocation
            if (exifTagOutDic.ContainsKey("G-GPSLocation")) {
                exifTagOutLists[exifTagOutDic["G-GPSLocation"] - 1].EValue = GetGPSLocation();
            }
            //　G-GPSHeight
            if (exifTagOutDic.ContainsKey("G-GPSHeight")) {
                exifTagOutLists[exifTagOutDic["G-GPSHeight"] - 1].EValue = GetGPSHeight();
            }
        }

        private string GetExifLastValue(string TagID) {
            if (!ExifToolValueDic.ContainsKey(TagID))
                return "";
            string foundTagValue = ExifToolValueDic[TagID];
            if (exifToolListsDic.ContainsKey(TagID)) {
                var ToolDicLists = exifToolListsDic[TagID];
                if (ToolDicLists.ContainsKey(foundTagValue))
                    return ToolDicLists[foundTagValue][foundTagValue];    // 辞書の内容を返す
            }
            return foundTagValue;
        }

        // 合成関数用関数
        // GPS情報を取得して緯度経度を返す
        private string GetGPSLocation() {
            string lat = GetExifLastValue("0x0001");
            string lon = GetExifLastValue("0x0003");
            string latStr = GetExifLastValue("0x0002");
            string lonStr = GetExifLastValue("0x0004");
            if(!string.IsNullOrEmpty(lat) && !string.IsNullOrEmpty(lon) && !string.IsNullOrEmpty(latStr) && !string.IsNullOrEmpty(lonStr)) {
                if (GetGPSCoordinates(latStr, out string latD) && GetGPSCoordinates(lonStr, out string lonD)) {
                    if (lat.ToUpper().StartsWith("N") || lat.StartsWith("北")) { // 北緯または南緯
                        lat = "N ";
                    } else if (lat.ToUpper().StartsWith("S") || lat.StartsWith("南")) { // 北緯または南緯
                        lat = "S ";
                    } else {
                        return ""; // 値を返す
                    }
                    if (lon.ToUpper().StartsWith("E") || lon.StartsWith("東")) { // 東経または西経
                        lon = " , E ";
                    } else if (lon.ToUpper().StartsWith("W") || lon.StartsWith("西")) { // 東経または西経
                        lon = " , W ";
                    } else {
                        return ""; // 値を返す
                    }
                    double latitude = 0.0, longitude = 0.0; // 緯度と経度の初期化
                                                            // ここからは緯度のnn deg nn ' nn.nn " 形式を値への変換
                    int posdeg = latStr.IndexOf(" deg");
                    string sDeg = latStr.Substring(0, posdeg).Trim(); // 度の部分を取得
                    int posmin = latStr.IndexOf('\'');
                    string sMin = latStr.Substring(posdeg + 4, posmin - posdeg - 4).Trim(); // 分の部分を取得
                    string sSec = latStr.Substring(posmin + 1).Trim(); // 秒の部分を取得
                    sSec = sSec.Replace("\"", "").Trim(); // 秒の部分からダブルクォーテーションを削除
                    if (int.TryParse(sDeg, out int deg) && int.TryParse(sMin, out int min) && double.TryParse(sSec, out double secD)) {
                        if (deg < 0 || deg >= 90 || min < 0 || min >= 60 || secD < 0 || secD >= 60) {
                            return ""; // 度、分、秒が不正な場合は空文字を返す
                        }
                        latitude = deg + min / 60.0 + secD / 3600.0; // 緯度を計算
                    }

                    // ここからは経度の 　nnn deg nn ' nn.nn " 形式を値への変換
                    posdeg = lonStr.IndexOf(" deg");
                    sDeg = lonStr.Substring(0, posdeg).Trim(); // 度の部分を取得
                    posmin = lonStr.IndexOf('\'');
                    sMin = lonStr.Substring(posdeg + 4, posmin - posdeg - 4).Trim(); // 分の部分を取得
                    sSec = lonStr.Substring(posmin + 1).Trim(); // 秒の部分を取得
                    sSec = sSec.Replace("\"", "").Trim(); // 秒の部分からダブルクォーテーションを削除
                    if (int.TryParse(sDeg, out int deg2) && int.TryParse(sMin, out int min2) && double.TryParse(sSec, out double secD2)) {
                        if (deg2 < 0 || deg2 >= 180 || min2 < 0 || min2 >= 60 || secD2 < 0 || secD2 >= 60) {
                            return ""; // 度、分、秒が不正な場合は空文字を返す
                        }
                        longitude = deg2 + min2 / 60.0 + secD2 / 3600.0; // 経度を計算
                    }
                    return lat + latitude.ToString("#.########") + lon + longitude.ToString("#.########"); // 緯度と経度を返す
                }                
            }
            return ""; // GPS情報が不完全な場合は空文字を返す
        }

        // GPS高度を取得して返す
        private string GetGPSHeight() {
            string sAlt = ExifToolValueDic.TryGetValue("0x0005", out string value) ? value : ""; // GPS高度の値を取得
            if(sAlt == "") {
                return ""; // GPS高度の値が無ければ空文字を返す
            }
            switch (sAlt) {
                case "Above Sea Level": sAlt = "GPS高度+"; break;
                case "Below Sea Level": sAlt = "GPS高度-"; break;
                case "Positive Sea Level (sea-level ref)": sAlt = "海抜+"; break;
                case "Negative Sea Level (sea-level ref)": sAlt = "海抜-"; break;
            }

            if (ExifToolValueDic.ContainsKey("0x0006"))
                return sAlt + ExifToolValueDic["0x0006"];
            return "";
        }

        /// <summary>
        /// exiftool -EXIF:ALL --IFD1:ALL --InteropIFD:All -G0:1 -H -a -s ファイル名 コマンドで取得されたデータを素早くTagIDと値の辞書に変換するメソッド
        /// </summary>
        /// <remarks>入力文字列の各行はタグ ID とその値を含むことが期待されタグ ID は 17 文字目から 6 文字に渡り、
        /// 値はコロン (':') 文字の後に始まる。</remarks>
        /// <param name="strs">EXIFツールデータを含む入力文字列。/// 辞書は解析されたデータで更新されます</param>
        /// <returns><see langword="true"/> 作成時には常にTrueを返す</returns>
        bool GetExifToolLDic(string strs) {
            string[] lines = strs.Split(new[] { "\r\n", "\n" }, StringSplitOptions.RemoveEmptyEntries);
            string TagGrp, TagID, TagName, Value;
            foreach (string line in lines) {
                string str = GetTagGrp(line, out TagGrp);  // グループ名を取り出し、残りの文字列を取得
                // ExifToolの出力値を得る方法を重複値はデフォルトで許さないようにし、拡張子によっては許す場合を設けるように変更
                GetTagIDetc(str, out TagID, out TagName, out Value); // タグID、タグ名、値を取得
                if (TagID.StartsWith("0x")) {       // TagIDが0xで始まる場合のみ追加
                    if (ExifToolValueDic.ContainsKey(TagID)) {  //すでに取得済みと同じTagIDの場合
                        if (PriorityExifGroup.ContainsKey(cExt)) {
                            if (TagGrp == PriorityExifGroup[cExt]) {
                                ExifToolValueDic[TagID] = Value; // 辞書に追加(上書き可能）
                            }
                        }
                    } else {
                        ExifToolValueDic[TagID] = Value;
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// strからグループ名を取り出す
        /// </summary>
        /// <param name="str">解析する文字列.</param>
        /// <param name="grpName">グループ名を戻す変数</param>
        /// <returns>グループ名を取り除いたTagIDから始まる残りの文字列</returns>
        private static string GetTagGrp(string str, out string grpName) {
            int pos = str.IndexOf(']');
            if (pos > 2) {
                grpName = str.Substring(1, pos - 1).Trim(); // グループ名を取得
                str = str.Substring(pos + 1).Trim(); // グループ名を削除
            } else {
                grpName = string.Empty;
            }
            return str.Trim();
        }

        /// <summary>
        /// TagID,TagName,値
        /// </summary>
        /// <param name="str">ExifToolの出力から先頭のグループ名を取り除いたもの "0x<tagID> <tagName>:<value>".</param>
        /// <param name="tagID">出力用のTagID.</param>
        /// <param name="tagName">出力用のTagName</param>
        /// <param name="value">出力用の値</param>
        void GetTagIDetc(string str, out string tagID, out string tagName, out string value) {
            int pos = str.IndexOf(' ');
            if (pos < 0 || !str.StartsWith("0x")) {
                tagID = string.Empty;
                tagName = string.Empty;
                value = string.Empty;
                return; // タグIDが見つからない場合は空の値を返す
            }
            tagID = str.Substring(0, pos).Trim(); // タグIDを取得
            str = str.Substring(pos).Trim(); // タグIDを削除
            pos = str.IndexOf(':'); // タグ名の区切り文字はコロン
            if (pos < 0) {
                tagName = string.Empty;
                value = string.Empty;
                return; // タグ名が見つからない場合は空の値を返す
            }
            tagName = str.Substring(0, pos).Trim(); // タグ名を取得
            value = str.Substring(pos + 1).Trim(); // 値を取得
        }        

        /// <summary>
        /// コマンドラインで exiftoolを実行した結果と同じものを取り込む
        /// </summary>
        /// <param name="fPath">調べるファイルのパス</param>
        /// <returns>標準出力</returns>
        string GetRawExif(string fPath) {
            // 実行するExifToolのコマンドは、引数にパス名を取る、項目名と値がコロンで区切られただけの形式
            // Exifグループすべてとその中のIFD１グループ=サムネイルとInterrop を除外し、グループ名のレベル０と１を表示し
            // TagIDを16進数で表示し、重複を許し名前は短い形式（スペースを除去）で表示する
            ProcessStartInfo processStartInfo = new("exiftool", "-EXIF:ALL --IFD1:ALL --InteropIFD:All -G0:1 -H -a -s " + fPath);
            //ウィンドーを表示しない
            processStartInfo.CreateNoWindow = true;
            processStartInfo.UseShellExecute = false;
            //標準主力、標準エラー出力を取得できるようにする
            processStartInfo.RedirectStandardOutput = true;
            processStartInfo.RedirectStandardError = true;
            //プロセスとして実行
            Process process = Process.Start(processStartInfo);
            //標準出力、標準エラー出力、終了コードを取得
            string standardOutput = process.StandardOutput.ReadToEnd();
            string standardError = process.StandardError.ReadToEnd();
            int exitCode = process.ExitCode;
            //標準エラー出力、終了コードを利用する場合は。これ以降に記す
            //必ずプロセスを終了させる
            process.Close();
            return standardOutput;
        }

        // 以下は変換関数

        // 0x829a 分数表示の場合は小数に変換して返す
        string GetExpodureDecimal(string inStr) {
            string[] parts = inStr.Split('/');
            if (parts.Length != 2 || !int.TryParse(parts[0], out int numerator) || !int.TryParse(parts[1], out int denominator)) {
                return inStr; // 分数でない場合はそのまま返す
            }
            if (denominator == 0) {
                return "分母がゼロ"; // 分母がゼロの場合はエラーを返す
            }
            return ((double)numerator / (double)denominator).ToString(); // 分数を小数に変換して返す

            //int nume = 0, deno = 0;
            //string[] parts = inStr.Split('/');
            //if (parts.Length != 2 && !Int32.TryParse(parts[0], out nume) && !Int32.TryParse(parts[1], out deno)) {
            //    return inStr; // 分数でない場合はそのまま返す
            //}
            //return ((double)nume / (double)deno).ToString(); // 分数を小数に変換して返す
        }

        // 0x8827 撮影感度の値を取得する　値が66535以上の場合は 0x8831,0x8832,0x8833の値を取得する
        string GetSensitivity(string inStr) {
            // 撮影感度の値を取得する
            // 66535以上の場合は、0x8831,0x8832,0x8833の値を取得する
            //string str = "";
            if (int.TryParse(inStr, out int sensitivity)) {
                if (sensitivity >= 66535) {
                    if (ExifToolValueDic.ContainsKey("0x8831")) {
                        return "SOS: " + ExifToolValueDic["0x8831"];
                    } else if (ExifToolValueDic.ContainsKey("0x8832")) {
                        return "REI " + ExifToolValueDic["0x8832"];
                    } else if (ExifToolValueDic.ContainsKey("0x8833")) {
                        return "ISO " + ExifToolValueDic["0x8833"];
                    }
                } else {
                    return "ISO " + sensitivity.ToString(); // 通常のISO感度を返す}

                }
            }
            return inStr + " 異常値"; // 異常値の場合はそのまま返す
        }


        // 0x9201の場合でシャッター速度（1/nnn)をAPEX値（TV値）に変換する
        string GetShutterSpeedToApex(string inStr) {
            // 1/nnn の形式であれば、nnnを取得してAPEX値に変換、nnn ならそれが秒数
            double Sdecimal = 0.0;
            double Tv = 0.0;
            int nnn = 0;
            if (inStr.StartsWith("1/")) {
                if (int.TryParse(inStr.Substring(2), out nnn)) {
                    Sdecimal = 1.0 / nnn;
                } else {
                    return inStr + " 異常値";
                }
            } else {
                if (int.TryParse(inStr, out nnn)) {
                    Sdecimal = (double)nnn;
                }
            }
            Tv = -Math.Log(Sdecimal, 2);
            return Tv.ToString("#.##");
        }

        // 0x9202 F値をAPEX値に変換
        private string GetFNumberToApex(string inStr) {
            // F値をAPEX値に変換する
            // F値は通常、F1.4, F2.8, F4.0 などの形式で与えられる
            // APEX値は log2(F値) で計算される
            if (double.TryParse(inStr, out double fValue)) {
                return (2.0 * Math.Log(fValue, 2)).ToString("#.##");
            }
            return inStr + " 異常値"; // 異常値の場合はそのまま返す
        }

        // 0x902a 焦点距離の表示がExifToolでは小数点が含まれるので小数点以下が0なら表示しない形式に変更
        private string GetFocusNumber(string inStr) {
            if (!double.TryParse(inStr.Replace("mm", "").Trim(), out double focusNum)) {
                return inStr;
            }
            if (focusNum - Math.Truncate(focusNum) == 0) {
                return focusNum.ToString("#.") + " mm";
            } else {
                return focusNum.ToString() + " mm";
            }
        }

        // 0xa000 対応フラッシュピックスバージョンが数字ならばCIPA風文字に直す
        private string GetFlashPixVersion(string inStr) {
            // CIPA既定では"0100"を返すことになっているが、ExifToolでは "100"の場合が多いのでそれも含める
            if (inStr == "0100" || inStr == "0100") {
                return "Flashpix Format Version 1.0";
            }
            return inStr; // それ以外はそのまま返す
        }

        // 0x0002,0x0004,0x00014,0x0016 deg ' " を度単位の小数に変換する
        private bool GetGPSCoordinates(string inStr, out string outStr) {
            outStr = "";
            outStr = inStr.Replace("deg", "\t") //度分秒の表示の場合にも対応した
                        .Replace("度", "\t")
                        .Replace("\'", "\t")
                        .Replace("分", "\t")
                        .Replace("\"", "")
                        .Replace("秒", "");
            string[] parts = outStr.Split('\t');
            if (parts.Length < 3 || !double.TryParse(parts[0].Trim(), out double deg) || !double.TryParse(parts[1].Trim(), out double min) || !double.TryParse(parts[2].Trim(), out double sec)) {
                return false; // GPS情報が不完全
            }
            double coord = min / 60.0 + sec / 3600.0; // 緯度または経度を計算
            if (deg < 0) {          // 度が負の場合は全体が負とする
                coord = deg - coord;
            } else
                coord = deg + coord;
            outStr = coord.ToString("#.########"); // 緯度または経度を計算
            return true;
        }
    }
}
