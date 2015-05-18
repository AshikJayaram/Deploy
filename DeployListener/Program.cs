using Topshelf;

namespace DeployListener
{
    class Program
    {
        static void Main(string[] args)
        {
            //creating windows service
            HostFactory.New(x =>
            {
                x.Service<ServiceFactory>(sc =>
                {
                    sc.ConstructUsing(() => new ServiceFactory());
                    sc.WhenStarted(s => s.Start());
                    sc.WhenStopped(s => s.Stop());
                });
                RunAsExtensions.RunAsLocalSystem(x);
                x.SetServiceName("DeployListenerService");
                x.SetDisplayName("DeployListenerService");
                x.SetDescription("Windows service to get the request for deployment and deploy the code");
                x.StartAutomatically();
            }).Run();
        }
    }
}
