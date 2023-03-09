using Microsoft.AspNetCore.Mvc;
using ProductSale.Web.Models.Order;
using ProductSale.Web.Services.Abstractions;

namespace ProductSale.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            OrderCreatedViewModel orderCreatedViewModel = await _orderService.CreateOrder(new CheckoutInputModel()
            {
                Street = "Baxcali",
                District = "Masazir",
                Province = "Abseron",
                ZipCode = "3169",
                CardNumber = "4169741431697231",
                CardName = "Kapital",
                CVV = "456",
                Expiration = "12/23",
                Line = "asd"
            });

            TempData["OrderId"] = orderCreatedViewModel.OrderId;

            return RedirectToAction("Index", "Course");
        }
    }
}
