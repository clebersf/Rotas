using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    public class Serializer
    {
        public static byte[] Serialize(Graph graph)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            {
                BinaryFormatter formatter = new BinaryFormatter();
                formatter.Serialize(memoryStream, graph);
                memoryStream.Position = 0;
                byte[] personData = memoryStream.ToArray();
                return personData;
            }
        }

        public static Graph Deserialize(byte[] binaryGraph)
        {
            using (MemoryStream ms = new MemoryStream(binaryGraph))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                Graph graph = (Graph)formatter.Deserialize(ms);
                return graph;
            }
        }
    }
}




