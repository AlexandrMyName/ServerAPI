using Quartz.Impl;
using Quartz;
using ServerService.Utils;


namespace ServerService.Services
{

    public class ScedulesFactory
    {

        public void CreateActivityTimer(int seconds)
        {

            var _scheduler = StdSchedulerFactory.GetDefaultScheduler();

            IJobDetail job = JobBuilder.Create<TimerActivityJob>()
                .WithIdentity("buisnessJob", "group1")
                .Build();

            job.JobDataMap["buisnesesList"] = LocalDataModel.XmlBuisneses;

            ITrigger trigger = TriggerBuilder.Create()
                .WithIdentity("TimerBuisnessActivator-trigger", "group1")
                .StartNow()
                .WithSimpleSchedule(x => x
                    .WithIntervalInSeconds(3)
                    .RepeatForever())
                .Build();

            _scheduler.Result.ScheduleJob(job, trigger);
            _scheduler.Result.Start();
        }
    }
}
