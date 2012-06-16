using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using ImageService;
using System.ServiceModel.Description;

namespace ImageServicing
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("I'm Host");
            Console.WriteLine();

            using (ServiceHost host = new ServiceHost(typeof(ImageService)))
            {
                try
                {
                    host.Open();
                    Console.WriteLine("Press <Enter> to stop the service.");
                    Console.ReadLine();
                }
                catch (AddressAlreadyInUseException)
                {
                    Console.WriteLine("Host was not opened because address is already in use!");
                    Console.ReadLine();
                }
            }
        }
    }
}
