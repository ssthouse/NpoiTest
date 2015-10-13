using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NpoiTest.Model.Database;
using NPOI.HSSF.UserModel;

namespace NpoiTest.Office.Word
{
    internal class ExcelGenerator
    {
        /// <summary>
        ///     数据源
        /// </summary>
        private readonly List<DbBean> dataList;

        private readonly string outputPath;

        /// <summary>
        ///     构造方法
        /// </summary>
        /// <param name="dataList">需要导出的数据</param>
        /// <param name="outputPath">输出路径</param>
        public ExcelGenerator(List<DbBean> dataList, string outputPath)
        {
            this.dataList = dataList;
            this.outputPath = outputPath;
        }

        /// <summary>
        ///     导出WOrd文件
        /// </summary>
        public void Generate()
        {
            //首先在内存中创建一个Excel文件
            var fileStream = new FileStream(outputPath, FileMode.OpenOrCreate);
            var workbook = new HSSFWorkbook();
            //创建一个表(sheet)--并获取他
            var sheet = (HSSFSheet) workbook.CreateSheet("华中科技大学工程");
            //首先把需要多少个cell创建好----有多少个数据就有多少行--每行有3个数据
            for (var i = 0; i < dataList.Count; i++)
            {
                sheet.CreateRow(i);
            }
            //然后给每行---创建单元格---添加数据
            for (var i = 0; i < dataList.Count; i++)
            {
                var row = (HSSFRow) sheet.GetRow(i);
                row.CreateCell(0).SetCellValue(dataList[i].PrjName);
                //如果可以解析---就把经纬度以double填充进去
                double temp;
                if (double.TryParse(dataList[i].Longitude, out temp))
                {
                    row.CreateCell(1).SetCellValue(Double.Parse((dataList[i].Longitude)));
                }
                else
                {
                    row.CreateCell(1).SetCellValue((dataList[i].Longitude));
                }
                if (double.TryParse(dataList[i].Latitude, out temp))
                {
                    row.CreateCell(2).SetCellValue(double.Parse(dataList[i].Latitude));
                }
                else
                {
                    row.CreateCell(2).SetCellValue(dataList[i].Latitude);
                }
            }
            try
            {
                //获取目标文件流--写入文件shuju
                workbook.Write(fileStream);
                fileStream.Close();
            }
            catch (Exception e)
            {
                Console.Error.WriteLine(e.StackTrace);
                MessageBox.Show("文件导出失败", "失败");
                return;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            MessageBox.Show("Excel文件导出成功", "完成");
        }
    }
}