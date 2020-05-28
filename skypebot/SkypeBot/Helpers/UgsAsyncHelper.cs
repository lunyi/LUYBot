using System;
using System.Threading;
using System.Threading.Tasks;

namespace SkypeBot.Helpers
{
    public static class UgsAsyncHelper
    {
        private static readonly TaskFactory MyTaskFactory = new
                TaskFactory(CancellationToken.None,
                TaskCreationOptions.None,
                TaskContinuationOptions.None,
                TaskScheduler.Default);

        public static void RunAsync(Func<object, Task> func, object arg0)
        {
            MyTaskFactory
                .StartNew(func, arg0)
                .Unwrap();
        }
    }
}
