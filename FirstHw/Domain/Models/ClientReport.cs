using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Models
{
    public class ClientReport
    {
        public long ClientId { get; init; }
        public float OrdersSum { get; set; }
        public string FavoriteItem { get; set; }

        public ClientReport(long clientId)
        {
            ClientId = clientId;
        }
    }
}
