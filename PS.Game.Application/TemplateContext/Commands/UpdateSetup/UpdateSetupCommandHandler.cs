﻿using Application.Services;
using Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TemplateContext.Commands.UpdateSetup
{
    public class UpdateSetupCommandHandler : Util, IRequestHandler<UpdateSetupCommand, bool>
    {
        private readonly MySqlContext _sqlContext;

        public UpdateSetupCommandHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<bool> Handle(UpdateSetupCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _setups = await _sqlContext.Set<Setup>()
                                        .Where(s => s.Active)
                                        .ToListAsync();

                var _hometitle = _setups.Where(s => s.Key.Equals("HomeTitle")).FirstOrDefault();
                _hometitle.Value = request.HomeTitle;

                var _regulation = _setups.Where(s => s.Key.Equals("Regulation")).FirstOrDefault();
                _regulation.Value = request.Regulation;

                if (request.ResponsabilityTerm != null)
                {
                    var _term = _setups.Where(s => s.Key.Equals("ResponsibilityTerm")).FirstOrDefault();

                    var _newTerm = UploadFile(request.ResponsabilityTerm, _term.Id.ToString(), request.virtualPath);
                    if (string.IsNullOrEmpty(_newTerm))
                        throw new Exception();

                    _term.Value = _newTerm;
                }

                _sqlContext.Setups.UpdateRange(_setups);

                await _sqlContext.SaveChangesAsync(cancellationToken);

                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}