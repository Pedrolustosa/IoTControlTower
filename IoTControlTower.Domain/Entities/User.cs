using IoTControlTower.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using IoTControlTower.Domain.Validation;

namespace IoTControlTower.Domain.Entities;

public class User : IdentityUser
{
    public string? FullName { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public Gender Gender { get; set; }

    public string? Address { get; set; }
    public string? City { get; set; }
    public string? State { get; set; }
    public string? Country { get; set; }
    public string? PostalCode { get; set; }
    public string? Role { get; set; }

    public DateTime? LastLogin { get; set; }
    public DateTime? UpdateDate { get; set; }
    public DateTime? RegistrationDate { get; set; }
    public DateTime? LastPasswordChangeDate { get; set; }

    [JsonIgnore]
    public ICollection<Device> Devices { get; set; }

    public User() { }

    public User(string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? registrationDate)
    {
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, role, null, null, registrationDate, null);
    }

    [JsonConstructor]
    public User(string id, string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        DomainValidation.When(string.IsNullOrEmpty(id), "Invalid Id value");
        Id = id;
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, role, lastLogin, updateDate, registrationDate, lastPasswordChangeDate);
    }

    public void Update(string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, role, lastLogin, updateDate, registrationDate, lastPasswordChangeDate);
    }

    private void ValidateDomain(string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
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
        Role = role;
        LastLogin = lastLogin;
        UpdateDate = updateDate;
        RegistrationDate= registrationDate;
        LastPasswordChangeDate = lastPasswordChangeDate;
    }
}
