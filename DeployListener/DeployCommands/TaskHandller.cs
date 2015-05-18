using System;
using System.Threading.Tasks;
using DeployListener.Helpers;

namespace DeployListener.DeployCommands
{
    /// <summary>
    /// To handle the chain of tasks
    /// </summary>
    public class TaskHandller 
    {
        public void ExcecuteTask(DeployRequestDto request)
        {
            try
            {
                var serverStopTask = new Task(() => new ServerStopHelper().Handle(request));
                var deploytask = serverStopTask.ContinueWith((task => new DeployRequestHandler().Handle(request)));
                var cacheCleartask = deploytask.ContinueWith(task => new ClearCacheHelper().Handle(request));
                var startServerTask = cacheCleartask.ContinueWith(task => new ServerStartHelper().Handle(request));
                serverStopTask.Start();
            }
                catch(AggregateException ex)
                {
                    throw ex;
                }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
