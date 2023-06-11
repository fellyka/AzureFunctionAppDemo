using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Data.SqlClient;
using static System.Net.Mime.MediaTypeNames;

namespace AzureFunctionAppDemo
{
    public static class AzDemoFunc
    {
        [FunctionName("GetCertifications")]
        public static async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = null)] HttpRequest req,
            ILogger log)
        {
            List<Certifications> _lst = new List<Certifications>();
           // string connectionStrings = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Cerifications;Integrated Security=True;Connect Timeout=30;Encrypt=False";
            string connectionStrings= "Data Source = .; Database = Certification;Integrated Security = sspi";
            string statement = "SELECT CourseId, CourseName, Rating from Certifications";
            SqlConnection connection = new SqlConnection(connectionStrings);
            connection.Open();
            SqlCommand cmd = new SqlCommand(statement, connection);
            using (SqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    Certifications certifications = new Certifications()
                    {
                        CourseId = reader.GetInt32(0),
                        CourseName = reader.GetString(1),
                        Rating = reader.GetInt32(2)
                    };

                    _lst.Add(certifications);
                }
            }
            connection.Close();
            return new OkObjectResult(_lst);
        }
    }
}
