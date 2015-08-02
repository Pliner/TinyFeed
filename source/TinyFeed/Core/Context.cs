using System.Data.Entity;
using System.Threading;

namespace TinyFeed.Core
{
    public class Context : DbContext, IContext
    {
        private static int total;

        private readonly int number;

        static Context()
        {
            Database.SetInitializer<Context>(null);
        }

        public Context()
            : base("SQLite")
        {
            number = Interlocked.Increment(ref total);
        }

        public IDbSet<Package> Packages { get; set; }
        
        public void Initialize()
        {
            Database.CreateIfNotExists();
            Database.ExecuteSqlCommand(@"CREATE TABLE IF NOT EXISTS Packages(
                    [Id] [nvarchar](64) NOT NULL,
                    [Version] [nvarchar](64) NOT NULL,
                    [Title] [nvarchar](64) NOT NULL,
                    [DisplayTitle] [nvarchar](64) NOT NULL,
                    [Authors] [nvarchar](64) NOT NULL,
                    [Owners] [nvarchar](64) NOT NULL,
                    [IconUrl] [nvarchar](64) NOT NULL,
                    [LicenseUrl] [nvarchar](64) NOT NULL,
                    [ProjectUrl] [nvarchar](64) NOT NULL,
                    [DownloadCount] [int] NOT NULL,
                    [RequireLicenseAcceptance] [bit] NOT NULL,
                    [Description] [nvarchar](64) NOT NULL,
                    [Summary] [nvarchar](64) NOT NULL,
                    [ReleaseNotes] [nvarchar](64) NOT NULL,
                    [Language] [nvarchar](64) NOT NULL,
                    [Created] [datetime2] NOT NULL,
                    [Published] [datetime2] NOT NULL,
                    [LastUpdated] [datetime2] NOT NULL,
                    [Dependencies] [nvarchar](64) NOT NULL,
                    [PackageHash] [nvarchar](64) NOT NULL,
                    [PackageHashAlgorithm] [nvarchar](64) NOT NULL,
                    [PackageSize] [BIGINT] NOT NULL,
                    [Copyright] [nvarchar](64) NOT NULL,
                    [Tags] [nvarchar](64) NOT NULL,
                    [IsAbsoluteLatestVersion] [bit] NOT NULL,
                    [IsLatestVersion] [bit] NOT NULL,
                    [IsPrerelease] [bit] NOT NULL,
                    [Listed] [bit] NOT NULL,
                    [DevelopmentDependency] [bit] NOT NULL,
                    [VersionDownloadCount] [int] NOT NULL,
                    [Score] [float] NOT NULL,
                 
                    CONSTRAINT PK_Packages PRIMARY KEY ([Id], [Version]),
                    CONSTRAINT UK_Packages_Id_Version_IsLatestVersion UNIQUE([Id], [Version], [IsLatestVersion])
                )");

            Database.ExecuteSqlCommand(@"CREATE INDEX IF NOT EXISTS IX_Packages_IsLatestVersion ON Packages (`IsLatestVersion` ASC);");
        }

        public override string ToString()
        {
            return "Context-" + number;
        }
    }
}