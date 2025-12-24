using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.Eventing.Reader;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.CompilerServices;
//using System.Runtime.Remoting.Messaging;
using System.Runtime.Serialization;
using System.Runtime.Versioning;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Documents;
using System.Windows.Forms.VisualStyles;
using System.Windows.Media.Imaging;
using static PictureList.Form1;

#pragma warning disable CA1416      //  #pragma warning restore CA1416 が出るまで CA1416 の警告が抑制される
//[assembly: SupportedOSPlatform("windows")]　
//CA1416はプラットフォームの互換性を警告するが、この文はこの位置が示すクラスや関数がWindowsのみをサポートすることを示し同じ結果を得る

#pragma warning disable IDE0066

namespace PictureList {
    // パスを受けて写真ならExif情報を得る
    //
    // 注意事項
    // このプロパティはExifの規定から作成していてプロパティとしてほとんどのExifタグを用意していたが特に外部から参照する必要が
    // ないものはコメントにしている。またExifタグの中には.NET FrameworkのSystem.Drawing.Imaging.PropertyItemで取得できない
    // ものもある。

    public class ExifAPI {

        private const int BYTE = 1, ASCII = 2, SHORT = 3, LONG = 4, RATIONAL = 5, UNDEFINED = 7, SLONG = 9, SRATIONAL = 10, UTF8 = 129;

        //ExifTagOutDicのこのクラスで使うための内部変数として用意した
        private List<Exiflist> _exifTagOutLists = new List<Exiflist>();
        private Dictionary<string, int> _exifTagOutDic = new Dictionary<string, int>();

        // 一部のExifタグを除いてそのプロパティは不要になるのでパフォーマンスを上げるために
        // 不要なプロパティはコメントにした方が良いかもしれない

        //以下のExif情報はNetFrameWorkで取得対象タグでないものは XXXX で示す
        //NetFrameWorkで取得対象タグでなくとも値が取得できるものは oooo で示す
        //Exif情報
        //
        //TIFF Rev6.0の付属情報からのもの
        //public string ImageWidth { get; private set; } //0x0100 画像の幅 JPRG圧縮データでは記録不可
        //public string ImageLength { get; private set; } //0x0101 画像の高さ JPRG圧縮データでは記録不可
        //public string BitsPerSample { get; private set; } //0x0102 画像のビットの深さ JPRG圧縮データでは記録不可
        //public string Compression { get; private set; } //0x0103 圧縮の種類 JPRG圧縮データでは記録不可
        //public string PhotometricInterpretation { get; private set; } //0x0106 画素構成
        //public string ImageDescription { get; private set; } //0x010E 画像タイトル
        //public string Make { get; private set; } //0x010F 画像入力機器のメーカー名
        //public string Model { get; private set; } //0x0110 画像入力機器のモデル名
        //public string StripOffsets { get; private set; } //0x0111 画像データのロケーション
        //public string Orientation { get; private set; } //0x0112 画像方向
        //public string SamplesPerPixel { get; private set; } //0x0115 コンポーネント数
        //public string RowsPerStrip { get; private set; } //0x0116 1ストリップあたりの行の数
        //public string StripByteCounts { get; private set; } //0x0117 ストリップの総バイト数
        public string XResolution { get; private set; } //0x011A 画像の幅の解像度
        public string YResolution { get; private set; } //0x011B 画像の高さの解像度
        public string PlanarConfiguration { get; private set; } //0x011C 画像データの並び
        public string ResolutionUnit { get; private set; } //0x0128 画像の幅と高さの解像度の単位
        //public string TransferFunction { get; private set; } //0x012D 再生階調カーブ特性
        //public string Software { get; private set; } //0x0131 ソフトウェア
        //public string DateTimeE { get; private set; } //0x0132 ファイル変更日時 Exifの規定では DateTimeだがC#のDateTimeと区別するためDateTimeEとした
        //public string Artist { get; private set; } //0x013B アーティスト
        //public string WhitePoint { get; private set; } //0x013E 参照白色点の色度座標値
        //public string PrimaryChromaticities { get; private set; } //0x013F 原色の色度座標値
        //public string JPEGInterchangeFormat { get; private set; } //0x0201 JPEGのSOIへのオフセット
        //public string JPEGInterchangeFormatLength { get; private set; } //0x0202 JPEGデータのバイト数(サムネイル）
        //public string YCbCrCoefficients { get; private set; } //0x0211 色変換マトリクス係数
        //public string YCbCrSubSampling { get; private set; } //0x0212 YCCの画素構成(Cの間引き率)
        //public string YCbCrPositioning { get; private set; } //0x0213 YCCの画素構成(YとCの位置)
        //public string ReferenceBlackWhite { get; private set; } //0x0214 参照黒色点値と参照白色点値
        //public string Copyright { get; private set; } //0x8298 撮影著作権者/編集著作権者        
        //public string Exif_IFD_Pointer { get; private set; } //0x8769 Exifタグ
        //public string GPSInfoIFDPointer { get; private set; } //0x8825 GPSタグ
        //
        //Exif固有のIFD
        //
        public string ExposureTime { get; private set; } //0x829A 露出時間
        //public string FNumber { get; private set; } //0x829D Fナンバー
        //public string ExposureProgram { get; private set; } //0x8822 露出プログラム
        //public string SpectralSensitivity { get; private set; } //0x8824 スペクトル感度
        //public string PhotographicSensitivity { get; private set; } //0x8827 撮影感度
        //public string OECF { get; private set; } //0x8828 光電変換関数
        //public string SensitivityType { get; private set; } //0x8830 感度種別 XXXX
        //public string StandardOutputSensitivity { get; private set; } //0x8831 標準出力感度 XXXX
        //public string RecommendedExposureIndex { get; private set; } //0x8832 推奨露光指数 XXXX
        //public string ISOSpeed { get; private set; } //0x8833 ISOスピード XXXX
        //public string ISOSpeedLatitudeyyy { get; private set; } //0x8834 ISOスピードラチチュードyyy XXXX
        //public string ISOSpeedLatitudezzz { get; private set; } //0x8835 ISOスピードラチチュードzzz XXXX
        //public string ExifVersion { get; private set; } //0x9000 Exifバージョン
        //public string DateTimeOriginal { get; private set; } //0x9003 原画像データの生成日時
        //public string DateTimeDigitized { get; private set; } //0x9004 デジタルデータの作成日時
        //public string OffsetTime { get; private set; } //0X9010 DateTimeの時差データ Exif2.31で追加 oooo
        //public string OffsetTimeOriginal { get; private set; } //0X9011 DateTimeOrigialの時差データ Exif2.31で追加 oooo
        //public string OffsetTimeDigitized { get; private set; } //0X9012 DateTimeDigitizedの時差データ Exif2.31で追加 oooo
        //public string ComponentsConfiguration { get; private set; } //0x9101 各コンポーネントの意味
        //public string CompressedBitsPerPixel { get; private set; } //0x9102 画像圧縮モード
        //public string ShutterSpeedValue { get; private set; } //0x9201 APEX(EV)値のシャッタースピード
        //public string ApertureValue { get; private set; } //0x9202 絞り値
        //public string BrightnessValue { get; private set; } //0x9203 輝度値
        //public string ExposureBiasValue { get; private set; } //0x9204 露光補正値
        //public string MaxApertureValue { get; private set; } //0x9205 レンズ最小Ｆ値
        //public string SubjectDistance { get; private set; } //0x9206 被写体距離
        //public string MeteringMode { get; private set; } //0x9207 測光方式
        //public string LightSource { get; private set; } //0x9208 光源
        //public string Flash { get; private set; } //0x9209 フラッシュ
        //public string FocalLength { get; private set; } //0x920A レンズ焦点距離
        //public string SubjectArea { get; private set; } //0x9214 被写体領域 XXXX
        //public string MakerNote { get; private set; } //0x927C メーカノート
        //public string UserComment { get; private set; } //0x9286 ユーザコメント
        //public string SubSecTime { get; private set; } //0x9290 DateTimeのサブセック
        //public string SubSecTimeOriginal { get; private set; } //0x9291 DateTimeOriginalのサブセック
        //public string SubSecTimeDigitized { get; private set; } //0x9292 DateTimeDigitizedのサブセック
        //public string Temperature { get; private set; } //0x9400 温度 XXXX
        //public string Humidity { get; private set; } //0x9401 湿度 XXXX
        //public string Pressure { get; private set; } //0x9402 圧力 XXXX
        //public string WaterDepth { get; private set; } //0x9403 水深 XXXX
        //public string Acceleration { get; private set; } //0x9404 加速度 XXXX
        //public string CameraElevationAngle { get; private set; } //0x9405 カメラの迎角 XXXX
        //public string FlashpixVersion { get; private set; } //0xA000 対応フラッシュピックスバージョン
        //public string ColorSpace { get; private set; } //0xA001 色空間情報
        public string PixelXDimension { get; private set; } //0xA002 実効画像幅
        public string PixelYDimension { get; private set; } //0xA003 実効画像高さ
        //public string RelatedSoundFile { get; private set; } //0xA004 関連音声ファイル
        //public string InteroperabilityIFDPointer { get; private set; } //0xA005 互換性IFDへのポインタ このプログラムでは不要？
        //public string FlashEnergy { get; private set; } //0xA20B フラッシュ強度
        //public string SpatialFrequencyResponse { get; private set; } //0xA20C 空間周波数応答
        //public string FocalPlaneXResolution { get; private set; } //0xA20E 焦点面の幅の解像度
        //public string FocalPlaneYResolution { get; private set; } //0xA20F 焦点面の高さの解像度
        //public string FocalPlaneResolutionUnit { get; private set; } //0xA210 焦点面解像度単位
        //public string SubjectLocation { get; private set; } //0xA214 被写体位置
        //public string ExposureIndex { get; private set; } //0xA215 露出インデックス
        //public string SensingMethod { get; private set; } //0xA217 センサ方式
        //public string FileSource { get; private set; } //0xA300 ファイルソース
        //public string SceneType { get; private set; } //0xA301 シーンタイプ
        //public string CFAPattern { get; private set; } //0xA302 CFAパターン
        //public string CustomRendered { get; private set; } //0xA401 個別画像処理 oooo
        //public string ExposureMode { get; private set; } //0xA402 露出モード oooo
        //public string WhiteBalance { get; private set; } //0xA403 ホワイトバランス oooo
        //public string DigitalZoomRatio { get; private set; } //0xA404 デジタルズーム倍率 oooo
        //public string FocalLengthIn35mmFilm { get; private set; } //0xA405 35mm換算レンズ焦点距離 XXXX
        //public string SceneCaptureType { get; private set; } //0xA406 撮影シーンタイプ oooo
        //public string GainControl { get; private set; } //0xA407 ゲイン制御 XXXX
        //public string Contrast { get; private set; } //0xA408 撮影コントラスト XXXX
        //public string Saturation { get; private set; } //0xA409 撮影彩度 XXXX
        //public string Sharpness { get; private set; } //0xA40A 撮影シャープネス XXXX
        //public string DeviceSettingDescription { get; private set; } //0xA40B 撮影条件記述情報 XXXX
        //public string SubjectDistanceRange { get; private set; } //0xA40C 被写体距離レンジ oooo
        //public string ImageUniqueID { get; private set; } //0xA420 画像ユニークID XXXX
        //public string CameraOwnerName { get; private set; } //0xA430 カメラ所有者名 XXXX
        //public string BodySerialNumber { get; private set; } //0xA431 カメラシリアル番号 XXXX
        //public string LensSpecification { get; private set; } //0xA432 レンズの仕様情報 XXXX
        //public string LensMake { get; private set; } //0xA433 レンズのメーカ名 XXXX
        //public string LensModel { get; private set; } //0xA434 レンズのモデル名 XXXX
        //public string LensSerialNumber { get; private set; } //0xA435 レンズシリアル番号 XXXX
        //public string ImageTitle { get; private set; } //0xA436 タイトル名 XXXX
        //public string Photographer { get; private set; } //0xA437 フォトグラファー XXXX
        //public string ImageEditor { get; private set; } //0xA438 画像編集者
        //public string CameraFirmware { get; private set; } //0xA439 カメラファームウェア XXXX
        //public string RAWDevelopingSoftware { get; private set; } //0xA43A RAW現像ソフトウェア XXXX
        //public string ImageEditingSoftware { get; private set; } //0xA43B 画像編集ソフトウェア XXXX
        //public string MetadataEditingSoftware { get; private set; } //0xA43C メタデータ編集ソフトウェア XXXX
        //public string CompositeImage { get; private set; } //0xA460 合成画像 XXXX
        //public string SourceImageNumberOfCompositeImage { get; private set; } //0xA461 合成画像のソース画像数 XXXX
        //public string SourceExposureTimesOfCompositeImage { get; private set; } //0xA462 合成画像のソース画像露光時間 XXXX
        //public string Gamma { get; private set; } //0xA500 再生ガンマ XXXX
        //
        //GPS
        //
        public string GPSVersionID { get; private set; } //0x0000 GPSタグのバージョン
        public string GPSLatitudeRef { get; private set; } //0x0001 北緯(N) or 南緯(S)
        public string GPSLatitude { get; private set; } //0x0002 緯度(数値)
        public string GPSLongitudeRef { get; private set; } //0x0003 東経(E) or 西経(W)
        public string GPSLongitude { get; private set; } //0x0004 経度(数値)
        public string GPSAltitudeRef { get; private set; } //0x0005 高度の基準
        public string GPSAltitude { get; private set; } //0x0006 高度(数値)
            //public string GPSTimeStamp { get; private set; } //0x0007 GPS時間(原子時計の時間)
            //public string GPSSatellites { get; private set; } //0x0008 測位に使った衛星信号
            //public string GPSStatus { get; private set; } //0x0009 GPS受信機の状態
            //public string GPSMeasureMode { get; private set; } //0x000A GPSの測位方法
            //public string GPSDOP { get; private set; } //0x000B 測位の信頼性
            //public string GPSSpeedRef { get; private set; } //0x000C 速度の単位
            //public string GPSSpeed { get; private set; } //0x000D 速度(数値)
            //public string GPSTrackRef { get; private set; } //0x000E 進行方向の単位
            //public string GPSTrack { get; private set; } //0x000F 進行方向(数値)
            //public string GPSImgDirectionRef { get; private set; } //0x0010 撮影した画像の方向の単位
            //public string GPSImgDirection { get; private set; } //0x0011 撮影した画像の方向(数値)
            //public string GPSMapDatum { get; private set; } //0x0012 測位に用いた地図データ
            //public string GPSDestLatitudeRef { get; private set; } //0x0013 目的地の北緯(N) or 南緯(S)
            //public string GPSDestLatitude { get; private set; } //0x0014 目的地の緯度(数値)
            //public string GPSDestLongitudeRef { get; private set; } //0x0015 目的地の東経(E) or 西経(W)
            //public string GPSDestLongitude { get; private set; } //0x0016 目的地の経度(数値)
            //public string GPSDestBearingRef { get; private set; } //0x0017 目的地の方角の単位
            //public string GPSDestBearing { get; private set; } //0x0018 目的の方角(数値)
            //public string GPSDestDistanceRef { get; private set; } //0x0019 目的地までの距離の単位
            //public string GPSDestDistance { get; private set; } //0x001A 目的地までの距離(数値)
            //public string GPSProcessingMethod { get; private set; } //0x001B 測位方式の名称 oooo
            //public string GPSAreaInformation { get; private set; } //0x001C 測位地点の名称 XXXX
            //public string GPSDateStamp { get; private set; } //0x001D GPS日付 oooo
            //public string GPSDifferential { get; private set; } //0x001E GPS補正測位 XXXX
            //public string GPSHPositioningError { get; private set; } //0x001F 水平方向測位誤差 XXXX

