using Microsoft.EntityFrameworkCore;
using SalesWebMvc.Data;
using SalesWebMvc.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SalesWebMvc.Services.Exceptions;

namespace SalesWebMvc.Services
{
  public class SellerService
  {
    private SalesWebMvcContext _context;

    public SellerService(SalesWebMvcContext context)
    {
      _context = context;
    }

    public List<Seller> FindAll()
    {
      return _context.Seller.ToList();
    }

    public async Task<List<Seller>> FindAllAsync()
    {
      return await _context.Seller.ToListAsync();
    }

    public async Task InsertAsync(Seller obj)
    {
      _context.Add(obj);
      await _context.SaveChangesAsync();
    }

    public Seller FindById(int id)
    {
      return _context.Seller.Include(obj => obj.Department).FirstOrDefault(s => s.Id == id);
    }

    public async Task<Seller> FindByIdAsync(int id)
    {
      return await _context.Seller.Include(obj => obj.Department).FirstOrDefaultAsync(s => s.Id == id);
    }

    public void Remove(int id)
    {
      var seller = FindById(id);
      _context.Seller.Remove(seller);
      _context.SaveChanges();
    }

    public async Task RemoveAsync(int id)
    {
      try
      {
        var seller = await FindByIdAsync(id);
        _context.Seller.Remove(seller);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateException e)
      {
        throw new IntegrityException(e.Message);
      }
    }

    public void Update(Seller obj)
    {
      if (!_context.Seller.Any(x => x.Id == obj.Id))
      {
        throw new NotFoundException("ID not found");
      }

      try
      {
        _context.Update(obj);
        _context.SaveChanges();
      }
      catch (DbUpdateConcurrencyException e)
      {
        throw new DbConcurrencyException(e.Message);
      }
    }

    public async Task UpdateAsync(Seller obj)
    {
      var hasAny = await _context.Seller.AnyAsync(x => x.Id == obj.Id);
      if (!hasAny)
      {
        throw new NotFoundException("ID not found");
      }

      try
      {
        _context.Update(obj);
        await _context.SaveChangesAsync();
      }
      catch (DbUpdateConcurrencyException e)
      {
        throw new DbConcurrencyException(e.Message);
      }
    }
  }
}
