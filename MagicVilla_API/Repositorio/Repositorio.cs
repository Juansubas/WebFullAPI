using MagicVilla_API.Datos;
using MagicVilla_API.Repositorio.IRepositorio;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace MagicVilla_API.Repositorio
{
    public class Repositorio<T> : IRepositorio<T> where T : class
    {
        private readonly ApplicationDbContext _db;
        internal DbSet<T> dbSet;

        public Repositorio(ApplicationDbContext db)
        {
            _db = db;
            this.dbSet = _db.Set<T>();
        }
        public Task Crear(T entidad)
        {
            throw new NotImplementedException();
        }

        public Task Grabar()
        {
            throw new NotImplementedException();
        }

        public Task<T> Obtener(Expression<Func<T, bool>>? filtro = null, bool traked = true)
        {
            throw new NotImplementedException();
        }

        public Task<List<T>> ObtenerTodos(Expression<Func<T, bool>>? filtro = null)
        {
            throw new NotImplementedException();
        }

        public Task Remover(T entidad)
        {
            throw new NotImplementedException();
        }
    }
}
