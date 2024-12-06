using AvtoService_3cursAA.Model;
using Microsoft.Win32;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AvtoService_3cursAA.PagesMenuAdmin.DataManagers
{
    public static class FilesManager
    {
        // Чек деталей Excel
        public static string ExcelDetails(Employee employee, Client client, Car car, Typeofrepair typeofrepair, 
            List<(int Count, string Name, int Cost)> detailsList, SaveFileDialog saveFileDialog, int idOrder, int costForClient, int costFinal)
        {
            string _filePath = "";
            // Устанавливаем контекст лицензии
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Создаем новый Excel документ
            using (var excelDocument = new ExcelPackage())
            {
                // Создаем страницу и называем ее
                var worksheet = excelDocument.Workbook.Worksheets.Add($"Чек для деталей (номер {idOrder})");

                #region Создание чека

                // Заголовок чека
                worksheet.Cells[1, 1].Value = $"Чек {idOrder}";
                worksheet.Cells[2, 1].Value = "Детали";
                worksheet.Cells[1, 1, 1, 3].Merge = true;
                worksheet.Cells[2, 1, 2, 3].Merge = true;
                worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[2, 1, 2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[3, 1].Value = "Наименование";
                worksheet.Cells[3, 2].Value = "Количество (шт.)";
                worksheet.Cells[3, 3].Value = "Цена (руб.)";
                worksheet.Cells[1, 1, 3, 3].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 3, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Заполнение элементов
                int row = 4; 
                foreach (var (count, name, cost) in detailsList)
                {
                    worksheet.Cells[row, 1].Value = name; 
                    worksheet.Cells[row, 2].Value = count; 
                    worksheet.Cells[row, 3].Value = cost;
                    row++;
                }
                worksheet.Cells[4, 1, row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                // Вывод итоговой цены
                worksheet.Cells[row, 2].Value = "Итого к оплате (руб.)";
                int costDetails = typeofrepair.Name == "Гарантийный случай"
                    ? costForClient
                    : costFinal;
                worksheet.Cells[row, 3].Value = costDetails.ToString();

                worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                worksheet.Cells[row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[row, 2, row, 3].Style.Font.Bold = true;
                row += 2;

                // Вывод данных
                int rowInfoEmployeeStart = row;
                worksheet.Cells[row++, 1].Value = $"Администратор: {employee.FullName}";
                worksheet.Cells[row++, 1].Value = $"Тип ремонта: {typeofrepair.Name}";
                worksheet.Cells[row++, 1].Value = $"Дата оформления: {DateTime.Now}";
                row++;

                int rowInfoClientStart = row;
                worksheet.Cells[row++, 1].Value = $"Клиент: {client.FullName}";
                worksheet.Cells[row++, 1].Value = $"Обслуживаемый автомобиль: {car.Title}";
                row++;

                worksheet.Cells[row, 1].Value = $"Спасибо за покупку! Приходите еще";
                worksheet.Cells[row, 1, row, 3].Merge = true;
                worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;
                worksheet.Cells[row, 1, row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                #endregion

                string filePath = @$"{saveFileDialog.FileName}.xlsx"; // чтобы путь был правильным
                excelDocument.SaveAs(filePath); // сохраняем по пути

                _filePath = filePath;
            }
            return _filePath;
        }

        // Чек услуг Excel
        public static string ExcelPrices(Employee employee, Client client, Car car, Typeofrepair typeofrepair,
    List<Price> pricesList, SaveFileDialog saveFileDialog, int idOrder, int costForClient, int costFinal)
        {
            string _filePath = "";
            // Устанавливаем контекст лицензии
            ExcelPackage.LicenseContext = OfficeOpenXml.LicenseContext.NonCommercial;

            // Создаем новый Excel документ
            using (var excelDocument = new ExcelPackage())
            {
                // Создаем страницу и называем ее
                var worksheet = excelDocument.Workbook.Worksheets.Add($"Чек об оказании (номер {idOrder})");

                #region Создание чека

                // Заголовок чека
                worksheet.Cells[1, 1].Value = $"Чек {idOrder}";
                worksheet.Cells[2, 1].Value = "Услуги";
                worksheet.Cells[1, 1, 1, 2].Merge = true;
                worksheet.Cells[2, 1, 2, 2].Merge = true;
                worksheet.Cells[1, 1, 1, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;
                worksheet.Cells[2, 1, 2, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                worksheet.Cells[3, 1].Value = "Наименование";
                worksheet.Cells[3, 2].Value = "Цена (руб.)";
                worksheet.Cells[1, 1, 3, 3].Style.Font.Bold = true;
                worksheet.Cells[1, 1, 3, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                // Заполнение элементов
                int row = 4;
                foreach (var price in pricesList)
                {
                    worksheet.Cells[row, 1].Value = price.Name;
                    worksheet.Cells[row, 2].Value = price.Cost;
                    row++;
                }
                worksheet.Cells[4, 1, row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;

                // Вывод итоговой цены
                worksheet.Cells[row, 1].Value = "Итого к оплате (руб.)";
                int costDetails = typeofrepair.Name == "Гарантийный случай"
                    ? costForClient
                    : costFinal;
                worksheet.Cells[row, 2].Value = costDetails.ToString();

                worksheet.Cells[row, 1].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Right;
                worksheet.Cells[row, 2].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Left;
                worksheet.Cells[row, 1, row, 2].Style.Font.Bold = true;
                row += 2;

                // Вывод данных
                int rowInfoEmployeeStart = row;
                worksheet.Cells[row++, 1].Value = $"Администратор: {employee.FullName}";
                worksheet.Cells[row++, 1].Value = $"Тип ремонта: {typeofrepair.Name}";
                worksheet.Cells[row++, 1].Value = $"Дата оформления: {DateTime.Now}";
                row++;

                int rowInfoClientStart = row;
                worksheet.Cells[row++, 1].Value = $"Клиент: {client.FullName}";
                worksheet.Cells[row++, 1].Value = $"Обслуживаемый автомобиль: {car.Title}";
                row++;

                worksheet.Cells[row, 1].Value = $"Спасибо за покупку! Приходите еще";
                worksheet.Cells[row, 1, row, 2].Merge = true;
                worksheet.Cells[row, 1, row, 3].Style.Font.Bold = true;
                worksheet.Cells[row, 1, row, 3].Style.HorizontalAlignment = OfficeOpenXml.Style.ExcelHorizontalAlignment.Center;

                #endregion

                string filePath = @$"{saveFileDialog.FileName}.xlsx"; // чтобы путь был правильным
                excelDocument.SaveAs(filePath); // сохраняем по пути

                _filePath = filePath;
            }
            return _filePath;
        }
    }
}
