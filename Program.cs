using alfa;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

class Program
{
    static async Task Main()
    {
        List<Organization> organizations = new List<Organization>();
        Excel.Excelobj(organizations);
        foreach (Organization organization in organizations)
        {
            Console.WriteLine(1);
            await Httprequest.Request(organization);
        }

        // Выполняем HTTP-запрос для каждой организации
       
    }
}