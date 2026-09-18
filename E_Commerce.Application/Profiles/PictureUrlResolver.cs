using AutoMapper;
using E_Commerce.Application.DTOs.Products;
using E_Commerce.Domain.Entities.Products;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace E_Commerce.Application.Profiles
{
    internal class PictureUrlResolver : IValueResolver<Product, ProductDto, string>
    {
        private readonly IOptions<UrlSettings> options;

        private readonly UrlSettings _urlsettings;
        public PictureUrlResolver(IOptions<UrlSettings>options)
        {
            _urlsettings = options.Value;
        }
        public string Resolve(Product source, ProductDto destination, string destMember, ResolutionContext context)
        {
            //source : "images/products/FormalBlazer.jpg"
            //destination : "https://localhost:7108/files/images/products/FormalBlazer.jpg"
            var baseUrl = _urlsettings.BaseUrl.TrimEnd('/');
            var path = source.PictureUrl.TrimStart('/');
            return $"{baseUrl}/Files/{path}";

        }
    }
    public class UrlSettings
    {
        public string BaseUrl { get; set; } = default!;
    }
}
