namespace Data.Generic.Cache.Settings
{
    public class ServerSettings
    {
        public string Address { get; }
        public int Port { get; }
        public string Password { get; }

        public ServerSettings(string address, int port, string password)
        {
            Address = address;
            Port = port;
            Password = password;
        }

        protected bool Equals(ServerSettings other)
        {
            return string.Equals(Address, other.Address) && Port == other.Port && string.Equals(Password, other.Password);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((ServerSettings) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Address?.GetHashCode() ?? 0;
                hashCode = (hashCode*397) ^ Port;
                hashCode = (hashCode*397) ^ (Password?.GetHashCode() ?? 0);
                return hashCode;
            }
        }
    }
}