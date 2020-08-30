using Application.Services;
using Application.Services.Interfaces;
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

        public UpdateSetupCommandHandler(MySqlContext sqlContext, IEmail email) : base(email)
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
                
                if (!string.IsNullOrEmpty(request.HomeTitle))
                {
                    var _hometitle = _setups.Where(s => s.Key.Equals("HomeTitle")).FirstOrDefault();

                    _hometitle.Value = request.HomeTitle;
                }

                if (request.HomeBanner != null)
                {
                    var _homeBanner = _setups.Where(s => s.Key.Equals("HomeBanner")).FirstOrDefault();

                    var _banner = UploadFile(request.HomeBanner, _homeBanner.Id.ToString());
                    if (string.IsNullOrEmpty(_banner))
                        throw new Exception();

                    _homeBanner.Value = _banner;
                }

                if (request.HomeBanner2 != null)
                {
                    var _homeBanner = _setups.Where(s => s.Key.Equals("HomeBanner2")).FirstOrDefault();

                    var _banner = UploadFile(request.HomeBanner2, _homeBanner.Id.ToString());
                    if (string.IsNullOrEmpty(_banner))
                        throw new Exception();

                    _homeBanner.Value = _banner;
                }

                if (request.HomeBanner3 != null)
                {
                    var _homeBanner = _setups.Where(s => s.Key.Equals("HomeBanner3")).FirstOrDefault();

                    var _banner = UploadFile(request.HomeBanner3, _homeBanner.Id.ToString());
                    if (string.IsNullOrEmpty(_banner))
                        throw new Exception();

                    _homeBanner.Value = _banner;
                }

                if (request.RegistryBanner != null)
                {
                    var _registryBanner = _setups.Where(s => s.Key.Equals("RegistryBanner")).FirstOrDefault();

                    var _banner = UploadFile(request.RegistryBanner, _registryBanner.Id.ToString());
                    if (string.IsNullOrEmpty(_banner))
                        throw new Exception();

                    _registryBanner.Value = _banner;
                }
                
                if (!string.IsNullOrEmpty(request.Regulation))
                {
                    var _regulation = _setups.Where(s => s.Key.Equals("Regulation")).FirstOrDefault();

                    _regulation.Value = request.Regulation;
                }

                if (request.ResponsabilityTerm != null)
                {
                    var _term = _setups.Where(s => s.Key.Equals("ResponsibilityTerm")).FirstOrDefault();

                    var _newTerm = UploadFile(request.ResponsabilityTerm, _term.Id.ToString());
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
