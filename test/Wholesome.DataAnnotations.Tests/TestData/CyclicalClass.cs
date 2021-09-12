using System.ComponentModel.DataAnnotations;

namespace Wholesome.DataAnnotations.Tests.TestData
{
    public class CyclicalClass
    {
        [Required] public string Something { get; set; }
        [Required] public CyclicalClass Cyclical { get; set; }
    }
}