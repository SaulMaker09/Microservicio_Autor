using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tienda.Microservicios.Autor.Api.Persistencia;

namespace Tienda.Microservicios.Autor.Api.Aplicacion
{
    public class ActualizarAutor
    {
        public class AutorActualizar : IRequest
        {
            public string AutorGuid { get; set; }
            public string Nombre { get; set; }
            public string Apellido { get; set; }
            public DateTime? FechaNacimiento { get; set; }
        }

        public class EjecutaValidacion : AbstractValidator<AutorActualizar>
        {
            public EjecutaValidacion()
            {
                RuleFor(x => x.AutorGuid).NotEmpty();
                RuleFor(x => x.Nombre).NotEmpty();
                RuleFor(x => x.Apellido).NotEmpty();
                RuleFor(x => x.FechaNacimiento).NotEmpty();
            }
        }

        public class Manejador : IRequestHandler<AutorActualizar>
        {
            private readonly ContextoAutor _context;

            public Manejador(ContextoAutor context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(AutorActualizar request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibros
                    .FirstOrDefaultAsync(a => a.AutorLibroGuid == request.AutorGuid, cancellationToken);

                if (autor == null)
                    throw new Exception("Autor no encontrado");

                autor.Nombre = request.Nombre ?? autor.Nombre;
                autor.Apellido = request.Apellido ?? autor.Apellido;
                autor.FechaNacimiento = request.FechaNacimiento ?? autor.FechaNacimiento;

                var result = await _context.SaveChangesAsync(cancellationToken);
                if (result > 0)
                    return Unit.Value;

                throw new Exception("No se pudo guardar la actualización del autor");
            }
        }
    }
}
