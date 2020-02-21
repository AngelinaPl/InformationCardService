using System.Collections.Generic;
using System.Data;
using System.IO;
using InformationCardService.Common;

namespace InformationCardService.Server
{
    public class StoreDB
    {
        public IEnumerable<InformationCard> GetCards()
        {
            var ds = new DataSet();
            ds.ReadXml("store.xml");

            var cards = new List<InformationCard>();
            foreach (DataRow cardRow in ds.Tables["InformationCards"].Rows)
            {
                var image = GetByteImage((string) cardRow["Image"]);
                cards.Add(new InformationCard(int.Parse((string) cardRow["CardId"]),
                    (string) cardRow["Name"], image));
            }

            return cards;
        }

        private byte[] GetByteImage(string imageName)
        {
            var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
            var imagePath = Path.Combine(imageDirectory, imageName);
            var imgdata = File.ReadAllBytes(imagePath);
            return imgdata;
        }
    }
}