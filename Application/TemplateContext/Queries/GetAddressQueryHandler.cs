using Domain.Entities;
using Domain.ViewModels;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Persistence.Contexts;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.TemplateContext.Queries
{
    public class GetAddressQueryHandler : IRequestHandler<GetAddressQuery, GetCondominiumQueryVM>
    {
        private readonly MySqlContext _sqlContext;

        public GetAddressQueryHandler(MySqlContext sqlContext)
        {
            _sqlContext = sqlContext;
        }

        public async Task<GetCondominiumQueryVM> Handle(GetAddressQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var _condominium = await _sqlContext.Set<Condominium>()
                                                .Where(c => c.Active &&
                                                            c.Validated &&
                                                            c.ZipCode.Equals(request.ZipCode) &&
                                                            c.Number.Equals(request.Number))
                                                .FirstOrDefaultAsync();

                var _result = new GetCondominiumQueryVM();

                if (_condominium == null)
                {
                    var _client = new HttpClient();
                    var _response = await _client.GetAsync("viacep.com.br/ws/" + request.ZipCode + "/json/");

                    if (_response.StatusCode != System.Net.HttpStatusCode.OK)
                        throw new Exception();

                    var _content = await _response.Content.ReadAsStringAsync();
                    var _address = JsonConvert.DeserializeObject<AddressVM>(_content);

                    _result = new GetCondominiumQueryVM(_address);
                }

                return _result;
            }
            catch(Exception)
            {
                return null;
            }
        }
    }
}
