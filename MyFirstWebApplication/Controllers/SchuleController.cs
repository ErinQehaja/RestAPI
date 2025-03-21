using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;

namespace _01_MyFirstWebApplication.Controllers
{
    [ApiController]
    [Route("api/schulverwaltung")]
    public class SchulverwaltungController : ControllerBase
    {
        private readonly Schuldatenbank _schuldatenbank;

        public SchulverwaltungController()
        {
            _schuldatenbank = new Schuldatenbank();
        }

        [HttpPost("schueler-hinzufuegen")]
        public IActionResult SchuelerHinzufuegen([FromBody] Schueler neuerSchueler)
        {
            if (neuerSchueler == null)
            {
                return BadRequest("Keine gültigen Schülerdaten übermittelt.");
            }

            try
            {
                _schuldatenbank.SchuelerRegistrieren(neuerSchueler);
                return Ok("Schüler erfolgreich eingetragen!");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Fehler beim Registrieren des Schülers: {ex.Message}");
            }
        }

        [HttpGet("alle-schueler")]
        public IActionResult AlleSchuelerAbrufen()
        {
            var schuelerListe = _schuldatenbank.AlleSchuelerHolen();
            return Ok(schuelerListe);
        }

        [HttpGet("schueler-nach-klasse/{klassenname}")]
        public IActionResult SchuelerNachKlasse(string klassenname)
        {
            var gefilterteSchueler = _schuldatenbank.AlleSchuelerHolen()
                .Where(s => s.Klasse.Equals(klassenname, StringComparison.OrdinalIgnoreCase))
                .ToList();
            return Ok(gefilterteSchueler);
        }

        [HttpGet("raumpruefung/{klasse}/{raum}")]
        public IActionResult RaumPruefen(string klasse, string raum)
        {
            bool passt = _schuldatenbank.IstRaumGeeignet(klasse, raum);
            string nachricht = passt
                ? "Ja, der Raum ist für die Klasse geeignet."
                : "Nein, der Raum bietet nicht genug Platz.";
            return Ok(nachricht);
        }
    }

    public class Schuldatenbank
    {
        private readonly Schule _schule;

        public Schuldatenbank()
        {
            _schule = new Schule();
        }

        public void SchuelerRegistrieren(Schueler schueler)
        {
            _schule.AddSchuelerToSchule(schueler);
        }

        public List<Schueler> AlleSchuelerHolen()
        {
            return _schule.SchuelerList;
        }

        public bool IstRaumGeeignet(string klasse, string raumName)
        {
            return _schule.KannKlasseUnterrichten(klasse, raumName);
        }
    }
}