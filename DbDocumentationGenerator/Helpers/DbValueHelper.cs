namespace DatabaseMetadataReporting.Helpers
{
    public static class DbValueHelper
    {
        public static T ConvertFromDBVal<T>(this object obj)
        {
            if (obj == null || obj == DBNull.Value)
            {
                return default(T); // returns the default value for the type
            }
            else
            {
                return (T)obj;
            }
        }

    }
}

