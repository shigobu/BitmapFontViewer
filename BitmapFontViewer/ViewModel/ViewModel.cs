using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace BitmapFontViewer
{
    public class ViewModel : INotifyPropertyChanged
    {
        #region INotifyPropertyChangedの実装
        public event PropertyChangedEventHandler? PropertyChanged;

        public void RaisePropertyChanged([CallerMemberName] string? propertyName = null)
          => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        #endregion

        public ViewModel(MainWindow mainWindow)
        {
            Owner = mainWindow;
            OpenBitmapCommand = new DelegateCommand(OpenBitmap);
            UpdateCommand = new DelegateCommand(UpdateBitmap);
        }

        #region プロパティ

        /// <summary>
        /// mainWindow
        /// </summary>
        public MainWindow Owner { get; set; }

        private string _BitmapFileName = "bitmap未指定";
        /// <summary>
        /// bitmapファイル名
        /// </summary>
        public string BitmapFileName
        {
            get => _BitmapFileName;
            set
            {
                if (_BitmapFileName == value) return;
                _BitmapFileName = value;
                RaisePropertyChanged();
            }
        }


        private int _FontWidth = 6;
        /// <summary>
        /// フォント幅
        /// </summary>
        public int FontWidth
        {
            get => _FontWidth;
            set
            {
                if (_FontWidth == value) return;
                _FontWidth = value;
                RaisePropertyChanged();
            }
        }

        private int _FontHeight = 8;
        /// <summary>
        /// フォント高さ
        /// </summary>
        public int FontHeight
        {
            get => _FontHeight;
            set
            {
                if (_FontHeight == value) return;
                _FontHeight = value;
                RaisePropertyChanged();
            }
        }

        private string _SearchChar = "";
        /// <summary>
        /// 検索文字
        /// </summary>
        public string SearchChar
        {
            get => _SearchChar;
            set
            {
                if (_SearchChar == value) return;
                _SearchChar = value;
                RaisePropertyChanged();
            }
        }

        private int[] _magnificationComboItem = {1, 2, 3, 4};
        /// <summary>
        /// コンボボックスアイテム
        /// </summary>
        public int[] MagnificationComboItem
        {
            get => _magnificationComboItem;
            set
            {
                if (_magnificationComboItem == value) return;
                _magnificationComboItem = value;
                RaisePropertyChanged();
            }
        }

        private int _Magnification = 1;
        /// <summary>
        /// 倍率
        /// </summary>
        public int Magnification
        {
            get => _Magnification;
            set
            {
                if (_Magnification == value) return;
                _Magnification = value;
                RaisePropertyChanged();
            }
        }


        private BitmapSource? _CharBitmap;
        public BitmapSource? CharBitmap
        {
            get => _CharBitmap;
            set
            {
                if (_CharBitmap == value) return;
                _CharBitmap = value;
                RaisePropertyChanged();
            }
        }
        #endregion プロパティ

        #region コマンド

        /// <summary>
        /// bitmap指定ボタンコマンド
        /// </summary>
        public ICommand OpenBitmapCommand { get; private set; }

        public ICommand UpdateCommand { get; private set; }

        #endregion

        #region コマンドメソッド

        private void OpenBitmap()
        {
            OpenFileDialog fileDialog = new();
            fileDialog.Filter = "画像ファイル|*.BMP;*.GIF;*.EXIF;*.JPG;*.PNG;*.TIFF";

            if (fileDialog.ShowDialog() ?? false)
            {
                BitmapFileName = fileDialog.FileName;
            }
        }

        /// <summary>
        /// フォント画像を更新します。
        /// </summary>
        private void UpdateBitmap()
        {
            if (string.IsNullOrWhiteSpace(BitmapFileName))
            {
                return;
            }

            if (!System.IO.File.Exists(BitmapFileName))
            {
                return;
            }

            if (FontWidth == 0 || FontHeight == 0)
            {
                return;
            }

            if (Magnification == 0)
            {
                return;
            }


            if (string.IsNullOrWhiteSpace(SearchChar))
            {
                return;
            }

            byte ku = 0;
            byte ten = 0;
            GetCharKuTen(SearchChar[0], out ku, out ten);

            BitmapImage fontBitmap = new BitmapImage(new Uri(BitmapFileName));
            CroppedBitmap croppedBitmap = new CroppedBitmap(fontBitmap, new Int32Rect(FontWidth * (ten - 1), FontHeight * (ku - 1), FontWidth, FontHeight));

            CharBitmap = croppedBitmap; 
        }

        #endregion コマンドメソッド

        #region メソッド

        void GetCharKuTen(char character, out byte ku, out byte ten)
        {
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Encoding iso2022jp = Encoding.GetEncoding("iso-2022-jp");
            byte[] bytes = iso2022jp.GetBytes(new char[] {character});
            if (bytes.Length < 5)
            {
                ku = 0;
                ten = 0;
                return;
            }

            ku = (byte)(bytes[3] - 0x20);
            ten = (byte)(bytes[4] - 0x20);
        }

        #endregion

    }
}
