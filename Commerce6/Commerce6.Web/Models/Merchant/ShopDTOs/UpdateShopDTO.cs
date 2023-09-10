﻿using System.ComponentModel.DataAnnotations;
using Commerce6.Web.Models.Contact.AddressDTOs;

namespace Commerce6.Web.Models.Merchant.ShopDTOs
{
    public class UpdateShopDTO
    {
        [StringLength(100, ErrorMessage = "Shop name can't be more than 100 characters")]
        public string? Name { get; set; }

        public IFormFile? Avatar { get; set; }

        [RegularExpression(@"0\d{9,10}", ErrorMessage = "Invalid phone number.")]
        public string? Phone { get; set; }

        [StringLength(20, ErrorMessage = "Bank account can't be more than 20 characters")]
        public string? BankAccount { get; set; }

        public AddressRequestDTO? Address { get; set; }
        //public ICollection<Promotion>? Promotion { get; set; }
    }
}
