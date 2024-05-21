using EmploymentSystem.Data;
using EmploymentSystem.DTOs;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmploymentSystem.Service
{
    public class VacancyService
    {
        private readonly ApplicationDbContext _context;

        public VacancyService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task CreateVacancy(VacancyDto vacancyDto)
        {
            var vacancy = new Vacancy
            {
                Title = vacancyDto.Title,
                Description = vacancyDto.Description,
                ExpiryDate = vacancyDto.ExpiryDate,
                MaxApplications = vacancyDto.MaxApplications,
                IsActive = true
            };

            _context.Vacancies.Add(vacancy);
            await _context.SaveChangesAsync();
        }

        public async Task<List<Vacancy>> GetVacancies()
        {
            return await _context.Vacancies.Where(v => v.IsActive && v.ExpiryDate > DateTime.Now).ToListAsync();
        }
    }

}
