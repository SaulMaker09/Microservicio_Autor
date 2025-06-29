using AutoMapper;
using MediatR;
using static Tienda.Microservicios.Autor.Api.Aplicacion.ConsultarFiltro;
using Tienda.Microservicios.Autor.Api.Modelo;
using Tienda.Microservicios.Autor.Api.Persistencia;
using Microsoft.EntityFrameworkCore;

namespace Tienda.Microservicios.Autor.Api.Aplicacion
{
    public class ConsultaFiltroNombre
    {

        public class AutorNombreF : IRequest<AutorDto>
        {
            public string AutorGuid { get; set; }
            public string Nombre {  get; set; }
        }

        public class ManejadorN : IRequestHandler<AutorNombreF, AutorDto>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;
            public ManejadorN(ContextoAutor context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<AutorDto> Handle(AutorNombreF request, CancellationToken cancellationToken)
            {
                var autor = await _context.AutorLibros.Where(p => p.Nombre == request.Nombre).FirstOrDefaultAsync();
                if (autor == null)
                {
                    throw new Exception("No se encontro el autor");
                }

                var autorDto = _mapper.Map<AutorLibro, AutorDto>(autor);
                return autorDto;
            }
        }
    }
}
