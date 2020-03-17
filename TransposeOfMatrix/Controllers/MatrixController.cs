using BLL.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Share.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace TransposeOfMatrix.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class MatrixController: ControllerBase
    {
        private readonly IMatrixService matrixService;

        public MatrixController(IMatrixService matrixService) => this.matrixService = matrixService;

        [HttpGet("{id}")]
        public Task<Matrix> GetMatrixById(string id) => this.matrixService.GetMatrixById(id);

        [HttpGet]
        public Task<IEnumerable<Matrix>> GetAllMatrix() => matrixService.GetAll();

        [HttpPost]
        public Task<Matrix> GenerateMatrix([FromBody] int size) => matrixService.GenerateMatrix(size);

        [HttpPut("{id}")]
        public Task<Matrix> TransposeMatrix(string id) => matrixService.TransposeMatrix(id);
    }
}
