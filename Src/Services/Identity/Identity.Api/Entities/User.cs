namespace Identity.Api.Entities
{
    public class User
    {
        private User() { }

        public int Id { get; set; }
        public required string Username { get; set; }
        public string? Roles { get; private set; }
        public required byte[] PasswordHash { get; init; }
        public required byte[] PasswordSalt { get; init; }


        public static User Create(string username, string password)
        {
            if (string.IsNullOrWhiteSpace(username)) throw new ArgumentNullException(nameof(username));
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));


            var sha = System.Security.Cryptography.SHA256.Create()!;
            var encodedPassword = System.Text.Encoding.UTF8.GetBytes(password);
            var passwordSalt = System.Security.Cryptography.RandomNumberGenerator.GetBytes(32);
            var concatenated = Enumerable.Concat(encodedPassword, passwordSalt).ToArray();
            var passwordHash = sha.ComputeHash(concatenated);


            var user = new User()
            {
                Username = username,
                PasswordHash = passwordHash,
                PasswordSalt = passwordSalt
            };

            return user;
        }

        public bool CheckPassword(string? password)
        {
            if (string.IsNullOrWhiteSpace(password)) throw new ArgumentNullException(nameof(password));

            var sha = System.Security.Cryptography.SHA256.Create()!;
            var encodedPassword = System.Text.Encoding.UTF8.GetBytes(password);
            var passwordSalt = this.PasswordSalt;
            var concatenated = Enumerable.Concat(encodedPassword, passwordSalt).ToArray();
            var passwordHash = sha.ComputeHash(concatenated);

            return Enumerable.SequenceEqual(passwordHash, this.PasswordHash);
        }

        public void SetRoleAsAdmin()
        {
            Roles = "admin";
        }
    }
}
