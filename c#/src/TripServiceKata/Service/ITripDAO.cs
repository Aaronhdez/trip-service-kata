using System.Collections.Generic;
using TripServiceKata.Entity;

namespace TripServiceKata.Service {
    public interface ITripDAO {
        List<Trip> FindTripsByUser(User user);
    }
}