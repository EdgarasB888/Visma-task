

namespace Unit_tests
{
	[TestClass]
	public class ShortagesControllerTests
	{
		private Mock<IShortagesRepository> _shortagesRepositoryMock;
		private Fixture _fixture;
		private ShortagesController _controller;

        public ShortagesControllerTests()
		{
			_fixture = new Fixture();
            _shortagesRepositoryMock = new Mock<IShortagesRepository>();
        }

        [TestMethod]
        public void GetShortages_ShouldReturnShortages()
        {
            var name = "Admin"; 
            var currentDate = new System.DateOnly(2023, 1, 1);
            var expectedShortages = _fixture.Build<Shortage>()
                                            .With(s => s.CreatedOn, currentDate)
                                            .CreateMany(3)
                                            .ToList();

            _shortagesRepositoryMock.Setup(repo => repo.GetShortages(name))
                                    .Returns(expectedShortages);

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            var result = _controller.GetShortages(name);

            if (name == "Admin")
            {
                CollectionAssert.AreEqual(expectedShortages, result);
            }
            else
            {
                CollectionAssert.AreEqual(expectedShortages.Where(s => s.Name == name).ToList(), result);
            }
        }

        [TestMethod]
        public void GetShortagesByTitle_ShouldReturnShortages()
        {
            var name = "Admin";
            var titleFilter = "SomeTitle";
            var isAdmin = true; 

            var currentDate = new System.DateOnly(2023, 1, 1);
            var expectedShortages = _fixture.Build<Shortage>()
                                            .With(s => s.CreatedOn, currentDate)
                                            .CreateMany(3)
                                            .ToList();

            _shortagesRepositoryMock.Setup(repo => repo.GetShortagesByTitle(titleFilter))
                                    .Returns(expectedShortages);

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            var result = _controller.GetShortagesByTitle(name, titleFilter);

            if (!isAdmin)
            {
                CollectionAssert.AreEqual(expectedShortages.Where(s => s.Name == name).ToList(), result);
            }
            else
            {
                CollectionAssert.AreEqual(expectedShortages, result);
            }
        }

        [TestMethod]
        public void GetShortagesByCreatedOn_ShouldReturnShortages()
        {
            var name = "Admin";
            var startDate = new DateOnly(2023, 1, 1);
            var endDate = new DateOnly(2023, 1, 31);
            var isAdmin = true;

            var expectedShortages = _fixture.Build<Shortage>()
                                            .With(s => s.CreatedOn, startDate)
                                            .CreateMany(3)
                                            .ToList();

            _shortagesRepositoryMock.Setup(repo => repo.GetShortagesByCreatedOn(startDate, endDate))
                                    .Returns(expectedShortages);

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            var result = _controller.GetShortagesByCreatedOn(name, startDate, endDate);

            if (!isAdmin)
            {
                CollectionAssert.AreEqual(expectedShortages.Where(s => s.Name == name).ToList(), result);
            }
            else
            {
                CollectionAssert.AreEqual(expectedShortages, result);
            }
        }

        [TestMethod]
        public void GetShortagesByCategory_ShouldReturnShortages()
        { 
            var name = "Admin"; 
            var categoryFilter = CategoryType.Electronics;
            var currentDate = new System.DateOnly(2023, 1, 1);
            var isAdmin = true;

            var expectedShortages = _fixture.Build<Shortage>()
                                            .With(s => s.CreatedOn, currentDate)
                                            .CreateMany(3)
                                            .ToList();

            _shortagesRepositoryMock.Setup(repo => repo.GetShortagesByCategory(categoryFilter))
                                    .Returns(expectedShortages);

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            var result = _controller.GetShortagesByCategory(name, categoryFilter);

            if (!isAdmin)
            {
                CollectionAssert.AreEqual(expectedShortages.Where(s => s.Name == name).ToList(), result);
            }
            else
            {
                CollectionAssert.AreEqual(expectedShortages, result);
            }
        }

