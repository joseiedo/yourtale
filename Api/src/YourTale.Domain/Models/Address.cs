using static System.String;

namespace YourTale.Domain.Models;

public class Address
{
     
    public string Uf { get; set; } = Empty;  
    
    public string City { get; set; } = Empty;

    public bool IsValid => !IsNullOrEmpty(Uf) && !IsNullOrEmpty(City);
}