namespace InformationCardService.Common
{
    public class InformationCard
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public byte[] Image { get; set; }

        public InformationCard(int id, string name, byte[] image)
        {
            Id = id;
            Name = name;
            Image = image;
        }
    }
}