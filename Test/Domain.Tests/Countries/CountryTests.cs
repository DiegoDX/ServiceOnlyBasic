using Core.Entities;
using Core.Exceptions;
using Core.ValueObjects;
using FluentAssertions; // Ensure this using directive is present

namespace Domain.Tests.Countries
{
    public class CountryTests
    {
        [Fact]
        public void CreateCountry_WithValidate_ShouldCreateCountry()
        {
            var country = new Country(name: "Argentina", code: new CountryCode("AR"), null, null);

            country.Name.Should().Be("Argentina");
            country.Code.Should().Be(new CountryCode("AR")); // Compare CountryCode objects directly
        }

        [Fact]
        public void CreateCountry_WithInvalidCode_ShouldThrowException()
        {
            Action act = () => new Country(name: "Argentina", code: new CountryCode("A1234"), null, null);
            act.Should().Throw<ValidationException>();
        }

        [Fact]
        public void UpdateName_ShouldChangeName()
        {
            var country = new Country("Chile", new CountryCode("CL"), null, null);

            country.UpdateName("Chile Updated");

            country.Name.Should().Be("Chile Updated");
        }
    }
}
