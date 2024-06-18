using alfa;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        List<Organization> organizations = new List<Organization>();
        Excel.Excelobj(organizations);
        List<Task> tasks = new List<Task>();

        int chunkSize = organizations.Count / 5;
        int remainder = organizations.Count % 5;

        for (int i = 0; i < 5; i++)
        {
            int startIndex = i * chunkSize;
            int count = (i <= 4) ? chunkSize : chunkSize + remainder;

            // Создаем отдельную задачу для каждого потока
            tasks.Add(Task.Run(() => Httprequest.Request(organizations.GetRange(startIndex, count))
            ));
        }

        // Ждем завершения всех задач
        await Task.WhenAll(tasks);
        Excel.INN_folder_create();
        Excel.INN_file_create();
        List<Organization> Filtredorganizations = new List<Organization>();
        filtrorg(organizations, Filtredorganizations);
        Excel.INN_file_addobj(Filtredorganizations);
       
    }
    public static void filtrorg(List<Organization> organizations, List<Organization> Filtredorganizations)
    {
        foreach (Organization organization in organizations)
        {
            if (organization.Dubl != "Да") {Filtredorganizations.Add(organization);}

        }
    }
}