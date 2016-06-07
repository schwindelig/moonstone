using System;

namespace moonstone.sql.context
{
    public class SqlInstalledVersion : SqlVersion, IComparable<SqlInstalledVersion>
    {
        public DateTime InstallDateUtc { get; set; }

        public SqlInstalledVersion()
            : base()
        {
            this.InstallDateUtc = DateTime.MinValue;
        }

        public SqlInstalledVersion(int major, int minor, int revision, DateTime installDateUtc)
            : base(major, minor, revision)
        {
            this.InstallDateUtc = installDateUtc;
        }

        public int CompareTo(SqlInstalledVersion other)
        {
            int baseResult = base.CompareTo(other);

            if (baseResult == 0)
            {
                if (this.InstallDateUtc > other.InstallDateUtc)
                {
                    return 1;
                }
                else if (this.InstallDateUtc < other.InstallDateUtc)
                {
                    return -1;
                }
                else
                {
                    return 0;
                }
            }
            else
            {
                return baseResult;
            }
        }

        public SqlVersion GetVersion()
        {
            return new SqlVersion(this.Major, this.Minor, this.Revision);
        }
    }
}