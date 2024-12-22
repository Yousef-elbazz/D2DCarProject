using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using PLProj.Models;

using Stripe.Checkout;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;


namespace PLProj.Controllers
{
    [Authorize(Roles = "Customer")]
    public class NewCartController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        public NewCartController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;


        }
        public async Task<IActionResult> Index()
        {

            var _user = await _userManager.GetUserAsync(User);


            var customerSpec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            var customer = _unitOfWork.Repository<Customer>().GetEntityWithSpec(customerSpec);

            if (customer == null)
            {
                TempData["error"] = "Customer not found.";
                return RedirectToAction("Error");
            }


            ShoppingCartVM cartVM = new ShoppingCartVM
            {
                CartList = _unitOfWork.shoppingCart.GetAll(
                    d => d.CustomerId == customer.Id,
                    Includes: "PartService"
                ),

            };


            foreach (var item in cartVM.CartList)
            {
                cartVM.TotalCarts += (item.count * item.PartService.Price);
            }

            return View(cartVM);
        }
        public IActionResult Plus(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            _unitOfWork.shoppingCart.IncreaseCount(shoopingcart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");

        }
        public IActionResult Minus(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            if (shoopingcart.count <= 1)
            {
                _unitOfWork.shoppingCart.Delete(shoopingcart);
                _unitOfWork.Complete();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                _unitOfWork.shoppingCart.DecreaseCount(shoopingcart, 1);
            }


            _unitOfWork.Complete();
            return RedirectToAction("Index");

        }
        public IActionResult Delete(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            _unitOfWork.shoppingCart.Delete(shoopingcart);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Summary()
        {
			var _user = await _userManager.GetUserAsync(User);


			var customerSpec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            
            
            var customer = _unitOfWork.Repository<Customer>().GetEntityWithSpec(customerSpec);
            

			if (customer == null)
			{
				TempData["error"] = "Customer not found.";
				return RedirectToAction("Error");
			}


            ShoppingCartVM cartVM = new ShoppingCartVM
            {
                CartList = _unitOfWork.shoppingCart.GetAll(
                    d => d.CustomerId == customer.Id,
                    Includes: "PartService"
                ),
                OrdeHeader = new(),
                

            };
              cartVM.OrdeHeader.Customer= _unitOfWork.Repository<Customer>().GetFirstOrDefault(x=>x.Id ==customer.Id);
            if (cartVM.OrdeHeader != null && cartVM.OrdeHeader.Customer != null && cartVM.OrdeHeader.Customer.AppUser != null)
            {
                cartVM.OrdeHeader.Name = cartVM.OrdeHeader.Customer.AppUser.Name;
                cartVM.OrdeHeader.City = cartVM.OrdeHeader.Customer.AppUser.City;
                cartVM.OrdeHeader.Street = cartVM.OrdeHeader.Customer.AppUser.Street;
                cartVM.OrdeHeader.ContactNumber = cartVM.OrdeHeader.Customer.AppUser.ContactNumber;
            }
            else
            {
                ModelState.AddModelError("", "Some required data is missing.");
                return View(cartVM); 
            }

            foreach (var item in cartVM.CartList)
			{
				cartVM.OrdeHeader.TotalPrice += (item.count * item.PartService.Price);
			}

			return View(cartVM);
		}

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Summary(ShoppingCartVM shoppingCartVM)
        {
			var _user = await _userManager.GetUserAsync(User);
       		var customerSpec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);            
			var customer = _unitOfWork.Repository<Customer>().GetEntityWithSpec(customerSpec);
            shoppingCartVM.CartList = _unitOfWork.shoppingCart.GetAll(u => u.CustomerId == customer.Id, Includes: "PartService");
            
            if (customer == null)
			{
				TempData["error"] = "Customer not found.";
				return RedirectToAction("Error");
			}            
            shoppingCartVM.OrdeHeader.OrderStatus = SD.Pending;
            shoppingCartVM.OrdeHeader.PaymentStatus = SD.Pending;
            shoppingCartVM.OrdeHeader.OrderDate = DateTime.Now;
            shoppingCartVM.OrdeHeader.Customer = customer;
            

            foreach (var item in shoppingCartVM.CartList)
			{
				shoppingCartVM.OrdeHeader.TotalPrice += (item.count * item.PartService.Price);
			}
            _unitOfWork.orderHeaderRepository.Add(shoppingCartVM.OrdeHeader);
            _unitOfWork.Complete();
            foreach(var item in shoppingCartVM.CartList)
            {
                OrderDetial orderDetial = new OrderDetial()
                {
                    PartServiceId= item.PartServiceId,
                    OrdeHeaderId=shoppingCartVM.OrdeHeader.Id,
                    Price=item.PartService.Price,
                    count=item.count
                };
                _unitOfWork.orderDetialsRepository.Add(orderDetial);
                _unitOfWork.Complete();

            }

            
            var domain = "https://localhost:44355/";
            var option = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>(),
               Mode = "payment",
               SuccessUrl = domain + $"NewCart/orderconfirmation?id={shoppingCartVM.OrdeHeader.Id}",
                CancelUrl = domain + $"Newcart/index",
            };
            foreach (var item in shoppingCartVM.CartList)
            {
                var sessionLineOption = new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long)(item.PartService.Price * 100),
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.PartService.Name,
                        }
                    },
                    Quantity = item.count,
                };
                option.LineItems.Add(sessionLineOption);
            }

            var service = new SessionService();
            Session session = service.Create(option);
            shoppingCartVM.OrdeHeader.PaymentIntentId = session.PaymentIntentId;
            shoppingCartVM.OrdeHeader.SessionId = session.Id;

            _unitOfWork.Complete();


            Response.Headers.Add("Location", session.Url);
            return new StatusCodeResult(303);
        }

        public IActionResult OrderConfirmation(int id)
        {
            OrdeHeader orderHeader = _unitOfWork.orderHeaderRepository.GetFirstOrDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            if (session.PaymentStatus.ToLower() == "paid")
            {
                _unitOfWork.orderHeaderRepository.UpdateStatus(id, SD.Approve, SD.Approve);
                orderHeader.PaymentIntentId = session.PaymentIntentId;
                _unitOfWork.Complete();
            }
            List<ShoppingCart> shoppingCarts = _unitOfWork.shoppingCart
                .GetAll(u => u.CustomerId == orderHeader.CustomerId).ToList();
            _unitOfWork.shoppingCart.RemoveRange(shoppingCarts);
            _unitOfWork.Complete();
            return View(id);
        }






    }
}

