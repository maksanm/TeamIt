using Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public interface IProfile
    {
        long Id { get; set; }
        User User { get; }
        Role Role { get; }
    }
}