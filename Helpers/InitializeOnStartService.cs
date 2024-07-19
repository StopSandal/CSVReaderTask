using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// A service responsible for initializing the database connection and schema.
    /// </summary>
    internal class InitializeOnStartService : IInitializeOnStartService
    {
        private CSVContext _context;
        private readonly IMessageDialog _messageDialog;

        /// <summary>
        /// Initializes a new instance of the <see cref="InitializeOnStartService"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        /// <param name="messageDialog">A service for displaying error messages to the user.</param>
        public InitializeOnStartService(CSVContext context, IMessageDialog messageDialog)
        {
            _context = context;
            _messageDialog = messageDialog;
        }
        /// <inheritdoc/>
        public bool Initialize()
        {
            try
            {
                _context.Database.Migrate();
                return true;
            }
            catch (SqlException ex)
            {
                if (ex.Number == 40)
                {
                    _messageDialog.ShowError("Unable to connect to the database. Please check your connection settings.");
                }
                else
                {
                    _messageDialog.ShowError("An error occurred while initializing the database: " + ex.Message);
                }

                return false;
            }
            catch (Exception ex)
            {
                _messageDialog.ShowError("An unexpected error occurred: " + ex.Message);
                return false;
            }
        }
        /// <inheritdoc/>
        public void SetNewConnectionString(string connectionString)
        {
            if (_context.Database.GetDbConnection().State != System.Data.ConnectionState.Closed)
            {
                _context.Database.CloseConnection();
            }

            _context.Database.SetConnectionString(connectionString);
        }
    }
}
