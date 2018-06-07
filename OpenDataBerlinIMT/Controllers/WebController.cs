using Newtonsoft.Json;
using OpenDataBerlinIMT.Models;
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
        public ActionResult banishedbooks(bool ascending = true, string orderBy = "ID")
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
                        List<Book> books = new List<Book>();

                        dynamic bookArray = jsonObject.index;
                        foreach(dynamic book in bookArray)
                        {
                            Book currentBookToAdd = new Book()
                            {
                                ID = book["id"],
                                AuthorFirstName = book["authorfirstname"],
                                AuthorLastName = book["authorlastname"],
                                FirstEditionPublicationPlace = book["firsteditionpublicationplace"],
                                FirstEditionPublicationYear = book["firsteditionpublicationyear"],
                                FirstEditionPublisher = book["firsteditionpublisher"],
                                Ocrresult = book["ocrresult"],
                                Title = book["title"],
                                SsFlag = book["ssflag"],
                                AdditionalInfos = book["additionalinfos"],

                                PageNumberInocrDocument = book["pagenumberinocrdocument"],
                                SecondEditionPublisher = book["secondeditionpublisher"],
                                SecondEditionPublicationPlace = book["secondeditionpublicationplace"],
                                SecondEditionPublicationYear = book["secondeditionpublicationyear"],
                            };

                            if(currentBookToAdd.AdditionalInfos != null && currentBookToAdd.AdditionalInfos != "")
                            {
                                string x = currentBookToAdd.AdditionalInfos;
                            }
                            books.Add(currentBookToAdd);
                        }

                        string booksString = JsonConvert.SerializeObject(books);

                        ViewBag.Books = booksString;
                    }
                }
            }

            return View();
        }
    }
}