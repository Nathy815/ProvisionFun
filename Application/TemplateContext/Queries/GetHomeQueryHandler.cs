using Domain.Entities;
using Domain.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TemplateContext.Queries
{
    public class GetHomeQueryHandler : IRequestHandler<GetHomeQuery, GetHomeQueryVM>
    {
        private readonly MySqlContext _sqlContext;

        public GetHomeQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<GetHomeQueryVM> Handle(GetHomeQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _setups = await _sqlContext.Set<Setup>()
                                        .Where(s => s.Active)
                                        .ToListAsync();

                var _result = new GetHomeQueryVM();
                _result.HomeBanner = _setups.Where(s => s.Key.Equals("Banner Home")).FirstOrDefault().Value;
                _result.HomeTitle = _setups.Where(s => s.Key.Equals("Home Title")).FirstOrDefault().Value;
                _result.ResponsibilityTerm = _setups.Where(s => s.Key.Equals("Responsibility Term")).FirstOrDefault().Value;
                _result.Regulation = _setups.Where(s => s.Key.Equals("Regulation")).FirstOrDefault().Value;
                _result.Logo = _setups.Where(s => s.Key.Equals("Logo")).FirstOrDefault().Value;

                return _result;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
