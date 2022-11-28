﻿using Application.Features.Colors.Dtos;
using Application.Features.Colors.Rules;
using Application.Services.Repositories;
using AutoMapper;
using Core.Application.Pipelines.Authorization;
using Domain.Entities;
using MediatR;
using static Application.Features.Colors.Constants.OperationClaims;
using static Domain.Constants.OperationClaims;

namespace Application.Features.Colors.Commands.UpdateColor;

public class UpdateColorCommand : IRequest<UpdatedColorDto>, ISecuredRequest
{
    public int Id { get; set; }
    public string Name { get; set; }

    public string[] Roles => new[] { Admin, ColorAdmin, ColorWrite, ColorUpdate };

    public class UpdateColorCommandHandler : IRequestHandler<UpdateColorCommand, UpdatedColorDto>
    {
        private IColorRepository _colorRepository { get; }
        private IMapper _mapper { get; }
        private readonly ColorBusinessRules _colorBusinessRules;

        public UpdateColorCommandHandler(IColorRepository colorRepository, IMapper mapper, ColorBusinessRules colorBusinessRules)
        {
            _colorRepository = colorRepository;
            _mapper = mapper;
            _colorBusinessRules = colorBusinessRules;
        }

        public async Task<UpdatedColorDto> Handle(UpdateColorCommand request, CancellationToken cancellationToken)
        {
            await _colorBusinessRules.ColorIdShouldExistWhenSelected(request.Id);
            Color mappedColor = _mapper.Map<Color>(request);
            Color updatedColor = await _colorRepository.UpdateAsync(mappedColor);
            UpdatedColorDto updatedColorDto = _mapper.Map<UpdatedColorDto>(updatedColor);
            return updatedColorDto;
        }
    }
}