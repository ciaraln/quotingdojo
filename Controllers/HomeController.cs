using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using quotingdojo.Models;
using quotingdojo.Connectors;

namespace quotingdojo.Controllers
{
    public class HomeController : Controller
    {
        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Route("Quotes")]
        public IActionResult Quotes()
        {
            ViewData["Message"] = " All Quotes";
            List<Dictionary<string, object>> AllQuotes = MySQLConnector.Query("SELECT * FROM quotes ORDER BY created_at DESC");
            ViewBag.AllQuotes = AllQuotes;
            System.Console.WriteLine(AllQuotes);
            // this will to seperate objects  into 
            foreach (var item in AllQuotes)
            {
                System.Console.WriteLine(" POST-QUOTE:" + string.Join(",", item));
            }
            return View("Quotes");
        }

  // add quotes to say /addquote
        [HttpPost]
        [Route("addQuote")]
        public IActionResult addQuote( string Name , string Quote)
        {
            if( Name == null || Quote == null)
            {
                ViewBag.Message = "Quote and name must be filled out.";
                Console.WriteLine("**************" + Name + "****************");
                Console.WriteLine("**************" + Quote + "****************");
                return View("Index");
            }
            else
            {
                Console.WriteLine("******POST********" + Name + "********YEA********");
                Console.WriteLine("******POST********" + Quote + "********YEA********");
                string query = $"insert into quotes (name, quote) values ('{Name}', '{ Quote.Replace( " ' " , " ' ' ")}')";
                System.Console.WriteLine(query);
                MySQLConnector.Execute(query);
                return Redirect("Quotes");
            }
            // ViewData["Message"] = "Add Quote";

        }


        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
