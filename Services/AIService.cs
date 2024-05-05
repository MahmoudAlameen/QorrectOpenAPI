using Azure.Search.Documents.Models;
using Azure.Search.Documents;
using Azure;
using Microsoft.AspNetCore.Mvc;

namespace QorrectOpenAPI.Services
{
    public class AIService
    {
        private readonly IConfiguration _configuration;
        public AIService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<string> OnPostAsync(string question)
        {
            string responseContent;
            try
            {
                // استرداد تكوين بحث Azure Cognitive

                string searchServiceName = _configuration["AzureSearch:ServiceName"];
                string indexName = _configuration["AzureSearch:IndexName"];
                string apiKey = _configuration["AzureSearch:ApiKey"];


                // Create a SearchClient instance
                Uri serviceEndpoint = new Uri($"https://{searchServiceName}.search.windows.net/");
                AzureKeyCredential credential = new AzureKeyCredential(apiKey);
                SearchClient searchClient = new SearchClient(serviceEndpoint, indexName, credential);

                // Search for the answer using Azure Cognitive Search
                SearchResults<SearchDocument> response = await searchClient.SearchAsync<SearchDocument>(question);

                // Retrieve the response content
                responseContent = ProcessSearchResponse(response);
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return responseContent;
        }
        private string ProcessSearchResponse(SearchResults<SearchDocument> response)
        {
            if (response.GetResults().Count() > 0)
            {
                // نفترض أن الوثيقة الأولى تحتوي على الإجابة المطلوبة
                var firstResult = response.GetResults().First();
                // استرداد الإجابة من الوثيقة
                string answer = firstResult.Document["Answer"].ToString();
                // تعيين الإجابة لخاصية StoryContent
                //StoryContent = answer;
                return answer;
            }
            else
            {
                return "لم يتم العثور على إجابة.";
            }
        }
    }
}
