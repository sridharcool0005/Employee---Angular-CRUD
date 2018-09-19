using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using LYC.StdMgt.Domain;
using LYC.StdMgt.Web.Models;
using AutoMapper;

namespace LYC.StdMgt.Web.AutoMapper
{
    public class MappingConfigurations
    {
        public static void Configure()
        {
            Mapper.CreateMap<Student, StudentViewModel>().ForMember(cv => cv.FullName, m => m.MapFrom(
    s => s.FirstName + " " + s.LastName));

            Mapper.CreateMap<StudentViewModel, Student>();
        }
    }
}