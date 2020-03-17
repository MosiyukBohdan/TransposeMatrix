using BLL.Interfaces;
using BLL.Services;
using DAL.DataContext;
using DAL.Interfaces;
using MongoDB.Bson;
using Moq;
using NUnit.Framework;
using NUnit.Framework.Constraints;
using Share.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Tests
{
    [TestFixture]
    public class MatrixServiceTest
    {
        private readonly Mock<IMatrixRepository> matrixRepositoryMock;
        public MatrixServiceTest()
        {
            var mockMatrixRepository = new Mock<IMatrixRepository>();
            mockMatrixRepository.Setup(r => r.Save(It.IsAny<Matrix>())).Returns<Matrix>(x => Task.FromResult(x));
            mockMatrixRepository.Setup(r => r.Update(It.IsAny<Matrix>())).Returns<Matrix>(x => Task.FromResult(x));

            matrixRepositoryMock = mockMatrixRepository;
        }

        [Test]
        public async Task TransposeMatrix_RotatedMatrixIsEqualToMatrixAfterService_True()
        {
            //Arrange
            var matrix = new int[5][]
            {
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 1, 2, 3, 4, 5 },
                new int[] { 1, 2, 3, 4, 5 }
            };
            var rotatedMatrix = new int[5][]
            {
                new int[] { 1, 1, 1, 1, 1 },
                new int[] { 2, 2, 2, 2, 2 },
                new int[] { 3, 3, 3, 3, 3 },
                new int[] { 4, 4, 4, 4 ,4 },
                new int[] { 5, 5, 5, 5, 5 }
            };
            var matrixId = "1";
            matrixRepositoryMock.Setup(r => r.Get(matrixId)).Returns<string>(id => Task.FromResult(new Matrix() { Rows = matrix }));
            var matrixRepository = matrixRepositoryMock.Object;
            var matrixService = new MatrixService(matrixRepository);

            //Act
            var result = await matrixService.TransposeMatrix(matrixId);

            //Assert
            matrixRepositoryMock.Verify(x => x.Get("1"), Times.Once);
            matrixRepositoryMock.Verify(x => x.Update(result), Times.Once);

            Assert.AreEqual(rotatedMatrix.Length, result.Rows.Length);

            for (var i = 0; i < rotatedMatrix.Length; i++)
                Assert.AreEqual(rotatedMatrix[i].Length, result.Rows[i].Length);

            for (var i = 0; i < rotatedMatrix.Length; i++)
                for (var j = 0; j < rotatedMatrix.Length; j++)
                    Assert.AreEqual(rotatedMatrix[i][j], result.Rows[i][j]);
        }

        [Test]
        public async Task GenerateMatrix_GenerateMatrixWithExactSize()
        {
            //Arrange
            var size = 3000;
            var matrixService = new MatrixService(matrixRepositoryMock.Object);

            //Act
            var result = await matrixService.GenerateMatrix(size);

            //Assert
            Assert.AreEqual(size, result.Rows.Length);

            foreach (var row in result.Rows)
                Assert.AreEqual(row.Length, size);
        }

        [Test]
        public void GenerateMatrix_GenerateMatrixWithIncorrectSize_Exception()
        {
            //Arrange
            var size = -9;
            var matrixService = new MatrixService(matrixRepositoryMock.Object);

            //Act
            ActualValueDelegate<Task<Matrix>> generateDelegate = async () => await matrixService.GenerateMatrix(size);

            //Assert
            Assert.That(generateDelegate, Throws.Exception);
        }

        [Test]
        public async Task GetMatrixById_ReturnMatrixWithSameId()
        {
            //Arrange
            var matrixObjectId = ObjectId.GenerateNewId();
            var matrix = new Matrix() { Id = matrixObjectId };

            matrixRepositoryMock.Setup(r => r.Get(matrixObjectId.ToString())).Returns<string>(id => Task.FromResult(matrix));
            var matrixService = new MatrixService(matrixRepositoryMock.Object);

            //Act
            var result = await matrixService.GetMatrixById(matrixObjectId.ToString());

            //Assert
            matrixRepositoryMock.Verify(x => x.Get(matrixObjectId.ToString()), Times.Once);

            Assert.AreEqual(matrixObjectId, result.Id);
        }

        [Test]
        public async Task GetAll_GetAllMatrixes()
        {
            //Arrange
            IEnumerable<Matrix> matrixes = new List<Matrix>() { new Matrix(), new Matrix(), new Matrix() };
            matrixRepositoryMock.Setup(r => r.GetAll()).Returns(() => Task.FromResult(matrixes));
            var matrixService = new MatrixService(matrixRepositoryMock.Object);

            //Act
            var result = await matrixService.GetAll();

            //Assert
            matrixRepositoryMock.Verify(x => x.GetAll(), Times.Once);

            Assert.AreEqual(matrixes, result);
        }
    }
}
