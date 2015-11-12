using System;
using System.Threading;
using System.Windows;
using System.Windows.Forms;
using DigitalMapToDB.DigitalMapParser;
using DigitalMapToDB.DigitalMapParser.Utils;

namespace NpoiTest.DigitalMapDbLib.View
{
    /// <summary>
    ///     MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class DigitalMapToDbView : Window
    {

        /// <summary>
        /// 操作的prjItem
        /// </summary>
        private PrjItem prjItem;

        //数字地图路径
        private string digitalFilePath;
        //输出路径
        private string outputPath;

        /// <summary>
        /// 数据库管理类
        /// </summary>
        private DbHelper dbHelper;

        /// <summary>
        /// 构造方法
        /// </summary>
        public DigitalMapToDbView()
        {
            InitializeComponent();

            //初始化View---事件
            initView();
        }

        //初始化View---事件
        private void initView()
        {
            //获取数字地图文件路径
            btnChooseDigitalMap.Click += delegate
            {
               FolderBrowserDialog dialog = new FolderBrowserDialog();
                if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    string path = dialog.SelectedPath;
                    if (!PrjItem.isDirectoryValid(path))
                    {
                        System.Windows.MessageBox.Show("当前路径不符合数字地图文件要求", "错误");
                        return;
                    }
                    //获取选中的文件夹
                    this.tboxDigitalPath.Text = dialog.SelectedPath;
                    digitalFilePath = dialog.SelectedPath;
                }
            };

            //获取输出路径
            btnChooseOutputPath.Click += delegate(object sender, RoutedEventArgs args)
            {
                var saveFileDialog = new SaveFileDialog();
                saveFileDialog.Filter = "数据库文件|*.db";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //获取得到的path
                    var path = saveFileDialog.FileName;
                    if (path != null && path.Length != 0)
                    {
                        outputPath = path;
                        tboxOutputPath.Text = path;
                    }
                }
            };

            //生成数据库文件的按钮
            this.btnGenerateDBFile.Click += delegate(object sender, RoutedEventArgs args)
            {
                //需要先选择数字地图文件夹
                if (digitalFilePath == null)
                {
                    System.Windows.MessageBox.Show("请先选择数字地图文件夹！");
                    return;
                }
                if (outputPath == null)
                {
                    System.Windows.MessageBox.Show("请先选择输出路径！");
                    return;
                }
                //启动线程---生成数据库文件
                Thread thread = new Thread(new ThreadStart(generateDbFile));
                thread.Start();
            };
        }

        /// <summary>
        /// 生成数据库文件(在新线程中执行)
        /// </summary>
        private void generateDbFile()
        {
            //让progressbar显示出来
            Action<System.Windows.Controls.ProgressBar, bool> updataAction = new Action<System.Windows.Controls.ProgressBar, bool>(updataProgressBar);
            this.pbDigitalMapConvert.Dispatcher.BeginInvoke(updataAction, pbDigitalMapConvert, true);
            if (prjItem == null)
            {
                prjItem = new PrjItem(digitalFilePath);
            }
            if (dbHelper == null)
            {
                dbHelper = new DbHelper(prjItem);
            }
            dbHelper.generateDbFile(outputPath);
            this.pbDigitalMapConvert.Dispatcher.BeginInvoke(updataAction, pbDigitalMapConvert, false);
        }

        /// <summary>
        /// 更新UI用的方法
        /// </summary>
        /// <param name="isShow"></param>
        private void updataProgressBar(System.Windows.Controls.ProgressBar pb, bool isShow)
        {
            if (isShow)
            {
                this.pbDigitalMapConvert.Visibility = Visibility.Visible;
            }
            else
            {
                this.pbDigitalMapConvert.Visibility = Visibility.Hidden;
            }
        }
    }
}