using System;

namespace Vale.Tops.Application.Presentation.AssetManager.Models
{
    [Serializable]
    public class Graph
    {
        private string jsonData;

        public string getJsonData()
        {
            return jsonData;
        }

        public void setJsonData(string jsonData)
        {
            this.jsonData = jsonData;
        }

    }
}