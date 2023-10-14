using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PictureList {
    public partial class Form1 : Form {
        public Form1() {
            InitializeComponent();
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
    }
}
