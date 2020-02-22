using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using InformationCardService.Common;

namespace InformationCardService.Server
{
    public class StoreDB
    {
        private readonly string _fileName = "store.xml";
        private List<InformationCard> cards;

        public IEnumerable<InformationCard> GetCards()
        {
            var ds = new DataSet();
            ds.ReadXml(_fileName);

            cards = new List<InformationCard>();
            foreach (DataRow cardRow in ds.Tables["InformationCards"].Rows)
            {
                var image = GetByteImage((string) cardRow["Image"]);
                cards.Add(new InformationCard(int.Parse((string) cardRow["CardId"]),
                    (string) cardRow["Name"], image));
            }

            return cards;
        }

        public void SaveCard(InformationCard informationCard)
        {
            var sb = new StringBuilder();
            foreach (var b in informationCard.Image)
            {
                sb.Append(b.ToString());
                sb.Append(" ");
            }
            var xDoc = XDocument.Load(_fileName);
            if (informationCard.Id != 0)
            {
                xDoc.Root.Elements("InformationCards")
                    .Where(el => el.Attribute("id").Value == informationCard.Id.ToString())
                    .Select(el => el.Element("Name"))
                    .FirstOrDefault().SetValue(informationCard.Name);
                xDoc.Root.Elements("InformationCards")
                    .Where(el => el.Attribute("id").Value == informationCard.Id.ToString())
                    .Select(el => el.Element("Image"))
                    .FirstOrDefault().SetValue(sb.ToString());
            }
            else
            {
                GetCards();
                var currentIndex = cards.Select(x => x.Id).ToArray().Max<int>();
                var root = new XElement("InformationCards");
                root.Add(new XAttribute("id", (currentIndex + 1).ToString()));
                root.Add(new XElement("CardId", informationCard.Id));
                root.Add(new XElement("Name", informationCard.Name));
                root.Add(new XElement("Image", sb.ToString()));
                xDoc.Element("NewDataSet").Add(root);
            }
            xDoc.Save(_fileName);
        }

        private byte[] GetByteImage(string image)
        {
            if (image.Contains(".png") || image.Contains(".jpg"))
            {
                var imageDirectory = Path.Combine(Directory.GetCurrentDirectory(), "Images");
                var imagePath = Path.Combine(imageDirectory, image);
                var imgdata = File.ReadAllBytes(imagePath);
                return imgdata;
            }

            var array = image.Trim().Split(" ").Select(x => byte.Parse(x)).ToArray();
            return array;
        }
    }
}