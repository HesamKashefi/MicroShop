namespace Orders.Domain
{
    public class Entity<TId> : IEquatable<Entity<TId>> where TId : struct
    {
        public TId Id { get; set; }

        public bool Equals(Entity<TId>? other)
        {
            if (other is null) return false;

            return Id.Equals(other.Id);
        }

        public override bool Equals(object? obj)
        {
            return Equals(obj as Entity<TId>);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public static bool operator ==(Entity<TId>? a, Entity<TId>? b)
        {
            return a?.Equals(b) == true;
        }

        public static bool operator !=(Entity<TId>? a, Entity<TId>? b)
        {
            return a?.Equals(b) == false;
        }
    }
}
