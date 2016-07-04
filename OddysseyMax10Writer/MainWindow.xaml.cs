
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace OdysseyWriter
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private OdysseyControler mController;
        private bool mConnected;
        private bool mLoaded;
        private String mPath;
        private FileSystemWatcher mFileWatcher;
        private bool mUpdate;
        public MainWindow()
        {
            InitializeComponent();
            mFileWatcher = new FileSystemWatcher();
            mFileWatcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.Size | NotifyFilters.LastAccess;
            mFileWatcher.Changed += mFileChanged;
            mUpdate = mConnected = mLoaded = false;

            for (int i = 1;i <= 10; i++ )
            {
                num.Items.Add(i);
            }
            num.SelectedIndex = 0;

            //            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            UpdateCommandButton();
            Connect();

        }

        private async void mFileChanged(object sender, FileSystemEventArgs e)
        {
            if (mUpdate) return;
            mUpdate = true;
            UpdateStatus("更新中");
            await Task.Run(() => {
                Task.Delay(1000);
                for (int i = 0; i < 100; i++)
                {
                    try
                    {
                        FileStream f = File.Open(mPath, FileMode.Open, FileAccess.Read, FileShare.None);
                        f.Close();
                        break;
                    }catch(Exception ex)
                    {
                        Task.Delay(500);
                    }

                }
            });
            Dispatcher.Invoke(new Action(async () =>
            {
                await WriteProgram();
                num.SelectedIndex = (num.SelectedIndex + 1) % num.Items.Count;
            }));
            mUpdate = false;


        }

        private async Task WriteProgram()
        {
            int n = (Int32)num.SelectedItem;
            UpdateCommandButton(true);
            UpdateStatus("書き込み中 #" + n.ToString());
            await mController.Write(mPath, n);
            UpdateStatus("プログラム中 #" + n.ToString());
            await mController.Program(n);
            UpdateStatus("完了");
            UpdateCommandButton(false);

        }

        public void UpdateStatus(string s)
        {
            Dispatcher.Invoke(new Action(() => {
                status.Content = s;
            }));
        }

        private void UpdateCommandButton(bool processing = false)
        {
            writeprogram.IsEnabled = write.IsEnabled = program.IsEnabled = mConnected && mLoaded && !processing;
            num.IsEnabled = autowp.IsEnabled = num.IsEditable = !processing;
        }

        private async Task<bool> Connect()
        {
            mConnected = false;
            connect.IsEnabled = false;
            UpdateStatus("接続中");
            try
            {

                mController = new OdysseyControler();
                mController.OnSerialIO += OnSerialIO;
                
                await mController.Connect();
                await mController.GoMenu();
                
                connect.Content = "Connected";
                UpdateStatus("接続済み");
                connect.IsEnabled = false;
                mConnected = true;
            }
            catch (Exception e)
            {
                UpdateStatus("未接続");
                connect.IsEnabled = true;
            }
            UpdateCommandButton();
            return mConnected;
        }

        private void OnSerialIO(string s)
        {
            Dispatcher.Invoke(new Action(() =>{
                log.AppendText(s + System.Environment.NewLine);
            }));          
        }

        private async void button_Click(object sender, RoutedEventArgs e)
        {
            int n = (Int32)num.SelectedItem;
            UpdateCommandButton(true);
            UpdateStatus("書き込み中 #" + n.ToString());
            await mController.Write(mPath, n);
            UpdateStatus("完了");
            UpdateCommandButton(false);
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JBC File (*.jbc)|*.jbc";
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                mPath = textPath.Text = dlg.FileName;
                mFileWatcher.Path = System.IO.Path.GetDirectoryName(mPath);
                mFileWatcher.Filter = System.IO.Path.GetFileName(mPath);
                mFileWatcher.EnableRaisingEvents = true;
                mLoaded = true;
            }
            UpdateCommandButton();
        }

        private void log_TextChanged(object sender, TextChangedEventArgs e)
        {
            log.CaretIndex = log.Text.Length;
            log.ScrollToEnd();
        }

        private void Grid_Loaded(object sender, RoutedEventArgs e)
        {
            log.CaretIndex = log.Text.Length;
            log.ScrollToEnd();
        }

        private async void program_Click(object sender, RoutedEventArgs e)
        {
            int n = (Int32)num.SelectedItem;
            UpdateCommandButton(true);
            UpdateStatus("プログラム中 #" + n.ToString());
            await mController.Program(n);
            UpdateStatus("完了");
            UpdateCommandButton(false);
        }

        private void connect_Click(object sender, RoutedEventArgs e)
        {
            Connect();
        }

        private async void writeprogram_Click(object sender, RoutedEventArgs e)
        {
            await WriteProgram();
        }

        private void autowp_Checked(object sender, RoutedEventArgs e)
        {
            if(mPath != null) mFileWatcher.EnableRaisingEvents = autowp.IsChecked == true;
        }
    }
}
