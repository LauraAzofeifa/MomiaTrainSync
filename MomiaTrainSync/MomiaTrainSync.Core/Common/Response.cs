using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MomiaTrainSync.Core.Common
{
    public class Response<T>
    {
        public bool Exito { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public T? Datos { get; set; }

        public static Response<T> Success(T data, string mensaje = "")
        {
            return new Response<T>
            {
                Exito = true,
                Datos = data,
                Mensaje = mensaje
            };
        }

        // NUEVO: opción para enviar Datos
        public static Response<T> Fail(string mensaje, T? data = default)
        {
            return new Response<T>
            {
                Exito = false,
                Mensaje = mensaje,
                Datos = data
            };
        }
    }
}
