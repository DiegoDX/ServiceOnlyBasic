using Core.Common;
using Core.Exceptions;
using Core.ValueObjects;

namespace Core.Entities
{
    /// <summary>
    /// Represents a country in the system.
    /// </summary>
    public class Country: BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public CountryCode? Code { get; set; }
        public string? Description { get; set; }
        public long? Population { get; set; }
        public decimal? AreaInSquareKm { get; set; }
        // Navigation property
        public ICollection<City> Cities { get; set; } = new List<City>();

        private Country() { } // For ORM
        public Country(string name, CountryCode code, long? population, decimal? area)
        {
            Code = code;
            UpdateName(name);
            UpdatePopulation(population);
            UpdateArea(area);
        }

        public void AddCity(City city)
        {
            if (city == null) throw new ArgumentNullException(nameof(city));
            city.CountryId = this.Id;
            Cities.Add(city);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(nameof(Name), "Country name cannot be empty");

            if (name.Length > 100)
                throw new ValidationException(nameof(Name), "Country name cannot exceed 100 characters");

            Name = name.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePopulation(long? population)
        {
            if (population.HasValue && population < 0)
                throw new ValidationException(nameof(Population), "Country population cannot be negative");

            Population = population;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateArea(decimal? area)
        {
            if (area.HasValue && area < 0)
                throw new ValidationException(nameof(AreaInSquareKm), "Country area cannot be negative");

            AreaInSquareKm = area;
            UpdatedAt = DateTime.UtcNow;
        }
    }
}
