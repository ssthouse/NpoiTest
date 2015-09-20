using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NpoiTest.Model;
using NPOI.XWPF.UserModel;

namespace NpoiTest.Office.Word
{
    /// <summary>
    /// Word操作的单例类
    /// </summary>
    class WordTest
    {
        
        
        /// <summary>
        /// 构造方法
        /// </summary>
        private WordTest()
        {
        }

        //唯一的单例
        private static WordTest mWordTest;

        public static WordTest GetInstance()
        {
            if (mWordTest == null)
            {
                mWordTest = new WordTest();
            }
            return mWordTest;
        }

        /// <summary>
        /// Word文件文字替换操作
        /// </summary>
        public void ReplaceText()
        {
            var docx = new XWPFDocument(new FileStream("F:/template.docx", FileMode.Open));

        }

        /// <summary>
        /// 测试Word文件读取
        /// </summary>
        public void ReadWord()
        {
            var docx = new XWPFDocument(new FileStream("F:/template.docx", FileMode.Open));
            foreach (var paragraph in docx.Paragraphs)
            {
//                foreach ( var run in paragraph.Runs)
//                {
//                    Console.WriteLine(run.Text);
//                }
                Console.WriteLine(paragraph.Text);
            }
        }

    }
}
