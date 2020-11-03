namespace Convience.Model.Models
{
    public class PageSortQueryModel
    {
        public int Page { get; set; }

        public int Size { get; set; }

        /// <summary>
        /// A string use to record the sort field connected by comma.Those fields'
        /// oreder is the order of "order by" operate 
        /// </summary>

        public string Sort { get; set; }

        /// <summary>
        /// A string record the sort field' order connected by comma.
        /// Contain values ascend and descend
        /// </summary>
        public string Order { get; set; }
    }
}
