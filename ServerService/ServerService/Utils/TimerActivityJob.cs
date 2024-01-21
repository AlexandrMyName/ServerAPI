using Quartz;
using ServerService.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace ServerService.Utils
{

    [DisallowConcurrentExecution]
    public class TimerActivityJob : IJob
    {

        public async Task Execute(IJobExecutionContext context)
        {


            JobDataMap dataMap = context.JobDetail.JobDataMap;
            
            List<XmlBuisness> buisneses = (List<XmlBuisness>) dataMap["buisnesesList"];
             
            if (buisneses != null)
            {
                for (int i = 0; i < buisneses.Count; i++)
                { RefreshActivity(buisneses[i]); }

                Console.WriteLine($" | Кол-во подразделений: {buisneses}");
            }
            Console.WriteLine($"[3 SEC] | TIMER ACTIVITY | ");
             
           await Task.CompletedTask;
        }


        private void RefreshActivity(XmlBuisness buisness)
        {

            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"Activity switched {buisness.Name}");
            Console.ResetColor();
            buisness.IsActive = !buisness.IsActive;
        }
    }
}
