using System.Collections.Generic;
using TripServiceKata.Entity;
using TripServiceKata.Exception;
using TripServiceKata.Service;

namespace TripServiceKata
{
    public class TripService
    {
        private readonly IUserSession userSession;
        private readonly ITripDAO tripDao;

        public TripService(IUserSession userSession, ITripDAO tripDao) {
            this.userSession = userSession;
            this.tripDao = tripDao;
        }

        public List<Trip> GetTripsByUser(User user)
        {
            List<Trip> tripList = new List<Trip>();
            User loggedUser = userSession.GetLoggedUser();
            bool isFriend = false;
            if (loggedUser == null) throw new UserNotLoggedInException();
            foreach (User friend in user.GetFriends())
            {
                if (friend.Equals(loggedUser))
                {
                    isFriend = true;
                    break;
                }
            }

            if (isFriend)
            {
                tripList = tripDao.FindTripsByUser(user);
            }

            return tripList;

        }
    }
}