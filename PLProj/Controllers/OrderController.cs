using BLLProject.Interfaces;
using BLLProject.Repositories;
using DALProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using PLProj.Models;
using Stripe;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace PLProj.Controllers
{
    [Authorize(Roles = "Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            IEnumerable<OrdeHeader> orderHeaders;
            orderHeaders = _unitOfWork.orderHeaderRepository.GetAll(Includes: "Customer,Technician,Driver");
            return View(orderHeaders);
        }
        public IActionResult Details(int Id)
        {
            OrderVM orderVM = new OrderVM()
            {
                OrdeHeader = _unitOfWork.orderHeaderRepository.GetFirstOrDefault(u => u.Id == Id, Includes: "Customer,Driver,Technician"),
                OrderDetials = _unitOfWork.orderDetialsRepository.GetAll(x => x.OrdeHeaderId == Id, Includes: "PartService")

            };
            ViewData["Technicain"] = new SelectList(_unitOfWork.Repository<Technician>().GetAll().Select(e => new { Id = e.Id, Name = e.user.Name }), "Id", "Name");
            ViewData["Driver"] = new SelectList(_unitOfWork.Repository<Driver>().GetAll().Select(e => new { Id = e.Id, Name = e.user.Name }), "Id", "Name");

            return View(orderVM);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Details(OrderVM orderVM)
        {
            var orderfromdb = _unitOfWork.orderHeaderRepository.GetFirstOrDefault(u => u.Id == orderVM.OrdeHeader.Id);
                orderfromdb.Name = orderVM.OrdeHeader.Name;
                orderfromdb.ContactNumber = orderVM.OrdeHeader.ContactNumber;
                orderfromdb.Street = orderVM.OrdeHeader.Street;
                orderfromdb.City = orderVM.OrdeHeader.City;
                orderfromdb.TechnicianId = orderVM.OrdeHeader.TechnicianId ?? null; 
                orderfromdb.DriverId = orderVM.OrdeHeader.DriverId;
                orderfromdb.TrackingNumber = orderVM.OrdeHeader.TrackingNumber;
                orderfromdb.StartDateTime = orderVM.OrdeHeader.StartDateTime;
                _unitOfWork.orderHeaderRepository.Update(orderfromdb);
                _unitOfWork.Complete();
            TempData["Update"]="Item has Updated Successfully";

            return RedirectToAction("Details", "Order", new {Id = orderfromdb.Id});

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartProcess(OrderVM orderVM)
        {
            _unitOfWork.orderHeaderRepository.UpdateStatus(orderVM.OrdeHeader.Id, SD.Proccessing, null); 
            _unitOfWork.Complete();
            TempData["Update"]="Start Process Successfully";

            return RedirectToAction("Details", "Order", new { Id = orderVM.OrdeHeader.Id });

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult StartShip(OrderVM orderVM)
        {
            var orderfromdb = _unitOfWork.orderHeaderRepository.GetFirstOrDefault(u => u.Id == orderVM.OrdeHeader.Id);
            orderfromdb.TrackingNumber = orderVM.OrdeHeader.TrackingNumber;
            orderfromdb.TechnicianId = orderVM.OrdeHeader.TechnicianId ?? null;
            orderfromdb.DriverId = orderVM.OrdeHeader.DriverId;
            orderfromdb.ShippingDate = DateTime.Now;
            orderfromdb.OrderStatus = SD.Shipped;
            _unitOfWork.orderHeaderRepository.Update(orderfromdb);
            _unitOfWork.Complete();
            TempData["Update"] = "order Has Shipped Successfully";
            return RedirectToAction("Details", "Order", new { Id = orderVM.OrdeHeader.Id });

        }
        public IActionResult CancelOrder(OrderVM orderVM)
        {
            var orderfromdb = _unitOfWork.orderHeaderRepository.GetFirstOrDefault(u => u.Id == orderVM.OrdeHeader.Id);
            if(orderfromdb.PaymentStatus == SD.Approve)
            {
                var option = new RefundCreateOptions
                {
                    Reason= RefundReasons.RequestedByCustomer,
                    PaymentIntent= orderfromdb.PaymentIntentId
                };
                var service = new RefundService();
                Refund refund = service.Create(option);
                _unitOfWork.orderHeaderRepository.UpdateStatus(orderfromdb.Id,SD.Cancelled,SD.Refund);
            }
            else
            {
                _unitOfWork.orderHeaderRepository.UpdateStatus(orderfromdb.Id, SD.Cancelled, SD.Cancelled);

            }
            _unitOfWork.Complete();
            TempData["Update"] = "Order Has Cancelled Successfully";

            return RedirectToAction("Details", "Order", new { Id = orderVM.OrdeHeader.Id });

        }


    }
}
