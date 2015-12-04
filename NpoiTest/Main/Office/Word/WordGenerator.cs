using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using NpoiTest.Model.Database;
using NPOI.OpenXmlFormats.Dml.Picture;
using NPOI.XWPF.UserModel;
using NPOI.OpenXmlFormats.Wordprocessing;
using ICSharpCode.SharpZipLib.Zip;
using System.Data.SQLite;
using DigitalMapToDB.DigitalMapParser.Utils;

namespace NpoiTest.Office.Word
{
    //传入数据---导出一份Word文件
    internal class WordGenerator
    {
        private const string TAG = "WordGenerator";

        /// <summary>
        /// 数据源
        /// </summary>
        private readonly List<DbBean> dataList;

        /// <summary>
        /// 禁止创建实例
        /// </summary>
        private WordGenerator()
        {
        }

        /// <summary>
        /// 生成一个工程的word文件
        /// </summary>
        /// <param name="datalist">当前工程的数据List</param>
        /// <param name="outputPath">word文件输出路径</param>
        public static void word_creat_one(List<DbBean> datalist, string outputPath)
        {
            XWPFDocument m_Docx = new XWPFDocument();
            word_init(m_Docx);
            //内容
            for (int i = 0; i < datalist.Count; i++)
            {
                word_inster_table(m_Docx, datalist[i], i + 1);
                //TODO 此处插入图片
                word_insert_picture(m_Docx, datalist[i].PrjName, datalist[i].PhotoPathName);
            }
            //输出
            FileStream sw = null;
            try
            {
                sw = File.Create(outputPath);
                if (m_Docx == null)
                {
                    Log.Err(TAG, "m_Docx is null");
                }
                if (sw == null)
                {
                    Log.Err(TAG, "sw is null");
                }
                m_Docx.Write(sw);
                sw.Close();
            }
            catch
            {
                Log.Err(TAG, "something is wrong");
            }
            finally
            {
                sw.Close();
            }
        }

        /// <summary>
        /// 生成模板文件
        /// </summary>
        /// <param name="m_Docx">根文档</param>
        private static void word_init(XWPFDocument m_Docx)
        {
            //1‘=1440twip=25.4mm=72pt(磅point)=96px(像素pixel)
            //1px(像素pixel)=0.75pt(磅point)
            // A4:W=11906 twip=8.269''=210mm,h=16838twip=11.693''=297mm
            //A5:W=8390 twip=5.827''=148mm,h=11906 twip=8.269''=210mm
            //A6:W=5953 twip=4.134''=105mm,h=8390twip=5.827''=1148mm
            //16k195mmX270mm:
            //16k184mmX260mm:
            //16k197mmX273mm:
            CT_SectPr m_SectPr = new CT_SectPr();
            //页面设置A4纵向
            m_SectPr.pgSz.w = (ulong) 11906;
            m_SectPr.pgSz.h = (ulong) 16838;
            m_Docx.Document.body.sectPr = m_SectPr;
            //第一页
            word_insert_space(4, m_Docx);
            word_insert_text(m_Docx, "宋体", 22, "【项目名称】");
            word_insert_text(m_Docx, "宋体", 22, "GSM-R 通信系统");
            word_insert_text(m_Docx, "宋体", 22, "现场勘查报告");

            word_insert_space(8, m_Docx);
            word_insert_text(m_Docx, "宋体", 22, "【日期】");

            word_insert_space(7, m_Docx);
            //第二页
            //表1
            XWPFTable table = m_Docx.CreateTable(4, 2);
            CT_Tbl ctbl = m_Docx.Document.body.GetTblArray()[0];
            CT_TblPr ctblpr = ctbl.AddNewTblPr();
            ctblpr.jc = new CT_Jc();
            ctblpr.jc.val = ST_Jc.center;
            table.Width = 4000;
            table.GetRow(0).GetCell(0).SetText("项目");
            table.GetRow(1).GetCell(0).SetText("日期");
            table.GetRow(2).GetCell(0).SetText("现场勘查人员");
            table.GetRow(3).GetCell(0).SetText("报告编制人员");
            CT_TcPr m_Pr = table.GetRow(0).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.tcW = new CT_TblWidth();
            m_Pr.tcW.w = "4000";
            m_Pr.tcW.type = ST_TblWidth.dxa; //设置单元格宽度
            word_insert_space(2, m_Docx);
            word_insert_text(m_Docx, "宋体", 12, "基站勘察表");
        }

        /*
         *插入空行函数
         *@n 插入的行数
         *@m_Docx 根文档
         *@longth 空行高
         */

        private static void word_insert_space(int n, XWPFDocument m_Docx, int longth = 250)
        {
            for (int i = 0; i < n; i++)
            {
                XWPFParagraph gp_space = m_Docx.CreateParagraph(); //创建XWPFParagraph
                gp_space.SetAlignment(ParagraphAlignment.CENTER);
                gp_space.SetSpacingBefore(longth);
                gp_space.SpacingAfter = longth;
            }
        }

        /// <summary>
        /// 插入文字函数
        /// </summary>
        /// <param name="m_Docx"></param>
        /// <param name="Font"></param>
        /// <param name="size"></param>
        /// <param name="text"></param>
        /// <param name="position"></param>
        private static void word_insert_text(XWPFDocument m_Docx, string Font, int size, string text,
            ParagraphAlignment position = ParagraphAlignment.CENTER)
        {
            XWPFParagraph gp = m_Docx.CreateParagraph(); //创建XWPFParagraph
            gp.SetAlignment(position);
            XWPFRun gr = gp.CreateRun();
            gr.SetFontFamily(Font);
            gr.SetFontSize(size);
            gr.SetText(text);
        }

