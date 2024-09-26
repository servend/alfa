using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace alfa
{
    internal class Excel
    {

        public static void Excelobj(List<Organization> organizations)
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
            // Путь к файлу Excel
            string filePath = "C:/Users/User/Desktop/Лиды/ноябрь/База.xlsx";

            // Создаем список объектов Organization для хранения данных
            

            // Открываем файл Excel
            using (ExcelPackage package = new ExcelPackage(new FileInfo(filePath)))
            {
                // Получаем первый лист
                ExcelWorksheet worksheet = package.Workbook.Worksheets[0];

                // Итерируемся по строкам, начиная со второй (первая - заголовки)
                for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                {
                    // Получаем значение ИНН

                    // Проверяем, есть ли во втором столбце значение
                    if (worksheet.Cells[row, 2].Value != null && worksheet.Cells[row, 1].Value != null)
                    {
                        string inn = worksheet.Cells[row, 1].Value.ToString();
                        // Получаем значение номера телефона
                        string number = worksheet.Cells[row, 2].Value.ToString();
                        number = Regex.Replace(number, "[^0-9]", "");
                        
                        Organization organization = new Organization(inn, number," ", " ");
                        organizations.Add(organization);
                    }
                }
            }   
        }
        public static void INN_folder_create()
        {
            string folderPath = @"C:\INN_Output";

            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
                Console.WriteLine("Папка INN_Output успешно создана.");
            }
            else
            {
                Console.WriteLine("Папка INN_Output уже существует на диске C.");
            }
        }
        public static void INN_file_create()
        {
            string folderPath = @"C:\INN_Output\ИНН.xlsx";

            if (!Directory.Exists(folderPath))
            {
                ExcelPackage excel = new ExcelPackage();
                ExcelWorksheet worksheet = excel.Workbook.Worksheets.Add("ИНН");
                Console.WriteLine("Файл ИНН успешно создан.");
                FileInfo excelFile = new FileInfo(folderPath);
                excel.SaveAs(excelFile);
            }
            else
            {
                Console.WriteLine("Файл ИНН уже существует на диске C.");
            }
        }
        public static void INN_file_addobj(List<Organization> organizations)
        {
            Console.WriteLine("Идет выгрузка в эксель не выключайте приложение");
            string folderPath = @"C:\INN_Output\ИНН.xlsx";
            using (ExcelPackage excelPackage = new ExcelPackage(new FileInfo(folderPath)))
            {
                ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[0];
                foreach(Organization organization in organizations)
                {
                    worksheet.Cells["A1"].LoadFromCollection(organizations.Select(obj => new { ((dynamic)obj).Name, ((dynamic)obj).Number, ((dynamic)obj).INN }), true);
                    worksheet.Cells["A1"].Value = "Организация";
                    worksheet.Cells["B1"].Value = "Телефон";
                    worksheet.Cells["C1"].Value = "ИНН";
                    FileInfo excelFile = new FileInfo(folderPath);
                    excelPackage.SaveAs(excelFile);
                    
                }
                Console.WriteLine("Успешная выгрузка");
            }
        }
    }
    
}
