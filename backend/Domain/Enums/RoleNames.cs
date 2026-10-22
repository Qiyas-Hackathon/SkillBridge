using System.Text.Json.Serialization;

namespace SkillBridge.Domain.Enums;

public static class RoleNames
{
    public const string Candidate = "Candidate"; // change if your seeded role names differ
    public const string Employer = "Employer";
    public const string Admin = "Admin";         // NEW
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum JobType { FullTime, PartTime, Contract, Internship, Remote }

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum ApplicationStatus { Pending, Reviewed, Shortlisted, Rejected, Accepted }
