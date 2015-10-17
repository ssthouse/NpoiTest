using System.Windows;
using Microsoft.Win32;
using NpoiTest.Model.Database;
using NpoiTest.Office.Word;

namespace NpoiTest
{
    /// <summary>
    ///     程序的入口
    ///     该程序的主要目的是-----将数据库中的文件导出成一份word和excel文档
    /// </summary>
    public partial class MainWindow : Window
    {
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

            //初始化View
            InitView();

            //TODO---测试代码
//            WordTest.GetInstance().ReadWord();
        }

        /// <summary>
        ///     初始化View
        /// </summary>
        private void InitView()
        {
            //打开数据库点击事件
            btnOpenDbFile.Click += delegate
            {
                //打开文件选择器
                FileDialog dialog = new OpenFileDialog();
                dialog.Filter = "数据库文件|*.db;*.pptx|hahaha|*.docx";
                dialog.ShowDialog(this);
                //获取选取的文件路径
                var dbPath = dialog.FileName;

                //更新textbox的文字
                tbDbPath.Text = dbPath;

                //实例化数据库数据
                dbData = new DbData(dbPath);

                //初始化project选择框
                initPrjSelectCombox();

                //初始化下面的datagrid的table
                initDataGrid();
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
                    MessageBox.Show("请先选择一个数据库文件", "提示");
                    return;
                }
                var outputPath = tbWordPath.Text;
                if (outputPath == null || outputPath.Length == 0)
                {
                    MessageBox.Show("请先选择输出路径");
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
                    MessageBox.Show("请先选择一个数据库文件", "提示");
                    return;
                }
                var outputPath = tbExcelPath.Text;
                if (outputPath == null || outputPath.Length == 0)
                {
                    MessageBox.Show("请先选择输出路径", "提示");
                    return;
                }
                //生成word文件
                new ExcelGenerator(dbData.GetSpecificDbData((string)cbPrjChoose.SelectedValue), outputPath)
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