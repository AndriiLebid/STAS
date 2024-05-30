using STAS.Model;
using STAS.Type;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using STAS.DAL;

namespace STAS.Repo
{
    public class EmployeeRepo
    {
        #region Private Fields

        private readonly DataAccess db = new();

        #endregion

        #region Public Methods

        public Employee AddEmployee(Employee emp)
        {

            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, null, 0, ParameterDirection.Output),
                new Parm("@FirstName", SqlDbType.NVarChar, emp.FirstName, 30),
                new Parm("@MiddleInitial", SqlDbType.NVarChar, (object)emp.MiddleInitial ?? DBNull.Value, 1),
                new Parm("@LastName", SqlDbType.NVarChar, emp.LastName, 30),
                new Parm("@EmployeeTypeId", SqlDbType.Int, (object)emp.TypeEmployeeId)
            };

            if (db.ExecuteNonQuery("spAddEmployee", parms) > 0)
            {
                emp.EmployeeId = (int?)parms.FirstOrDefault(p => p.Name == "@EmployeeId")?.Value ?? 0;
            }
            else
            {
                throw new DataException("There was an error adding an employee.");
            }

            return emp; 
        
        }


        public Employee UpdateEmployee(Employee emp)
        {

            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, emp.EmployeeId),
                new Parm("@FirstName", SqlDbType.NVarChar, emp.FirstName, 30),
                new Parm("@MiddleInitial", SqlDbType.NVarChar, (object)emp.MiddleInitial ?? DBNull.Value, 1),
                new Parm("@LastName", SqlDbType.NVarChar, emp.LastName, 30),
                new Parm("@EmployeeTypeId", SqlDbType.Int, (object)emp.TypeEmployeeId)
            };

            if (db.ExecuteNonQuery("spUpdateEmployee", parms) != 1)
            {
                throw new DataException("An employee update error occurred.");
            }

            return emp;

        }

        public Employee SearchEmployeeById(int id)
        {

            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, id),
            };

            DataTable dt = db.Execute("spSearchEmployeesById", parms);

            Employee emp = new Employee();

            if (dt != null) 
            {
                return PopulateEmployee(dt.Rows[0]);

            }
            else
            {
                return emp;
            }
        }

        public async Task<Employee> SearchEmployeeByIdAsync(int id)
        {

            List<Parm> parms = new()
            {
                new Parm("@EmployeeId", SqlDbType.Int, id),
            };

            DataTable dt = await db.ExecuteAsync("spSearchEmployeesById", parms);

            Employee emp = new Employee();

            if (dt.Rows.Count != 0)
            {
                return PopulateEmployee(dt.Rows[0]);

            }
            else
            {
                return emp;
            }
        }

        public Employee SearchEmployeeByEmployeeNumber(string num)
        {

            List<Parm> parms = new()
            {
                new Parm("@EmployeeNumber", SqlDbType.NVarChar, num),
            };

            DataTable dt = db.Execute("spSearchEmployeesByNumber", parms);

            Employee emp = new Employee();

            if (dt != null)
            {
                return PopulateEmployee(dt.Rows[0]);

            }
            else
            {
                return emp;
            }
        }

        #endregion





        #region Private Methods

        private Employee PopulateEmployee(DataRow dataRow)
        {
           Employee emp = new Employee();
           emp.EmployeeId = Convert.ToInt32(dataRow["EmployeeId"]);
           emp.FirstName = dataRow["EmployeeCardNumber"].ToString();
           emp.FirstName = dataRow["FirstName"].ToString();
           emp.LastName = dataRow["LastName"].ToString();
           emp.MiddleInitial = dataRow["MiddleInitial"].ToString() ?? "";
           emp.TypeEmployeeId = Convert.ToInt32(dataRow["TypeEmployeeId"]);


           return emp;
        }

        #endregion
    }
}
