﻿using System.Collections.Generic;
using TourOfHeroesData.Models;
using TourOfHeroesServices.Mapping;

namespace TourOfHeroesDTOs
{
    // ReSharper disable once InconsistentNaming
    public class GetHeroDetailDTO : IMapFrom<Hero>
    {
        public GetHeroDetailDTO()
        {
            this.EditHistory = new List<EditHistory>();
        }

        public int Id { get; set; }

        public string Name { get; set; }
        
        public string Description { get; set; }

        public string Image { get; set; }
        
        public string CoverImage { get; set; }
        
        public string RealName { get; set; }
        
        public string Birthday { get; set; }
        
        public string Gender { get; set; }

        public IEnumerable<EditHistory> EditHistory { get; set; }
    }
}