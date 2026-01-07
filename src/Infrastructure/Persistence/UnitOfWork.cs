using Application.Interfaces.Repositories;
using Infrastructure.Data;

namespace Infrastructure.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        public readonly AppDbContext _context;
        private readonly ICountryRepository _countryRepository;
        private readonly ICityRepository _cityRepository;
        public UnitOfWork(AppDbContext context, ICountryRepository countryRepository, ICityRepository cityRepository)
        {
            _context = context;
            _countryRepository = countryRepository;
            _cityRepository = cityRepository;
        }


        public ICountryRepository Countries { get { return _countryRepository; } }

        public ICityRepository Cities => _cityRepository;

        public async Task SaveChangesAsync(CancellationToken cancellationToken)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

    }
}
