using System;
using WechatBusiness.Api.ViewModels.ResultModels;
using WechatBusiness.Entities;
using AutoMapper;
using NetCore.Framework;
using WechatBusiness.Api.ViewModels.InputModels;
using NetCore.Framework.Snowflake;

namespace WechatBusiness.Api.AutoMappingProfiles
{
    public class MappingProfile : Profile, IProfile
    {
        public MappingProfile()
        {
            //此文件中添加所有的实体到实体间的映射
            ///CreateMap<AdminUsers, AdminUsersDto>();
            CreateMap<AdminUsersDto, AdminUsers>()
                .ForMember(p => p.Id, opt => opt.UseValue(SingletonIdWorker.GetInstance().NextId()))
                .ForMember(p => p.PassWord, opt => opt.MapFrom(src => EncryptDecrypt.EncryptMD5(src.PassWord)))
                .ForMember(p => p.State, opt => opt.UseValue<int>(0))
                .ForMember(p => p.ReferrerID, opt => opt.UseValue<int>(0))
                .ForMember(p => p.BusinessID, opt => opt.UseValue<int>(0))
                .ForMember(p => p.IsDisabled, opt => opt.UseValue<bool>(false))
                .ForMember(p => p.AddTime, opt => opt.UseValue<DateTime>(DateTime.Now))
                .ForMember(p => p.ValidityTime, opt => opt.UseValue<DateTime>(DateTime.Now))
                .ForMember(p => p.HeadPortraitUrl, opt => opt.Ignore()).ReverseMap();

            CreateMap<ProductInfoDto, ProductInfoToClass>()
            .ForMember(p => p.BusinessID, t => t.Ignore())
            .ForMember(p => p.ID, t => t.MapFrom(s => s.ID))
            .ForMember(p => p.ClassID, t => t.MapFrom(s => s.ClassID))
            .ForMember(p => p.ProductID, t => t.MapFrom(s => s.ProductID)).IgnoreAllPropertiesWithAnInaccessibleSetter();
            ////Source->Destination2
            ////CreateMap<Source, Destination2>().ForMember(d => d.AnotherValue2, opt =>
            ////{
            ////    opt.MapFrom(s => s.AnotherValue);
            ////});

            //Mapper.AssertConfigurationIsValid();
        }
    }
}