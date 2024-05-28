using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public Employee AddEmployee(Employee emp)
        {

            if (ValidateEmployee(emp))
            {
                return repo.AddEmployee(emp);
            }

            return emp;

        }

        public Employee UpdateEmployee(Employee emp)
        {

            if (ValidateEmployee(emp))
            {
                return repo.UpdateEmployee(emp);
            }

            return emp;

        }

        public Employee SearchEmployeeById(int id)
        {
                return repo.SearchEmployeeById(id);
        }


        public Employee SearchEmployeeByCardNumber(string num)
        {
            return repo.SearchEmployeeByEmployeeNumber(num);
        }



        #endregion

        #region Private Methods

        private bool ValidateEmployee(Employee employee)
        {
            return employee.Errors.Count == 0;
        }


        #endregion




    }
}
