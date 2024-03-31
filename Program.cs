using System;
using System.Net;
using System.Threading.Tasks;
namespace DZ_9
{
    public class ImageDownloader
    {
        public event EventHandler ImageStarted;
        public event EventHandler ImageCompleted;

        public async Task DownloadAsync(string remoteUri, string fileName)
        {
            OnImageStarted();

            using (var myWebClient = new WebClient())
            {
                Console.WriteLine("Downloading \"{0}\" from \"{1}\" .......\n\n", fileName, remoteUri);
                await myWebClient.DownloadFileTaskAsync(remoteUri, fileName);
                Console.WriteLine("Successfully downloaded \"{0}\" from \"{1}\"", fileName, remoteUri);
            }

            OnImageCompleted();
        }

        protected virtual void OnImageStarted()
        {
            ImageStarted?.Invoke(this, EventArgs.Empty);
        }

        protected virtual void OnImageCompleted()
        {
            ImageCompleted?.Invoke(this, EventArgs.Empty);
        }
        static async Task Main()
        {
            string[] remoteUris = new string[]
            {
            "https://webneel.com/daily/sites/default/files/images/daily/09-2014/1-rose-drawing-stephen-ainsworth.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/09-2014/6-rose-watercolor-painting.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/01-2014/14-drawings-of-flowers-hibiscus.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/01-2014/1-flower-drawings-rose.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/09-2014/7-rose-color-pencil-drawing.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/09-2014/12-rose-painting.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/09-2014/14-rose-painting.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/01-2014/20-drawings-of-flowers.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/11-2013/10-flower-paintings.jpg",
            "https://webneel.com/daily/sites/default/files/images/daily/11-2013/17-flower-painting-rose.jpg"
            };

            List<Task> downloadTasks = new List<Task>();

            ImageDownloader downloader = new ImageDownloader();
            downloader.ImageStarted += (sender, e) => Console.WriteLine("Downloading started...");
            downloader.ImageCompleted += (sender, e) => Console.WriteLine("Downloading completed...");

            foreach (var uri in remoteUris)
            {
                string fileName = uri.Substring(uri.LastIndexOf('/') + 1);
                downloadTasks.Add(downloader.DownloadAsync(uri, fileName));
            }

            Console.WriteLine("Press 'A' to exit or any other key to check download status...");

            while (true)
            {
                if (Console.KeyAvailable)
                {
                    var key = Console.ReadKey(true).Key;
                    if (key == ConsoleKey.A)
                    {
                        break;
                    }
                    else
                    {
                        foreach (var task in downloadTasks)
                        {
                            Console.WriteLine($"Image Downloaded: {task.IsCompleted}");
                        }
                    }
                }
            }

            Console.WriteLine("Exiting...");
        }

    }
}