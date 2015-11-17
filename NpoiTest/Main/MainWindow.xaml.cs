using NpoiTest.Model.Database;
using NpoiTest.Office.Word;
using System;
using System.Windows;
using System.Windows.Forms;
using DigitalMapToDB.DigitalMapParser.Utils;
using NpoiTest.DigitalMapDbLib.View;
using NpoiTest.ExcelInputToXml;

namespace NpoiTest.Main
{
    /// <summary>
    ///     程序的入口
    ///     该程序的主要目的是-----将数据库中的文件导出成一份word和excel文档
    /// </summary>
    public partial class MainWindow : Window
    {
        private const string TAG = "MainWIindow";

        /// <summary>
        ///     唯一的数据库数据
        /// </summary>
        private DbData dbData;

        /// <summary>
        ///     入口方法
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();

            //初始化View---以及点击事件
            InitView();

            //初始化MenuItem的相应时间
            InitMenuItem();

            //TODO
            double [] result = CoordinateConverter.UTMWGSXYtoBL(3379160.647545, 475096.362006);
            Log.Err(TAG, ""+result[0]+"     "+result[1]);
        }

        /// <summary>
        /// 初始化MenuItem
        /// </summary>
        private void InitMenuItem()
        {
            //数字地图转换为数据库的menu调用
            this.menuDigitalMapToDb.Click += delegate(object sender, RoutedEventArgs args)
            {
                DigitalMapToDbView digitalMapToDbView = new DigitalMapToDbView();
                digitalMapToDbView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                digitalMapToDbView.ResizeMode = ResizeMode.NoResize;
                digitalMapToDbView.Show();
            };

            //Excel输入文件转换为xml输出文件
            this.menuExcelInputToXml.Click += delegate(object sender, RoutedEventArgs args)
            {
                ExcelInputToXmlView excelInputToXmlView = new ExcelInputToXmlView();
                excelInputToXmlView.WindowStartupLocation = WindowStartupLocation.CenterScreen;
                excelInputToXmlView.ResizeMode = ResizeMode.NoResize;
                excelInputToXmlView.Show();
            };
        }

        /// <summary>
        ///     初始化View
        /// </summary>
        private void InitView()
        {
            //初始位置定位到中心
            this.WindowStartupLocation = WindowStartupLocation.CenterScreen;
            this.ResizeMode = ResizeMode.NoResize;

            //打开数据库点击事件
            btnOpenDbFile.Click += delegate
            {
                //打开文件选择器
                FileDialog dialog = new OpenFileDialog();
                dialog.Filter = "数据库文件|*.db";
                dialog.ShowDialog();
                //获取选取的文件路径
                if (dialog.FileName == null || dialog.FileName.Length == 0)
                {
                    return;
                }
                var dbPath = dialog.FileName;
                //判断当前数据库文件是否可用
                if (!DbData.IsDbFileValid(dbPath))
                {
                    System.Windows.MessageBox.Show("当前数据库文件不可用","出错");
                    return;
                }
                else
                {
                    //更新textbox的文字
                    tbDbPath.Text = dbPath;
                    //实例化数据库数据
                    dbData = new DbData(dbPath);
                    //初始化project选择框
                    initPrjSelectCombox();
                    //初始化下面的datagrid的table
                    initDataGrid();
                }
            };

            //工程的ComboBox点击事件
            cbPrjChoose.SelectionChanged += delegate
            {
                //更新下面的表格数据
                initDataGrid();
            };

            //word输出路径的监听
            btnGetWordPath.Click += delegate
            {
                //打开文件保存对话框
                var dialog = new SaveFileDialog();
                dialog.Filter = "word文件|*.docx";
                dialog.ShowDialog();
                //获取得到的path
                var path = dialog.FileName;
                if (path != null && path.Length != 0)
                {
                    tbWordPath.Text = path;
                }
            };

            //Excel输出路径的监听
            btnGetExcelPath.Click += delegate
            {
                //打开文件保存对话框
                var dialog = new SaveFileDialog();
                dialog.Filter = "excel文件|*.xls";
                dialog.ShowDialog();
                //获取得到的path
                var path = dialog.FileName;
                if (path != null && path.Length != 0)
                {
                    tbExcelPath.Text = path;
                }
            };

            //Word输出监听事件
            btnGenerateWord.Click += delegate
            {
                if (dbData == null)
                {
                    System.Windows.MessageBox.Show("请先选择一个数据库文件", "提示");
                    return;
                }
                var outputPath = tbWordPath.Text;
                if (outputPath == null || outputPath.Length == 0)
                {
                    System.Windows.MessageBox.Show("请先选择输出路径", "提示");
                    return;
                }
                //生成word文件
                new WordGenerator(dbData.GetSpecificDbData((string) cbPrjChoose.SelectedValue), outputPath)
                    .Generate();
            };

            //Excel输出监听事件
            btnGenerateExcel.Click += delegate
            {
                if (dbData == null)
                {
                    System.Windows.MessageBox.Show("请先选择一个数据库文件", "提示");
                    return;
                }
                var outputPath = tbExcelPath.Text;
                if (outputPath == null || outputPath.Length == 0)
                {
                    System.Windows.MessageBox.Show("请先选择输出路径", "提示");
                    return;
                }
                //生成word文件
                new ExcelGenerator(dbData.GetSpecificDbData((string) cbPrjChoose.SelectedValue), outputPath)
                    .Generate();
            };
        }

        /// <summary>
        ///     初始化project选择框
        /// </summary>
        private void initPrjSelectCombox()
        {
            var prjStrList = dbData.GetProjectStrList();
            cbPrjChoose.ItemsSource = prjStrList;
            //将当前值设为第一个project
            cbPrjChoose.SelectedValue = prjStrList[0];
        }

        /// <summary>
        ///     初始化下面的数据表格
        /// </summary>
        private void initDataGrid()
        {
            //获取当前project名称
            var prjName = (string) cbPrjChoose.SelectedValue;
            if (prjName != null)
            {
                //从数据库获取数据
                var dataList = dbData.GetSpecificDbData(prjName);
                //将数据绑定上去
                dgTable.ItemsSource = dataList;
            }
        }
    }
}