namespace AssetManagementApp.Data
{
    public class AssetNotAvailableException : Exception
    {
        public AssetNotAvailableException(string message) : base(message) { }
    }

    public class EmployeeNotActiveException : Exception
    {
        public EmployeeNotActiveException(string message) : base(message) { }
    }

    public class AssetAlreadyAssignedException : Exception
    {
        public AssetAlreadyAssignedException(string message) : base(message) { }
    }

    public class CannotDeleteException : Exception
    {
        public CannotDeleteException(string message) : base(message) { }
    }
}