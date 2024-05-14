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
                        // Создаем объект Organization и добавляем его в список
                        Organization organization = new Organization(inn, number);
                        organizations.Add(organization);
                    }
                }
            }

            // Выводим список организаций
            
        }
    }
    
}
