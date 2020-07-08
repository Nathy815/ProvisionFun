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
    public class ListCondominiumsQueryHandler : IRequestHandler<ListCondominiumsQuery, List<GetCondominiumQueryVM>>
    {
        private readonly MySqlContext _sqlContext;

        public ListCondominiumsQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<List<GetCondominiumQueryVM>> Handle(ListCondominuimsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _condominiums = await _sqlContext.Set<Condominium>()
                                                .Where(c => c.Active && c.Validated)
                                                .ToListAsync();

                var _list = new List<GetCondominiumQueryVM>();
                foreach (var _condominium in _condominiums)
                    _list.Add(new GetCondominiumQueryVM(_condominium));

                return _list;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
