namespace MeetAndPlay.Data.DTO
{
    public class CountArray<TItem>
    {
        public int Count { get; set; }
        public TItem[] Items { get; set; }

        public CountArray(TItem[] items, int count)
        {
            Items = items;
            Count = count;
        }
    }
}