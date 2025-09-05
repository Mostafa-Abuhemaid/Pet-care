﻿using Microsoft.AspNetCore.Identity;
using PetCare.Api.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web.Domain.Enums;

namespace Web.Domain.Entites
{
    public class AppUser : IdentityUser
    {
        public string FullName { get; set; }=string.Empty;
        public Gender gender {  get; set; }

        public ICollection<Pet> Pets { get; set; } =[];
        public ICollection<Cart> Carts { get; set; }=[];
        public ICollection<Address> addresses { get; set; }=[];
        public ICollection<Order> orders { get; set; }=[];
    }
}
