using CSVReaderTask.EF;
using CSVReaderTask.Helpers.Interfaces;
using CSVReaderTask.Models;

namespace CSVReaderTask.Helpers
{
    /// <summary>
    /// Represents the default implementation of the <see cref="IUnitOfWork"/>.
    /// </summary>
    /// <inheritdoc cref="IUnitOfWork"/>
    public class UnitOfWork : IUnitOfWork
    {
        private readonly CSVContext context;
        private IRepositoryAsync<Person> personRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UnitOfWork"/> class with the specified context.
        /// </summary>
        /// <param name="csvContext">The database context.</param>
        public UnitOfWork(CSVContext csvContext)
        {
            context = csvContext;
        }
        /// <inheritdoc />
        public IRepositoryAsync<Person> PersonRepository
        {
            get
            {
                if (this.personRepository == null)
                {
                    this.personRepository = new GenericRepository<Person>(context);
                }
                return personRepository;
            }
        }

        /// <inheritdoc/>
        public Task SaveAsync()
        {
            return context.SaveChangesAsync();
        }

        private bool disposed = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
