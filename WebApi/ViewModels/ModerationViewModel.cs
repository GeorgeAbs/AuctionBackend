namespace WebApi.ViewModels
{
    public class ModerationViewModel<TEntity>
    {
        public int TotalItemsToModerate { get; set; }

        public TEntity? Item { get; set; }

        public List<string>? SystemComments { get; set; }

        public ModerationViewModel(TEntity? item, int totalItemsToModerate,  List<string>? systemComments)
        {
            TotalItemsToModerate = totalItemsToModerate;
            Item = item;
            SystemComments = systemComments;
        }
    }
}
