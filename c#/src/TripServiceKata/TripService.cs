using System.Collections.Generic;
using System.Linq;
using TripServiceKata.Entity;
using TripServiceKata.Exception;
using TripServiceKata.Service;

namespace TripServiceKata {
    public class TripService {
        private readonly IUserSession userSession;
        private readonly ITripDAO tripDao;

        public TripService(IUserSession userSession, ITripDAO tripDao) {
            this.userSession = userSession;
            this.tripDao = tripDao;
        }

        public List<Trip> GetTripsByUser(User user) {
            var loggedUser = userSession.GetLoggedUser();
            if (loggedUser == null) throw new UserNotLoggedInException();
            if (user.IsAFriend(loggedUser)) {
                return tripDao.FindTripsByUser(user);
            }

            return new List<Trip>();
        }
    }
}