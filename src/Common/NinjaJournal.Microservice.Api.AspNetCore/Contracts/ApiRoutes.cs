namespace NinjaJournal.Microservice.Api.AspNetCore.Contracts;

public static class ApiRoutes
{
    private const string Root = "api";
    private const string Version = "v1";
    private const string Base = $"{Root}/{Version}";

    public static class StudentsManagement
    {
        private const string StudentsManagementBase = "/students-management";
        public const string GetAll = StudentsManagementBase;
        public const string Get = StudentsManagementBase + "/{id}";
        public const string Create = StudentsManagementBase;
        public const string Update = StudentsManagementBase + "/{id}";
        public const string Delete = StudentsManagementBase + "/{id}";
    }

    public static class IdentityService
    {
        public const string RemoveRoleFromUser = "remove-role";
    }
}