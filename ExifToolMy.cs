using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PictureList {
    internal class ExifToolMy {

        public Dictionary<string, string> ExifToolLists = new();

        public ExifToolMy(string fullpath) {
            string stdout;
            ExifToolLists.Clear();
            stdout = GetRawExif(fullpath);
            string[] stdouts = stdout.Split("\r\n");
            foreach (string line in stdouts) {
                //Debug.Print(line);
                string[] lines = line.Split(":", 2);

                if (lines.Length > 1) {
                    string key = lines[0].Trim();
                    string value = lines[1].Trim();
                    if (ExifToolLists.TryGetValue(key, out string _value)) {
                        value = value + " & " + _value;
                        ExifToolLists[key] = value;
                    } else {
                        ExifToolLists.Add(key, value);
                    }
                }
            }
        }


        /// <summary>
        /// コマンドラインで exiftoolを実行した結果と同じものを取り込む
        /// </summary>
        /// <param name="fPath">調べるファイルのパス</param>
        /// <returns>標準出力</returns>
        static string GetRawExif(string fPath) {
            // 実行するExifToolのコマンドは、引数にパス名を取る、項目名と値がコロンで区切られただけの形式
            ProcessStartInfo processStartInfo = new("exiftool", fPath);
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
    }
}
