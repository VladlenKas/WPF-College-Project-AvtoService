using AvtoService_3cursAA.Model;
using Microsoft.Win32;
using OfficeOpenXml;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Xceed.Document.NET;
using Xceed.Words.NET;

namespace AvtoService_3cursAA.PagesMenuAdmin.DataManagers
{
    public static class FilesManager
    {
        // Excel услуги
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

        // Excel детали
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

        // Word услугм
        public static string WordPrices(Employee employee, Client client, Car car, Typeofrepair typeofrepair,
            List<Price> pricesList, SaveFileDialog saveFileDialog, int idOrder, int costForClient, int costFinal)
        {
            string _filePath = "";

            // Создаем новый документ Word
            using (var document = DocX.Create($"{saveFileDialog.FileName}.docx"))
            {
                // Указываем формат заголовка
                Formatting titleFormat = new Formatting() 
                { 
                    Bold = true, 
                    Size = 16
                };
                // Заголовок документа
                document.InsertParagraph($"Чек по предоставлению услуг ({idOrder})",
                    false, titleFormat).Alignment = Alignment.center; // Выравниваем по центру

                // Создаем таблицу
                var table = document.InsertTable(pricesList.Count + 1, 2);
                table.AutoFit = AutoFit.Contents;

                // Заголовки таблицы
                table.Rows[0].Cells[0].Paragraphs[0].Append("Наименование").Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[1].Paragraphs[0].Append("Цена (руб.)").Bold().Alignment = Alignment.center;

                // Заполнение элементов таблицы
                for (int i = 0; i < pricesList.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(pricesList[i].Name);
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(pricesList[i].Cost.ToString());
                }

                // Выравниваем таблицу по центру
                table.Alignment = Alignment.center;

                // Вывод итоговой цены
                document.InsertParagraph(); // Пустая строка перед итогом
                if (typeofrepair.Name == "Гарантийный случай")
                {
                    document.InsertParagraph($"Итого к оплате: {costFinal} руб.", false, new Formatting() { Bold = true });
                    document.InsertParagraph($"Скидка 20%", false, new Formatting() { Bold = true });
                    document.InsertParagraph($"Итого: {costForClient} руб.", false, new Formatting() { Bold = true });

                }
                else
                {
                    document.InsertParagraph($"Итого к оплате: {costFinal} руб.", false, new Formatting() { Bold = true }); 
                }

                // Вывод данных о сотруднике и клиенте
                document.InsertParagraph(); // Пустая строка перед информацией
                document.InsertParagraph($"Администратор: {employee.FullName}");
                document.InsertParagraph($"Тип ремонта: {typeofrepair.Name}");
                document.InsertParagraph($"Дата оформления: {DateTime.Now}");

                document.InsertParagraph(); // Пустая строка перед информацией о клиенте
                document.InsertParagraph($"Клиент: {client.FullName}");
                document.InsertParagraph($"Обслуживаемый автомобиль: {car.Title}");

                // Завершение документа
                document.InsertParagraph(); // Пустая строка перед сообщением благодарности
                document.InsertParagraph("Спасибо за покупку! Приходите еще", false, new Formatting() { Bold = true })
                    .Alignment = Alignment.center;

                // Сохранение документа
                _filePath = $"{saveFileDialog.FileName}.docx";
                document.Save();
            }

            return _filePath;
        }

