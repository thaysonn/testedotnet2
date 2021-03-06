﻿using luby_app.Application.Common.Exceptions;
using luby_app.Application.Common.Interfaces;
using MediatR;
using System.Threading;
using System.Threading.Tasks;

namespace luby_app.Application.Desenvolvedor.Commands.UpdateDesenvolvedor
{
    public class UpdateDesenvolvedorCommand : IRequest
    {
        public int Id { get; set; }

        public string Nome { get; set; }

        public string Email { get; set; }

        public string Senha { get; set; }

        public string CPF { get; set; }

        public int ProjetoId { get; set; }
    }

    public class UpdateDesenvolvedorCommandHandler : IRequestHandler<UpdateDesenvolvedorCommand>
    {
        private readonly IApplicationDbContext _context;
        private readonly IIdentityService _identityService;

        public UpdateDesenvolvedorCommandHandler(IApplicationDbContext context, IIdentityService identityService)
        {
            _context = context;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateDesenvolvedorCommand request, CancellationToken cancellationToken)
        {
            var entity = await _context.Desenvolvedor.FindAsync(request.Id);

            if (entity == null)
            {
                throw new NotFoundException(nameof(Domain.Entities.Desenvolvedor), request.Id);
            }

            if (entity.Email != request.Email)
            {
                await _identityService.UpdateUserEmail(entity.UsuarioId, request.Email);
            }

            entity.Nome = request.Nome;
            entity.CPF = request.CPF;
            entity.Email = request.Email;
            entity.ProjetoId = request.ProjetoId;

            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
