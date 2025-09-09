namespace MunicipalServicesMVP.Models.DataStructures
{
    public class CategoryNode<T>
    {
        public T Data;
        public CategoryNode<T> Next;

        public CategoryNode(T data)
        {
            Data = data;
            Next = null;
        }
    }
}

