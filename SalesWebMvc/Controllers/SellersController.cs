﻿using Microsoft.AspNetCore.Mvc;
using SalesWebMvc.Models;
using SalesWebMvc.Models.ViewModels;
using SalesWebMvc.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Controllers
{
  public class SellersController : Controller
  {
    private SellerService _sellerService;
    private DepartmentService _departmentService;

    public SellersController(SellerService sellerService, DepartmentService departmentService)
    {
      _sellerService = sellerService;
      _departmentService = departmentService;
    }

    public IActionResult Index()
    {
      var list = _sellerService.FindAll();
      return View(list);
    }

    public IActionResult Create()
    {
      var departments = _departmentService.FindAll();
      var viewModel = new SellerFormViewModel() { Departments = departments };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(Seller seller)
    {
      _sellerService.Insert(seller);
      return RedirectToAction(nameof(Index));
    }

    public IActionResult Delete(int? id)
    {
      if (id == null)
        return NotFound();

      var seller = _sellerService.FindById(id.Value);
      if (seller == null)
        return NotFound();

      return View(seller);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Delete(int id)
    {
      _sellerService.Remove(id);
      return RedirectToAction(nameof(Index));
    }

    public IActionResult Details(int? id)
    {
      if (id == null)
        return NotFound();

      var seller = _sellerService.FindById(id.Value);
      if (seller == null)
        return NotFound();

      return View(seller);
    }

    public IActionResult Edit(int? id)
    {
      if(id == null)
        return NotFound();

      var obj = _sellerService.FindById(id.Value);
      if (obj == null)
        return NotFound();

      List<Department> departments = _departmentService.FindAll();
      SellerFormViewModel viewModel = new SellerFormViewModel() { Seller = obj, Departments = departments };

      return View(viewModel);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Edit(int id, Seller seller)
    {
      if (id != seller.Id)
        return BadRequest();

      try
      {
        _sellerService.Update(seller);
        return RedirectToAction(nameof(Index));
      }
      catch(NotFoundException)
      {
        return NotFound();
      }
      catch (DbConcurrencyException)
      {
        return BadRequest();
      }
    }
  }
}
