using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using Azure;
using STAS.Model;
using STAS.Repo;
using STAS.Type;

namespace STAS.Services
{
    public class EmployeeService
    {
        #region Private Fields

        private readonly EmployeeRepo repo = new();

        #endregion


        #region Public Methods
        /// <summary>
        /// Create new employee 
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public async Task<Employee> AddEmployeeAsync(Employee emp)
        {

            if (ValidateEmployee(emp))
            {
                return await repo.AddEmployeeAsync(emp);
            }

            return emp;

        }

        /// <summary>
        /// Update employee
        /// </summary>
        /// <param name="emp"></param>
        /// <returns></returns>
        public async Task<Employee> UpdateEmployeeAsync(Employee emp)
        {

            if (ValidateEmployee(emp))
            {
                return await repo.UpdateEmployeeAsync(emp);
            }

            return emp;

        }

        /// <summary>
        /// Get Employee By Id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<Employee> SearchEmployeeByIdAsync(int id)
        {
            return await repo.SearchEmployeeByIdAsync(id);
        }


        /// <summary>
        /// Get All employee
        /// </summary>
        /// <returns></returns>
        public async Task<List<Employee>> GetAllEmployeesAsync()
        {
            
            List<Employee> emp = await repo.GetAllEmployeesAsync();

            emp = emp.Where(e => e.TypeEmployeeId == 2).OrderBy(em => em.FullName).ToList();

            return emp;
        }



        #endregion

        #region Private Methods
        /// <summary>
        /// Validate employee
        /// </summary>
        /// <param name="employee"></param>
        /// <returns></returns>
        private bool ValidateEmployee(Employee employee)
        {

            ValidateCardNumber(employee);
            
            return employee.Errors.Count == 0;
        }

        /// <summary>
        /// Validate card number
        /// </summary>
        /// <param name="employee"></param>
        private void ValidateCardNumber(Employee employee)
        {
            
            if (employee.EmployeeNumber == null)
            {
                employee.Errors.Add(new ValidationError("Please, check Employee Number.", ErrorType.Business));
                return;
            }

            if (!int.TryParse(employee.EmployeeNumber, out int num))
            {
                employee.Errors.Add(new ValidationError("The Employee Number is not a number.", ErrorType.Business));
                return;
            }

            Employee emp = repo.SearchEmployeeByEmployeeNumber(employee.EmployeeNumber!);

            if ((emp != null && employee.EmployeeId == 0) || (emp != null && employee.EmployeeId != emp.EmployeeId))
            {
               employee.Errors.Add(new ValidationError("Employee number is not unique.", ErrorType.Business));
            }

        }
        #endregion
    }



}
