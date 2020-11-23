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

            const string openTag  = "<span class=highlight>";
            const string closeTag = "</span>";
            int index = text.IndexOf(searchTerm, StringComparison.CurrentCultureIgnoreCase);

            do
            {
                text = text.Insert(index, openTag);
                index += openTag.Length + searchTerm.Length;
                text = text.Insert(index, closeTag);
                index += closeTag.Length;
                index = text.IndexOf(searchTerm, index, StringComparison.CurrentCultureIgnoreCase);
            } while (index > 0 && index < text.Length);

            return text;
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
                ViewBag.searchType = "all";
                jobs = JobData.FindAll();
                ViewBag.title = "Viewing All " + jobs.Count.ToString() + " Jobs";
            } else
            {
                ViewBag.searchType = searchType;
                jobs = JobData.FindByColumnAndValue(searchType, searchTerm);
                string jobsStr = (jobs.Count != 1) ? " Jobs" : " Job"; // 1 Job, 2 Jobs
                ViewBag.title = jobs.Count.ToString() + jobsStr + " Found  (" + ListController.ColumnChoices[searchType] + ": " + searchTerm + ")";
            }

            ViewBag.columns = ListController.ColumnChoices;
            ViewBag.searchTerm = searchTerm;            
            ViewBag.jobs = jobs;

            return View("Index");
        }
    }
}