        /// <summary>
        /// word 插入表格功能（13行2列）
        /// </summary>
        /// <param name="m_Docx">根文档</param>
        /// <param name="device_type">设备类型</param>
        /// <param name="kilometer_mark">公里标</param>
        /// <param name="side_direction">下行侧向</param>
        /// <param name="longitude">经度</param>
        /// <param name="latitude">纬度</param>
        private static void word_inster_table(XWPFDocument m_Docx, DbBean bean, int i = 1)
        {
            XWPFTable table = m_Docx.CreateTable(12, 2);
            CT_Tbl ctbl = m_Docx.Document.body.GetTblArray()[i];
            CT_TblPr ctblpr = ctbl.AddNewTblPr();
            ctblpr.jc = new CT_Jc();
            ctblpr.jc.val = ST_Jc.center;

            table.Width = 3500;
            table.GetRow(0).GetCell(0).SetText("设备类型");
            table.GetRow(0).GetCell(1).SetText(bean.DeviceType);
            table.GetRow(1).GetCell(0).SetText("公里标");
            table.GetRow(1).GetCell(1).SetText(bean.KilometerMark);
            table.GetRow(2).GetCell(0).SetText("下行侧向");
            table.GetRow(2).GetCell(1).SetText(bean.SideDirection);
            table.GetRow(3).GetCell(0).SetText("距线路中心距离（m）");
            table.GetRow(4).GetCell(0).SetText("经度");
            table.GetRow(4).GetCell(1).SetText(bean.Longitude);
            table.GetRow(5).GetCell(0).SetText("纬度");
            table.GetRow(5).GetCell(1).SetText(bean.Latitude);
            table.GetRow(6).GetCell(0).SetText("杆塔类型");
            table.GetRow(6).GetCell(1).SetText(bean.TowerType);
            table.GetRow(7).GetCell(0).SetText("杆塔高度");
            table.GetRow(7).GetCell(1).SetText(bean.TowerHeight);
            table.GetRow(8).GetCell(0).SetText("天线1方向角");
            table.GetRow(8).GetCell(1).SetText(bean.AntennaDirection1);
            table.GetRow(9).GetCell(0).SetText("天线2方向角");
            table.GetRow(9).GetCell(1).SetText(bean.AntennaDirection2);
            table.GetRow(10).GetCell(0).SetText("天线3方向角");
            table.GetRow(10).GetCell(1).SetText(bean.AntennaDirection3);
            table.GetRow(11).GetCell(0).SetText("天线4方向角");
            table.GetRow(11).GetCell(1).SetText(bean.AntennaDirection4);
            CT_TcPr m_Pr = table.GetRow(2).GetCell(1).GetCTTc().AddNewTcPr();
            m_Pr.tcW = new CT_TblWidth();
            m_Pr.tcW.w = "3500";
            m_Pr.tcW.type = ST_TblWidth.dxa; //设置单元格宽度

            XWPFTableRow m_Row = table.InsertNewTableRow(0);
            XWPFTableCell cell = m_Row.CreateCell();
            CT_Tc cttc = cell.GetCTTc();
            CT_TcPr ctPr = cttc.AddNewTcPr();
            ctPr.gridSpan = new CT_DecimalNumber();
            ctPr.gridSpan.val = "2";
            cttc.GetPList()[0].AddNewR().AddNewT().Value = "SITE 【序号】";

            word_insert_space(1, m_Docx, 100);
            word_insert_text(m_Docx, "宋体", 11, "SITE 【序号】勘站照片");
            word_insert_text(m_Docx, "宋体", 11, "（3-10张照片）");

            word_insert_space(1, m_Docx, 100);
        }

        /// <summary>
        /// word文档插入图片
        /// </summary>
        private static void word_insert_picture(XWPFDocument m_Docx, string prjName, string photoPathName)
        {
            System.IO.DirectoryInfo dir = new System.IO.DirectoryInfo(".\\Picture\\" + prjName + "\\" + photoPathName);
            if (dir.Exists == false)
            {
                Log.Err(TAG, "路径不存在");
                return;
            }
            System.IO.FileInfo[] files = dir.GetFiles(); // 获取所有文件信息。。
            foreach (System.IO.FileInfo file in files)
            {
                FileStream gfs = new FileStream(".\\Picture\\" + prjName + "\\" + photoPathName + "\\" + file.Name,
                    FileMode.Open, FileAccess.Read);
                XWPFParagraph gp = m_Docx.CreateParagraph(); //创建XWPFParagraph
                gp.SetAlignment(ParagraphAlignment.CENTER);
                XWPFRun gr = gp.CreateRun();
                //添加图片（注意设置图片的大小） 默认最小的一边为1000000
                System.Drawing.Image image = System.Drawing.Image.FromFile(".\\Picture\\" + prjName + "\\" + photoPathName + "\\" + file.Name);
                int height, width;
                if (image.Height > image.Width)
                {
                    height = 3000000;
                    width = 2000000;
                }
                else
                {
                    height = 2000000;
                    width = 3000000;
                }
                gr.AddPicture(gfs, (int) NPOI.XWPF.UserModel.PictureType.JPEG, file.Name, width, height);
                gfs.Close();
            }
        }
    }
}