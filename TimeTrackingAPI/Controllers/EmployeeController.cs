﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeTrackingAPI.Models;

namespace TimeTrackingAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeeController : ControllerBase
    {
        private readonly TimeTrackingDBContext _context;

        public EmployeeController(TimeTrackingDBContext context)
        {
            _context = context;
        }

        // GET: api/Employee
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> GetEmployees()
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            return await _context.Employees.ToListAsync();
        }

        // GET: api/Employee/5
        [HttpGet("{id}")]
        public async Task<ActionResult<EmployeeEntity>> GetEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);

            if (employee == null)
            {
                return NotFound();
            }

            return employee;
        }

        // PUT: api/Employee/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmployee(int id, EmployeeEntity employee)
        {
            if (id != employee.EmployeeID)
            {
                return BadRequest();
            }

            _context.Entry(employee).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmployeeExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Employee
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<EmployeeEntity>> PostEmployee(EmployeeEntity employee)
        {
            if (_context.Employees == null)
            {
                return Problem("Entity set 'TimeTrackingDBContext.Employees'  is null.");
            }
            _context.Employees.Add(employee);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmployee", new { id = employee.EmployeeID }, employee);
        }

        // DELETE: api/Employee/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmployee(int id)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }
            var employee = await _context.Employees.FindAsync(id);
            if (employee == null)
            {
                return NotFound();
            }

            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool EmployeeExists(int id)
        {
            return (_context.Employees?.Any(e => e.EmployeeID == id)).GetValueOrDefault();
        }

        // GET: api/Employee/by-department/5
        [HttpGet("by-department/{departmentId}")]
        public async Task<ActionResult<IEnumerable<EmployeeEntity>>> GetEmployeesByDepartment(int departmentId)
        {
            if (_context.Employees == null)
            {
                return NotFound();
            }

            var employees = await _context.Employees
                                          .Where(e => e.DepartmentID == departmentId)
                                          .ToListAsync();

            return employees;
        }
    }
    }
