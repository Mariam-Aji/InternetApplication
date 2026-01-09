namespace WebAPI.Application.Services
{
  
    using Microsoft.Data.SqlClient;
       public class DatabaseBackupService
       {
        private readonly IConfiguration _configuration;
        private const string LastBackupFile = @"C:\SqlBackups\last_backup.txt";

        public DatabaseBackupService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task RunBackupIfNeededAsync()
        {
            var now = DateTime.Now;

            if (File.Exists(LastBackupFile))
            {
                var lastBackup = DateTime.Parse(File.ReadAllText(LastBackupFile));

                if ((now - lastBackup).TotalDays < 7)
                    return; 
            }

            await RunBackupAsync();

            File.WriteAllText(LastBackupFile, now.ToString());
        }

        private async Task RunBackupAsync()
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");

            var backupPath = @"C:\SqlBackups\ne_" +
                             DateTime.Now.ToString("yyyyMMdd_HHmmss") +
                             ".bak";

            var sql = $@"
            BACKUP DATABASE ne
            TO DISK = '{backupPath}'
            WITH INIT;
        ";

            using var connection = new SqlConnection(connectionString);
            using var command = new SqlCommand(sql, connection);

            await connection.OpenAsync();
            await command.ExecuteNonQueryAsync();
        }
    }

}