        /// <summary>
        /// コンストラクタ
        /// </summary>
        /// <param name="fullPath">絶対パス</param>
        /// <param name="Dictionary<string,int> OrgExiftTagOutDic">OrgExiftTagOutDic</param>
        public ExifAPI(string fullPath, List<Form1.Exiflist> ExifTagOutLists, Dictionary<string, int> ExifTagOutDic) {
            this._exifTagOutLists = ExifTagOutLists;
            this._exifTagOutDic = ExifTagOutDic;
            this._exifPciture(fullPath);
        }

        private void _exifPciture(string fullPath) {

            // デバッグ用
            // System.Diagnostics.Debug.Print(fullPath);
            //


            //            System.Drawing.Bitmap bmp = null;

            //            try {
            //                bmp = new System.Drawing.Bitmap(fullPath);
            //                //bmp = new System.Drawing.Bitmap(fullPath);
            //                //bmp= Image.FromFile(fullPath);
            //            } catch (Exception exception) {
            //                string error = fullPath + " : " + exception.Message;
            //                // Console.WriteLine(error);
            //#if DEBUG
            //                System.Diagnostics.Debug.Print(error);
            //#endif
            //                //System.Windows.Forms.MessageBox.Show(error);
            //                //DateTimeOriginal = ""; //以前はなぜか error;
            //            }

            Bitmap bmp = MakeBMP(fullPath);


            if (bmp != null) {


                //ここからがPropertyItemsデータの取得
                foreach (System.Drawing.Imaging.PropertyItem item in bmp.PropertyItems) {
                    string DicKey = "0x" + item.Id.ToString("x4");
                    // Debug.Print("{0} , {1}", item.Id, DicKey);
                    if (_exifTagOutDic.ContainsKey(DicKey)) {
                        string _eVal = "";
                        switch (item.Id) {
                            // TIFF IFD
                            case 0x0100: _eVal = AnyTypeAndCount(item); /*ImageWidth = _eVal;*/ break;
                            case 0x0101: _eVal = AnyTypeAndCount(item); /*ImageLength = _eVal;*/ break;
                            case 0x0102: _eVal = AnyTypeAndCount(item, " "); /*BitsPerSample = _eVal;*/ break;
                            case 0x0103: _eVal = GetCommpression(item); /* Compression = _eVal; */ break;
                            case 0x0106: _eVal = GetPhotometricInterpretation(item); /* PhotometricInterpretation = _eVal; */ break;
                            //case 0x010E: _eVal = GetExifAsci(item).Replace(Environment.NewLine, "⏎"); /* ImageDescription = _eVal; */ break;
                            case 0x010E: _eVal = GetExifAsci(item).Replace("\n", "."); /* ImageDescription = _eVal; */ break;
                            case 0x010F: _eVal = GetExifAsci(item); /* Make = _eVal; */ break;
                            case 0x0110: _eVal = GetExifAsci(item); /* Model = _eVal; */ break;
                            case 0x0111: _eVal = AnyTypeAndCount(item, ":"); /* StripOffsets = _eVal; */ break;
                            case 0x0112: _eVal = GetOrientation(item); /* Orientation = _eVal; */ break;
                            case 0x0115: _eVal = AnyTypeAndCount(item); /* SamplesPerPixel = _eVal; */ break;
                            case 0x0116: _eVal = AnyTypeAndCount(item); /* RowsPerStrip = _eVal; */ break;
                            case 0x0117: _eVal = AnyTypeAndCount(item, ":"); /* StripByteCounts = _eVal; */ break;
                            case 0x011A: _eVal = AnyTypeAndCount(item); XResolution = _eVal; break;
                            case 0x011B: _eVal = AnyTypeAndCount(item); YResolution = _eVal; break;
                            case 0x011C: _eVal = GetPlanarConfiguration(item); /* PlanarConfiguration = _eVal; */ break;
                            case 0x0128: _eVal = GetResolutionUnit(item); ResolutionUnit = _eVal; break;
                            case 0x012D: _eVal = "未実装"; /* TransferFunction = _eVal; */ break;
                            case 0x0131: _eVal = GetExifAsci(item); /* Software = _eVal; */ break;
                            case 0x0132: _eVal = GetExifAsci(item); /* DateTimeE = _eVal; */ break;
                            case 0x013B: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* Artist = _eVal; */ break;
                            case 0x013E: _eVal = GetMultiRationalToString(item, " "); /* WhitePoint = _eVal; */ break;
                            case 0x013F: _eVal = GetMultiRationalToString(item, " "); /* PrimaryChromaticities = _eVal; */ break;
                            case 0x0201: _eVal = AnyTypeAndCount(item) + "(CIPA値)"; /* JPEGInterchangeFormat = _eVal; */ break;
                            case 0x0202: _eVal = AnyTypeAndCount(item); /* JPEGInterchangeFormatLength = _eVal; */ break;
                            case 0x0211: _eVal = GetMultiRationalToString(item, " "); /* YCbCrCoefficients = _eVal; */ break;
                            case 0x0212: _eVal = GetYCbCrSubSampling(item); /* YCbCrSubSampling = _eVal; */ break;
                            case 0x0213: _eVal = GetYCbCrPositioning(item); /* YCbCrPositioning = _eVal; */ break;
                            case 0x0214: _eVal = GetMultiRationalToString(item, " "); /* ReferenceBlackWhite = _eVal; */ break;
                            case 0x8298: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* Copyright = _eVal; */ break;
                            case 0x829A: _eVal = AnyTypeAndCount(item); ExposureTime = GetShutterSpeed(_eVal); break;
                            case 0x829D: _eVal = "F " + AnyTypeAndCount(item); /* FNumber = _eVal; */ break;
                            case 0x8769: _eVal = AnyTypeAndCount(item); /* Exif_IFD_Pointer = _eVal; */ break;
                            case 0x8822: _eVal = GetExposureProgram(item); /* ExposureProgram = _eVal; */ break;
                            case 0x8824: _eVal = GetExifAsci(item); /* SpectralSensitivity = _eVal; */ break;
                            case 0x8825: _eVal = AnyTypeAndCount(item); /* GPSInfoIFDPointer = _eVal; */ break;
                            case 0x8827: _eVal = GetSensitivity(item); /* PhotographicSensitivity = _eVal; */  break;
                            case 0x8828: _eVal = GetOECF(item); /* OECF = _eVal; */ break;
                            case 0x8830: _eVal = GetSensitivityType(item); /* SensitivityType = _eVal; */ break; // XXXX 
                            case 0x8831: _eVal = AnyTypeAndCount(item); /* StandardOutputSensitivity = _eVal; */ break; // XXXX 
                            case 0x8832: _eVal = AnyTypeAndCount(item); /* RecommendedExposureIndex = _eVal; */ break; // XXXX 
                            case 0x8833: _eVal = AnyTypeAndCount(item); /* ISOSpeed = _eVal; */ break; // XXXX 
                            case 0x8834: _eVal = AnyTypeAndCount(item); /* ISOSpeedLatitudeyyy = _eVal; */ break; // XXXX 
                            case 0x8835: _eVal = AnyTypeAndCount(item); /* ISOSpeedLatitudezzz = _eVal; */ break; // XXXX 
                            case 0x9000: _eVal = GetUndefinedMultiByteToString(item, ""); /* ExifVersion=_eVal: */ break;
                            case 0x9003: _eVal = GetExifAsci(item); /* DateTimeOriginal = _eVal; */ break;
                            case 0x9004: _eVal = GetExifAsci(item); /* DateTimeDigitized = _eVal; */ break;
                            case 0X9010: _eVal = GetExifAsci(item); /* OffsetTime = _eVal; */ break; // oooo 
                            case 0X9011: _eVal = GetExifAsci(item); /* OffsetTimeOriginal = _eVal; */ break; // oooo 
                            case 0X9012: _eVal = GetExifAsci(item); /* OffsetTimeDigitized = _eVal; */ break; // oooo 
                            case 0x9101: _eVal = GetComponentsConfiguration(item); /* ComponentsConfiguration = _eVal; */ break;
                            case 0x9102: _eVal = AnyTypeAndCount(item); /* CompressedBitsPerPixel = _eVal; */ break;
                            case 0x9201: _eVal = AnyTypeAndCount(item); /* ShutterSpeedValue = _eVal; */ break;
                            case 0x9202: _eVal = GetExifRational(item).ToString("#.##"); /* ApertureValue = _eVal; */ break;
                            case 0x9203: _eVal = AnyTypeAndCount(item); /* BrightnessValue = _eVal; */ break;
                            case 0x9204: _eVal = AnyTypeAndCount(item); /* ExposureBiasValue = _eVal; */ break;
                            case 0x9205: _eVal = GetAperture(item); /* MaxApertureValue = _eVal; */ break;
                            case 0x9206: _eVal = AnyTypeAndCount(item) + " m"; /* SubjectDistance = _eVal; */ break;
                            case 0x9207: _eVal = GetMeteringMode(item); /* MeteringMode = _eVal; */ break;
                            case 0x9208: _eVal = GetLightSource(item); /* LightSource = _eVal; */ break;
                            case 0x9209: _eVal = GetFlash(item); /* Flash = _eVal; */ break;
                            case 0x920A: _eVal = AnyTypeAndCount(item) + " mm"; /* FocalLength = _eVal; */ break;
                            case 0x9214: _eVal = GetSubjectArea(item); /* SubjectArea = _eVal; */ break; // XXXX 
                            case 0x927C: _eVal = GetMakerNote(item).Replace("\n", "⏎"); /* MakerNote = _eVal; */ break;
                            case 0x9286: _eVal = "未実装"; /* UserComment = _eVal; */ break;
                            case 0x9290: _eVal = GetExifAsci(item); /* SubSecTime = _eVal; */ break;
                            case 0x9291: _eVal = GetExifAsci(item); /* SubSecTimeOriginal = _eVal; */ break;
                            case 0x9292: _eVal = GetExifAsci(item); /* SubSecTimeDigitized = _eVal; */ break;
                            case 0x9400: _eVal = AnyTypeAndCount(item) + " ℃"; /* Temperature = _eVal; */ break;
                            case 0x9401: _eVal = AnyTypeAndCount(item) + "%"; /* Humidity = _eVal; */ break;
                            case 0x9402: _eVal = AnyTypeAndCount(item) + " hPa"; /* Pressure = _eVal; */ break;
                            case 0x9403: _eVal = AnyTypeAndCount(item) + "m"; /* WaterDepth = _eVal; */ break;
                            case 0x9404: _eVal = AnyTypeAndCount(item) + "mGal"; /* Acceleration = _eVal; */ break;
                            case 0x9405: _eVal = AnyTypeAndCount(item) + " °"; /* CameraElevationAngle = _eVal; */ break;
                            case 0xA000: _eVal = (string)GetFlashpixVersion(item); /* FlashpixVersion = _eVal; */ break;
                            case 0xA001: _eVal = GetColorSpace(item); /* ColorSpace = _eVal; */ break;
                            case 0xA002: _eVal = AnyTypeAndCount(item); PixelXDimension = _eVal; break;
                            case 0xA003: _eVal = AnyTypeAndCount(item); PixelYDimension = _eVal; break;
                            case 0xA004: _eVal = GetExifAsci(item); /* RelatedSoundFile = _eVal; */ break;
                            case 0xA005: _eVal = AnyTypeAndCount(item); /* InteroperabilityIFDPointer = _eVal; */ break;
                            case 0xA20B: _eVal = AnyTypeAndCount(item); /* FlashEnergy = _eVal; */ break;
                            case 0xA20C: _eVal = "未実装"; /* SpatialFrequencyResponse = _eVal; */ break;
                            case 0xA20E: _eVal = AnyTypeAndCount(item); /* FocalPlaneXResolution = _eVal; */ break;
                            case 0xA20F: _eVal = AnyTypeAndCount(item); /* FocalPlaneYResolution = _eVal; */ break;
                            case 0xA210: _eVal = GetResolutionUnit(item); /* FocalPlaneResolutionUnit = _eVal; */ break;
                            case 0xA214: _eVal = GetSubjectLocation(item); /* SubjectLocation = _eVal; */ break;
                            case 0xA215: _eVal = GetExifRational(item).ToString(); /* ExposureIndex = _eVal; */ break;
                            case 0xA217: _eVal = GetSensingMethod(item); /* SensingMethod = _eVal; */ break;
                            case 0xA300: _eVal = GetFileSource(item); /* FileSource = _eVal; */ break;
                            case 0xA301: _eVal = GetSceneType(item); /* SceneType = _eVal; */ break;
                            case 0xA302: _eVal = GetCFAPattern(item).Replace("\n", "⏎"); /* CFAPattern = _eVal ; */ break;
                            case 0xA401: _eVal = GetCustomRendered(item); /* CustomRendered = _eVal; */ break; // oooo
                            case 0xA402: _eVal = GetExposureMode(item); /* ExposureMode = _eVal; */ break; // oooo
                            case 0xA403: _eVal = GetWhiteBalance(item); /* WhiteBalance = _eVal; */ break; // oooo
                            case 0xA404: _eVal = GetDigitalZoomRatio(item); /* DigitalZoomRatio = _eVal; */ break; // oooo
                            case 0xA405: _eVal = GetFocalLengthIn35mmFilm(item) + " mm"; /* FocalLengthIn35mmFilm = _eVal; */ break; // oooo
                            case 0xA406: _eVal = GetSceneCaptureType(item); /* SceneCaptureType = _eVal; */ break; // oooo
                            case 0xA407: _eVal = GetGainControl(item); /* GainControl = _eVal; */ break; // XXXX
                            case 0xA408: _eVal = GetContrast(item); /* Contrast = _eVal; */ break; // XXXX
                            case 0xA409: _eVal = GetSaturation(item); /* Saturation = _eVal; */ break; // XXXX
                            case 0xA40A: _eVal = GetSharpness(item); /* Sharpness = _eVal; */ break; // XXXX
                            case 0xA40B: _eVal = "未実装"; /* DeviceSettingDescription = _eVal; */ break; // XXXX
                            case 0xA40C: _eVal = GetSubjectDistanceRange(item); /* SubjectDistanceRange = _eVal; */ break; // oooo
                            case 0xA420: _eVal = GetExifAsci(item); /* ImageUniqueID = _eVal; */ break; // XXXX
                            case 0xA430: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* CameraOwnerName = _eVal; */ break; // XXXX
                            case 0xA431: _eVal = "S/N " + GetExifAsci(item).Replace("\n", "⏎"); /* BodySerialNumber = _eVal; */ break; // XXXX
                            case 0xA432: _eVal = GetLensSpecificatGet(item); /* LensSpecification = _eVal; */ break; // XXXX
                            case 0xA433: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* LensMake = _eVal; */ break; // XXXX
                            case 0xA434: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* LensModel = _eVal; */ break; // XXXX
                            case 0xA435: _eVal = "S/N " + GetExifAsci(item).Replace("\n", "⏎"); /* LensSerialNumber = _eVal; */ break; // XXXX
                            case 0xA436: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* ImageTitle = _eVal; */ break; // XXXX
                            case 0xA437: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* Photographer = _eVal; */ break; // XXXX
                            case 0xA438: _eVal = GetExifAsci(item).Replace("\n", "⏎"); /* ImageEditor = _eVal; */ break; // XXXX
                            case 0xA439: _eVal = GetExifAsci(item); /* CameraFirmware = _eVal; */ break; // XXXX
                            case 0xA43A: _eVal = GetExifAsci(item); /* RAWDevelopingSoftware = _eVal; */ break; // XXXX
                            case 0xA43B: _eVal = GetExifAsci(item); /* ImageEditingSoftware = _eVal; */ break; // XXXX
                            case 0xA43C: _eVal = GetExifAsci(item); /* MetadataEditingSoftware = _eVal; */ break; // XXXX
                            case 0xA460: _eVal = GetCompositeImage(item); /* CompositeImage = _eVal; */ break; // XXXX
                            case 0xA461: _eVal = AnyTypeAndCount(item, ","); /* SourceImageNumberOfCompositeImage = _eVal; */ break; // XXXX
                            case 0xA462: _eVal = "未実装"; /* SourceExposureTimesOfCompositeImage = _eVal; */ break; // XXXX
                            case 0xA500: _eVal = AnyTypeAndCount(item); /* Gamma = _eVal; */ break; // XXXX
                            // GPS
                            case 0x0000: _eVal = GetGPSVersionID(item); GPSVersionID = _eVal; break;
                            case 0x0001: _eVal = GetGPSLatitudeRef(GetExifAsci(item)); GPSLatitudeRef = _eVal; break;
                            case 0x0002: _eVal = $"{GetGPSValue(item):#.########}"; GPSLatitude = _eVal; break;
                            case 0x0003: _eVal = GetGPSLongitudeRef(GetExifAsci(item)); GPSLongitudeRef = _eVal; break;
                            case 0x0004: _eVal = $"{GetGPSValue(item):#.########}"; GPSLongitude = _eVal; break;
                            case 0x0005: _eVal = GetGPSAltitudeRef(item); GPSAltitudeRef = _eVal; break;
                            case 0x0006: _eVal = AnyTypeAndCount(item); _eVal += " m"; GPSAltitude = _eVal; break;
                            case 0x0007: _eVal = GetGPSTimeStamp(item); /* GPSTimeStamp = _eVal; */ break;
                            case 0x0008: _eVal = GetExifAsci(item); /* GPSSatellites = _eVal; */ break;
                            case 0x0009: _eVal = GetGPSStatus(GetExifAsci(item)); /* GPSStatus = _eVal; */ break;
                            case 0x000A: _eVal = GetGPSMeasureMode(GetExifAsci(item)); /* GPSMeasureMode = _eVal; */ break;
                            case 0x000B: _eVal = AnyTypeAndCount(item); /* GPSDOP = _eVal; */ break;
                            case 0x000C: _eVal = GetGPSSpeedRef(GetExifAsci(item)); /* GPSSpeedRef = _eVal; */ break;
                            case 0x000D: _eVal = AnyTypeAndCount(item); /* GPSSpeed = _eVal; */ break;
                            case 0x000E: _eVal = GetGPSTrackRef(GetExifAsci(item)); /* GPSTrackRef = _eVal; */ break;
                            case 0x000F: _eVal = AnyTypeAndCount(item); /* GPSTrack = _eVal; */ break;
                            case 0x0010: _eVal = GetGPSTrackRef(GetExifAsci(item)); /* GPSImgDirectionRef = _eVal; */ break;
                            case 0x0011: _eVal = GetExifRational(item).ToString(); /* GPSImgDirection = _eVal; */ break;
                            case 0x0012: _eVal = GetExifAsci(item); /* GPSMapDatum = _eVal; */ break;
                            // 0x0001 GPSLatitudeRef と同じ
                            case 0x0013: _eVal = GetGPSLatitudeRef(GetExifAsci(item)); /* GPSDestLatitudeRef = _eVal; */ break;
                            case 0x0014: _eVal = $"{GetGPSValue(item):#.########}"; /* GPSDestLatitude = _eVal; */ break;
                            // 0x0003 GPSLongitudeRef と同じ
                            case 0x0015: _eVal = GetGPSLongitudeRef(GetExifAsci(item)); /* GPSDestLongitudeRef = _eVal; */ break;
                            case 0x0016: _eVal = $"{GetGPSValue(item):#.########}"; /* GPSDestLongitude = _eVal; */ break;
                            case 0x0017: _eVal = GetGPSTrackRef(GetExifAsci(item)); /* GPSDestBearingRef = _eVal; */ break;
                            case 0x0018: _eVal = AnyTypeAndCount(item); /* GPSDestBearing = _eVal; */ break;
                            case 0x0019: _eVal = GetGPSDestDistanceRef(GetExifAsci(item)); /* GPSDestDistanceRef = _eVal; */ break;
                            case 0x001A: _eVal = AnyTypeAndCount(item); /* GPSDestDistance = _eVal; */ break;
                            case 0x001B: _eVal = GetUndefinedUniStringValue(item); /* GPSProcessingMethod = _eVal; */ break; // oooo
                            case 0x001C: _eVal = GetUndefinedUniStringValue(item); /* GPSAreaInformation = _eVal; */ break; // oooo
                            case 0x001D: _eVal = GetExifAsci(item).Replace(":", "/"); /* GPSDateStamp = _eVal; */ break; // XXXX
                            case 0x001E: _eVal = GetGPSDifferential(item); /* GPSDifferential = _eVal; */ break; // XXXX
                            case 0x001F: _eVal = AnyTypeAndCount(item); /* GPSHPositioningError = _eVal; */ break; // XXXX
                        }
                        if (_exifTagOutDic[DicKey] > 0) {       //合成関数だけで使用し出力しない場合は -1 なので                     
                            _exifTagOutLists[_exifTagOutDic[DicKey] - 1].EValue = _eVal;

                        }
                        if (_eVal != "")
                            _exifTagOutDic.Remove(DicKey); //出力済みは削除
                    }
                }
                //ここから合成関数用の値を求める _exifTagOutListsの中に合成関数名があれば処理する
                string gValue;
                if (_exifTagOutDic.ContainsKey("G-WidthHeight")) {
                    //BitMapではWidthとHeightが有効でExif値よりもこれを優先する
                    gValue = bmp.Width.ToString() + " x " + bmp.Height.ToString();
                    _exifTagOutLists[_exifTagOutDic["G-WidthHeight"] - 1].EValue = gValue;
                    _exifTagOutDic.Remove("G-WidthHeight"); //出力済みは削除
                                    }
                if (_exifTagOutDic.ContainsKey("G-ShutterSpeed")) {
                    _exifTagOutLists[_exifTagOutDic["G-ShutterSpeed"] - 1].EValue = ExposureTime;
                    _exifTagOutDic.Remove("G-ShutterSpeed"); //出力済みは削除
                }

                if (_exifTagOutDic.ContainsKey("G-Resolution")) {
                    gValue = GetResolution();
                    if (gValue != "") {
                        _exifTagOutLists[_exifTagOutDic["G-Resolution"] - 1].EValue = gValue;
                        _exifTagOutDic.Remove("G-Resolution"); //出力済みは削除
                    }
                }
                if (_exifTagOutDic.ContainsKey("G-GPSLocation")) {
                    gValue = GetGPSLocation();
                    if (gValue != "") {
                        _exifTagOutLists[_exifTagOutDic["G-GPSLocation"] - 1].EValue = gValue;
                        _exifTagOutDic.Remove("G-GPSLocation"); //出力済みは削除
                    }
                }
                if (_exifTagOutDic.ContainsKey("G-GPSHeight")) {
                    gValue = GetGPSHeight();
                    if (gValue != "") {
                        _exifTagOutLists[_exifTagOutDic["G-GPSHeight"] - 1].EValue = gValue;
                        _exifTagOutDic.Remove("G-GPSHeight"); //出力済みは削除
                    }
                }

                bmp.Dispose();
            }


        }

