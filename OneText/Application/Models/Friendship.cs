using Spark.Library.Database;

namespace OneText.Application.Models;

public class Friendship : BaseModel
{
    public int User1Id { get; set; }
    public int User2Id { get; set; }
    public virtual User User1 {get; set;}
    public virtual User User2 {get; set;}
    public bool Blocked { get; set; }

}