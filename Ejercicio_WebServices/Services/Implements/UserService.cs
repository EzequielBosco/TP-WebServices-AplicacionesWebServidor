using Ejercicio_WebServices.Models;

namespace Ejercicio_WebServices.Services.Implements
{
    public class UserService : IUserService
    {
        private static List<User> _usuarios;

        public UserService()
        {
            if (_usuarios == null)
            {
                _usuarios = new List<User>();
                _usuarios.Add(new User { Id = 1, Nombre = "Juan", Apellido = "Perez", NombreUsuario = "juanperez", Correo = "juanperez@gmail.com", Contraseña = "123456", Rol = "Admin", Estado = "Activo" });
                _usuarios.Add(new User { Id = 2, Nombre = "Maria", Apellido = "Gomez", NombreUsuario = "mariagomez", Correo = "mariagomez@gmail.com", Contraseña = "123456", Rol = "User", Estado = "Activo" });
                _usuarios.Add(new User { Id = 3, Nombre = "Pedro", Apellido = "Gonzalez", NombreUsuario = "pedrogonzalez", Correo = "prdro gonzalez@gmail.com", Contraseña = "123456", Rol = "User", Estado = "Activo" });
            }
        }

        public Task<IEnumerable<User>> GetAllUsers()
        {
            var users = _usuarios.Select(u => u);
            return Task.FromResult(users);
        }

        public Task<User> GetUser(string nombreUsuario, string contraseña)
        {
            var user = _usuarios.SingleOrDefault(u => u.NombreUsuario == nombreUsuario && u.Contraseña == contraseña);
            return Task.FromResult(user);
        }
    }
}
