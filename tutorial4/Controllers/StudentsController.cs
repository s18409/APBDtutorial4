using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using tutorial4.Models;

namespace tutorial4.Controllers
{

    [ApiController]
    [Route("api/students")]
    public class StudentsController : ControllerBase
    {

        private string ConnString = "Data Source=db-mssql;Initial Catalog=s18409;Integrated Security=True";

        [HttpGet]
        public IActionResult GetStudents()
        {


            var listStudents = new List<Student>();
            using (SqlConnection con = new SqlConnection(ConnString))  
            using (SqlCommand com = new SqlCommand())  
            {                                              
                com.Connection = con;
                com.CommandText = "select FirstName, LastName, BirthDate, s.Name 'studies',e.Semester from Student stu " +
                    "Join Enrollment e on stu.idEnrollment = e.idEnrollment " +
                    "Join Studies s on s.idStudy = e.idStudy; ";

                con.Open();  
                SqlDataReader dr = com.ExecuteReader();  
                
                while (dr.Read())  
                {
                    var student = new Student();

                    student.FirstName = dr["FirstName"].ToString();  
                    student.LastName = dr["LastName"].ToString();
                    student.BirthDate = dr["BirthDate"].ToString();
                    student.studies = dr["studies"].ToString();
                    student.semester = dr.GetInt32(4);
                    

                    listStudents.Add(student);
                }
               

                return Ok(listStudents);
            }



        }

        [HttpGet("{id}")]
        public IActionResult GetSemesterEntry(string id)
        {
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                
                com.CommandText = "select e.StartDate from Student s " +
                    "Join Enrollment e on s.idEnrollment = e.idEnrollment where s.IndexNumber=@index";

                com.Parameters.AddWithValue("index", id);
                string date;
                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read()) 
                {
                    date = dr["StartDate"].ToString();
                    return Ok(date);
                }
                return Ok();


            }
        }

        
        [HttpPost]
        public IActionResult CreateStudent(Student student)
        {
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())
            {

                var index = $"s{new Random().Next(1, 20000)}";
                com.Connection = con;
                com.CommandText = "insert into Student values('" + index + "',@par1,@par2,@par3,2);";
                com.Parameters.AddWithValue("par1", student.FirstName);
                com.Parameters.AddWithValue("par2", student.LastName);
                com.Parameters.AddWithValue("par3", student.BirthDate);
                con.Open();
                int affected = com.ExecuteNonQuery();
                return Ok(affected);
            }
            return Ok();


        }


        [HttpGet("procedure")]
        public IActionResult getStudentwithProc()
        {
            using (SqlConnection con = new SqlConnection(ConnString))
            using (SqlCommand com = new SqlCommand())
            {
                com.Connection = con;
                com.CommandText = "TestProc";
                com.CommandType = System.Data.CommandType.StoredProcedure;
                com.Parameters.AddWithValue("FirstName", "John");

                con.Open();
                var dr = com.ExecuteReader();
                if (dr.Read())
                {
                    var student = new Student();

                    student.FirstName = dr["FirstName"].ToString();
                    student.LastName = dr["LastName"].ToString();
                    student.BirthDate = dr["BirthDate"].ToString();
                    return Ok(student);
                }

            }
            return Ok();
        }
    }
}