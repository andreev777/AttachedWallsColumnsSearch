namespace AttachedWallsColumnsSearch.Models
{
    public class AttachedElement
    {
        public int Id { get; set; }
        public string TypeName { get; set; }
        public string IsBottomAttached { get; set; }
        public string IsTopAttached { get; set; }

        public AttachedElement(int id, string typeName, bool isBottomAttached, bool isTopAttached)
        {
            Id = id;
            TypeName = typeName;
            IsBottomAttached = isBottomAttached == false? "Нет" : "Да";
            IsTopAttached = isTopAttached == false ? "Нет" : "Да";
        }
    }
}
