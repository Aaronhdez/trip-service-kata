using System.Collections.Generic;
using System.Linq;
using TripServiceKata.Entity;
using TripServiceKata.Exception;
using TripServiceKata.Service;

namespace TripServiceKata {
    public class TripService {
        private readonly IUserSession _userSession;
        private readonly ITripDAO _tripDao;

        public TripService(IUserSession userSession, ITripDAO tripDao) {
            _userSession = userSession;
            _tripDao = tripDao;
        }

        public IEnumerable<Trip> GetTripsByUser(User user) {
            var loggedUser = _userSession.GetLoggedUser();
            if (loggedUser == null) throw new UserNotLoggedInException();
            return !Enumerable.Contains(user.GetFriends(), loggedUser) ?
                new List<Trip>() : 
                _tripDao.FindTripsByUser(user);
        }
    }
}