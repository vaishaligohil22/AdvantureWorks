using AdventureWorks.Models;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AdventureWorks
{
    public class AutoMapping : Profile
    {
        public AutoMapping()
        {
            CreateMap<DBModels.Product, Models.Product>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.ProductNumber, opt => opt.MapFrom(src => src.ProductNumber))
                .ForMember(dest => dest.Color, opt => opt.MapFrom(src => src.Color))
                .ForMember(dest => dest.StandardCost, opt => opt.MapFrom(src => src.StandardCost))
                .ForMember(dest => dest.ListPrice, opt => opt.MapFrom(src => src.ListPrice))
                .ForMember(dest => dest.Size, opt => opt.MapFrom(src => src.Size))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ForMember(dest => dest.ProductLine, opt => opt.MapFrom(src => src.ProductLine))
                .ForMember(dest => dest.Class, opt => opt.MapFrom(src => src.Class))
                .ForMember(dest => dest.Style, opt => opt.MapFrom(src => src.Style));

            CreateMap<DBModels.ProductSubcategory, Models.ProductSubCategory>()
                .ForMember(dest => dest.ProductSubCategoryID, opt => opt.MapFrom(src => src.ProductSubcategoryId))
                .ForMember(dest => dest.ProductCategoryID, opt => opt.MapFrom(src => src.ProductCategory))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<DBModels.ProductCategory, Models.ProductCategory>()
                .ForMember(dest => dest.ProductCategoryID, opt => opt.MapFrom(src => src.ProductCategoryId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name));

            CreateMap<DBModels.ProductPhoto, Models.ProductPhoto>()
                .ForMember(dest => dest.ProductPhotoID, opt => opt.MapFrom(src => src.ProductPhotoId))
                .ForMember(dest => dest.ThumbNailPhoto, opt => opt.MapFrom(src => Convert.ToBase64String(src.ThumbNailPhoto)))
                .ForMember(dest => dest.ThumbNailPhotoFileName, opt => opt.MapFrom(src => src.ThumbnailPhotoFileName))
                .ForMember(dest => dest.LargePhoto, opt => opt.MapFrom(src => Convert.ToBase64String(src.LargePhoto)))
                .ForMember(dest => dest.LargePhotoFileName, opt => opt.MapFrom(src => src.LargePhotoFileName));

            CreateMap<DBModels.ProductModel, Models.ProductModel>()
                .ForMember(dest => dest.ProductModelID, opt => opt.MapFrom(src => src.ProductModelId))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.CatalogDescription, opt => opt.MapFrom(src => src.CatalogDescription))
                .ForMember(dest => dest.Instructions, opt => opt.MapFrom(src => src.Instructions));

            CreateMap<DBModels.ProductReview, Models.ProductReview>()
               .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
               .ForMember(dest => dest.ProductReviewID, opt => opt.MapFrom(src => src.ProductReviewId))
               .ForMember(dest => dest.Rating, opt => opt.MapFrom(src => src.Rating))
               .ForMember(dest => dest.ReviewDate, opt => opt.MapFrom(src => src.ReviewDate))
               .ForMember(dest => dest.ReviewerName, opt => opt.MapFrom(src => src.ReviewerName))
               .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(src => src.EmailAddress))
               .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
        }
    }
}
