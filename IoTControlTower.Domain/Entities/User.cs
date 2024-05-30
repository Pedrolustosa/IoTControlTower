using IoTControlTower.Domain.Validation;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;

namespace IoTControlTower.Domain.Entities;

public class User : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string? Gender { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }

    public DateTime? LastLogin { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? LastPasswordChangeDate { get; set; }
    public User() { }

    public User(string fullName, string userName, string password, string email, DateTime? dateOfBirth, string gender, string phoneNumber, string address, string city, string state, string country, string postalCode, DateTime? registrationDate)
    {
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, null, null, registrationDate, null);
    }

    [JsonConstructor]
    public User(string id, string fullName, string userName, string password, string email, DateTime? dateOfBirth, string gender, string phoneNumber, string address, string city, string state, string country, string postalCode, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        DomainValidation.When(string.IsNullOrEmpty(id) || Guid.Parse(id) == Guid.Empty, "Invalid Id value");
        Id = id;
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, lastLogin, updateDate, registrationDate, lastPasswordChangeDate);
    }

    public void Update(string fullName, string userName, string password, string email, DateTime? dateOfBirth, string gender, string phoneNumber, string address, string city, string state, string country, string postalCode, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, lastLogin, updateDate, registrationDate, lastPasswordChangeDate);
    }

    private void ValidateDomain(string fullName, string userName, string password, string email, DateTime? dateOfBirth, string gender, string phoneNumber, string address, string city, string state, string country, string postalCode, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        DomainValidation.When(string.IsNullOrEmpty(fullName), "Invalid fullName. Required");
        DomainValidation.When(string.IsNullOrEmpty(userName), "Invalid userName. Required");
        DomainValidation.When(string.IsNullOrEmpty(password), "Invalid password. Required");

        UserName = userName;
        PasswordHash = password;
        FullName = fullName;
        Email = email;
        DateOfBirth = dateOfBirth;
        Gender = gender;
        Address = address;
        City = city;
        PhoneNumber = phoneNumber;
        State = state;
        Country = country;
        PostalCode = postalCode;
        LastLogin = lastLogin;
        UpdateDate = updateDate;
        RegistrationDate= registrationDate;
        LastPasswordChangeDate = lastPasswordChangeDate;
    }
}
