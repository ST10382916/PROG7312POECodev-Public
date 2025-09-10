namespace MunicipalServicesMVP.Models.DataStructures
{
    public class MediaAttachmentNode<T>
    {
        public T Data;
        public MediaAttachmentNode<T> Next;

        public MediaAttachmentNode(T data)
        {
            Data = data;
            Next = null;
        }
    }
}



