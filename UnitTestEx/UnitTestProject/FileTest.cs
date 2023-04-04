using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;

namespace UnitTestProject
{
    [TestClass]
    public class FileTest //Класс FileTest содержит тесты для получения размера файла - GetSizeTest,получение имени -GetFilenameTest.
    {
        public string filename = "File1";
        public const string SIZE_EXCEPTION = "Wrong size";
        public const string NAME_EXCEPTION = "Wrong name";
        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public double lenght;

        /* ПРОВАЙДЕР */
        static object[] FilesData =
        {
            new object[] {new File(FILE_PATH_STRING, CONTENT_STRING), FILE_PATH_STRING, CONTENT_STRING},
            new object[] { new File(SPACE_STRING, SPACE_STRING), SPACE_STRING, SPACE_STRING}
        };
   
        /* Тестируем получение размера */
        
        [TestMethod]
        public void GetSizeTest()
        {
          
            string content = CONTENT_STRING;
            lenght = content.Length / 2;
            Assert.AreEqual(lenght,content.Length/2);
            
        }
        
        /* Тестируем получение имени */
        //  [Test, TestCaseSource(nameof(FilesData))]
        [TestMethod]
        public void GetFilenameTest()
        {
            String name="File1";
            Assert.AreEqual(GetFilename(), name, NAME_EXCEPTION);
        }
        public string GetFilename()
        {
            return filename;
        }
        [TestMethod]                           //Дополнительный тест №1. Проверка на название файла,состоящего из символов.Если имя файла содержит перечисленные символы,то выводится NAME_EXCEPTION,а если нет,то все хорошо.
        public void GetFilenameTestIfSymbols()
        {
            GetFilename();
            bool haveSymbolsInNameFile = false;
            if (GetFilename().Contains("!,/,*,@,$,%,^,>,&"))
            {
                haveSymbolsInNameFile = true;
                Assert.IsTrue(haveSymbolsInNameFile, NAME_EXCEPTION);
            }
            else
            {
                Assert.AreEqual(GetFilename(), "File1");
            }
           
        }
    }
}
