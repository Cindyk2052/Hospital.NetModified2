using Comun.ViewModels;
using Modelo.Modelos;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;


namespace Datos.DAL
{
    public class EgresoDAL
    {
        public static ListadoPaginadoVMR<EgresoVMR> LeerTodo(int cantidad, int pagina, string textoBusqueda)
        {
            ListadoPaginadoVMR<EgresoVMR> resultado = new ListadoPaginadoVMR<EgresoVMR>();
            using (var db = DbConexion.Create())
            {
                var query = db.Egreso.Where(e => !e.borrado).Select(e => new EgresoVMR
                {
                    id = e.id,
                    fecha = e.fecha,
                    monto = e.monto,
                    //LAMBDA
                    /*medico = new MedicoVMR
                    {
                        cedula = e.Medico.cedula,
                        nombre = e.Medico.nombre + " " + e.apellidoPaterno + (e.apellidoMaterno != null ? " " + e.apellidoMaterno : ""),,
                    },*/

                    //LINQ
                    medico = (from m in db.Medico
                        where
                            m.id == e.medicoId
                        select new MedicoVMR
                        {
                            cedula = m.cedula,
                            nombre = m.nombre + " " + m.apellidoPaterno + (m.apellidoMaterno != null ? " " + m.apellidoMaterno : ""),
                        }).FirstOrDefault(),

                    paciente = db.Paciente.Where(p => p.id == p.Ingreso.pacienteId).Select(p => new PacienteVMR
                    {
                        cedula = p.cedula,
                        nombre = p.nombre + " " + p.apellidoPaterno + (p.apellidoMaterno != null ? " " + p.apellidoMaterno : ""),
                    })
                }).FirstOrDefault();

                if (!string.IsNullOrEmpty(textoBusqueda))
                {
                    query = query.Where(e => e.medico.cedula.Contains(textoBusqueda) || e.medico.nombre.Contains(textoBusqueda) || e.paciente.cedula.Contains(textoBusqueda) || e.paciente.nombre.Contains(textoBusqueda));
                }

                resultado.cantidadTotal = query.Count();
                resultado.elementos = query
                    .OrderBy(e => e.id)
                    .Skip(cantidad * pagina)
                    .Take(cantidad)
                    .ToList();

            }
        }
        
        public static EgresoVMR LeerUno(long id)
        {
            EgresoVMR resultado = null;

            using (var db = DbConexion.Create())
            {
                resultado = db.Egreso.Where(e => e.id == id && !e.borrado).Select...
                    id = e.id;
                    fecha = e.fecha,
                    ingresoId = e.ingresoId,
                    medicoId = e.medicoId,
                    monto = e.monto,
                    tratamiento = e.tratamiento

            }).FirstOrDefault();

        }

        public static long Crear(Egreso item)
        {
            using (var db = DbConexion.Create())
            {
                item.borrado = false;
                item.fecha = DataSetDateTime.Now;
                db.Egreso.Add(item);
                db.SaveChanges();
                
            }

            return item.id;

        }

        public static void Actualizar(EgresoVMR item)
        {
            using (var db = DbConexion.Create())
            {
                Egreso itemUpdate = db.Egreso.Find(item.id);
                itemUpdate.tratamiento = item.tratamiento;
                itemUpdate.monto = item.monto;

                db.Entry(itemUpdate).State = EntityState.Modified;
                db.SaveChanges();
            }

        }

        public static void Eliminar(List<long> ids)
        {
            using (var db = DbConexion.Create())
            {
                var items = db.Egreso.Where(e => ids.Contains(e.id));

                foreach (var item in items)
                {
                    item.borrado = true;
                    //BORRAR: db.Egreso.Remove();
                    db.Entry(item).State = EntityState.Modified;
                }

                db.SaveChanges();
            }
        }
    }
}
