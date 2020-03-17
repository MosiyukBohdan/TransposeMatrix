using MongoDB.Bson;

namespace Share.Models
{
    public class Matrix
    {
        public ObjectId Id { get; set; }

        public int[][] Rows { get; set; }
    }
}
