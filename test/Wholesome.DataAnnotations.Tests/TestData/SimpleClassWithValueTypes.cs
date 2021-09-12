using System.ComponentModel.DataAnnotations;

namespace Wholesome.DataAnnotations.Tests.TestData
{
    public class SimpleClassWithRequiredProps
    {
        [Required] public string RequiredString { get; set; }
        public string NotRequiredString { get; set; }
    }
}