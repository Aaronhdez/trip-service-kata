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
            var tripList = new List<Trip>();
            var loggedUser = userSession.GetLoggedUser();
            if (loggedUser == null) throw new UserNotLoggedInException();
            foreach (var friend in user.GetFriends())
            {
                if (friend.Equals(loggedUser))
                {
                    tripList = tripDao.FindTripsByUser(user);
                    break;
                }
            }

            return tripList;

        }
    }
}