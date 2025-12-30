using Core.Common;
using Core.Exceptions;

namespace Core.Entities
{
    /// <summary>
    /// Represents a city that belongs to a country.
    /// </summary>
    public class City : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
        public Guid CountryId { get; set; }
        public long? Population { get; set; }
        public decimal? AreaInSquareKm { get; set; }
        public string? Description { get; set; }    
        public bool IsCapital { get; set; }

        // Navigation property
        public virtual Country Country { get; set; } = null!;

        private City() { } // For ORM
        public City(string name, Guid countryId, long? population, decimal? area, string? description, bool isCapital = false)
        {
            CountryId = countryId;
            IsCapital = isCapital;
            Description = description;
            UpdateName(name);
            UpdatePopulation(population);
            UpdateArea(area);
        }

        public void UpdateName(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ValidationException(nameof(Name), "City name cannot be empty");

            if (name.Length > 100)
                throw new ValidationException(nameof(Name), "City name cannot exceed 100 characters");

            Name = name.Trim();
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePopulation(long? population)
        {
            if (population.HasValue && population < 0)
                throw new ValidationException(nameof(Population), "City population cannot be negative");

            Population = population;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateArea(decimal? area)
        {
            if (area.HasValue && area < 0)
                throw new ValidationException(nameof(AreaInSquareKm), "City area cannot be negative");

            AreaInSquareKm = area;
            UpdatedAt = DateTime.UtcNow;
        }

        // Domain methods
        public void SetAsCapital()
        {
            //if (Country == null)
            //    throw new DomainException("Cannot set as capital: Country reference is not loaded");

            //var currentCapital = Cities.FirstOrDefault(c => c.IsCapital && c.Id != Id);

            //if (currentCapital != null)
            //    throw new DomainException($"Country already has a capital: {currentCapital.Name}. Remove capital status first.");

            IsCapital = true;
        }

        public void RemoveCapitalStatus()
        {
            IsCapital = false;
        }
    }
}
