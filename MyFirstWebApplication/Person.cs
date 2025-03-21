namespace _01_MyFirstWebApplication
{
    public class Person
    {
        private string _geschlecht;

        public string Geschlecht
        {
            get => _geschlecht;
            set
            {
                if (value != "männlich" && value != "weiblich")
                {
                    Console.WriteLine("Ungültiges Geschlecht eingegeben!");
                    _geschlecht = "unbekannt";
                }
                else
                {
                    _geschlecht = value;
                }
            }
        }

        public DateTime Geburtstag { get; set; }

        public Person(DateTime geburtstag, string geschlecht)
        {
            Geburtstag = geburtstag;
            Geschlecht = geschlecht; // Setter wird aufgerufen
        }
    }
}