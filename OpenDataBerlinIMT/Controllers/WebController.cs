using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace OpenDataBerlinIMT.Controllers
{
    public class WebController : Controller
    {
        public ActionResult BurnedBooks()
        {
            // REFERENZ-LINK daten.berlin.de:
            // https://daten.berlin.de/datensaetze/liste-der-verbannten-b%C3%BCcher-0
            // bzw. LINK zur Ressource
            // http://www.berlin.de/berlin-im-ueberblick/geschichte/berlin-im-nationalsozialismus/verbannte-buecher/suche/index.php/index/all.json?q=

            if ((string.IsNullOrWhiteSpace((string)ViewBag.BurnedBooksJsonString)))
            {
                const string BURNED_BOOKS_RESOURCE_LINK = "http://www.berlin.de/berlin-im-ueberblick/geschichte/berlin-im-nationalsozialismus/verbannte-buecher/suche/index.php/index/all.json?q=";

                string jsonString = "";

                using (WebClient client = new WebClient())
                {
                    jsonString = client.DownloadString(BURNED_BOOKS_RESOURCE_LINK);
                }

                if (!string.IsNullOrWhiteSpace(jsonString))
                {
                    dynamic jsonObject = JsonConvert.DeserializeObject(jsonString);
                    if(jsonObject.index?.Count > 0)
                    {
                        dynamic bookArray = jsonObject.index;
                        foreach(dynamic book in bookArray)
                        {
                            string authorFirstName = book.authorfirstname;
                            string authorLastName = book.authorlastname;
                            string firstEditionPublicationPlace = book.firsteditionpublicationplace;
                            string firstEditionPublicationYear = book.firsteditionpublicationyear;
                            string firstEditionPublisher = book.firsteditionpublisher;
                            string id = book.id;
                            string ocrresult = book.ocrresult;
                            string title = book.title;
                            string ssFlag = book.ssflag;
                            string additionalInfos = book.additionalinfos;
                        }
                    }
                }
            }

            return View();
        }
    }
}