using System;
using System.IO;
using System.Runtime.ConstrainedExecution;
using NPOI.XWPF.UserModel;

namespace NpoiTest.Office.Word
{
    /// <summary>
    ///     Word操作的单例类
    /// </summary>
    internal class WordTest
    {
        /// <summary>
        ///     唯一的单例
        /// </summary>
        private static WordTest mWordTest;

        /// <summary>
        ///     构造方法
        /// </summary>
        private WordTest()
        {
        }

        /// <summary>
        ///     获取唯一的单例
        /// </summary>
        /// <returns></returns>
        public static WordTest GetInstance()
        {
            if (mWordTest == null)
            {
                mWordTest = new WordTest();
            }
            return mWordTest;
        }

        /// <summary>
        ///     Word文件文字替换操作
        /// </summary>
        public void ReplaceText()
        {
            // var docx = new XWPFDocument(new FileStream("F:/template.docx", FileMode.Open));
        }

        /// <summary>
        ///     测试Word文件读取
        /// </summary>
        public void ReadWord()
        {
            //TODO----这里是测试文档
            var fileStream = File.OpenRead("F:\\template.docx");
            var docx = new XWPFDocument(fileStream);
            //            foreach (var paragraph in docx.Paragraphs)
            //            {
            //                foreach ( var run in paragraph.Runs)
            //                {
            //                    Console.WriteLine(run.Text);
            //                }
            //                Console.WriteLine(paragraph.Text);
            //            }
            foreach (var para in docx.Paragraphs)
            {
                string text = para.ParagraphText; //获得文本
                var runs = para.Runs;
                string styleid = para.Style;

                for (var i = 0; i < runs.Count; i++)
                {
                    var run = runs[i];
                    text = run.ToString(); //获得run的文本
                    Console.WriteLine(text);
                }
            }
        }
    }
}