using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TripServiceKata.Entity;
using TripServiceKata.Service;

namespace TripServiceKata.Tests {
    public class TripServiceShould {
        [Test]
        public void RetrieveEmptyListIfUserIsNotAFriend() {
            var userSession = Substitute.For<IUserSession>();
            var user = new User();
            var anotherUser = new User();
            userSession.GetLoggedUser().Returns(user);
            TripService tripService = new TripService(userSession);

            List<Trip> tripList = tripService.GetTripsByUser(anotherUser);

            tripList.Should().BeEmpty();
        }
    }
}