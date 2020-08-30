using Domain.Entities;
using Domain.ViewModels;
using PS.Game.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PS.Game.Domain.ViewModels
{
    public class ListSubscryptionsQueryVM
    {
        public List<ListSubscryptionsQueryStatusVM> Totalizers { get; set; }
        public List<GetSubscryptionVM> Subscryptions { get; set; }

        public ListSubscryptionsQueryVM(List<Team> teams)
        {
            Totalizers = new List<ListSubscryptionsQueryStatusVM>();
            var _status = Enum.GetValues(typeof(eStatus));

            foreach (var status in _status)
            {
                var value = teams.Where(t => t.Status == (eStatus)status).ToList().Count;
                Totalizers.Add(new ListSubscryptionsQueryStatusVM((eStatus)status, value));
            }
            
            Subscryptions = new List<GetSubscryptionVM>();
            var _teams = teams.Where(t => t.Active).ToList();
            foreach (var _team in _teams)
                Subscryptions.Add(new GetSubscryptionVM(_team));
        }
    }

    public class ListSubscryptionsQueryStatusVM
    {
        public string Status { get; set; }
        public int Value { get; set; }

        public ListSubscryptionsQueryStatusVM(eStatus status, int value)
        {
            Status = status.ToString();
            Value = value;
        }
    }

}
