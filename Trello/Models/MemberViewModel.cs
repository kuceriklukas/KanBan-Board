using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Trello.DAL.Models;

namespace Trello.Models
{
    public class MemberViewModel
    {
        public List<UserBoard> UserBoards { get; set; }
    }
}