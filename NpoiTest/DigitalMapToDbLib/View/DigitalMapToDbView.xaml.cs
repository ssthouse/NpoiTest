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
                    //显示选中的文件夹
                    this.tboxDigitalPath.Text = dialog.SelectedPath;
                    //创建prjItem
                    prjItem = new PrjItem(this.tboxDigitalPath.Text);
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
                if (prjItem == null)
                {
                    System.Windows.MessageBox.Show("请先选择数字地图文件夹！");
                    return;
                }
                if (outputPath == null)
                {
                    System.Windows.MessageBox.Show("请先选择输出路径！");
                    return;
                }
                //如果数据库管理器还是空的---就初始化
                if (dbHelper == null)
                {
                    dbHelper = new DbHelper(prjItem);
                }
                dbHelper.generateDbFile(outputPath);
            };
        }
    }
}