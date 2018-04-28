namespace Splitter.ConsoleApp
{
    using System;
    using Splitter.Framework;
    using YoutubeExplode;
    using System.Diagnostics;
    using System.IO;
    using CommandLine;
    using Ninject;

    /// <summary>
    /// Main Program.
    /// </summary>
    class Program
    {
        /// <summary>
        /// Entry point.
        /// </summary>
        /// <param name="args">command line arguments.</param>
        static void Main(string[] args)
        {
            var options = ParseArguments(args);

            var stopwatch = new Stopwatch();
            stopwatch.Start();

            var url = options.Url;
            var ffmpegLocation = options.ffmpegLocation;
            var ffmpegTimeout = options.ffmpegTimeout;
            var verbose = options.Verbose;
            var tempFile = "downloaded.tmp";

            var kernel = BindDependencies(options);

            var repository = kernel.Get<IYoutubeRepository>();
            var descriptionParser = kernel.Get<IDescriptionParser>();
            var fileIo = kernel.Get<IFileIoService>();
            var downloadService = kernel.Get<IDownloadService>();
            var splitterService = kernel.Get<ISplitterService>();
            var cleanupService = kernel.Get<ICleanupService>();

            WriteLine("Getting Metadata", verbose);
            var metadata = repository.GetMetadata(url);

            WriteLine("Found " + metadata.Title, true);

            WriteLine("Getting Thumbnail", verbose);
            repository.GetThumbnail(metadata);

            WriteLine("Parsing Tracks", verbose);
            metadata.Tracks = descriptionParser.ParseTracks(metadata.Description);

            if (verbose)
            {
                WriteLine($"Found {metadata.Tracks.Count} Tracks:", verbose);
                foreach(var currentTrack in metadata.Tracks)
                {
                    Console.WriteLine($"{currentTrack.Key} {currentTrack.Value}");
                }
            }

            WriteLine("Extracting Audio: " + metadata.Url, true);
            metadata.tempFileLocation = tempFile;
            downloadService.Download(metadata);

            WriteLine($"Splitting Audio into {metadata.Tracks.Count} tracks", true);
            var tracks =  splitterService.Split(metadata);

            WriteLine("Output:", true);
            foreach (var track in tracks)
            {
                Console.WriteLine(track);
            }

            WriteLine("Cleaning temp files", verbose);
            cleanupService.CleanUp(metadata);

            if (verbose)
            {
                Console.WriteLine("Done");
                Console.WriteLine(stopwatch.ElapsedMilliseconds + " ms.");
            }
        }

        /// <summary>
        /// Writes user output.
        /// </summary>
        /// <param name="text">output to write.</param>
        private static void WriteLine(string text, bool show)
        {
            if (show)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine(text);
                Console.ResetColor();
            }
        }

        /// <summary>
        /// Parses the given command line arguments.
        /// </summary>
        /// <param name="args">arguments to parse</param>
        /// <returns>Parsed Arguments or if there was a failure, writes help.</returns>
        private static Arguments ParseArguments(string[] args)
        {
            var option = default(Arguments);
            var writer = new StringWriter();
            var parser = new Parser(conf => conf.HelpWriter = writer);

            var result = parser.ParseArguments<Arguments>(args);
            result.WithNotParsed(_ =>
            {
                Console.WriteLine(writer.ToString());
                System.Environment.Exit(1);
            })
            .WithParsed(opt =>
            {
                option = opt;
            });

            return option;
        }

        /// <summary>
        /// Binds Dependencies to a Ninject Kernel.
        /// </summary>
        private static IKernel BindDependencies(Arguments arguments)
        {
            var descriptionRegex = @"(?<time>\d{1,2}:\d{2}|\d{1,2}:\d{2}:\d{2})(\s|-)(?<title>.+)";

            var kernel = new StandardKernel();
            kernel.Bind<IYoutubeClient>().To<YoutubeClient>();
            kernel.Bind<IYoutubeRepository>().To<YoutubeRepository>();
            kernel.Bind<IDescriptionParser>().To<DescriptionParser>().WithConstructorArgument("descriptionRegex", descriptionRegex);;
            kernel.Bind<IFileIoService>().To<FileIoService>();
            kernel.Bind<IDownloadService>().To<DownloadService>();
            kernel.Bind<IFFmpegService>().To<FFmpegService>()
                .WithConstructorArgument("ffmpegLocation", arguments.ffmpegLocation)
                .WithConstructorArgument("processWaitBeforeTimeoutMs", arguments.ffmpegTimeout)
                .WithConstructorArgument("verbose", arguments.Verbose);
            kernel.Bind<ISplitterService>().To<SplitterService>();
            kernel.Bind<ICleanupService>().To<CleanupService>();

            return kernel;
        }
    }
}