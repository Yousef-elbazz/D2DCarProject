﻿using BLLProject.Interfaces;
using BLLProject.Specifications;
using DALProject.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PLProj.Models;
using System;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace PLProj.Controllers
{
	public class CarController : Controller
	{
        private readonly UserManager<AppUser> _userManager;
        private readonly ILogger<HomeController> _logger;
		private readonly IUnitOfWork unitOfWork;
        private readonly IWebHostEnvironment env;

        public CarController(UserManager<AppUser> userManager, ILogger<HomeController> logger, IUnitOfWork unitOfWork, IWebHostEnvironment env)
		{
            _userManager = userManager;
            _logger = logger;
			this.unitOfWork = unitOfWork;
            this.env = env;
        }
		public IActionResult Get()
		{
			var spec = new BaseSpecification<Car>();
			spec.Includes.Add(e => e.Model);
			spec.Includes.Add(e => e.Model.Brand);
			spec.Includes.Add(e => e.Color);

			ViewData["Models"] = unitOfWork.Repository<Model>().GetAll();
			ViewData["Brands"] = unitOfWork.Repository<Brand>().GetAll();

            var models = unitOfWork.Repository<Car>().GetAllWithSpec(spec)
				.Select(s => (CarViewModel)s).ToList();
            return View(models);
		}

		public IActionResult CreateCar()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> CreateCar(CarViewModel car)
		{

			var _user = await _userManager.GetUserAsync(User);
			var spec = new BaseSpecification<Customer>(c => c.AppUserId == _user.Id);
			var customer = unitOfWork.Repository<Customer>().GetEntityWithSpec(spec);

            if (ModelState.IsValid)
			{
				unitOfWork.Repository<Car>().Add((Car)car);
				var count = unitOfWork.Complete();
				if (count > 0)
				{
					TempData["message"] = "vehicle has been Added Successfully";
					return RedirectToAction("Get", "Car");
				}
			}
			return View(car);
		}

		[HttpGet]
		public IActionResult GetModelByBrands(int BrandId)
		{
			var spec = new BaseSpecification<Model>(e => e.BrandId == BrandId);
			var Models = unitOfWork.Repository<Model>().GetAllWithSpec(spec)
				.Select(e => new { Id = e.Id, Name = e.Name }).ToList();

			return new JsonResult(Models);
		}


        #region Details

        public IActionResult Details(int? Id, string viewName = "Details")
        {
            if (!Id.HasValue)
                return BadRequest();

            var spec = new BaseSpecification<Car>(e => e.Id == Id.Value);
            spec.Includes.Add(e => e.Model);
            spec.Includes.Add(e => e.Model.Brand);
            spec.Includes.Add(e => e.Color);
            var service = unitOfWork.Repository<Car>().GetEntityWithSpec(spec);

            if (service is null)
                return NotFound();

            return View(viewName, (CarViewModel)service);
        }

        #endregion

        #region Edit

        public IActionResult Edit(int? Id)
        {
            return Details(Id, nameof(Edit));
        }

        [HttpPost]
        public IActionResult Edit(CarViewModel emp)
        {
            if (!ModelState.IsValid)
                return View(emp);

            try
            {
                unitOfWork.Repository<Car>().Update((Car)emp);
                unitOfWork.Complete();
                TempData["message"] = "Service Updated Successfully";
                return RedirectToAction(nameof(Get));
            }
            catch (Exception ex)
            {
                if (env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Updating the Employee");
                }
                return View(emp);
            }
        }

        #endregion

        #region Delete
        public IActionResult Delete(int? Id)
        {
            return Details(Id, nameof(Delete));
        }

        [HttpPost]
        public IActionResult Delete(CarViewModel sev)
        {
            try
            {

                unitOfWork.Repository<Car>().Delete((Car)sev);
                unitOfWork.Complete();
                TempData["message"] = "Service Deleted Successfully";
                return RedirectToAction(nameof(Get));
            }
            catch (Exception ex)
            {

                if (env.IsDevelopment())
                {
                    ModelState.AddModelError(string.Empty, ex.Message);
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "An Error Has Occurred during Deleted the Service");
                }
                return View(sev);
            }
        }
        #endregion


    }
}
