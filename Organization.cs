namespace alfa
{
    internal class Organization
    {
        public string INN {  get; set; }
        public string Number { get; set; }
        public string Dubl { get; set; }
        public string Name { get; set; }
        public Organization(string inn,string number,string dubl, string name) {
            INN= inn;
            Number= number;
            Dubl = dubl;
            Name = name;    
        }
    }  
}
