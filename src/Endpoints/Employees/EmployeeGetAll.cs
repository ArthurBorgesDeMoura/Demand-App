using Dapper;
using IDemandApp.Data;
using IDemandApp.Data.Repository;
using IDemandApp.Domain.Products;
using IDemandApp.Endpoints.Employees.DTO;
using Microsoft.AspNetCore.Identity;
using Microsoft.Data.SqlClient;
using System.Security.Claims;

namespace IDemandApp.Endpoints.Employees;

public class EmployeeGetAll
{
    public static string Template => "/employees";
    public static string[] Methods => new string[] { HttpMethod.Get.ToString() };
    public static Delegate Handle => Action;

    public static IResult Action(int page, int rows, UserRepository userRepository)
    {
        //if(page == null || page)
        
        return Results.Ok(userRepository.GetAll(page, rows));
    }
}
