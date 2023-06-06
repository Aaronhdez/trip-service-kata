using System.Collections.Generic;
using System.Linq;

namespace TripServiceKata.Entity
{
    public class User
    {
        private List<User> friends = new List<User>();

        public void AddFriend(User user)
        {
            friends.Add(user);
        }

        public bool IsAFriend(User loggedUser) {
            return friends.Contains(loggedUser);
        }
    }
}