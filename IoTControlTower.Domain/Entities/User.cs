using IoTControlTower.Domain.Enum;
using Microsoft.AspNetCore.Identity;
using System.Text.Json.Serialization;
using IoTControlTower.Domain.Validation;

namespace IoTControlTower.Domain.Entities;

public class User : IdentityUser
{
    public string? FullName { get; private set; }
    public DateTime? DateOfBirth { get; private set; }
    public Gender Gender { get; private set; }

    public string? Address { get; private set; }
    public string? City { get; private set; }
    public string? State { get; private set; }
    public string? Country { get; private set; }
    public string? PostalCode { get; private set; }
    public string? Role { get; private set; }

    public DateTime? LastLogin { get; private set; }
    public DateTime? UpdateDate { get; private set; }
    public DateTime? RegistrationDate { get; private set; }
    public DateTime? LastPasswordChangeDate { get; private set; }

    [JsonIgnore]
    public ICollection<Device> Devices { get; private set; }

    public User() { }

    public User(string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? registrationDate)
    {
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, role, null, null, registrationDate, null);
    }

    [JsonConstructor]
    public User(string id, string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        DomainExceptions.When(string.IsNullOrEmpty(id), "Invalid Id value");
        Id = id;
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, role, lastLogin, updateDate, registrationDate, lastPasswordChangeDate);
    }

    public void Update(string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        ValidateDomain(fullName, userName, password, email, dateOfBirth, gender, phoneNumber, address, city, state, country, postalCode, role, lastLogin, updateDate, registrationDate, lastPasswordChangeDate);
    }

    private void ValidateDomain(string fullName, string userName, string password, string email, DateTime? dateOfBirth, Gender gender, string phoneNumber, string address, string city, string state, string country, string postalCode, string role, DateTime? lastLogin, DateTime? updateDate, DateTime? registrationDate, DateTime? lastPasswordChangeDate)
    {
        DomainExceptions.When(string.IsNullOrEmpty(userName), "Username is required");
        DomainExceptions.When(string.IsNullOrEmpty(password), "Password is required");
        DomainExceptions.When(string.IsNullOrEmpty(email), "Email is required");
        DomainExceptions.When(string.IsNullOrEmpty(role), "Role is required");

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
        RegistrationDate = registrationDate;
        LastPasswordChangeDate = lastPasswordChangeDate;
    }
}
