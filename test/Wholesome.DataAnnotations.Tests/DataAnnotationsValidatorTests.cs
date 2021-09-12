using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Wholesome.DataAnnotations.Tests.TestData;
using Xunit;

namespace Wholesome.DataAnnotations.Tests
{
    public class DataAnnotationsValidatorTests
    {
        [Fact]
        public void TryValidateRecursive_ShouldBeFalse_WhenNoPropertiesSet()
        {
            var validator = new DataAnnotationsValidator();
            var input = new SimpleClassWithRequiredProps();

            var results = new List<ValidationResult>();
            validator.TryValidateRecursive(input, results).Should().BeFalse();
            results.Select(x => x.ErrorMessage).Should().ContainMatch("*RequiredString*");
        }

        [Fact]
        public void TryValidateRecursive_ShouldBeTrue_WhenRequiredPropertiesSet()
        {
            var validator = new DataAnnotationsValidator();
            var input = new SimpleClassWithRequiredProps { RequiredString = "hello" };

            var results = new List<ValidationResult>();
            validator.TryValidateRecursive(input, results).Should().BeTrue();
            results.Should().BeEmpty();
        }

        [Fact]
        public void TryValidateRecursive_ShouldBeFalse_WhenCyclicalClassNotSet()
        {
            var validator = new DataAnnotationsValidator();
            var input = new CyclicalClass { Something = "hi" };

            var results = new List<ValidationResult>();
            validator.TryValidateRecursive(input, results).Should().BeFalse();
        }
        
        [Fact]
        public void TryValidateRecursive_ShouldBeTrue_WhenCyclicalClassSet()
        {
            var validator = new DataAnnotationsValidator();
            var input = new CyclicalClass { Something = "hi" };
            input.Cyclical = input;

            var results = new List<ValidationResult>();
            validator.TryValidateRecursive(input, results).Should().BeTrue();
        }
    }
}