using Quartz;
using ServerService.Models;
using System;
using System.Threading.Tasks;


namespace ServerService.Utils
{

    [DisallowConcurrentExecution]
    public class TimerActivityJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {
           
            for(int i = 0; i < LocalDataModel.XmlBuisneses.Count; i++)
                { RefreshActivity(LocalDataModel.XmlBuisneses[i]); }

            await Task.CompletedTask;
        }


        private void RefreshActivity(XmlBuisness buisness)
        {

            Console.WriteLine("Activity switched");
            buisness.IsActive = !buisness.IsActive;
        }
    }
}
