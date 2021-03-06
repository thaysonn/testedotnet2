﻿using luby_app.Application.Common.Exceptions;
using luby_app.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace luby_app.Application.Desenvolvedor.Commands.DeleteDesenvolvedor
{
    public class DeleteDesenvolvedorCommand : IRequest
    {
        public int Id { get; set; }
    }

    public class DeleteDesenvolvedorCommandHandler : IRequestHandler<DeleteDesenvolvedorCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public DeleteDesenvolvedorCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(DeleteDesenvolvedorCommand  request, CancellationToken cancellationToken)
        {
            var entity = await _context.Desenvolvedor
                                            .Where(l => l.Id == request.Id)
                                            .SingleOrDefaultAsync(cancellationToken);

            if (entity == null) 
                throw new NotFoundException(nameof(Domain.Entities.Desenvolvedor), request.Id);

            await _identityService.DeleteUserAsync(entity.UsuarioId);

            _context.Desenvolvedor.Remove(entity);

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
