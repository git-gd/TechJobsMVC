using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using TechJobsMVC.Data;
using TechJobsMVC.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TechJobsMVC.Controllers
{
    public class SearchController : Controller
    {
        public static string Highlight(string text, string searchTerm)
        {
            if (String.IsNullOrEmpty(searchTerm)) return text;
            if (!text.Contains(searchTerm, StringComparison.CurrentCultureIgnoreCase)) return text;

            int index = text.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase);
            //NOTE: The CLOSING tag is inserted first so that we do not need to recalculate the index
            string highlighted = text.Insert(index + searchTerm.Length, "</span>");
            highlighted = highlighted.Insert(index, "<span class=highlight>");

            return highlighted;
        }
        // GET: /<controller>/
        public IActionResult Index()
        {
            ViewBag.columns = ListController.ColumnChoices;
            return View();
        }

        public IActionResult Results(string searchType, string searchTerm)
        {
            List<Job> jobs;

            if (String.IsNullOrEmpty(searchTerm))
            {
                ViewBag.title = "All Jobs";
                ViewBag.searchType = "all";
                jobs = JobData.FindAll();
            } else
            {
                ViewBag.title = "Jobs with " + ListController.ColumnChoices[searchType] + ": " + searchTerm;
                ViewBag.searchType = searchType;
                jobs = JobData.FindByColumnAndValue(searchType, searchTerm);
            }

            ViewBag.columns = ListController.ColumnChoices;
            ViewBag.searchTerm = searchTerm;            
            ViewBag.jobs = jobs;

            return View("Index");
        }
    }
}
