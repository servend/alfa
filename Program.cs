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
        await Httprequest.Request(organizations);
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
