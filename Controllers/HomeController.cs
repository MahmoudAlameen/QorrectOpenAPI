using Microsoft.AspNetCore.Mvc;
using QorrectOpenAPI.Models;
using QorrectOpenAPI.Services;
using System.Diagnostics;

namespace QorrectOpenAPI.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly AIService AIService; 

        public HomeController(ILogger<HomeController> logger, AIService aIService)
        {
            _logger = logger;
            AIService = aIService;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpGet("qorrect/ai/search-view")]
        public IActionResult QorrectAI()
        {
            return View("QorrectAI");
        }

        [HttpPost("qorrect/ai/search")]
        public async Task<IActionResult> Search(SearchModel searchModel)
        {
            var searchResult =  await AIService.OnPostAsync(searchModel.Name);
            return RedirectToAction("SearchResult", new QorrectAISearchResultModel() { SearchResult = searchResult } );
        }

        [HttpGet("qorrect/ai/search-result")]
        public IActionResult SearchResult(QorrectAISearchResultModel searcResultModel)
        {
            return View("QorrectAISearchResult", searcResultModel);
        }


    }
}
