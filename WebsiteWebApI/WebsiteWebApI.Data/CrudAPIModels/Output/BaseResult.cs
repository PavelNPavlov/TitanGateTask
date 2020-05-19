namespace CrudBaseServices.CrudAPIModels.Output
{
    public class BaseResult
    {
        public long StatusNumber { get; set; }

        public BaseStatusMessage StatusMessage { get; set; }

        public object Data { get; set; }
    }
}