        // Word детвлм
        public static string WordDetails(Employee employee, Client client, Car car, Typeofrepair typeofrepair,
            List<(int Count, string Name, int Cost)> detailsList, SaveFileDialog saveFileDialog, int idOrder, int costForClient, int costFinal)
        {
            string _filePath = "";

            // Создаем новый документ Word
            using (var document = DocX.Create($"{saveFileDialog.FileName}.docx"))
            {
                // Указываем формат заголовка
                Formatting titleFormat = new Formatting()
                {
                    Bold = true,
                    Size = 16
                };

                // Заголовок документа
                document.InsertParagraph($"Чек по проданным деталям ({idOrder})", false, titleFormat)
                    .Alignment = Alignment.center; // Выравниваем по центру

                // Создаем таблицу
                var table = document.InsertTable(detailsList.Count + 1, 3);
                table.AutoFit = AutoFit.Contents;

                // Заголовки таблицы
                table.Rows[0].Cells[0].Paragraphs[0].Append("Наименование").Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[1].Paragraphs[0].Append("Количество").Bold().Alignment = Alignment.center;
                table.Rows[0].Cells[2].Paragraphs[0].Append("Цена (руб.)").Bold().Alignment = Alignment.center;

                // Заполнение элементов таблицы
                for (int i = 0; i < detailsList.Count; i++)
                {
                    table.Rows[i + 1].Cells[0].Paragraphs[0].Append(detailsList[i].Name);
                    table.Rows[i + 1].Cells[1].Paragraphs[0].Append(detailsList[i].Count.ToString());
                    table.Rows[i + 1].Cells[2].Paragraphs[0].Append(detailsList[i].Cost.ToString());
                }

                // Выравниваем таблицу по центру
                table.Alignment = Alignment.center;

                // Вывод итоговой цены
                document.InsertParagraph(); // Пустая строка перед итогом
                if (typeofrepair.Name == "Гарантийный случай")
                {
                    document.InsertParagraph($"Итого к оплате: {costFinal} руб.", false, new Formatting() { Bold = true });
                    document.InsertParagraph($"Скидка 20%", false, new Formatting() { Bold = true });
                    document.InsertParagraph($"Итого: {costForClient} руб.", false, new Formatting() { Bold = true });
                }
                else
                {
                    document.InsertParagraph($"Итого к оплате: {costFinal} руб.", false, new Formatting() { Bold = true });
                }

                // Вывод данных о сотруднике и клиенте
                document.InsertParagraph(); // Пустая строка перед информацией
                document.InsertParagraph($"Администратор: {employee.FullName}");
                document.InsertParagraph($"Тип ремонта: {typeofrepair.Name}");
                document.InsertParagraph($"Дата оформления: {DateTime.Now}");

                document.InsertParagraph(); // Пустая строка перед информацией о клиенте
                document.InsertParagraph($"Клиент: {client.FullName}");
                document.InsertParagraph($"Обслуживаемый автомобиль: {car.Title}");

                // Завершение документа
                document.InsertParagraph(); // Пустая строка перед сообщением благодарности
                document.InsertParagraph("Спасибо за покупку! Приходите еще", false, new Formatting() { Bold = true })
                    .Alignment = Alignment.center;

                // Сохранение документа
                _filePath = $"{saveFileDialog.FileName}.docx";
                document.Save();
            }

            return _filePath;
        }

        // Pdf услуги
        public static string PdfPrices(Employee employee, Client client, Car car, Typeofrepair typeofrepair,
            List<Price> pricesList, SaveFileDialog saveFileDialog, int idOrder, int costForClient, int costFinal)
        {
            string _filePath = $"{saveFileDialog.FileName}.pdf";

            // Создаем новый PDF документ
            using (PdfDocument document = new PdfDocument())
            {
                // Устанавливаем заголовок документа
                document.Info.Title = $"Чек по предоставлению услуг ({idOrder})";

                // Создаем страницу
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
                XFont regularFont = new XFont("Arial", 12, XFontStyleEx.Regular);
                XFont regularFontBold = new XFont("Arial", 12, XFontStyleEx.Bold);

                // Выравнивание по центру
                double titleX = (page.Width - gfx.MeasureString(document.Info.Title, titleFont).Width) / 2;
                gfx.DrawString(document.Info.Title, titleFont, XBrushes.Black, titleX, 50);

                // Рисуем таблицу
                double tableY = 100;
                double cellHeight = 20;

                // Заголовки таблицы
                gfx.DrawString("Наименование", regularFontBold, XBrushes.Black, 50, tableY + 5);
                gfx.DrawString("Цена (руб.)", regularFontBold, XBrushes.Black, 300, tableY + 5);

                tableY += cellHeight;

                // Заполнение элементов таблицы
                foreach (var price in pricesList)
                {
                    gfx.DrawRectangle(XBrushes.White, 50, tableY, 500, cellHeight);
                    gfx.DrawString(price.Name, regularFont, XBrushes.Black, 50, tableY + 5);
                    gfx.DrawString(price.Cost.ToString(), regularFont, XBrushes.Black, 300, tableY + 5);
                    tableY += cellHeight;
                }
                tableY += cellHeight;

                // Вывод итоговой цены
                gfx.DrawString($"Итого к оплате: {costFinal} руб.", regularFontBold, XBrushes.Black, 50, tableY);

                if (typeofrepair.Name == "Гарантийный случай")
                {
                    tableY += cellHeight;
                    gfx.DrawString($"Скидка 20%", regularFontBold, XBrushes.Black, 50, tableY);
                    tableY += cellHeight;
                    gfx.DrawString($"Итого: {costForClient} руб.", regularFontBold, XBrushes.Black, 50, tableY);
                }

                // Вывод данных о сотруднике и клиенте
                tableY += cellHeight * 2; // Пустая строка перед информацией
                gfx.DrawString($"Администратор: {employee.FullName}", regularFont, XBrushes.Black, 50, tableY);
                tableY += cellHeight;
                gfx.DrawString($"Тип ремонта: {typeofrepair.Name}", regularFont, XBrushes.Black, 50, tableY);
                tableY += cellHeight;
                gfx.DrawString($"Дата оформления: {DateTime.Now}", regularFont, XBrushes.Black, 50, tableY);

                // Информация о клиенте
                tableY += cellHeight * 2; // Пустая строка перед информацией о клиенте
                gfx.DrawString($"Клиент: {client.FullName}", regularFont, XBrushes.Black, 50, tableY);
                tableY += cellHeight;
                gfx.DrawString($"Обслуживаемый автомобиль: {car.Title}", regularFont, XBrushes.Black, 50, tableY);

                // Завершение документа с сообщением благодарности
                tableY += cellHeight * 2; // Пустая строка перед сообщением благодарности
                gfx.DrawString("Спасибо за покупку! Приходите еще", titleFont,
                    XBrushes.Black,
                    (page.Width - gfx.MeasureString("Спасибо за покупку! Приходите еще", titleFont).Width) / 2,
                    tableY);

                // Сохранение документа
                document.Save(_filePath);
            }

            return _filePath;
        }

