namespace mediaboxservice
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IConfiguration _configuration;
        private FileSystemWatcher _watcher;

        public Worker(ILogger<Worker> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            string folderPath = _configuration["WatchFolderPath"]!;
            bool includeSubdirectories = bool.Parse(_configuration["IncludeSubdirectories"]!);
            string fileFilter = _configuration["FileFilter"]!;

            _watcher = new ()
            {
                Path = folderPath,
                NotifyFilter = NotifyFilters.FileName | NotifyFilters.LastWrite | NotifyFilters.LastAccess | NotifyFilters.DirectoryName | NotifyFilters.CreationTime,
                Filter = fileFilter,
                IncludeSubdirectories = includeSubdirectories,
                EnableRaisingEvents = true
            };

            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }
                await Task.Delay(1000, stoppingToken);
            }
        }
    }
}
