using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Comun.ViewModels;
using Modelo.Modelos;

namespace Datos.DAL
{
    public class PacienteDAL
    {
        public static ListadoPaginadoVMR<PacienteVMR> LeerTodo(int cantidad, int pagina, string textoBusqueda)
        {
            ListadoPaginadoVMR<PacienteVMR> result = new ListadoPaginadoVMR<PacienteVMR>();
            using (var db = DbConexion.Create())
            {
                var query = db.Paciente.Where(p => !p.borrado).Select(p => new PacienteVMR
                {
                    id = p.id,
                    cedula = p.cedula,
                    nombre = p.nombre + " " + p.apellidoPaterno + (p.apellidoMaterno != null ? " " + p.apellidoMaterno : ""),
                    correoElectronico = p.correoElectronico
                });

                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    query = query.Where(p => p.cedula.Contains(textoBusqueda) || p.nombre.Contains(textoBusqueda) || p.correoElectronico.Contains(textoBusqueda));
                }

                result.cantidadTotal = query.Count();
                result.elementos = query
                    .OrderBy(p => p.id)
                    .Skip(cantidad * pagina)
                    .Take(cantidad)
                    .ToList();
            }

            return result;
        }
        LeerUno()
        {

        }
        Crear()
        {

        }
        Actualizar()
        {

        }
        Eliminar()
        {

        }
    }
}