        [TestMethod]
        public void GetShortagesByRoom_ShouldReturnShortages()
        {
            var name = "Admin"; 
            var roomFilter = RoomType.Kitchen;
            var currentDate = new System.DateOnly(2023, 1, 1);
            var isAdmin = true;

            var expectedShortages = _fixture.Build<Shortage>()
                                            .With(s => s.CreatedOn, currentDate)
                                            .CreateMany(3)
                                            .ToList();

            _shortagesRepositoryMock.Setup(repo => repo.GetShortagesByRoom(roomFilter))
                                    .Returns(expectedShortages);

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            var result = _controller.GetShortagesByRoom(name, roomFilter);

            if (!isAdmin)
            {
                CollectionAssert.AreEqual(expectedShortages.Where(s => s.Name == name).ToList(), result);
            }
            else
            {
                CollectionAssert.AreEqual(expectedShortages, result);
            }
        }

        [TestMethod]
		public void AddShortage_ShouldAddShortage()
		{
            var shortageToAdd = _fixture.Build<Shortage>()
                              .With(s => s.CreatedOn, new System.DateOnly(2023, 1, 1)) 
                              .Create();

            _shortagesRepositoryMock.Setup(repo => repo.GetShortageByTitleAndRoom(It.IsAny<string>(), It.IsAny<RoomType>()))
                                    .Returns((Shortage)null);

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            _controller.AddShortage(shortageToAdd);

            _shortagesRepositoryMock.Verify(repo => repo.AddShortage(It.IsAny<Shortage>()), Times.Once);
        }

        [TestMethod]
        public void AddShortage_ShouldNotAddOrUpdateExistingShortage()
        {
            var shortageToAdd = _fixture.Build<Shortage>().With(s => s.CreatedOn, new System.DateOnly(2023, 1, 1))
                                        .Create();
            var existingShortage = _fixture.Build<Shortage>().With(s => s.CreatedOn, new System.DateOnly(2023, 1, 1))
                                        .Create(); ;
            _shortagesRepositoryMock.Setup(repo => repo.GetShortageByTitleAndRoom(It.IsAny<string>(), It.IsAny<RoomType>()))
                                    .Returns(existingShortage); 

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            _controller.AddShortage(shortageToAdd);

            _shortagesRepositoryMock.Verify(repo => repo.AddShortage(It.IsAny<Shortage>()), Times.Never);
            _shortagesRepositoryMock.Verify(repo => repo.UpdateShortage(It.IsAny<Shortage>(), It.IsAny<Shortage>()), Times.Never);
        }

        [TestMethod]
        public void UpdateShortage_ShouldUpdateShortage()
        {
            var existingShortage = _fixture.Build<Shortage>()
                                          .With(s => s.CreatedOn, new System.DateOnly(2023, 1, 1))
                                          .Create();

            var newShortage = _fixture.Build<Shortage>()
                                        .With(s => s.CreatedOn, new System.DateOnly(2023, 1, 1))
                                        .Create();

            _shortagesRepositoryMock.Setup(repo => repo.UpdateShortage(existingShortage, newShortage));

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            _controller.UpdateShortage(existingShortage, newShortage);

            _shortagesRepositoryMock.Verify(repo => repo.UpdateShortage(existingShortage, newShortage), Times.Once);
        }

        [TestMethod]
        public void DeleteShortage_ShouldDeleteShortage()
        {
            var name = "Admin";
            var title = "SomeTitle";
            var room = RoomType.Kitchen;
            var isAdmin = true;

            var shortageToDelete = _fixture.Build<Shortage>()
                                            .With(s => s.Title, title)
                                            .With(s => s.Room, room)
                                            .With(s => s.CreatedOn, new System.DateOnly(2023, 1, 1))
                                            .Create();

            _shortagesRepositoryMock.Setup(repo => repo.GetShortageByTitleAndRoom(title, room))
                                    .Returns(shortageToDelete);

            _controller = new ShortagesController(_shortagesRepositoryMock.Object);

            _controller.DeleteShortage(name, title, room);

            if (isAdmin || (!isAdmin && name == shortageToDelete.Name))
            {
                _shortagesRepositoryMock.Verify(repo => repo.DeleteShortage(shortageToDelete), Times.Once);
            }
            else
            {
                _shortagesRepositoryMock.Verify(repo => repo.DeleteShortage(It.IsAny<Shortage>()), Times.Never);
            }
        }
    }
}

