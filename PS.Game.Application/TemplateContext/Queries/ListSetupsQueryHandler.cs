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
    public class ListSetupsQueryHandler : IRequestHandler<ListSetupsQuery, GetSetupQueryVM>
    {
        private readonly MySqlContext _sqlContext;

        public ListSetupsQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }
            
        public async Task<GetSetupQueryVM> Handle(ListSetupsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _setups = await _sqlContext.Set<Setup>()
                                        .Where(s => s.Active)
                                        .ToListAsync();

                var _result = new GetSetupQueryVM();
                _result.HomeTitle = _setups.Where(s => s.Key.Equals("HomeTitle")).FirstOrDefault().Value;
                _result.Regulation = _setups.Where(s => s.Key.Equals("Regulation")).FirstOrDefault().Value;
                _result.ResponsabilityTerm = _setups.Where(s => s.Key.Equals("ResponsabilityTerm")).FirstOrDefault().Value;

                return _result;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
