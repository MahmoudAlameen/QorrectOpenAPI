using Azure.Storage.Blobs;
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
        private string _connectionstring = "DefaultEndpointsProtocol=https;AccountName=qorrectopenai;AccountKey=XYHiJJJkl2VvEj1tP1kQEgwRIPf8iSTFuJs8zZG73dzPSLpRp0ue7JSpIWI19SagNjW8XDSKEByX+ASt46HxBQ==;EndpointSuffix=core.windows.net";


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
            var searchResult =  await AIService.OnPostAsync("قواعد السطوة");
            return View("QorrectAISearchResult", new QorrectAISearchResultModel() { SearchResult = searchResult });
            
        }

        [HttpPost("qorrect/ai/search-result")]
        public IActionResult SearchResult([FromBody] QorrectAISearchResultModel searcResultModel)
        {
            return View("QorrectAISearchResult", searcResultModel);
        }
        [HttpPost("upload")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Upload(List<IFormFile> files)
        {

            BlobContainerClient blobContainerClient = new BlobContainerClient(_connectionstring, "qorrectopenai");
            foreach (IFormFile file in files)
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0;
                    await blobContainerClient.UploadBlobAsync(file.FileName, stream);
                }
            }
            return Ok("File uploaded successfully");
        }

        [HttpGet("view-upload")]
        public async Task<IActionResult> ViewUpload()
        {
            return View("UploadFile");
        }

    }
}
