using api_sqlstoredprocedures.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace api_sqlstoredprocedures.Controllers
{
    public class ValuesController : ApiController
    {
        SqlConnection con = new SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings["webapi_conn"].ConnectionString);
        Employee emp = new Employee();
        // GET api/values
        public List<Employee> Get()
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("dbo.GetAllEmployeeRecords", con);
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            List<Employee> employeeList = new List<Employee>();
            if (dataTable.Rows.Count > 0)
            {
                for (int i = 0; i< dataTable.Rows.Count; i++)
                {
                    Employee objEmployee = new Employee();
                    objEmployee.Id = Convert.ToInt32(dataTable.Rows[i]["Id"]);
                    objEmployee.Name = dataTable.Rows[i]["Name"].ToString();
                    objEmployee.Age = Convert.ToInt32(dataTable.Rows[i]["Age"]);
                    objEmployee.Active = Convert.ToInt32(dataTable.Rows[i]["Active"]);
                    employeeList.Add(objEmployee);
                }
            }
            if (employeeList.Count > 0)
            {
                return employeeList;
            }
            else
            {
                return null;
            }
        }

        // GET api/values/5
        public Employee Get(int id)
        {
            SqlDataAdapter dataAdapter = new SqlDataAdapter("dbo.GetEmployeeRecordbyId", con);
            dataAdapter.SelectCommand.CommandType = CommandType.StoredProcedure;
            dataAdapter.SelectCommand.Parameters.AddWithValue("@Id", id);
            DataTable dataTable = new DataTable();
            dataAdapter.Fill(dataTable);
            Employee emp = new Employee();
            if (dataTable.Rows.Count > 0)
            {
                emp.Id = Convert.ToInt32(dataTable.Rows[0]["Id"]);
                emp.Name = dataTable.Rows[0]["Name"].ToString();
                emp.Age = Convert.ToInt32(dataTable.Rows[0]["Age"]);
                emp.Active = Convert.ToInt32(dataTable.Rows[0]["Active"]);
                
            }
            if (emp != null)
            {
                return emp;
            }
            else
            {
                return null;
            }
        }

        // POST api/values
        public string Post(Employee objEmployee)
        {
            string msg = "";
            if (objEmployee != null)
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.AddNewEmployeeRecord", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                //Id is auto incremented.
                sqlCommand.Parameters.AddWithValue("@Name", objEmployee.Name);
                sqlCommand.Parameters.AddWithValue("@Age", objEmployee.Age);
                sqlCommand.Parameters.AddWithValue("@Active", objEmployee.Active);

                con.Open();
                int i = sqlCommand.ExecuteNonQuery();

                if (i>0)
                {
                    msg = "Data has been inserted";
                }
                else
                {
                    msg = "Error";
                }
            }
            return msg;
        }

        // PUT api/values/5
        public string Put(int id, Employee employee)
        {
            string msg = "";
            if (employee != null)
            {
                SqlCommand sqlCommand = new SqlCommand("dbo.UpdateEmployeeRecord", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);
                sqlCommand.Parameters.AddWithValue("@Name", employee.Name);
                sqlCommand.Parameters.AddWithValue("@Age", employee.Age);
                sqlCommand.Parameters.AddWithValue("@Active", employee.Active);

                con.Open();
                int i = sqlCommand.ExecuteNonQuery();

                if (i>0)
                {
                    msg = "Data has been updated";
                }
                else
                {
                    msg = "Error";
                }
            }
            return msg;
        }

        // DELETE api/values/5
        public string Delete(int id)
        {
            string msg = "";
        
                SqlCommand sqlCommand = new SqlCommand("dbo.DeleteEmployeeRecord", con);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@Id", id);
                con.Open();
                int executed = sqlCommand.ExecuteNonQuery();

                if (executed>0)
                {
                    msg = "Data has been deleted";
                }
                else
                {
                    msg = "Error";
                }
            return msg;
        }
    }
}
