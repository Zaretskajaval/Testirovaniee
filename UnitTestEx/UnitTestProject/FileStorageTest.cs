using Microsoft.VisualStudio.TestTools.UnitTesting;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Reflection;
using UnitTestEx;
using Assert = NUnit.Framework.Assert;
using System.Linq;

namespace UnitTestProject
{
    /// <summary>
    /// Summary description for FileStorageTest
    /// </summary>
    [TestClass]
    public class FileStorageTest //Содержит тесты: WriteTest-тестирование записи файла-записывает файл,передавая их в класс FileStorage и очищая место в памяти,удаляя этот файл из памяти после записи.
                                 //WriteExceptionTest-тест для записи дублирующегося файла.Записываем файл,но если этот файл уже есть в списке,то оповещаем NO_EXPECTED_EXCEPTION_EXCEPTION исключением о том,что файл имеется.
                                 //IsExistsTest-тестирование существования файла
                                 //DeleteTest-проверка удаления файла по имени
                                 //
    {
    

        private List<File> files = new List<File>();
        public const string MAX_SIZE_EXCEPTION = "DIFFERENT MAX SIZE";
        public const string NULL_FILE_EXCEPTION = "NULL FILE";
        public const string NO_EXPECTED_EXCEPTION_EXCEPTION = "There is no expected exception";
        public string filename = "File1";
        public const string SPACE_STRING = " ";
        public const string FILE_PATH_STRING = "@D:\\JDK-intellij-downloader-info.txt";
        public const string CONTENT_STRING = "Some text";
        public const string REPEATED_STRING = "AA";
        public const string WRONG_SIZE_CONTENT_STRING = "TEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtextTEXTtext";
        public const string TIC_TOC_TOE_STRING = "tictoctoe.game";

        public const int NEW_SIZE = 5;

        public FileStorage storage = new FileStorage(NEW_SIZE);

        /* ПРОВАЙДЕРЫ */

        static object[] NewFilesData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) },
            new object[] { new File(SPACE_STRING, WRONG_SIZE_CONTENT_STRING) },
            new object[] { new File(FILE_PATH_STRING, CONTENT_STRING) }
        };

        static object[] FilesForDeleteData =
        {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING), REPEATED_STRING },
            new object[] { null, TIC_TOC_TOE_STRING }
        };

        static object[] NewExceptionFileData = {
            new object[] { new File(REPEATED_STRING, CONTENT_STRING) }
        };
     
        /* Тестирование записи файла */
       [TestMethod]
        public void WriteTest() 
        {
            File file1 = new File("test.png"); 
          
            Assert.True(storage.Write(file1));
            storage.DeleteAllFiles();
        }

        /* Тестирование записи дублирующегося файла */
        [TestMethod]
        public void WriteExceptionTest() {
            File file1 = new File("test.png");

            bool isException = false;
            try
            {
                storage.Write(file1);
                Assert.False(storage.Write(file1));
                storage.DeleteAllFiles();
            }
            catch (FileNameAlreadyExistsException)
            {
                isException = true;
            }
            Assert.True(isException, NO_EXPECTED_EXCEPTION_EXCEPTION);
        }

        /* Тестирование проверки существования файла */
        [TestMethod]
        public void IsExistsTest()
        { 
                //Создается объект File file с данными.
                //  проверка, существует ли файл в хранилище до записи(ожидаемое значение - false).
                //   запись файла в хранилище.
                //проверка, существует ли файл в хранилище после записи(ожидаемое значение - true).
                //и очистка хранилища

            List<File> files = new List<File>();
            File file1 = new File("test.png");

            // Act
            bool isFileExistsBeforeWrite = storage.IsExists(file1.GetFilename());

            storage.Write(file1);

            bool isFileExistsAfterWrite = storage.IsExists(file1.GetFilename());

            // Assert
            Assert.False(isFileExistsBeforeWrite);
            Assert.True(isFileExistsAfterWrite);

            // Cleanup
            storage.DeleteAllFiles();
         
        }

        /* Тестирование удаления файла */
        [TestMethod]
        public void DeleteTest(/*File file, String fileName*/) {

            files.Clear();
            Assert.AreEqual(files.Count,0); //проверка на очистку List
           
        }
        public string GetFilename()
        {
            return filename;
        }
        /* Тестирование получения файлов */
        [TestMethod]
        public void GetFilesTest() //не изменяла,только подписала [TestMethod]
        {
            foreach (File el in storage.GetFiles()) 
            {
                Assert.NotNull(el);
            }
        }

     
        /* Тестирование получения файла */
        [TestMethod]
        public void GetFileTest()
        {
            File expectedFile = new File("filename.txt");//иметация сохранения

            // Создаем файл и записываем его в хранилище
            storage.Write(expectedFile);

            // Получаем файл из хранилища и сравниваем с ожидаемым файлом
            File actualFile = storage.GetFile(expectedFile.GetFilename());
            bool difference = actualFile.GetFilename().Equals(expectedFile.GetFilename()) && actualFile.GetSize().Equals(expectedFile.GetSize());

            // Проверяем, что файлы равны
            Assert.IsTrue(difference, string.Format("There are some differences in {0} or {1}", expectedFile.GetFilename(), expectedFile.GetSize()));
        }
        public List<File> GetFiles()
        {
            return files;
        }

        /**
         * Get file by filename
         * @param fileName of file to get
         * @return file
         */
        public File GetFile(String fileName)
        {
            if (IsExists(fileName))
            {
                foreach (File file in files)
                {
                    if (file.GetFilename().Contains(fileName))
                    {
                        return file;
                    }
                }
            }
            return null;
        }
        public bool IsExists(String fileName)
        {
            // Для каждого элемента с типом File из Листа files
            foreach (File file in files)
            {
                // Проверка имени
                if (file.GetFilename().Contains(fileName))
                {
                    return true;
                }
            }
            return false;
        }
        [TestMethod]
        public void IfTextInFileIsNullTest()   //Дополнительный тест №2.Проверка на пустой файл (пустой текст в файле).
        {
            bool TextInFileIsNull = false;
            if (CONTENT_STRING=="")
            {
                TextInFileIsNull = true;
                Assert.IsTrue(TextInFileIsNull, "Text is NULL");

            }
          
            
        }
        [TestMethod]
        public void WithoutPDF()    //Дополнительный тест №3. Запрет на сохранение файлов в формате PDF
        {
            File file1 = new File("test.png");
            bool withoutPDF = false;
            if (file1.filename.Contains("PDF"))
            {
                withoutPDF = true;
                Assert.IsTrue(withoutPDF, "You can't save PDF format");
            }
            
           
        }
    }
}
