namespace MagicVilla_API.Modelos.Especificaciones
{
    public class PagedList<T> : List<T>
    {
        public MetaData MetaData { get; set; }

        public PagedList(List<T> Items, int count, int pageNumber, int pageSize) 
        {
            MetaData = new MetaData()
            {
                TotalCount = count,
                PageSize = pageSize,
                TotalPages = (int)Math.Ceiling((double)count / (double)pageSize) // Por ejemplo 1.5 lo
                //redondea a 2 , dado que no se puede tener una pagina y media
            };
            AddRange(Items);
        }

        public static PagedList<T> ToPagedList(IEnumerable<T> entidad, int pageNumber, int pageSize)
        {
            var count = entidad.Count(); // Calculamos la cantidad de registros

            //Aqui es donde empiezo a cortar 
            //Traigo el numero de paginas menos uno, pues no debo cortar nada si solo hay una pagina
            //Luego multiplico el tamano de las paginas y asi puedo conocer la cantidad de items que hay, antes
            // de la ultima pagina
            //esto con la finalidad que al usar skips quitemos todos esos elementos, por lo tanto si son
            // 10 paginas de 10 elementos, y queremos solo los ultimos 10, quitaremos 90,
            //Luego de los que quedan con Take tomaremos los que abarcan el pageSize, debido a que si
            //estuvieramos en la mitad por ejemplo en los 50, si quisieramos traer del 51 al 60, entonces
            //solo nos interesa el tamano de pagina, o sea la pagina 6.
            // De esta forma tenemos los elementos por pagina. 

            //Sin embargo es IEnumerable, necesitamos pasarlo a lista, por lo tanto usamos
            //ToList
            var items = entidad.Skip((pageNumber - 1) * pageSize)
                               .Take(pageSize).ToList();

            return new PagedList<T>(items, count, pageNumber, pageSize);
        }
    }
}
