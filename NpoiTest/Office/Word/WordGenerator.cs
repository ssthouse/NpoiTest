using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiTest.Model.Database;
using NPOI.XWPF.UserModel;

namespace NpoiTest.Office.Word
{
    //传入数据---导出一份Word文件
    class WordGenerator
    {

        /// <summary>
        /// 数据源
        /// </summary>
        private List<DbBean> dataList;

        //输出路径
        private string outputPath;

        /// <summary>
        /// 构造方法
        /// </summary>
        /// <param name="dataList">需要导出的数据</param>
        /// <param name="outputPath">输出路径</param>
        public WordGenerator(List<DbBean> dataList, string outputPath)
        {
            this.dataList = dataList;
            this.outputPath = outputPath;
        }

        /// <summary>
        /// 导出WOrd文件
        /// </summary>
        public void Generate()
        {
            //首先在内存中生成一个word文档
            XWPFDocument document = new XWPFDocument();
            
            //创建一个paragraph---作为标题
            XWPFParagraph paragraph = document.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.CENTER;
            XWPFRun run = paragraph.CreateRun();
            run.IsBold = true;
            run.SetText("华中科技大学工程");

        }
    }
}
