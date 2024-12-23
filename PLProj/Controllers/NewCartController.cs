using BLLProject.Interfaces;
using BLLProject.Repositories;
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
    
    public class NewCartController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IUnitOfWork _unitOfWork;
        #region Customer
        public NewCartController(IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;


        }
        [Authorize(Roles = "Customer")]
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
        [Authorize(Roles = "Customer")]
        public IActionResult Plus(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            _unitOfWork.shoppingCart.IncreaseCount(shoopingcart, 1);
            _unitOfWork.Complete();
            return RedirectToAction("Index");

        }
        [Authorize(Roles = "Customer")]
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
        [Authorize(Roles = "Customer")]
        public IActionResult Delete(int cartid)
        {
            var shoopingcart = _unitOfWork.shoppingCart.GetFirstOrDefault(x => x.ShoppingCartId == cartid);
            _unitOfWork.shoppingCart.Delete(shoopingcart);
            _unitOfWork.Complete();
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Customer")]

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
            cartVM.OrdeHeader.Customer = _unitOfWork.Repository<Customer>().GetFirstOrDefault(x => x.Id == customer.Id);
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
        [Authorize(Roles = "Customer")]
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
            foreach (var item in shoppingCartVM.CartList)
            {
                OrderDetial orderDetial = new OrderDetial()
                {
                    PartServiceId = item.PartServiceId,
                    OrdeHeaderId = shoppingCartVM.OrdeHeader.Id,
                    Price = item.PartService.Price,
                    count = item.count
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
        [Authorize(Roles = "Customer")]
        public IActionResult OrderConfirmation(int id)
        {
            OrdeHeader orderHeader = _unitOfWork.orderHeaderRepository.GetFirstOrDefault(u => u.Id == id);
            var service = new SessionService();
            Session session = service.Get(orderHeader.SessionId);
            orderHeader.PaymentIntentId = session.PaymentIntentId;
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


        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> MyOrders()
        {
            // الحصول على المستخدم الحالي
            var _user = await _userManager.GetUserAsync(User);

            // إنشاء مواصفات لتحميل العميل مع الطلبات (OrdeHeaders)
            var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
            spec.Includes.Add(c => c.OrdeHeaders);

            // استرجاع العميل والطلبات
            var customer = _unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);

            // التأكد من وجود العميل والطلبات
            if (customer == null || customer.OrdeHeaders == null)
            {
                return View(new List<OrdeHeader>()); // عرض صفحة فارغة
            }

            // تمرير الطلبات إلى الـ View
            return View(customer.OrdeHeaders.ToList());
        }
        [Authorize(Roles = "Customer")]
        public async Task<IActionResult> OrderDetails(int Id)
        {
            // الحصول على المستخدم الحالي
            var user = await _userManager.GetUserAsync(User);

            // التحقق من أن المستخدم الحالي هو صاحب الطلب
            var specCustomer = new BaseSpecification<Customer>(c => c.AppUserId == user.Id);
            specCustomer.Includes.Add(c => c.OrdeHeaders);
            var customer = _unitOfWork.Repository<Customer>().GetEntityWithSpec(specCustomer);

            if (customer == null || !customer.OrdeHeaders.Any(o => o.Id == Id))
            {
                TempData["Error"] = "You are not authorized to view this order.";
                return RedirectToAction("MyOrders");
            }

            // استرجاع تفاصيل الطلب
            var specOrderDetails = new BaseSpecification<OrderDetial>(d => d.OrdeHeaderId == Id);
            specOrderDetails.Includes.Add(d => d.PartService);
            specOrderDetails.Includes.Add(d => d.OrdeHeader);

            var orderDetails = _unitOfWork.Repository<OrderDetial>().GetAllWithSpec(specOrderDetails);

            if (!orderDetails.Any())
            {
                TempData["Error"] = "No details found for this order.";
                return RedirectToAction("MyOrders");
            }

            return View(orderDetails);
        }


        #endregion

        #region Technician
        [Authorize(Roles = "Technician")]
        public async Task<IActionResult> MyOrdersForTechnician()
        {
            var _user = await _userManager.GetUserAsync(User);            
            var spec = new BaseSpecification<Technician>(c => c.AppUserId == _user.Id);
            spec.Includes.Add(c => c.OrdeHeaders);           
            var technician = _unitOfWork.Repository<Technician>().GetEntityWithSpec(spec);          
            if (technician == null || technician.OrdeHeaders == null)
            {
                return View(new List<OrdeHeader>()); 
            }
			var activeOrders = technician.OrdeHeaders.Where(o => o.OrderStatus != "Finished").ToList();
			return View(activeOrders);            
        }
        [Authorize(Roles = "Technician")]
        public async Task<IActionResult> FinishOrder(int id)
        {
            var Spec = new BaseSpecification<OrdeHeader>(e => e.Id == id);
            var order = _unitOfWork.Repository<OrdeHeader>().GetEntityWithSpec(Spec);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost]
        [Authorize(Roles = "Technician")]
        public async Task<IActionResult> FinishOrder(OrdeHeader ordeHeader)
        {
            if (ModelState.IsValid)
            {
                var Spec = new BaseSpecification<OrdeHeader>(e => e.Id == ordeHeader.Id);
                var order = _unitOfWork.Repository<OrdeHeader>().GetEntityWithSpec(Spec);

                if (order != null)
                {
                    order.EndtDateTime = ordeHeader.EndtDateTime;
                    order.FinalReport = ordeHeader.FinalReport;
					order.OrderStatus = "Finished";

					_unitOfWork.Repository<OrdeHeader>().Update(order);
                    _unitOfWork.Complete();

                    TempData["Message"] = "Order has been finished successfully";
                    return RedirectToAction(nameof(MyOrdersForTechnician));
                }

                return NotFound();
            }

            return View(ordeHeader);
        }
        #endregion

        #region Driver
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> MyOrdersForDriver()
        {
            var _user = await _userManager.GetUserAsync(User);
            var spec = new BaseSpecification<Driver>(c => c.AppUserId == _user.Id);
            spec.Includes.Add(c => c.OrdeHeaders);
            var driver = _unitOfWork.Repository<Driver>().GetEntityWithSpec(spec);
            if (driver == null || driver.OrdeHeaders == null)
            {
                return View(new List<OrdeHeader>());
            }

            var activeOrders = driver.OrdeHeaders.Where(o => o.OrderStatus != "Finish").ToList();
            return View(activeOrders);
        }
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> FinishOrderDriver(int id)
        {
            var Spec = new BaseSpecification<OrdeHeader>(e => e.Id == id);
            var order = _unitOfWork.Repository<OrdeHeader>().GetEntityWithSpec(Spec);

            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }
        [HttpPost]
        [Authorize(Roles = "Driver")]
        public async Task<IActionResult> FinishOrderDriver(OrdeHeader ordeHeader)
        {
            if (ModelState.IsValid)
            {
                var Spec = new BaseSpecification<OrdeHeader>(e => e.Id == ordeHeader.Id);
                var order = _unitOfWork.Repository<OrdeHeader>().GetEntityWithSpec(Spec);

                if (order != null)
                {
                    order.EndtDateTime = ordeHeader.EndtDateTime;
                    order.FinalReport = ordeHeader.FinalReport;
                    order.OrderStatus = "Finish";

                    _unitOfWork.Repository<OrdeHeader>().Update(order);
                    _unitOfWork.Complete();

                    TempData["Message"] = "Order has been finished successfully";
                    return RedirectToAction(nameof(MyOrdersForTechnician));
                }

                return NotFound();
            }

            return View(ordeHeader);
        }
        #endregion
    }
}

