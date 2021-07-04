using System;
using Leemo.Model.Domain;
using System.ComponentModel.DataAnnotations;

/// <summary>
/// Namespace represt Leemo.Model project
/// </summary>
namespace Leemo.Model.Domain
{
    /// <summary>
    /// Represents user address
    /// </summary>
    public class UserAddress
    {
        public Guid Id { get; set; }

        public Guid UserId { get; set; }
        //public User User { get; set; }
        
        public Guid AddressTypeId { get; set; }

        public AddressType AddressType { get; set; }

        [Required(ErrorMessage = "Please enter Street.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "Street", Prompt = "street")]
        public string Street { get; set; }

        [Required(ErrorMessage = "Please enter City.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "City", Prompt = "city")]
        public string City { get; set; }

        [Required(ErrorMessage = "Please enter State.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "State", Prompt = "state")]
        public string State { get; set; }

        [Required(ErrorMessage = "Please enter ZipCode.")]
        [MinLength(5, ErrorMessage = "Invalid Zip Code")]
        [MaxLength(6, ErrorMessage = "Invalid Zip Code")]
        [RegularExpression(@"^(?=.*\d.*)[A-Za-z0-9]{4,6}$", ErrorMessage = "Invalid Zip Code")]
        [Display(Name = "Zip Code", Prompt = "zip code")]
        public string ZipCode { get; set; }

        [Required(ErrorMessage = "Please enter Country.")]
        [RegularExpression(@".*\S+.*", ErrorMessage = "No white space allowed")]
        [Display(Name = "Country", Prompt = "country")]
        public string Country { get; set; }

        public DateTime CreatedOn { get; set; }

        public DateTime? ModifiedOn { get; set; }
    }
}
