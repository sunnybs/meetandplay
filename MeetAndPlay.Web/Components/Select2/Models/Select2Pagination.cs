namespace MeetAndPlay.Web.Components.Select2.Models
{
    public class Select2Pagination
    {
        public Select2Pagination(bool more) => More = more;

        public bool More { get; set; }
    }
}