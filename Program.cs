using alfa;

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
        tasks.Add(Task.Run(() => Httprequest.Request(organizations.GetRange(startIndex, count))));
        }
        await Task.WhenAll(tasks);
        List<Organization> Filtredorganizations = new List<Organization>();
        filtrorg(organizations, Filtredorganizations);
        await Httprequest.DaData(Filtredorganizations);
        List<Organization> newFiltredorganizations = new List<Organization>();
        filtrorg(Filtredorganizations, newFiltredorganizations);
        Excel.INN_folder_create();
        Excel.INN_file_create();
        Excel.INN_file_addobj(newFiltredorganizations);

        }


    public static void filtrorgName(List<Organization> Filtredorganizations, List<Organization> newFiltredorganizations)
    {
        foreach (Organization Filtredorganization in Filtredorganizations)
        {
            if ( Filtredorganization.Dubl != "Да" ) { newFiltredorganizations.Add(Filtredorganization); }

        }
    }


    public static void filtrorg(List<Organization> organizations, List<Organization> Filtredorganizations)
        {
            foreach (Organization organization in organizations)
            {
                if (organization.Dubl != "Да") { Filtredorganizations.Add(organization); }

            }
        } 
    
}