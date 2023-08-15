namespace Orders.Domain
{
    public class ValueObject<T> : IEquatable<ValueObject<T>> where T : class
    {
        public static bool operator ==(ValueObject<T> a, ValueObject<T> b)
        {
            return a.Equals(b);
        }

        public static bool operator !=(ValueObject<T> a, ValueObject<T> b)
        {
            return !a.Equals(b);
        }

        public override bool Equals(object? obj)
        {
            if (obj is T other)
            {
                return this.Equals(other);
            }
            return base.Equals(obj);
        }

        public override int GetHashCode()
        {
            var props = typeof(T).GetProperties();
            int hash = 0;
            foreach (var prop in props)
            {
                hash += prop.GetValue(this)?.GetHashCode() ?? 0;
            }

            return hash;
        }

        public bool Equals(ValueObject<T>? other)
        {
            var props = typeof(T).GetProperties();
            foreach (var prop in props)
            {
                var a = prop.GetValue(this);
                var b = prop.GetValue(other);

                if (a is null && b is null)
                {
                    continue;
                }
                if (a is not null && b is not null && a.Equals(b))
                {
                    continue;
                }
                return false;
            }
            return true;
        }
    }
}
