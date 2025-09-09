namespace MunicipalServicesMVP.Models.DataStructures
{
    public class IssueReportNode<T>
    {
        public T Data;
        public IssueReportNode<T> Next;

        public IssueReportNode(T data)
        {
            Data = data;
            Next = null;
        }
    }
}
