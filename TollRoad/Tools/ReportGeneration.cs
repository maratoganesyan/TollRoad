using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TollRoad.Models;
using Word = Microsoft.Office.Interop.Word;
using Microsoft.Office.Interop.Word;
using TollRoad.Tools.ReportModels;
using Microsoft.Win32;
using System.IO;

namespace TollRoad.Tools
{
    internal class ReportGeneration
    {
        #region CheckpointReport
        public static async System.Threading.Tasks.Task DoACheckpointReportAsync(CheckpointReportModel Model)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Microsoft Word Document (*.docx)|*.docx";
            if (sv.ShowDialog() == true)
            {
                await System.Threading.Tasks.Task.Run(() => GenerateCheckpointReport(Model, sv.FileName));
                MessageBoxManager manager = new MessageBoxManager();
                manager.Show("Успешно","Генерация отчёта о пропускном пункте успешно завершена!");
            }

        }
        private static void GenerateCheckpointReport(CheckpointReportModel Model, string NewPath)
        {
            try
            {
                byte[] FileBytes = Properties.Resources.CheckpointReport;
                string TempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(TempFilePath, FileBytes);

                Word.Application App = new Word.Application();
                App.Visible = false;
                Word.Document document = App.Documents.Open(TempFilePath);

                ChangeWordsCheckpoint(Model, document);
                GenerateTableCheckpointReport(Model, document);

                document.SaveAs2(FileName: NewPath);
                document.Close();
                App.Quit();
            }
            catch (System.Exception ex)
            {
                MessageBoxManager manager = new MessageBoxManager();
                manager.Show("Ошибка генерации", "Отчёт не сгенерирован");
            }
        }
        private static void GenerateTableCheckpointReport(CheckpointReportModel Model, Word.Document document)
        {
            Table table = document.Tables[1];
            int CountOfOrders = Model.Trips.Count;
            for (int i = 1; i <= CountOfOrders; i++)
                table.Rows.Add();
            int index = 0;
            foreach (Row row in table.Rows)
            {
                if (row.Index == 1)
                    continue;
                foreach (Cell cell in row.Cells)
                {
                    if (cell.ColumnIndex == 1)
                    {
                        Checkpoint AdjacentPoint = Model.Trips[index].IdRouteNavigation.IdFirstCheckpointNavigation ==
                            Model.Checkpoint ?
                            Model.Trips[index].IdRouteNavigation.IdSecondCheckpointNavigation :
                            Model.Trips[index].IdRouteNavigation.IdFirstCheckpointNavigation;
                        cell.Range.Text = $"{AdjacentPoint.Address}";
                    }
                    else if (cell.ColumnIndex == 2)
                        cell.Range.Text = $"{Model.Trips[index].IdRouteNavigation.Fare:0.00}";
                    else if (cell.ColumnIndex == 3)
                        cell.Range.Text = Model.Trips[index].IdVehicleNavigation.StateNumber;
                    else if (cell.ColumnIndex == 4)
                        cell.Range.Text = $"{Model.Trips[index].StartDateTimeOfTrip:dd.MM.yyyy:HH.mm}";
                    else if (cell.ColumnIndex == 5)
                        cell.Range.Text = $"{Model.Trips[index].EndDateTimeOfTrip:dd.MM.yyyy:HH.mm}";
                    else if (cell.ColumnIndex == 6)
                        cell.Range.Text = $"{Model.Trips[index].TotalPriceOfTrip:0.00}";
                }
                index++;
            }
            //ToolsForGeneration.AutoSizeColumn(table, 4);
        }
        private static void ChangeWordsCheckpoint(CheckpointReportModel Model, Word.Document document)
        {
            ToolsForGeneration.ReplaceWordWithUnderline("{DateOfDrawingUp}", $"{DateTime.Now:dd.MM.yyyy}", document);
            ToolsForGeneration.ReplaceWordWithBold("{idCheckPoint}", Model.Checkpoint.Id.ToString() + "\n" + Model.Checkpoint.Address, document);
            ToolsForGeneration.ReplaceWord("{idCheckPoint2}", Model.Checkpoint.Id.ToString() + "\n" + Model.Checkpoint.Address, document);
            ToolsForGeneration.ReplaceWord("{CurrentDate}", $"{DateTime.Now:dd.MM.yyyy}", document);
            ToolsForGeneration.ReplaceWord("{StartDate}", $"{Model.StartGenerationDate:dd.MM.yyyy}", document);
            ToolsForGeneration.ReplaceWord("{EndDate}", $"{Model.EndGenerationDate:dd.MM.yyyy}", document);
            ToolsForGeneration.ReplaceWordWithBold("{TotalCost}", $"{Model.TotalCost:0.00} ₽", document);
        }

        #endregion

        #region Check
        public static async System.Threading.Tasks.Task DoCheckAsync(Trip Model)
        {
            SaveFileDialog sv = new SaveFileDialog();
            sv.Filter = "Microsoft Word Document (*.docx)|*.docx";
            if (sv.ShowDialog() == true)
            {
                await System.Threading.Tasks.Task.Run(() => GenerateCheck(Model, sv.FileName));
                MessageBoxManager manager = new MessageBoxManager();
                manager.Show("Успешно", "Генерация чекаы успешно завершена!");
            }

        }


        private static void GenerateCheck(Trip Model, string NewPath)
        {
            try
            {
                byte[] FileBytes = Properties.Resources.Check;
                string TempFilePath = Path.GetTempFileName();
                File.WriteAllBytes(TempFilePath, FileBytes);

                Word.Application App = new Word.Application();
                App.Visible = false;
                Word.Document document = App.Documents.Open(TempFilePath);

                ChangeWordsForCheck(Model, document);

                document.SaveAs2(FileName: NewPath);
                document.Close();
                App.Quit();
            }
            catch (System.Exception ex)
            {
                MessageBoxManager manager = new MessageBoxManager();
                manager.Show("Ошибка генерации", "Чек не сгенерирован");
            }
        }

        private static void ChangeWordsForCheck(Trip trip, Word.Document document)
        {
            ToolsForGeneration.ReplaceWordWithUnderline("{DateOfDrawingUp}", $"{DateTime.Now:dd.MM.yyyy}", document);
            ToolsForGeneration.ReplaceWord("{CheckpointAddress}", $"{Global.CurrentEmployee.IdCheckpointNavigation.Address}", document);
            ToolsForGeneration.ReplaceWord("{IdCheckpoint}", $"{Global.CurrentEmployee.IdCheckpoint}", document);
            ToolsForGeneration.ReplaceWord("{Manager}", $"{Global.CurrentEmployee.Surname} {Global.CurrentEmployee.Name[0]}. {Global.CurrentEmployee.Patronymic[0]}.", document);
            ToolsForGeneration.ReplaceWord("{IdTrip}", $"{trip.Id}", document);
            ToolsForGeneration.ReplaceWordWithUnderline("{EndDateTimeOfTrip}", $"{trip.EndDateTimeOfTrip}", document);
            ToolsForGeneration.ReplaceWord("{VehicleType}", $"{trip.IdVehicleNavigation.CategoryOfVehicleNavigation.CategoryName}", document);
            ToolsForGeneration.ReplaceWordWithBold("{TotalCost}", $"{trip.TotalPriceOfTrip}", document);
        }

        #endregion
    }
}
