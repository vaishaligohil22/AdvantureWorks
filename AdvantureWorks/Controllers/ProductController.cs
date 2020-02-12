using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AdventureWorks.Attributes;
using AdventureWorks.DBModels;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace AdventureWorks.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly AdventureWorks2017Context _context;
        private readonly IMapper _mapper;

        public ProductController(AdventureWorks2017Context context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        //[HttpGet]
        //[Route("/v1/product/search")]
        //[ValidateModelState]
        //public virtual IActionResult FindProduct([FromQuery]string name, [FromQuery]string color, [FromQuery]string category, [FromQuery]string subCategory, [FromQuery]string model, [FromQuery]int? size, [FromQuery]int? minPrice, [FromQuery][Range(1, 100)]int? maxPrice)
        //{
        //    string exampleJson = null;
        //    exampleJson = "<Product>\n  <productId>123456789</productId>\n  <name>aeiou</name>\n  <productNumber>aeiou</productNumber>\n  <color>aeiou</color>\n  <standardCost>3.149</standardCost>\n  <listPrice>3.149</listPrice>\n  <size>aeiou</size>\n  <weight>aeiou</weight>\n  <productLine>aeiou</productLine>\n  <class>aeiou</class>\n  <style>aeiou</style>\n  <review>\n  </review>\n</Product>";
        //    exampleJson = "[ {\n  \"subCategory\" : {\n    \"productCategoryID\" : {\n      \"productCategoryID\" : 5,\n      \"name\" : \"name\"\n    },\n    \"productSubCategoryID\" : 5,\n    \"name\" : \"name\"\n  },\n  \"productId\" : 0,\n  \"color\" : \"color\",\n  \"weight\" : \"weight\",\n  \"photo\" : {\n    \"largePhotoFileName\" : \"largePhotoFileName\",\n    \"thumbNailPhoto\" : \"thumbNailPhoto\",\n    \"productPhotoID\" : 7,\n    \"largePhoto\" : \"largePhoto\",\n    \"thumbNailPhotoFileName\" : \"thumbNailPhotoFileName\"\n  },\n  \"productNumber\" : \"productNumber\",\n  \"standardCost\" : 6.027456183070403,\n  \"productLine\" : \"productLine\",\n  \"listPrice\" : 1.4658129805029452,\n  \"size\" : \"size\",\n  \"review\" : [ {\n    \"reviewerName\" : \"reviewerName\",\n    \"emailAddress\" : \"emailAddress\",\n    \"comments\" : \"comments\",\n    \"productId\" : 3,\n    \"reviewDate\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"rating\" : 2,\n    \"productReviewID\" : 9\n  }, {\n    \"reviewerName\" : \"reviewerName\",\n    \"emailAddress\" : \"emailAddress\",\n    \"comments\" : \"comments\",\n    \"productId\" : 3,\n    \"reviewDate\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"rating\" : 2,\n    \"productReviewID\" : 9\n  } ],\n  \"name\" : \"name\",\n  \"style\" : \"style\",\n  \"model\" : {\n    \"instructions\" : \"instructions\",\n    \"name\" : \"name\",\n    \"productModelID\" : 2,\n    \"catalogDescription\" : \"catalogDescription\"\n  },\n  \"class\" : \"class\"\n}, {\n  \"subCategory\" : {\n    \"productCategoryID\" : {\n      \"productCategoryID\" : 5,\n      \"name\" : \"name\"\n    },\n    \"productSubCategoryID\" : 5,\n    \"name\" : \"name\"\n  },\n  \"productId\" : 0,\n  \"color\" : \"color\",\n  \"weight\" : \"weight\",\n  \"photo\" : {\n    \"largePhotoFileName\" : \"largePhotoFileName\",\n    \"thumbNailPhoto\" : \"thumbNailPhoto\",\n    \"productPhotoID\" : 7,\n    \"largePhoto\" : \"largePhoto\",\n    \"thumbNailPhotoFileName\" : \"thumbNailPhotoFileName\"\n  },\n  \"productNumber\" : \"productNumber\",\n  \"standardCost\" : 6.027456183070403,\n  \"productLine\" : \"productLine\",\n  \"listPrice\" : 1.4658129805029452,\n  \"size\" : \"size\",\n  \"review\" : [ {\n    \"reviewerName\" : \"reviewerName\",\n    \"emailAddress\" : \"emailAddress\",\n    \"comments\" : \"comments\",\n    \"productId\" : 3,\n    \"reviewDate\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"rating\" : 2,\n    \"productReviewID\" : 9\n  }, {\n    \"reviewerName\" : \"reviewerName\",\n    \"emailAddress\" : \"emailAddress\",\n    \"comments\" : \"comments\",\n    \"productId\" : 3,\n    \"reviewDate\" : \"2000-01-23T04:56:07.000+00:00\",\n    \"rating\" : 2,\n    \"productReviewID\" : 9\n  } ],\n  \"name\" : \"name\",\n  \"style\" : \"style\",\n  \"model\" : {\n    \"instructions\" : \"instructions\",\n    \"name\" : \"name\",\n    \"productModelID\" : 2,\n    \"catalogDescription\" : \"catalogDescription\"\n  },\n  \"class\" : \"class\"\n} ]";

        //    var example = exampleJson != null
        //    ? JsonConvert.DeserializeObject<List<DBModels.Product>>(exampleJson)
        //    : default(List<DBModels.Product>);
        //    return new ObjectResult(example);
        //}

        [HttpGet]
        [Route("/v1/product")]
        [ValidateModelState]
        public IActionResult GetProduct()
        {
            var product = (from p in _context.Product
                           from pm in _context.ProductModel.Where(pm => pm.ProductModelId == p.ProductModelId).DefaultIfEmpty()
                           from ps in _context.ProductSubcategory.Where(ps => ps.ProductSubcategoryId == p.ProductSubcategoryId).DefaultIfEmpty()
                           from pc in _context.ProductCategory.Where(pc => pc.ProductCategoryId == ps.ProductCategoryId).DefaultIfEmpty()
                           from ppp in _context.ProductProductPhoto.Where(ppp => ppp.ProductId == p.ProductId).DefaultIfEmpty()
                           from pp in _context.ProductPhoto.Where(pp => pp.ProductPhotoId == ppp.ProductPhotoId).DefaultIfEmpty()
                           orderby p.ProductId
                           //where p.ProductId == 798
                           select new Models.Product
                           {
                               ProductId = p.ProductId,
                               Name = p.Name,
                               ProductNumber = p.ProductNumber,
                               Color = p.Color,
                               StandardCost = (double)p.StandardCost,
                               ListPrice = (double)p.ListPrice,
                               Size = p.Size,
                               Weight = (decimal)p.Weight,
                               ProductLine = p.ProductLine,
                               Class = p.Class,
                               Style = p.Style,
                               Model = _mapper.Map<Models.ProductModel>(pm),
                               SubCategory = _mapper.Map<Models.ProductSubCategory>(ps),
                               Photo = _mapper.Map<Models.ProductPhoto>(pp),
                               Review = _mapper.Map<List<Models.ProductReview>>(_context.ProductReview.Where(x => x.ProductId == p.ProductId).ToList())
                           }).ToList();

            //List <Models.Product> prod = _mapper.Map<List<Models.Product>>(product);

            return new ObjectResult(product);
        }
        
        [HttpGet]
        [Route("/v1/product/{productId}")]
        [ValidateModelState]
        public IActionResult GetProduct([FromRoute][Required]int productId)
        {
            Models.Product product = (from p in _context.Product
                                      from pm in _context.ProductModel.Where(pm => pm.ProductModelId == p.ProductModelId).DefaultIfEmpty()
                                      from ps in _context.ProductSubcategory.Where(ps => ps.ProductSubcategoryId == p.ProductSubcategoryId).DefaultIfEmpty()
                                      from pc in _context.ProductCategory.Where(pc => pc.ProductCategoryId == ps.ProductCategoryId).DefaultIfEmpty()
                                      from ppp in _context.ProductProductPhoto.Where(ppp => ppp.ProductId == p.ProductId).DefaultIfEmpty()
                                      from pp in _context.ProductPhoto.Where(pp => pp.ProductPhotoId == ppp.ProductPhotoId).DefaultIfEmpty()
                                      where p.ProductId == productId
                                      select new Models.Product
                                      {
                                          ProductId = p.ProductId,
                                          Name = p.Name,
                                          ProductNumber = p.ProductNumber,
                                          Color = p.Color,
                                          StandardCost = (double)p.StandardCost,
                                          ListPrice = (double)p.ListPrice,
                                          Size = p.Size,
                                          Weight = (decimal)p.Weight,
                                          ProductLine = p.ProductLine,
                                          Class = p.Class,
                                          Style = p.Style,
                                          Model = _mapper.Map<Models.ProductModel>(pm),
                                          SubCategory = _mapper.Map<Models.ProductSubCategory>(ps),
                                          Photo = _mapper.Map<Models.ProductPhoto>(pp),
                                          Review = _mapper.Map<List<Models.ProductReview>>(_context.ProductReview.Where(x => x.ProductId == p.ProductId).ToList())
                                      }).FirstOrDefault();

            if (product == null)
            {
                return NotFound();
            }

            return new ObjectResult(product);
        }

        [HttpGet]
        [Route("/v1/product/category")]
        [ValidateModelState]
        public IActionResult GetProductCategory()
        {
            var category = (from p in _context.ProductCategory
                            select new
                            {
                                ProductCategoryID = p.ProductCategoryId,
                                Name = p.Name,
                                SubCategory = _context.ProductSubcategory.Where(x => x.ProductCategoryId == p.ProductCategoryId).Select(p => new { p.ProductSubcategoryId, p.Name }).ToList()
                            }).ToList();

            if (category == null)
            {
                return NotFound();
            }

            return new ObjectResult(category);
        }
    }
}