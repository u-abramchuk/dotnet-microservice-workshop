using System.Threading;
using Microsoft.AspNetCore.Hosting;

namespace Workshop
{
    public static class GracefulShutdown
    {
        public static bool SigtermReceived { get; private set; }

        public static void ShutdownGracefully(this IApplicationLifetime applicationLifetime, int timeout)
        {
            applicationLifetime.ApplicationStopping.Register(() =>
                        {
                            SigtermReceived = true;

                            Thread.Sleep(timeout * 1000);
                        });
        }
    }
}
