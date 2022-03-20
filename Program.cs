using System.ServiceProcess;

namespace My_Service
{
    static class Program
    {
        static void Main()
        {
            ServiceBase[] ServicesToRun;
            ServicesToRun = new ServiceBase[]{new Service1()};
            ServiceBase.Run(ServicesToRun);
        }
    }
}
