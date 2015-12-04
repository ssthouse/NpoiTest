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
using System.Windows.Shapes;
using Microsoft.Win32;

namespace NpoiTest.ExcelInputToXml
{
    /// <summary>
    /// ExcelInputToXml.xaml 的交互逻辑
    /// </summary>
    public partial class ExcelInputToXmlView : Window
    {
        /// <summary>
        /// 文件转换器
        /// </summary>
        private ExcelToXmlConverter excelToXmlConverter;

        /// <summary>
        /// 文件输出路径
        /// </summary>
        private string outputPath;

        /// <summary>
        /// 入口方法
        /// </summary>
        public ExcelInputToXmlView()
        {
            InitializeComponent();

            //初始化View
            InitView();
        }

        /// <summary>
        /// 初始化View
        /// </summary>
        private void InitView()
        {
            //Excel文件路径
            this.btnChooseExcelFile.Click += delegate(object sender, RoutedEventArgs args)
            {
                System.Windows.Forms.OpenFileDialog fileDialog = new System.Windows.Forms.OpenFileDialog();
                fileDialog.Filter = "Excel文件|*.xlsx";
                if (fileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //初始化文件转换器
                    excelToXmlConverter = new ExcelToXmlConverter(fileDialog.FileName);
                    this.tboxExcelFilePath.Text = fileDialog.FileName;
                    //检测Excel数据是否可用
                    if (!excelToXmlConverter.IsDataValid())
                    {
                        MessageBox.Show("Excel输入文件数据格式有误！", "提示");
                        this.tboxExcelFilePath.Text = "";
                    }
                }
            };

            //Xml输出路径
            this.btnChooseOutputPath.Click += delegate(object sender, RoutedEventArgs args)
            {
                System.Windows.Forms.SaveFileDialog saveFileDialog = new System.Windows.Forms.SaveFileDialog();
                saveFileDialog.Filter = "Xml|*.xml";
                if (saveFileDialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                {
                    //初始化输出路径
                    outputPath = saveFileDialog.FileName;
                    this.tboxOutputPath.Text = saveFileDialog.FileName;
                }
            };

            //生成Xml文件
            this.btnGenerateXmlFile.Click += delegate(object sender, RoutedEventArgs args)
            {
                //文件转换器已初始化 excel路径已有 输出路径已有
                if (excelToXmlConverter != null && outputPath != null)
                {
                    excelToXmlConverter.ExportXmlFile(outputPath);
                    MessageBox.Show("文件生成完毕", "提示");
                }
                else
                {
                    MessageBox.Show("请先选择输入输出路径", "提示");
                }
            };
        }
    }
}
