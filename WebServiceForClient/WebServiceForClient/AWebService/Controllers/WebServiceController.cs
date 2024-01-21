using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Management.Automation;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using WebServiceForClient.AWebService.Models;
 


namespace WebServiceForClient.AWebService.Controllers
{

    public class WebServiceController : Controller
    {

        private IConfigurationRoot _api_Data;
        private Comendant _comendant;

        

        public WebServiceController(IHostEnvironment env)
        {

            _api_Data = new ConfigurationBuilder().SetBasePath(env.ContentRootPath).AddJsonFile("api_connection.json").Build();

            LocalMemoryDataModel.Resposes ??= new();
            LocalMemoryDataModel.Activities ??= new();

            _comendant = Comendant
                .Create(_api_Data
                .GetSection("ApiServer")
                .GetValue<string>("ip_address")
                );
        }


        [HttpGet]
        public ViewResult MainView()
        {

            return View();  
        }


        [HttpPost]
        public async Task<ViewResult> OnSyncServerAPI(string value)
        {

            var rootTreeID = _api_Data.GetSection("ApiServer").GetValue<string>("treeRootID");
            var rootTreeResponses = await GetTreeItemsRootRequest(rootTreeID);
          
            return  View("OnSyncServerAPI", rootTreeResponses);
        }


        private async Task<List<RootTreeResponse>> GetTreeItemsRootRequest(string rootTreeID)
        {

            HttpClient client = new HttpClient();
            
            client.BaseAddress = new Uri(Comendant.BaseUrl);
             
            using StringContent jsonContent = new(
                  JsonSerializer.Serialize(new
                  {
                      treekey = rootTreeID,
                      fromindex = "0",
                      toindex = "10",
                      ExpandedTreeItemIds = new List<string> { "c02d283f-70e5-43a2-8f5b-69f4ee93492e", 
                          "3af8fc64-08c2-4e7d-a80a-5636f689dc15",
                      "7b091725-feb4-4acd-9077-bdfd4c08c31a"}
                  }),// need Auto regenerate api, time's short(
                  Encoding.UTF8,
                  "application/json");

            using HttpResponseMessage response = await client.PostAsync("FlatTree", jsonContent);
              
            response.EnsureSuccessStatusCode();

            LocalMemoryDataModel.Resposes = await response.Content.ReadFromJsonAsync<List<RootTreeResponse>>();
           
            foreach(var resp in LocalMemoryDataModel.Resposes)
            {

                HttpClient clientActivity = new HttpClient() { BaseAddress = new Uri(Comendant.BaseUrl) };

                using HttpResponseMessage responseActivity = await clientActivity.PostAsJsonAsync("CheckActivity", resp);
                responseActivity.EnsureSuccessStatusCode();

                var activityJson =  await responseActivity.Content.ReadAsStringAsync();
                resp.Path = activityJson;//no have time to refact
               
            }

            return LocalMemoryDataModel.Resposes;
            
        }
         
        
        public async Task<List<string>> ExecutePowershell(string command)
        {

            var resultsAsString = new List<string>();

            using (var ps = PowerShell.Create())
            {
                var results = await ps.AddScript(command).InvokeAsync();

                foreach (var result in results)
                {
                    resultsAsString.Add(result.ToString());
                }
            }
            return resultsAsString;
        }
    }
}
