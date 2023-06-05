using System.Collections.Generic;
using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using TripServiceKata.Entity;
using TripServiceKata.Service;

namespace TripServiceKata.Tests {
    public class TripServiceShould {
        private IUserSession _userSession;
        private User _defaultUser;
        private TripService _tripService;

        [SetUp]
        public void SetUp() {
            _userSession = Substitute.For<IUserSession>();
            _defaultUser = new User();
            _userSession.GetLoggedUser().Returns(_defaultUser);
            _tripService = new TripService(_userSession);
        }
        
        [Test]
        public void RetrieveEmptyListIfUserIsNotAFriend() {
            var anotherUser = new User();

            var tripList = _tripService.GetTripsByUser(anotherUser);

            tripList.Should().BeEmpty();
        }
    }
}