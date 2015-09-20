using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
using NpoiTest.Model;
using NpoiTest.Office.Word;
using NPOI.XWPF.UserModel;

namespace NpoiTest
{
    /// <summary>
    /// MainWindow.xaml 的交互逻辑
    /// </summary>
    public partial class MainWindow : Window
    {



        //dataGrid中的数据
        private ObservableCollection<dgBean> dgBeanList;

        public MainWindow()
        {
            InitializeComponent();

            //初始化View
            InitView();

            WordTest.GetInstance().ReadWord();
        }

        /// <summary>
        /// 初始化View
        /// </summary>
        private void InitView()
        {
            dgBeanList = new ObservableCollection<dgBean>();
            dgBeanList.Add(new dgBean());
            dgBeanList.Add(new dgBean());
            dgBeanList.Add(new dgBean());
            dgBeanList.Add(new dgBean());
        }
}
}
