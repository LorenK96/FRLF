namespace FRLF.Models
{
    public enum HomeView
    {
        LOGIN,
        GRID1,
        GRID2,
        GRID3
    }

    public class HomeModel : BaseModel
    {
        public HomeView View { get; set; }
    }
}
