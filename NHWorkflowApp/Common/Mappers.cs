using NHWorkflow.Core;
using NHWorkflow.Core.Infra;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;

namespace NHWorkflowApp.Common
{
    public class NHWorkflowMappers : Profile, IMapperProfile 
    {
        // priority of order of execution
        
        public int  Order => 0;

        public NHWorkflowMappers()
        {
            CreateMap<Member, MemberModel>()
                .ForMember(dest => dest.FirstName, mo => mo.Ignore())
                .ForMember(dest => dest.LastName, mo => mo.Ignore())
                .ForMember(dest => dest.DOB, mo => mo.Ignore())

                .ForMember(dest => dest.HearingAid, mo => mo.Ignore())
                .ForMember(dest => dest.Gender, mo => mo.Ignore())
                .ForMember(dest => dest.Email, mo => mo.Ignore())
                
                .ForMember(dest => dest.MemberId, mo => mo.Ignore());


            CreateMap<BenefitCategory, BenefitCategoryModel>()
               .ForMember(dest => dest.CategoryId, mo => mo.Ignore())
               .ForMember(dest => dest.CategoryType, mo => mo.Ignore())
               .ForMember(dest => dest.FederalDiscountPercentage, mo => mo.Ignore())
               .ForMember(dest => dest.MfgDiscountPercentage, mo => mo.Ignore());

         
            CreateMap<ApprovalQueue, ApprovalQueueModel>()
               .ForMember(dest => dest.ApprovalId, mo => mo.Ignore())
               .ForMember(dest => dest.CategoryId, mo => mo.Ignore())
               .ForMember(dest => dest.MemberId, mo => mo.Ignore())

               .ForMember(dest => dest.TotalCost, mo => mo.Ignore())
               .ForMember(dest => dest.Discount, mo => mo.Ignore())
               .ForMember(dest => dest.NetCost, mo => mo.Ignore())

               .ForMember(dest => dest.ApprovedBy, mo => mo.Ignore())
              
               .ForMember(dest => dest.ApprovalStatus, mo => mo.Ignore());


            CreateMap<Login, LoginModel>()
             .ForMember(dest => dest.LoginId, mo => mo.Ignore())
             .ForMember(dest => dest.Username, mo => mo.Ignore())
             .ForMember(dest => dest.Email, mo => mo.Ignore());
             //.ForMember(dest => dest.PasswordHash, mo => mo.Ignore());

        }
        
    }
}
