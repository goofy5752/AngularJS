﻿using System.ComponentModel.DataAnnotations.Schema;

namespace TourOfHeroesData.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class Hero
    {
        public Hero()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.EditHistory = new List<EditHistory>();
            // ReSharper disable once VirtualMemberCallInConstructor
            this.Movies = new List<Movie>();
        }

        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string Name { get; set; }

        [Required]
        [MaxLength(5000)]
        public string Description { get; set; }

        [Required]
        [MaxLength(500)]
        public string Image { get; set; }

        [Required]
        [MaxLength(500)]
        public string CoverImage { get; set; }
        [Required]
        [MaxLength(40)]
        public string RealName { get; set; }

        [Required]
        [MaxLength(5000)]
        public DateTime Birthday { get; set; }

        [Required]
        [MaxLength(10)]
        public string Gender { get; set; }

        public virtual List<EditHistory> EditHistory { get; set; }
        
        public virtual List<Movie> Movies { get; set; }
    }
}