        // Pdf детали
        public static string PdfDetails(Employee employee, Client client, Car car, Typeofrepair typeofrepair,
            List<(int Count, string Name, int Cost)> detailsList, SaveFileDialog saveFileDialog, int idOrder, int costForClient, int costFinal)
        {
            string _filePath = $"{saveFileDialog.FileName}.pdf";

            // Создаем новый PDF документ
            using (PdfDocument document = new PdfDocument())
            {
                // Устанавливаем заголовок документа
                document.Info.Title = $"Чек по предоставлению услуг ({idOrder})";

                // Создаем страницу
                PdfPage page = document.AddPage();
                XGraphics gfx = XGraphics.FromPdfPage(page);
                XFont titleFont = new XFont("Arial", 16, XFontStyleEx.Bold);
                XFont regularFont = new XFont("Arial", 12, XFontStyleEx.Regular);
                XFont regularFontBold = new XFont("Arial", 12, XFontStyleEx.Bold);

                // Выравнивание по центру
                double titleX = (page.Width - gfx.MeasureString(document.Info.Title, titleFont).Width) / 2;
                gfx.DrawString(document.Info.Title, titleFont, XBrushes.Black, titleX, 50);

                // Рисуем таблицу
                double tableY = 100;
                double cellHeight = 20;

                // Заголовки таблицы
                gfx.DrawString("Наименование", regularFontBold, XBrushes.Black, 50, tableY + 5);
                gfx.DrawString("Количество (шт.)", regularFontBold, XBrushes.Black, 250, tableY + 5);
                gfx.DrawString("Цена (руб.)", regularFontBold, XBrushes.Black, 380, tableY + 5);

                tableY += cellHeight;

                // Заполнение элементов таблицы
                foreach (var detail in detailsList)
                {
                    gfx.DrawRectangle(XBrushes.White, 50, tableY, 500, cellHeight);
                    gfx.DrawString(detail.Name, regularFont, XBrushes.Black, 50, tableY + 5);
                    gfx.DrawString(detail.Count.ToString(), regularFont, XBrushes.Black, 250, tableY + 5);
                    gfx.DrawString(detail.Cost.ToString(), regularFont, XBrushes.Black, 380, tableY + 5);
                    tableY += cellHeight;
                }
                tableY += cellHeight;

                // Вывод итоговой цены
                gfx.DrawString($"Итого к оплате: {costFinal} руб.", regularFontBold, XBrushes.Black, 50, tableY);

                if (typeofrepair.Name == "Гарантийный случай")
                {
                    tableY += cellHeight;
                    gfx.DrawString($"Скидка 20%", regularFontBold, XBrushes.Black, 50, tableY);
                    tableY += cellHeight;
                    gfx.DrawString($"Итого: {costForClient} руб.", regularFontBold, XBrushes.Black, 50, tableY);
                }

                // Вывод данных о сотруднике и клиенте
                tableY += cellHeight * 2; // Пустая строка перед информацией
                gfx.DrawString($"Администратор: {employee.FullName}", regularFont, XBrushes.Black, 50, tableY);
                tableY += cellHeight;
                gfx.DrawString($"Тип ремонта: {typeofrepair.Name}", regularFont, XBrushes.Black, 50, tableY);
                tableY += cellHeight;
                gfx.DrawString($"Дата оформления: {DateTime.Now}", regularFont, XBrushes.Black, 50, tableY);

                // Информация о клиенте
                tableY += cellHeight * 2; // Пустая строка перед информацией о клиенте
                gfx.DrawString($"Клиент: {client.FullName}", regularFont, XBrushes.Black, 50, tableY);
                tableY += cellHeight;
                gfx.DrawString($"Обслуживаемый автомобиль: {car.Title}", regularFont, XBrushes.Black, 50, tableY);

                // Завершение документа с сообщением благодарности
                tableY += cellHeight * 2; // Пустая строка перед сообщением благодарности
                gfx.DrawString("Спасибо за покупку! Приходите еще", titleFont,
                    XBrushes.Black,
                    (page.Width - gfx.MeasureString("Спасибо за покупку! Приходите еще", titleFont).Width) / 2,
                    tableY);

                // Сохранение документа
                document.Save(_filePath);
            }

            return _filePath;
        }


    }
}
