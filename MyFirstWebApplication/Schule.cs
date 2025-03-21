namespace _01_MyFirstWebApplication
{
    public class Schule
    {
        public List<Schueler> SchuelerList = new List<Schueler>();
        public List<Klassenraum> KlassenraumList = new List<Klassenraum>();

        public void AddSchuelerToSchule(Schueler schueler)
        {
            SchuelerList.Add(schueler);
        }
        public void AddKlassenraumToSchule(Klassenraum klassenraum)
        {
            KlassenraumList.Add(klassenraum);
        }
        public int AnzahlSchueler
        {
            get { return SchuelerList.Count; }
        }
        public int AnzahlKlassenRaum
        {
            get { return KlassenraumList.Count; }
        }
        public List<Klassenraum> AnzahlRauemeCynap()
        {
            List<Klassenraum> KlassenraumCynap = new List<Klassenraum>();
            foreach (Klassenraum klassenraum in KlassenraumList)
            {
                if (klassenraum.HasCynap)
                {
                    KlassenraumCynap.Add(klassenraum);
                }
            }
            return KlassenraumCynap;
        }
        public int AnzahlKlassen(Schueler schueler)
        {
            return schueler.klassen.Count;
        }
        public float DurchschnittsalterSchueler()
        {
            int sumAlter = 0;
            foreach (Schueler schueler in SchuelerList)
            {
                sumAlter += schueler.Alter;
            }
            return (float)sumAlter / AnzahlSchueler;
        }
        public double BerechneFrauenanteilInProzent(List<Schueler> schuelerListe, string klasse)
        {
            int anzahlSchueler = 0;
            int anzahlFrauen = 0;

            foreach (Schueler schueler in schuelerListe)
            {
                if (schueler.Klasse == klasse)
                {
                    anzahlSchueler++;
                    if (schueler.Geschlecht == "weiblich")
                    {
                        anzahlFrauen++;
                    }
                }
            }

            if (anzahlSchueler == 0)
                return 0;

            return (double)anzahlFrauen / anzahlSchueler * 100;
        }

        public bool KannKlasseUnterrichten(string klasse, string raumName)
        {
            int schuelerInKlasse = 0;
            Klassenraum raum = null;

            foreach (Schueler schueler in SchuelerList)
            {
                if (schueler.Klasse == klasse)
                {
                    schuelerInKlasse++;
                }
            }

            foreach (Klassenraum klassenraum in KlassenraumList)
            {
                if (klassenraum.RaumInQm.ToString() == raumName)
                {
                    raum = klassenraum;
                    break;
                }
            }
            if (raum == null) return false;

            return raum.Plaetze >= schuelerInKlasse;
        }

        public string AnzahlSchuelerGeschlecht
        {
            get
            {
                int männlicheSchueler = 0;
                int weiblicheSchueler = 0;

                foreach (Schueler schueler in SchuelerList)
                {
                    if (schueler.Geschlecht == "männlich")
                    {
                        männlicheSchueler++;
                    }
                    else if (schueler.Geschlecht == "weiblich")
                    {
                        weiblicheSchueler++;
                    }
                }
                return $"männliche: {männlicheSchueler} / weibliche: {weiblicheSchueler}";
            }
        }
    }
}