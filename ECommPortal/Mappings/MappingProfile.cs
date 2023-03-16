using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using ECommPortal.Models;

namespace ECommPortal.Mappings
{
    public class MappingProfile : Profile
    
    {
        public MappingProfile()
        {
            //CreateMap<, >(); // means you want to map from User to UserDTO
        }
    }
}
