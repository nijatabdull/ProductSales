﻿using AutoMapper;
using ProductSale.Services.Order.BL.Dtos;

namespace ProductSale.Services.Order.BL.Statics
{
    public class CustomMapping : Profile
    {
        public CustomMapping()
        {
            CreateMap<Core.Models.Order, OrderDto>().ReverseMap();
            CreateMap<Core.Models.OrderItem, OrderItemDto>().ReverseMap();
            CreateMap<Core.Models.Address, AddressDto>().ReverseMap();
        }
    }
}
