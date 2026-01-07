namespace Application.Interfaces.Repositories
{
    public interface IUnitOfWork
    {
        ICountryRepository Countries { get; }
        ICityRepository Cities { get; }

        Task SaveChangesAsync(CancellationToken cancellationToken);
    }
}
