
using System.Threading.Tasks;
using TechSupportBrain.Models;
using TechSupportBrain.Interfaces;
using FirebaseAdmin;
using FirebaseAdmin.Auth;
using System.Collections.Generic;
using FirebaseAdmin.Messaging;

namespace TechSupportBrain.Services
{
   public class UserService : IUserService
    {
        private readonly FirebaseApp _app;

        public UserService(FirebaseApp app)
        {
            _app = app;
        }

       public async Task<bool> Register(User user)
        {
            try
            {
                var userRecordArgs = new UserRecordArgs()
                {
                    Email = user.Email,
                    Password = user.Password, // In a production environment, hash the password before storing it.  However, Firebase handles hashing for us.
                };
                UserRecord userRecord = await FirebaseAuth.GetAuth(_app).CreateUserAsync(userRecordArgs);

                var claims = new Dictionary<string, object>()
                {
                    { "role", user.Role }
                };
                await FirebaseAuth.GetAuth(_app).SetCustomUserClaimsAsync(userRecord.Uid, claims);

                // Capture the generated Uid
                user.Uid = userRecord.Uid;
                return true;
            }
            catch (FirebaseAuthException ex)
            {
                // Handle specific Firebase Authentication exceptions, e.g., email already exists
                Console.WriteLine($"Error registering user: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return false;
            }
        }

        public async Task<User> Login(string email, string password)
        {
            try
            {
                var auth = FirebaseAuth.GetAuth(_app);
                UserRecord userRecord = await auth.GetUserByEmailAsync(email);

                if (userRecord == null)
                {
                    throw new Exception(new Message("User not found."));
                }

                if (!userRecord.IsEmailVerified)
                {
                    throw new Exception(new AuthError { Message = "Invalid email or password." });
                }

                // Use VerifyPassword method from UserRecord
                if (!userRecord.VerifyPassword(password))
                {
                    throw new Exception(new AuthError { Code = AuthErrorCode.InvalidPassword, Message = "Invalid email or password." });
                }

                // Retrieve custom claims (including role)
                string role = userRecord.CustomClaims.TryGetValue("role", out var roleClaim) ? roleClaim.ToString() : null;

                return new User { Email = email, Role = role, Uid = userRecord.Uid };
            } catch (FirebaseAuthException ex) {
                Console.WriteLine($"Firebase Auth Error during login: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An unexpected error occurred: {ex.Message}");
                return null;
            }
        }
    }

}