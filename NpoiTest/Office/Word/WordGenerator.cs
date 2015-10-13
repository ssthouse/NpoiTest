using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NpoiTest.Model.Database;
using NPOI.XWPF.UserModel;

namespace NpoiTest.Office.Word
{
    //传入数据---导出一份Word文件
    internal class WordGenerator
    {
        /// <summary>
        ///     数据源
        /// </summary>
        private readonly List<DbBean> dataList;

        //输出路径
        private readonly string outputPath;

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="dataList">需要导出的数据</param>
        /// <param name="outputPath">输出路径</param>
        public WordGenerator(List<DbBean> dataList, string outputPath)
        {
            this.dataList = dataList;
            this.outputPath = outputPath;
        }

        /// <summary>
        ///     导出WOrd文件
        /// </summary>
        public void Generate()
        {
            //首先在内存中生成一个word文档
            var document = new XWPFDocument();
            var fileStream = new FileStream(outputPath, FileMode.OpenOrCreate);

            //创建一个paragraph---作为标题
            var paragraph = document.CreateParagraph();
            paragraph.Alignment = ParagraphAlignment.CENTER;
            var run = paragraph.CreateRun();
            run.IsBold = true;
            var prjName = dataList[0].PrjName;
            run.SetText(prjName);
            //在paragraph后面添加几个换行
            for (var i = 0; i < 3; i++)
            {
                var tempParagraph = document.CreateParagraph();
                var tempRun = tempParagraph.CreateRun();
                tempRun.SetText(" ");
            }

            //创建一个表格---填充所有的数据
            var table = document.CreateTable(dataList.Count, 3);
            var spaceStr = "      ";
            for (var i = 0; i < dataList.Count; i++)
            {
                table.GetRow(i).GetCell(0).SetText(dataList[i].PrjName + spaceStr);
                table.GetRow(i).GetCell(1).SetText(dataList[i].Longitude + spaceStr);
                table.GetRow(i).GetCell(2).SetText(dataList[i].Latitude + spaceStr);
            }
            try
            {
                //保存文件
                document.Write(fileStream);
                document.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.StackTrace);
                return;
            }
            finally
            {
                fileStream.Close();
            }
            MessageBox.Show("Word文件生成成功");
        }
    }
}