namespace TalentoPlus.Web.DTOs;

public class ExcelEmployeeDto
{
    public string FirstName { get; set; } = null!;              
    public string LastName { get; set; } = null!;               
    public DateTime BirthDate { get; set; }                     
    public string Address { get; set; } = null!;                
    public string Phone { get; set; } = null!;                  
    public string Email { get; set; } = null!;                  
    public string JobTitle { get; set; } = null!;               
    public decimal Salary { get; set; }                         
    public DateTime HireDate { get; set; }                      
    public string Status { get; set; } = null!;                 
    public string Education { get; set; } = null!;             
    public string ProfessionalProfile { get; set; } = null!;    
    public string Department { get; set; } = null!;             
}