using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ImageCompressConverter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (!checkInputPath())
            {
                return;
            }

            if (!isExistDestDirectory())
            {
                System.IO.Directory.CreateDirectory(destPath.Text);
            }

            String[] convertFiles = System.IO.Directory.GetFiles(targetPath.Text);

            execute(convertFiles);

            MessageBox.Show("正常に変換できました。");
        }

        private bool checkInputPath()
        {
            if (exePath.Text == "")
            {
                MessageBox.Show("ImageMagickの実行ファイルが未指定です。");
                return false;
            }
            if (targetPath.Text == "")
            {
                MessageBox.Show("変換対象ファイルのディレクトリが未指定です。");
                return false;
            }
            if (destPath.Text == "")
            {
                MessageBox.Show("変換ファイルの出力先が未指定です。");
                return false;
            }
            return true;
        }

        /// <summary>
        /// 変換ファイルの出力先ディレクトリの存在チェック
        /// </summary>
        /// <returns></returns>
        private bool isExistDestDirectory()
        {
            return System.IO.Directory.Exists(destPath.Text);
        }

        private void execute(string[] convertFiles)
        {
            for (int i = 0; i < convertFiles.Length; i++)
            {
                System.Diagnostics.ProcessStartInfo process = new System.Diagnostics.ProcessStartInfo()
                {
                    FileName = exePath.Text,
                    Arguments = makeArgument(convertFiles[i]),
                    CreateNoWindow = true,
                    UseShellExecute = false
                };
                System.Diagnostics.Process p = System.Diagnostics.Process.Start(process);
                p.WaitForExit();
                p.Refresh();
            }
        }

        private String makeArgument(String convertFileName)
        {
            return "-filter Lanczos -quality 90 \"" + convertFileName + "\" \"" + destPath.Text + "\\" + System.IO.Path.GetFileName(convertFileName) + "\"";
        }
    }
}
