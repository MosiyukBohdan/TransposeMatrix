using BLL.Interfaces;
using DAL.Interfaces;
using Share.Models;
using System.Collections.Generic;
using System;
using System.Threading.Tasks;

namespace BLL.Services
{
    public class MatrixService : IMatrixService
    {
        private readonly IMatrixRepository matrixRepository;
        private readonly Random random;

        public MatrixService(IMatrixRepository matrixRepository)
        {
            this.matrixRepository = matrixRepository;
            this.random = new Random();
        }

        public Task<Matrix> GenerateMatrix(int size)
        {
            if (size == 0 || size < 0)
                throw new Exception("Incorrect size");

            var matrix = new Matrix() { Rows = new int[size][] };

            for (int i = 0; i < size; i++)
            {
                matrix.Rows[i] = new int[size];

                for (int j = 0; j < size; j++)
                    matrix.Rows[i][j] = random.Next(-10, 10);
            }

            return matrixRepository.Save(matrix);
        }

        public Task<IEnumerable<Matrix>> GetAll() => matrixRepository.GetAll();

        public Task<Matrix> GetMatrixById(string id) => matrixRepository.Get(id);

        public async Task<Matrix> TransposeMatrix(string id)
        {
            var matrix = await matrixRepository.Get(id);

            RotateMatrix(matrix);

            return await matrixRepository.Update(matrix);
        }

        private Matrix RotateMatrix(Matrix matrix)
        {
            var size = matrix.Rows.Length;

            for (int i = 0; i < size / 2; i++)
            {
                for (int j = i; j < size - 1 - i; j++)
                {
                    var temp = matrix.Rows[i][j];
                    matrix.Rows[i][j] = matrix.Rows[size - j - 1][i];
                    matrix.Rows[size - j - 1][i] = matrix.Rows[size - i - 1][size - j - 1];
                    matrix.Rows[size - i - 1][size - j - 1] = matrix.Rows[j][size - i - 1];
                    matrix.Rows[j][size - i - 1] = temp;
                }
            }

            return matrix;
        }
    }
}
