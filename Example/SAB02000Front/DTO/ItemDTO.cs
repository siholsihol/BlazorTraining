namespace SAB02000Front.DTO
{
    public class ItemDTO
    {
        public string Code { get; set; } = "";
        public string Name { get; set; } = "";
        public ItemDTO()
        {
        }
        public ItemDTO(string data)
        {
            Code = data;
            Name = data;
        }
    }
}
