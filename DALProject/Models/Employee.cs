﻿using System;
using System.ComponentModel.DataAnnotations;

namespace DALProject.Models
{
    abstract public class Employee : ModelClass
    {
		

		public string Availability { get; set; }

        public DateTime BirthDate  { get; set; } // .net 5 not support date only

        public string AppUserId { get; set; }

        public virtual AppUser user { get; set; }
    }
}
