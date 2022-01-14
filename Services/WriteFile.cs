namespace WebApiAuthors.Services
{
    public class WriteFile : IHostedService
    {
        private readonly IWebHostEnvironment _env;
        private readonly string filename = "file.txt";
        private Timer timer;

        public WriteFile(IWebHostEnvironment env)
        {
            this._env = env;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            timer = new Timer(ReWrite, null, TimeSpan.Zero, TimeSpan.FromSeconds(10));
            OnWrite("Process started.");
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            timer.Dispose();
            OnWrite("Process finished.");
            return Task.CompletedTask;
        }

        public void OnWrite(string msg)
        {
            var route = $@"{_env.ContentRootPath}\wwwroot\{filename}";
            using (StreamWriter sw = new StreamWriter(route, append: true))
            {
                sw.WriteLine(msg);
            }
        }

        public void ReWrite(object state)
        {
            OnWrite("Running process." + DateTime.Now.ToString("dd/mm/yyyy hh:mm:ss"));
        }
    }
}
