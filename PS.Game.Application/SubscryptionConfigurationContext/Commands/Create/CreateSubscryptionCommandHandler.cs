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

namespace Application.SubscryptionConfigurationContext.Commands.Create
{
    public class CreateSubscryptionCommandHandler : Util, IRequestHandler<CreateSubscryptionCommand, bool>
    {
        private readonly MySqlContext _sqlContext;
        private readonly IEmail _email;

        public CreateSubscryptionCommandHandler(MySqlContext sqlContext, IEmail email) : base(email)
        {
            _sqlContext = sqlContext;
            _email = email;
        }

        public async Task<bool> Handle(CreateSubscryptionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var _condominium = new Condominium();

                if (request.CondominiumID.HasValue)
                {
                    _condominium = await _sqlContext.Set<Condominium>()
                                                .Where(c => c.Id == request.CondominiumID.Value)
                                                .FirstOrDefaultAsync();
                }
                else
                {
                    _condominium = new Condominium
                    {
                        Id = Guid.NewGuid(),
                        Address = request.Condominium.Address,
                        City = request.Condominium.City,
                        Number = request.Condominium.Number,
                        District = request.Condominium.District,
                        State = request.Condominium.State,
                        ZipCode = request.Condominium.ZipCode
                    };

                    await _sqlContext.Condominiums.AddAsync(_condominium, cancellationToken);
                }

                var _tournament = await _sqlContext.Set<Tournament>()
                                            .Where(t => t.Id == request.TournamentId)
                                            .FirstOrDefaultAsync();

                var _price = 20D;
                var _startDate = _tournament.StartSubscryption.Date;
                if ((request.Mode == PS.Game.Domain.Enums.eMode.Solo && DateTime.Now.Date > _startDate.AddDays(21)) ||
                    (request.Mode == PS.Game.Domain.Enums.eMode.Team && DateTime.Now.Date > _startDate.AddDays(14)))
                    _price = 30D;

                var _team = new Team
                {
                    Id = Guid.NewGuid(),
                    Color = request.Color,
                    CondominiumID = _condominium.Id,
                    Icon = request.Icon,
                    Mode = request.Mode,
                    Name = request.Nickname,
                    TournamentID = request.TournamentId,
                    Price = _price
                };

                await _sqlContext.Teams.AddAsync(_team, cancellationToken);

                var _player = await _sqlContext.Set<Player>()
                                        .Where(p => p.CPF.Equals(request.Player.CPF))
                                        .FirstOrDefaultAsync();
                
                if (_player == null)
                {
                    var _id = Guid.NewGuid();

                    _player = new Player
                    {
                        Id = _id,
                        Cellphone = request.Player.Cellphone,
                        BirthDate = request.Player.BirthDate,
                        CPF = request.Player.CPF,
                        Email = request.Player.Email,
                        Name = request.Player.Name,
                        Document = UploadFile(request.Player.Document, _id.ToString())
                    };

                    await _sqlContext.Players.AddAsync(_player, cancellationToken);
                }
                else
                {
                    _player.Document = UploadFile(request.Player.Document, _player.Id.ToString());

                    _sqlContext.Players.Update(_player);
                }

                var _teamPlayer = new TeamPlayer
                {
                    Id = Guid.NewGuid(),
                    IsPrincipal = true,
                    PlayerID = _player.Id,
                    TeamID = _team.Id
                };

                await _sqlContext.TeamPlayers.AddAsync(_teamPlayer, cancellationToken);

                if (request.Mode == PS.Game.Domain.Enums.eMode.Team)
                {
                    foreach (var _component in request.Team)
                    {
                        var _id = Guid.NewGuid();

                        var _dbPlayer = await _sqlContext.Set<Player>()
                                                    .Where(p => p.CPF.Equals(_component.CPF))
                                                    .FirstOrDefaultAsync();

                        if (_dbPlayer == null)
                        {
                            _dbPlayer = new Player
                            {
                                Id = _id,
                                Name = _component.Name,
                                CPF = _component.CPF,
                                Cellphone = _component.Cellphone,
                                BirthDate = _component.BirthDate,
                                Email = _component.Email,
                                Document = UploadFile(_component.Document, _id.ToString()),
                            };

                            await _sqlContext.Players.AddAsync(_dbPlayer, cancellationToken);
                        }
                        else
                        {
                            _dbPlayer.Document = UploadFile(_component.Document, _dbPlayer.Id.ToString());

                            _sqlContext.Players.Update(_dbPlayer);
                        }

                        _teamPlayer = new TeamPlayer
                        {
                            Id = Guid.NewGuid(),
                            IsPrincipal = false,
                            PlayerID = _dbPlayer.Id,
                            TeamID = _team.Id
                        };

                        await _sqlContext.TeamPlayers.AddAsync(_teamPlayer, cancellationToken);
                    }
                }

                await _sqlContext.SaveChangesAsync(cancellationToken);

                try
                {
                    _team.SubscryptionSent = await _email.SendEmail(_team, PS.Game.Domain.Enums.eStatus.Validation);

                    _sqlContext.Teams.Update(_team);

                    await _sqlContext.SaveChangesAsync(cancellationToken);
                }
                catch (Exception) { }

                return true;
            }
            catch(Exception ex)
            {
                return false;
            }
        }
    }
}
