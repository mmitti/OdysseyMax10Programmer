
using System;
using System.Collections.Generic;
using System.Diagnostics;
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

namespace OddysseyMax10Writer
{
    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private OddysseyControler mController;
        public MainWindow()
        {
            InitializeComponent();

//            Debug.Listeners.Add(new TextWriterTraceListener(Console.Out));
            mController = new OddysseyControler();
            mController.OnSerialIO += OnSerialIO;
            mController.Connect();
            mController.GoMenu();

        }

        private void OnSerialIO(string s)
        {
            log.AppendText(s + System.Environment.NewLine);            
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            mController.Write(textPath.Text);
        }

        private void open_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            dlg.Filter = "JBC File (*.jbc)|*.jbc";
            if(dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                textPath.Text = dlg.FileName;
            }
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

        private void program_Click(object sender, RoutedEventArgs e)
        {
            mController.Program();
        }
    }
}
