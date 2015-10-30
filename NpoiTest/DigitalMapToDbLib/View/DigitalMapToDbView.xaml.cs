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

        /// <summary>
        /// 数据库管理类
        /// </summary>
        private DbHelper dbHelper;

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
                    //显示选中的文件夹
                    this.tboxDigitalPath.Text = dialog.SelectedPath;

                    //TODO---这里应该进行判断---这个文件夹是不是可用的
                    prjItem = new PrjItem(this.tboxDigitalPath.Text);
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
                //如果数据库管理器还是空的---就初始化
                if (dbHelper == null)
                {
                    dbHelper = new DbHelper(prjItem);
                }
                dbHelper.generateDbFile(@"C:\Users\ssthouse\Desktop");
            };
        }
    }
}