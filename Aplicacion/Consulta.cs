﻿using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Tienda.Microservicios.Autor.Api.Modelo;
using Tienda.Microservicios.Autor.Api.Persistencia;

namespace Tienda.Microservicios.Autor.Api.Aplicacion
{
    public class Consulta
    {
        public class ListaAutor : IRequest<List<AutorDto>>
        {

        }

        public class Manejador : IRequestHandler<ListaAutor, List<AutorDto>>
        {
            private readonly ContextoAutor _context;
            private readonly IMapper _mapper;

            public Manejador(ContextoAutor context, IMapper mapper)
            {
                this._context = context;
                this._mapper = mapper;
            }

            public async Task<List<AutorDto>> Handle(ListaAutor request, CancellationToken cancellationToken)
            {
                var autores = await _context.AutorLibros.ToListAsync(cancellationToken);
                var autoresDto = _mapper.Map<List<AutorLibro>, List<AutorDto>>(autores);
                return autoresDto;
            }
        }
    }
}
