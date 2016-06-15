using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;
using System.Web.SessionState;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.SignalR;
using Trello.DAL.Models;

namespace Trello
{
    public class User
    {

        public string Name { get; set; }
        public string LoggedInOn { get; set; }
        public HashSet<string> ConnectionIds { get; set; }
    }

    [Authorize]
    public class UserActivityHub: Hub
    {

        DataModel db = new DataModel();
        private static readonly ConcurrentDictionary<string, User> Users
            = new ConcurrentDictionary<string, User>(StringComparer.InvariantCultureIgnoreCase);

        public IEnumerable<string> GetConnectedUsers() // not current
        {
            return Users.Where(x => {

                lock (x.Value.ConnectionIds)
                {
                    return !x.Value.ConnectionIds.Contains(Context.ConnectionId, StringComparer.InvariantCultureIgnoreCase);
                }

            }).Select(x => x.Key);
        }

        public override Task OnConnected()
        {
            string userName = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;
            

            var user = Users.GetOrAdd(userName, i => new User
            {
                Name = userName,
                LoggedInOn = DateTime.Now.ToShortDateString(),
                ConnectionIds = new HashSet<string>()
            });

            lock (user.ConnectionIds)
            {
                user.ConnectionIds.Add(connectionId);
                if (user.ConnectionIds.Count == 1)
                {
                    Clients.Others.connected(userName);
                }
            }

            return base.OnConnected();
        }

        public override Task OnDisconnected(bool e)
        {

            string email = Context.User.Identity.Name;
            string connectionId = Context.ConnectionId;

            User user;
            Users.TryGetValue(email, out user);

            if (user != null)
            {
                lock (user.ConnectionIds)
                {
                    user.ConnectionIds.RemoveWhere(cid => cid.Equals(connectionId));

                    if (!user.ConnectionIds.Any())
                    {
                        User removedUser;
                        Users.TryRemove(email, out removedUser);
                        Clients.Others.disconnect(email);
                    }
                }
            }

            return base.OnDisconnected(e);
        }

    }
}