        //
        //各プロパティ計算用メソッド
        //
        static private string GetCommpression(System.Drawing.Imaging.PropertyItem Pitem) { //0x0103
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 1: str = "非圧縮"; break;
                case 2: str = "CCITT 1D"; break;
                case 3: str = "T4/Group 3 Fax"; break;
                case 4: str = "T6/Group 4 Fax"; break;
                case 5: str = "LZW"; break;
                case 6: str = "サムネイルもJPEG圧縮"; break;
                case 7: str = "JPEG"; break;
                case 8: str = "Adobe Deflate"; break;
                case 9: str = "JBIG B&W"; break;
                case 10: str = "JBIG Color"; break;
                case 99: str = "JPEG"; break;
                case 262: str = "Kodak 262"; break;
                case 32766: str = "Next"; break;
                case 32767: str = "Sony ARW Compressed"; break;
                case 32769: str = "Packed RAW"; break;
                case 32770: str = "Samsung SRW Compressed"; break;
                case 32771: str = "CCIRLEW"; break;
                case 32772: str = "Samsung SRW Compressed 2"; break;
                case 32773: str = "PackBits"; break;
                case 32809: str = "Thunderscan"; break;
                case 32867: str = "Kodak KDC Compressed"; break;
                case 32895: str = "IT8CTPAD"; break;
                case 32896: str = "IT8LW"; break;
                case 32897: str = "IT8MP"; break;
                case 32898: str = "IT8BL"; break;
                case 32908: str = "PixarFilm"; break;
                case 32909: str = "PixarLog"; break;
                case 32946: str = "Deflate"; break;
                case 32947: str = "DCS"; break;
                case 33003: str = "Aperio JPEG 2000 YCbCr"; break;
                case 33005: str = "Aperio JPEG 2000 RGB"; break;
                case 34661: str = "JBIG"; break;
                case 34676: str = "SGILog"; break;
                case 34677: str = "SGILog24"; break;
                case 34712: str = "JPEG 2000"; break;
                case 34713: str = "Nikon NEF Compressed"; break;
                case 34715: str = "JBIG2 TIFF FX"; break;
                case 34718: str = "Microsoft Document Imaging (MDI) Binary Level Codec"; break;
                case 34719: str = "Microsoft Document Imaging (MDI) Progressive Transform Codec"; break;
                case 34720: str = "Microsoft Document Imaging (MDI) Vector"; break;
                case 34887: str = "ESRI Lerc"; break;
                case 34892: str = "Lossy JPEG"; break;
                case 34925: str = "LZMA2"; break;
                case 34926: str = "Zstd (old)"; break;
                case 34927: str = "WebP (old)"; break;
                case 34933: str = "PNG"; break;
                case 34934: str = "JPEG XR"; break;
                case 50000: str = "Zstd"; break;
                case 50001: str = "WebP"; break;
                case 50002: str = "JPEG XL (old)"; break;
                case 52546: str = "JPEG XL"; break;
                case 65000: str = "Kodak DCR Compressed"; break;
                case 65535: str = "Pentax PEF Compressed"; break;                               
                default: str = "予約"; break;
            }
            return str;
        }
        static private string GetPhotometricInterpretation(System.Drawing.Imaging.PropertyItem Pitem) { //0x0106
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 2: str = "RGB"; break;
                case 6: str = "YCbCr"; break;
                // 以下はExif3.0にはないがExifToolにあるので追加した
                case 0: str = "WhiteIsZero"; break;
                case 1: str = "BlackIsZero"; break;
                case 3: str = "RGB Palette"; break;
                case 4: str = "Transparency Mask"; break;
                case 5: str = "CMYK"; break;
                case 8: str = "CIELab"; break;
                case 9: str = "ICCLab"; break;
                case 10: str = "ITULab"; break;
                case 32803: str = "Color Filter Array"; break;
                case 32844: str = "Pixar LogL"; break;
                case 32845: str = "Pixar LogLuv"; break;
                case 32892: str = "Sequential Color Filter"; break;
                case 34892: str = "Linear Raw"; break;
                case 51177: str = "Depth Map"; break;
                case 52527: str = "Semantic Mask"; break;
                // ここまで
                default: str = "予約"; break;
            }
            return str;
        }
        static private string GetOrientation(System.Drawing.Imaging.PropertyItem Pitem) { //0x0112
            long n;
            n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 1: str = "Exif回転無し"; break;
                case 2: str = "上下反転"; break;
                case 3: str = "180度回転"; break;
                case 4: str = "左右反転"; break;
                case 5: str = "上下反転+CW270度"; break;
                case 6: str = "CW90度"; break;
                case 7: str = "上下反転CW90度"; break;
                case 8: str = "CW270度"; break;
                default: str = "未定義" + n.ToString(); break;
            }
            return str;
        }
        static private string GetPlanarConfiguration(System.Drawing.Imaging.PropertyItem Pitem) { //0x011C
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 1: str = "点順次フォーマット"; break;
                case 2: str = "面順次フォーマット"; break;
                default: str = "予約"; break;
            }
            return str;
        }
        static private string GetResolutionUnit(System.Drawing.Imaging.PropertyItem Pitem) { //0x0128
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 2:
                    str = "インチ";
                    break;
                case 3:
                    str = "cm";
                    break;
                default:
                    str = "不明";
                    break;
            }
            return str;
        }

        static private string GetYCbCrSubSampling(System.Drawing.Imaging.PropertyItem Pitem) { //0x0212
            string str = "予約";
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 2) str = "異常値";
            else {
                ushort vl1 = BitConverter.ToUInt16(Pitem.Value, 0);
                ushort vl2 = BitConverter.ToUInt16(Pitem.Value, 2);
                if (vl1 == 2) {
                    if (vl2 == 1) str = "YCbCr4:2:2";
                    else if (vl2 == 2) str = "YCbCr4:2:0";
                }
            }
            return str;
        }
        static private string GetYCbCrPositioning(System.Drawing.Imaging.PropertyItem Pitem) { //0x0213
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 1: str = "中心"; break;
                case 2: str = "一致"; break;
                default: str = "不明"; break;
            }
            return str;
        }

        // 0xA432 LensSpecification用
        static private string GetLensSpecificatGet(System.Drawing.Imaging.PropertyItem Pitem) {
            string str;
            string[] dstr = new string[4];
            for (int i = 0; i < 4; i++)
                dstr[i] = AnyValueToString(Pitem, i);   // まずは４個のRATIONALの値を取得
            if (dstr[0] != "NaN") {                 //一つ目がNaNでない有効な場合は焦点距離として記す
                str = dstr[0];
                if (dstr[0] != dstr[1] && dstr[1] != "NaN") {  //二つ目が一つ目と異なりNanでないならば焦点距離の範囲を記す
                    str += "-" + dstr[1];
                    if (dstr[2] != "NaN") {     //三つ目がNaNでないならばF値を記す
                        str += "mm f/" + dstr[2];
                        if (dstr[3] != "NaN") {     //四つ目がNaNでないならばF値の範囲を記す
                            str += "-" + dstr[3];
                        }
                    }
                } else {            //二つ目が一つ目と同じかNaNならば焦点距離は一つ目の値のみ
                    str += "mm";
                    if (dstr[2] != "NaN") {     //
                        str += " F" + dstr[2];
                    }
                }

            } else {
                //一つ目がNaNならば焦点距離は不明
                str = "異常値";
            }
            return str;
        }


        static private string GetExposureProgram(System.Drawing.Imaging.PropertyItem Pitem) { //0x8822
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                int vl = GetExifByte(Pitem);
                switch (vl) {
                    case 0: str = "未定義"; break;
                    case 1: str = "マニュアル"; break;
                    case 2: str = "ノーマルプログラム"; break;
                    case 3: str = "絞り優先"; break;
                    case 4: str = "シャッター優先"; break;
                    case 5: str = "creative プログラム(被写界深度方向にバイアス)"; break;
                    case 6: str = "action プログラム(シャッタースピード高速側にバイアス)"; break;
                    case 7: str = "ポートレイトモード(クローズアップ撮影、背景はフォーカス外す)"; break;
                    case 8: str = "ランドスケープモード(landscape 撮影、背景はフォーカス合う)"; break;
                    case 9: str = "Bulb"; break;     //CIPAにはBulbはなくExifToolがあるので設けた
                    default: str = "予約(未実装)"; break;
                }
            }
            return str;
        }

        static private string GetSensitivity(System.Drawing.Imaging.PropertyItem Pitem) { //0x8827
            string str;
            if (Pitem.Type != SHORT) str = "異常値";
            else {
                long vl = GetExifShort(Pitem);
                if (vl < 0) vl += 65536; //符号なしなので
                if (vl >= 65535) {
                    str = "表示可能値以上";
                } else {
                    str = "ISO " + vl.ToString();
                }
            }
            return str;
        }

        static private string GetOECF(System.Drawing.Imaging.PropertyItem Pitem) { //0x0x8828
            int type = Pitem.Type;
            string str;
            if (type == UNDEFINED)
                str = "OECF(0x8828) Size = " + LenToCount(Pitem).ToString() + "Byte";
            else
                str = "MakerNote 異常値";
            return str;
        }

        static private string GetSensitivityType(System.Drawing.Imaging.PropertyItem Pitem) { //0x8830
            string str;
            if (Pitem.Type != SHORT && LenToCount(Pitem) != 1) str = "異常値";
            else {
                int vl = GetExifByte(Pitem);
                switch (vl) {
                    case 0: str = "不明"; break;
                    case 1: str = "標準出力感度(SOS)"; break;
                    case 2: str = "推奨露光指数(REI)"; break;
                    case 3: str = "ISO スピード(ISO Speed)"; break;
                    case 4: str = "標準出力感度(SOS)及び推奨露光指数(REI)"; break;
                    case 5: str = "標準出力感度(SOS)及びISO スピード(ISO Speed)"; break;
                    case 6: str = "推奨露光指数(REI)及びISO スピード(ISO Speed)"; break;
                    case 7: str = "標準出力感度(SOS)、推奨露光指数(REI)及びISO スピード(ISO Speed)"; break;
                    default: str = "予約(未実装)"; break;
                }
            }
            return str;
        }

        //2025/04/12 ExifToolの表記に合わせ"-"を追加し、各値をカンマで区切るように変更
        static private string GetComponentsConfiguration(System.Drawing.Imaging.PropertyItem Pitem) { //0x9101
            string str = "";
            for (int i = 0; i < 4; i++) {
                switch (Pitem.Value[i]) {
                    case 0x00: str += "-"; break;
                    case 0x01: str += "Y"; break;
                    case 0x02: str += "Cb"; break;
                    case 0x03: str += "Cr"; break;
                    case 0x04: str += "R"; break;
                    case 0x05: str += "G"; break;
                    case 0x06: str += "B"; break;
                    default: str = "不明な予約値"; break;
                }
                if (i < 3) str += ",";
            }
            return str;
        }
        static private string GetMeteringMode(System.Drawing.Imaging.PropertyItem Pitem) { //0x9207
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 0:
                    str = "不明"; break;
                case 1:
                    str = "平均"; break;
                case 2:
                    str = "中央重点"; break;
                case 3:
                    str = "スポット"; break;
                case 4:
                    str = "マルチスポット"; break;
                case 5:
                    str = "分割測光"; break;
                case 6:
                    str = "部分測光"; break;
                case 255:
                    str = "その他の測光方式"; break;
                default:
                    str = "予約"; break;
            }
            return str;
        }
        static private string GetLightSource(System.Drawing.Imaging.PropertyItem Pitem) { //0x9208
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 0: str = "不明"; break;
                case 1: str = "昼光"; break;
                case 2: str = "蛍光灯"; break;
                case 3: str = "タングステン"; break;
                case 4: str = "フラッシュ"; break;
                case 9: str = "晴天"; break;
                case 10: str = "曇天"; break;
                case 11: str = "日陰"; break;
                case 12: str = "昼光食蛍光灯"; break;
                case 13: str = "昼白色蛍光灯"; break;
                case 14: str = "白色蛍光灯"; break;
                case 15: str = "温白色蛍光灯"; break;
                case 16: str = "電球色蛍光灯"; break;
                case 17: str = "標準光A"; break;
                case 18: str = "標準光B"; break;
                case 19: str = "標準光C"; break;
                case 20: str = "D55"; break;
                case 21: str = "D65"; break;
                case 22: str = "D75"; break;
                case 23: str = "D50"; break;
                case 24: str = "ISO studio tungsten"; break;
                case 255: str = "その他の光源"; break;
                default: str = "予約"; break;
            }
            return str;
        }
        static private string GetFlash(System.Drawing.Imaging.PropertyItem Pitem) { //0x9209
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 0x0: str = "フラッシュなし"; break;
                case 0x1: str = "発光あり"; break;
                case 0x5: str = "発光、戻り検出せず"; break;
                case 0x7: str = "発光、戻り検出"; break;
                case 0x8: str = "オン、未発光"; break;
                case 0x9: str = "オン、発火"; break;
                case 0xd: str = "オン、戻り検出せず"; break;
                case 0xf: str = "オン、戻り検出"; break;
                case 0x10: str = "オフ, 発火しなかった"; break;
                case 0x14: str = "オフ、発火せず、戻り検出せず"; break;
                case 0x18: str = "自動, 発火せず"; break;
                case 0x19: str = "自動, 発火"; break;
                case 0x1d: str = "自動, 発火, 戻りは検出されない"; break;
                case 0x1f: str = "自動、発光、戻り検出済み"; break;
                case 0x20: str = "フラッシュ機能なし"; break;
                case 0x30: str = "消灯、フラッシュ機能なし"; break;
                case 0x41: str = "発光、赤目軽減"; break;
                case 0x45: str = "発光、赤目軽減、戻り未検出"; break;
                case 0x47: str = "発光、赤目軽減、戻り検出あり"; break;
                case 0x49: str = "オン、赤目軽減"; break;
                case 0x4d: str = "ON, 赤目軽減, 戻り検出なし"; break;
                case 0x4f: str = "ON, 赤目軽減, 戻りを検出"; break;
                case 0x50: str = "オフ, 赤目軽減"; break;
                case 0x58: str = "自動, 発光せず, 赤目軽減"; break;
                case 0x59: str = "自動, 発光, 赤目軽減"; break;
                case 0x5d: str = "自動, 発射済, 赤目軽減, 戻りを検出せず"; break;
                case 0x5f: str = "自動, 発光, 赤目軽減, 戻り検出済み"; break;
                default: str = "予約"; break;

            }
            return str;
        }

        static private string GetSubjectArea(System.Drawing.Imaging.PropertyItem Pitem) { //0x9214
            int cnt = LenToCount(Pitem);
            string str;
            switch (cnt) {
                case 2: str = "X,Y座標 "; break;
                case 3: str = "円位置X,Y 半径 "; break;
                case 4: str = "矩形座標X,Y,幅,高さ "; break;
                default: return "未定義";
            }
            for (int i = 0; i < cnt; i++) {
                str += BitConverter.ToInt16(Pitem.Value, i * 2).ToString() + BitConverter.ToInt16(Pitem.Value, i * 1 + 1).ToString();
            }
            return str;
        }


        // MakerNoteは各社独自で一貫性がないので長さのみ返す
        // https://exiftool.org/TagNames/EXIF.html　に詳しい説明がある
        static private string GetMakerNote(System.Drawing.Imaging.PropertyItem Pitem) { //0x0x927C
            int type = Pitem.Type;
            string str;
            if (type == UNDEFINED)
                str = "MakerNote Size = " + LenToCount(Pitem).ToString() + "Byte";
            else
                str = "MakerNote 異常値";
            return str;
        }


        static private object GetFlashpixVersion(System.Drawing.Imaging.PropertyItem Pitem) { //0xA000
            string str;
            str = Encoding.ASCII.GetString(Pitem.Value).Replace("\0", "");
            // Debug.Print( Pitem.Type.ToString() + " " + Encoding.ASCII.GetString(Pitem.Value));
            if (str == "0100")
                str = "Flashpix Format Version 1.0";
            else
                str = "未定義";
            return str;
        }
        static private string GetColorSpace(System.Drawing.Imaging.PropertyItem PItem) { //0xA001
            long n = GetExifShort(PItem);
            string str;
            if (n == 1)
                str = "sRGB";
            else if (n == 0xFFFF)
                str = "Uncalibrated";
            else
                str = "未定義";
            return str;
        }

        static private string GetSubjectLocation(System.Drawing.Imaging.PropertyItem Pitem) { //0xA214
            string str;
            if (Pitem.Type == SHORT || LenToCount(Pitem) == 2)
                str = "X,Y = " + BitConverter.ToInt16(Pitem.Value, 0) + " , " + BitConverter.ToInt16(Pitem.Value, 2);
            else
                str = "未定義";

            return str;
        }
        static private string GetSensingMethod(System.Drawing.Imaging.PropertyItem Pitem) { //0xA217
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 1: str = "未定義"; break;
                case 2: str = "単板カラーセンサー"; break;
                case 3: str = "2板カラーセンサー"; break;
                case 4: str = "3板カラーセンサー"; break;
                case 5: str = "色順次カラーセンサー"; break;
                case 7: str = "3線リニアセンサー"; break;
                case 8: str = "色順次リニアセンサー"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private string GetFileSource(System.Drawing.Imaging.PropertyItem Pitem) { //0xA300
            int type = Pitem.Type;
            byte val = Pitem.Value[0];
            string str;
            switch (val) {
                case 0x00: str = "その他"; break;
                case 0x01: str = "透過型スキャナ"; break;
                case 0x02: str = "反射型スキャナ"; break;
                case 0x03: str = "DSC"; break;
                default:
                    str = "予約"; break;
            }
            if (type != UNDEFINED && LenToCount(Pitem) != 1) str = "異常値";
            return str;
        }

        static private string GetSceneType(System.Drawing.Imaging.PropertyItem Pitem) { //0xA301
            string str;
            if ( /* Pitem.Type != UNDEFINED || */ LenToCount(Pitem) != 1) str = "異常値";
            else {
                byte val = Pitem.Value[0];
                switch (val) {
                    case 0x01: str = "直接撮影された画像"; break;
                    default: str = "予約"; break;
                }
                if (Pitem.Type != UNDEFINED) {        // 0xA301はUNDEFINEDであるべきだそうでないものもあったので
                    str += " (非正規値)";
                }
            }
            return str;
        }

        static private string GetCFAPattern(System.Drawing.Imaging.PropertyItem Pitem) { //0xA302
            string str;
            if (Pitem.Type != UNDEFINED) str = "異常値";
            else {
                short xn, yn;
                //CFAパターン（イメージセンサーのRGB画素などの配置を示すルーティンだが 横のパターン数 xn と　縦の yn　に値の各2バイト
                //を使って計算しているが多分デジカメ側がリトル（ビッグ）エンディアン

                // 2025/04/05 CFAパターンの計算方法に間違いがあり修正
                // yn = BitConverter.ToInt16(Pitem.Value, 2);

                Byte x1, x2, y1, y2;
                x1 = Pitem.Value[0];
                x2 = Pitem.Value[1];
                y1 = Pitem.Value[2];
                y2 = Pitem.Value[3];
                xn = (short)(x2 * 256); xn += x1;
                yn = (short)(y2 * 256); yn += y1;
                if (xn + yn > Pitem.Len - 4) {  //ビッグエンディアンのときの処理
                    xn = (short)(x1 * 256); xn += x2;
                    yn = (short)(y1 * 256); yn += y2;
                }
                int ptr = 4; str = "";
                for (int i = 0; i < yn; i++) {
                    str += "[";
                    for (int j = 0; j < xn; j++) {
                        str += GetFilterColor(Pitem.Value[ptr++]);
                        if (j < xn - 1) str += ",";
                        else str += "]";
                    }
                }

                //if (xn + yn > Pitem.Len - 4) {
                //    str = "未解決" + AnyTypeAndCount(Pitem, ",");
                //    return str;
                //}
                //str = "X = " + xn.ToString() + ",Y = " + yn.ToString() + " : ";
                //for (int i = 0; i < xn; i++) {
                //    str += GetFilterColor(Pitem.Value[i]);
                //    if (i != xn - 1) str += ",";
                //}
                //str += " : ";
                //for (int i = 0; i < yn; i++) {
                //    str += GetFilterColor(Pitem.Value[i + xn]);
                //    if (i != yn - 1) str += ",";
                //}
            }
            return str;
        }
        static private string GetFilterColor(byte bt) { //0xA302 GetCFAPattern()のサブ
            switch (bt) {
                case 0x00: return "Red";
                case 0x01: return "Green";
                case 0x02: return "Blue";
                case 0x03: return "Cyan";
                case 0x04: return "Magenta";
                case 0x05: return "Yellow";
                case 0x06: return "White";
                default: return "未定義";
            }
        }

        static private string GetCustomRendered(System.Drawing.Imaging.PropertyItem Pitem) { //0xA401
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "通常処理"; break;
                    case 1: str = "特殊処理"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetExposureMode(System.Drawing.Imaging.PropertyItem Pitem) { //0xA402
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "露出自動"; break;
                    case 1: str = "露出マニュアル"; break;
                    case 2: str = "オートブラケット"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetWhiteBalance(System.Drawing.Imaging.PropertyItem Pitem) { //0xA403
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "ホワイトバランス自動"; break;
                    case 1: str = "ホワイトバランスマニュアル"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetDigitalZoomRatio(System.Drawing.Imaging.PropertyItem Pitem) { //0xA404
            double rtn = GetExifRational(Pitem);
            if (Double.IsNaN(rtn)) {
                //if (GetSRational(Pitem.Value) == 0) {
                if (BitConverter.ToInt32(Pitem.Value, 0) == 0) {
                    return "未定義";
                }
            }
            return rtn.ToString();
        }

        static private string GetFocalLengthIn35mmFilm(System.Drawing.Imaging.PropertyItem Pitem) { //0xA405
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                if (n == 0) str = "不明";
                else str = n.ToString();
            }
            return str;
        }

        static private string GetSceneCaptureType(System.Drawing.Imaging.PropertyItem Pitem) { //0xA406
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "標準"; break;
                    case 1: str = "風景"; break;
                    case 2: str = "人物"; break;
                    case 3: str = "夜景"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetGainControl(System.Drawing.Imaging.PropertyItem Pitem) { //0xA407
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "無し"; break;
                    case 1: str = "弱い増感"; break;
                    case 2: str = "強い増感"; break;
                    case 3: str = "弱い減感"; break;
                    case 4: str = "強い減感"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetContrast(System.Drawing.Imaging.PropertyItem Pitem) { //0xA408
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "標準"; break;
                    case 1: str = "軟調"; break;
                    case 2: str = "硬調"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetSaturation(System.Drawing.Imaging.PropertyItem Pitem) { //0xA409
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "標準"; break;
                    case 1: str = "低彩度"; break;
                    case 2: str = "高彩度"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }
        [SupportedOSPlatform("linux")]
        static private string GetSharpness(System.Drawing.Imaging.PropertyItem Pitem) { //0xA40A
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "標準"; break;
                    case 1: str = "弱い"; break;
                    case 2: str = "強い"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetSubjectDistanceRange(System.Drawing.Imaging.PropertyItem Pitem) { //0xA40C
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "不明"; break;
                    case 1: str = "マクロ"; break;
                    case 2: str = "近景"; break;
                    case 3: str = "遠景"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetCompositeImage(System.Drawing.Imaging.PropertyItem Pitem) { //0xA460
            string str;
            if (Pitem.Type != SHORT || LenToCount(Pitem) != 1) str = "異常値";
            else {
                long n = GetExifShort(Pitem);
                switch (n) {
                    case 0: str = "不明"; break;
                    case 1: str = "非合成画像"; break;
                    case 2: str = "一般合成画像"; break;
                    case 3: str = "撮影時取得合成画像"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetGPSVersionID(System.Drawing.Imaging.PropertyItem Pitem) { //0x0000
            string str;
            if (Pitem.Type != BYTE || LenToCount(Pitem) != 4) str = "異常値";
            else {
                str = $"{Pitem.Value[0]:X}.{Pitem.Value[1]:X}.{Pitem.Value[2]:X}.{Pitem.Value[3]:X}";
            }
            return str;
        }

        static private string GetGPSLatitudeRef(string instr) { //0x0001
            string str;
            switch (instr) {
                case "N": str = "北緯"; break;
                case "S": str = "南緯"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private string GetGPSLongitudeRef(string instr) { //0x0003
            string str;
            switch (instr) {
                case "E": str = "東経"; break;
                case "W": str = "西経"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private double GetGPSValue(System.Drawing.Imaging.PropertyItem Pitem) { //0x0002,0x0004,0x0014,0x0016用
            UInt32 deg_numerator = BitConverter.ToUInt32(Pitem.Value, 0);
            UInt32 deg_denominator = BitConverter.ToUInt32(Pitem.Value, 4);
            double deg = (double)deg_numerator / (double)deg_denominator;

            UInt32 min_numerator = BitConverter.ToUInt32(Pitem.Value, 8);
            UInt32 min_denominator = BitConverter.ToUInt32(Pitem.Value, 12);
            double min = (double)min_numerator / (double)min_denominator;

            UInt32 sec_numerator = BitConverter.ToUInt32(Pitem.Value, 16);
            UInt32 sec_denominator = BitConverter.ToUInt32(Pitem.Value, 20);
            double sec;
            if (sec_denominator == 0) {
                sec = 0;
            } else {
                sec = (double)sec_numerator / (double)sec_denominator;
            }
            double Val = deg + min / 60.0 + sec / 3600.0;
            return Val;
        }

        static private string GetGPSAltitudeRef(System.Drawing.Imaging.PropertyItem Pitem) { //0x0005
            string str;
            if (Pitem.Type != BYTE && LenToCount(Pitem) != 1) str = "異常値";
            else {
                int vl = GetExifByte(Pitem);
                switch (vl) {
                    case 0: str = "GPS高度+"; break;
                    case 1: str = "GPS高度-"; break;
                    case 2: str = "海抜+"; break;
                    case 3: str = "海抜-"; break;
                    default: str = "予約"; break;
                }
            }
            return str;
        }

        static private string GetGPSTimeStamp(System.Drawing.Imaging.PropertyItem Pitem) { //0x0007
            string str = "";
            if (Pitem.Type != RATIONAL || LenToCount(Pitem) != 3) return "異常値";
            for (int i = 0; i < LenToCount(Pitem); i++) {
                str += GetRational(Pitem.Value, i * 8).ToString();
                switch (i) {
                    case 0: str += ":"; break;
                    case 1: str += ":"; break;
                    case 2: str += ":"; break;
                    default: str += "異常値"; break;
                }
            }
            return str;
        }

        static private string GetGPSStatus(string instr) { //0x0009
            string str;
            switch (instr) {
                case "A": str = "測位中"; break;
                case "V": str = "未測位（中断中)"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private string GetGPSMeasureMode(string instr) { //0x000A
            string str;
            switch (instr) {
                case "2": str = "2次元測位中"; break;
                case "3": str = "3次元測位中)"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private string GetGPSSpeedRef(string instr) { //0x000C
            string str;
            switch (instr) {
                case "K": str = "km/h"; break;
                case "M": str = "mph"; break;
                case "N": str = "knots"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private string GetGPSTrackRef(string instr) { //0x000E,10,17
            string str;
            switch (instr) {
                case "M": str = "磁気方位"; break;
                case "T": str = "真方位"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private string GetGPSDestDistanceRef(string instr) { //0x0019
            string str;
            switch (instr) {
                case "K": str = "km"; break;
                case "M": str = "マイル"; break;
                case "N": str = "海里"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        static private string GetGPSDifferential(System.Drawing.Imaging.PropertyItem Pitem) { //0x001E
            long n = GetExifShort(Pitem);
            string str;
            switch (n) {
                case 0: str = "単独測位"; break;
                case 1: str = "Differential補正測位"; break;
                default: str = "予約"; break;
            }
            return str;
        }

        private string GetGPSLocation() {
            string str, LatiStr, LongStr;
            if (string.IsNullOrEmpty(GPSLatitude)) str = "";
            else {
                switch (GPSLatitudeRef) {
                    case "北緯":
                    case "N":
                        LatiStr = "N";
                        break;
                    case "南緯":
                    case "S":
                        LatiStr = "S";
                        break;
                    default:
                        LatiStr = "異常値";
                        break;
                }
                switch (GPSLongitudeRef) {
                    case "東経":
                    case "E":
                        LongStr = "E";
                        break;
                    case "西経":
                    case "W":
                        LongStr = "W";
                        break;
                    default:
                        LongStr = "異常値";
                        break;
                }

                str = $"{LatiStr} {GPSLatitude:#.########} , {LongStr} {GPSLongitude:#.##########}";
            }
            return str;
            //string str;
            //if (string.IsNullOrEmpty(GPSVersionID)) str = "";
            //else {
            //    str = $"{GPSLatitudeRef} {GPSLatitude:#.########} , {GPSLongitudeRef} {GPSLongitude:#.##########}";
            //}
            //return str;
        }


        /// <summary>
        /// sppedStrの数値を表す文字列から "1/n 秒" または "n 秒" というシャッター速度を返す。小数点以下が長い場合は有効桁数が3桁になるようにする
        /// </summary>
        /// <param name="speedStr"></param>
        /// <returns></returns>
        static private string GetShutterSpeed(string speedStr) {
            //double sSpeed;
            bool lessThan1 = false;
            string str;
            if (!double.TryParse(speedStr, out double sSpeed)) {
                return "";
            }
            if (sSpeed <= 1) {
                lessThan1 = true; sSpeed = 1.0 / sSpeed;
            }
            double sSpeed0 = Math.Round(sSpeed, MidpointRounding.AwayFromZero);
            str = sSpeed0.ToString();
            if (sSpeed < 100) {
                double sSpeed1 = Math.Round(sSpeed, 1, MidpointRounding.AwayFromZero);
                if (sSpeed < 10) {
                    double sSpeed2 = Math.Round(sSpeed, 2, MidpointRounding.AwayFromZero);
                    if (sSpeed2 != sSpeed1) {
                        str = sSpeed2.ToString();
                    } else if (sSpeed1 != sSpeed0) {
                        str = sSpeed1.ToString();
                    } else {
                        str = sSpeed0.ToString();
                    }
                }
            }
            if (lessThan1) {
                str = "1/" + str + " 秒";
            } else {
                str += " 秒";
            }
            return str;
        }

        private string GetAperture(PropertyItem Pitem) {
            double fnum = GetExifRational(Pitem);
            if (fnum <= 0.0) return "未定義";
            fnum = Math.Pow(2, fnum / 2);
            if (fnum >= 10)
                return "F " + fnum.ToString("##");
            if (fnum > 1)
                return "F " + fnum.ToString("#.#");
            if (fnum > 0.1)
                return "F " + fnum.ToString("0.##");
            return "F" + fnum.ToString();
        }

        // F値のAPEX値を返す　未使用
        private string GetApertureAPEX(PropertyItem Pitem) {
            double fnum = GetExifRational(Pitem);
            return (-2 * Math.Log(fnum) / Math.Log(2)).ToString("#.##");
        }

        private string GetGPSHeight() {
            if (!string.IsNullOrEmpty(GPSAltitude))
                return GPSAltitudeRef.Replace("(楕円体基準)", "") + GPSAltitude;
            return "";
        }

        private string GetResolution() {
            if (!string.IsNullOrEmpty(XResolution) && !string.IsNullOrEmpty(YResolution))
                return XResolution + " x " + YResolution + " /" + ResolutionUnit;
            return "";
        }


        //画像の幅は Bitmap bmp のWidthプロパティで得る方式に変更
        //private string GetWidthHeight() {
        //    //if (!string.IsNullOrEmpty(PixelXDimension) && !string.IsNullOrEmpty(PixelYDimension)) {
        //    //    return PixelXDimension + " x " + PixelYDimension;
        //    //}
        //    return "";
        //}

        //
        // Typeによって値を得る
        //
        static private string GetExifAsci(System.Drawing.Imaging.PropertyItem PItem) {
            string val;
            switch (PItem.Type) {
                case ASCII: val = System.Text.Encoding.ASCII.GetString(PItem.Value); break;
                case UTF8: val = System.Text.Encoding.UTF8.GetString(PItem.Value); break;
                default: val = ""; break;
            }
            return val.Replace("\0", "").Trim(); //ValueにはNuullが含まれるとあるので削除する
        }
        // Byte[]の文字をCharとし、 COUNT数 区切子 spr でつなぐ 文字にNullが含まれている場合があったのでNullは削除する
        static private string GetUndefinedMultiByteToString(System.Drawing.Imaging.PropertyItem Pitem, string spr) {
            string str = "";
            int cnt = LenToCount(Pitem);
            for (int i = 0; i < cnt; i++) {
                str += (char)Pitem.Value[i];
                if (i != cnt - 1)
                    str += spr;
            }
            return str.Replace("\0", "").Trim();
        }
        // Undefinedを一つのAscii文字列として返す
        static private string GetUndefinedUniStringValue(System.Drawing.Imaging.PropertyItem Pitem) {
            string val = System.Text.Encoding.ASCII.GetString(Pitem.Value);
            return val.Replace("\0", "");
        }
        static private int GetExifByte(System.Drawing.Imaging.PropertyItem Pitem) {

            //return BitConverter.ToChar(Pitem.Value, 0);
            return (int)Pitem.Value[0];
        }

        static private long GetExifShort(System.Drawing.Imaging.PropertyItem Pitem) {
            return BitConverter.ToUInt16(Pitem.Value, 0);
        }

        static private double GetExifRational(System.Drawing.Imaging.PropertyItem Pitem) {
            switch (Pitem.Type) {
                case RATIONAL: return GetRational(Pitem.Value);
                case SRATIONAL: return GetSRational(Pitem.Value);
                default: return 0.0;
            }
        }

        static private double GetRational(byte[] vl) {
            return GetRational(vl, 0);
        }

        static private double GetRational(byte[] vl, int strt) {
            long numerator = BitConverter.ToUInt32(vl, strt);
            long nominator = BitConverter.ToUInt32(vl, strt + 4);
            double dbl = (double)numerator / (double)nominator;
            return dbl;
        }

        static private double GetSRational(byte[] vl) {
            return GetSRational(vl, 0);
        }

        static private double GetSRational(byte[] vl, int strt) {
            int numerator = BitConverter.ToInt32(vl, strt);
            int nominator = BitConverter.ToInt32(vl, strt + 4);
            double dbl = (double)numerator / (double)nominator;
            return dbl;
        }

        /// <summary>
        /// 複数のRATIONALを小数文字列として書き出す
        /// </summary>
        /// <param name="Pitem"></param>
        /// <param name="spr">複数のRATIONALを区切る区切子</param>
        /// <returns></returns>
        static private string GetMultiRationalToString(System.Drawing.Imaging.PropertyItem Pitem, string spr) {
            string str = "";
            //double dbl;

            int cnt = LenToCount(Pitem);
            for (int i = 0; i < cnt; i++) {
                str += GetRational(Pitem.Value, i * 8).ToString();
                //    str += BitConverter.ToUInt32(Pitem.Value, i * 8).ToString() + spr;
                //    str += BitConverter.ToUInt32(Pitem.Value, i * 8 + 4).ToString();
                //    if (i != cnt - 1)
                str += spr;
            }
            return str.Trim();
        }
        //TypeとCOUNTが変動しても順に spr（セパレーター）でつないで数値を文字列にする
        //ASCIIとUNDEFINEDはサポートしない。ASCIIはCOUNTが文字列の全体の文字数。
        //UNDEFINEDはこの文字列の数などにも使われるため個々のケースで対応する
        static private string AnyTypeAndCount(System.Drawing.Imaging.PropertyItem Pitem, string spr) {
            if (Pitem.Type == ASCII || Pitem.Type == UTF8) return "数値でない文字列を呼ぶ誤りがありました";
            string str = "";
            int len = LenToCount(Pitem);
            string st;
            for (int i = 0; i < len; i++) {
                st = AnyValueToString(Pitem, i);
                if (i != 0) str += spr;
                str += st;
            }
            return str;
        }
        //オーバーロード
        static private string AnyTypeAndCount(System.Drawing.Imaging.PropertyItem Pitem) {
            return AnyTypeAndCount(Pitem, "");
        }
        static private string AnyValueToString(System.Drawing.Imaging.PropertyItem Pitem, int i) {
            string st = "";
            switch (Pitem.Type) {
                case BYTE: st = Pitem.Value[i].ToString(); break;
                case SHORT: st = BitConverter.ToUInt16(Pitem.Value, i * 2).ToString(); break;
                case LONG: st = BitConverter.ToUInt32(Pitem.Value, i * 4).ToString(); break;
                case RATIONAL: st = GetRational(Pitem.Value, i * 8).ToString(); break;
                case SLONG: st = BitConverter.ToInt32(Pitem.Value, i * 4).ToString(); break; //2024/04/14 i * 8 を i * 4 に修正
                case SRATIONAL: st = GetSRational(Pitem.Value, i * 8).ToString(); break;
                default: break;
            }
            return st;
        }
        static private int LenToCount(System.Drawing.Imaging.PropertyItem Pitem) {
            return LenToCount(Pitem.Len, Pitem.Type);
        }
        static private int LenToCount(int len, short type) {
            int rtn = 0;
            switch (type) {
                case BYTE:
                case UNDEFINED: rtn = len; break;
                case SHORT: rtn = len / 2; break;
                case LONG:
                case SLONG: rtn = len / 4; break;
                case RATIONAL:
                case SRATIONAL:
                    rtn = len / 8; break;
            }
            return rtn;
        }
    }

}
