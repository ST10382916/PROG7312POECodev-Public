namespace MunicipalServicesMVP.Models.DataStructures
{

//CODE ATTRIBUTION:
    //01
    //C# LinkedList: Linked List Implementation In C#.
    //Adapted from: GeeksforGeeks (2019). C# LinkedList. [online] GeeksforGeeks. 
    //Available at: https://www.geeksforgeeks.org/c-sharp/linked-list-implementation-in-c-sharp/
    //Date Accessed: 10 September 2025

    //02
    //LinkedList<T>. Remove Method.
    //Adapted from: Microsoft Learn (2025). LinkedList<T>.Remove Method. [online] Microsoft Learn.
    //Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1.remove?view=net-9.0
    //Date Accessed: 10 September 2025

    //03
    //LinkedList<T>. Find(T) Method.
    //Adapted from: Microsoft Learn (2025). LinkedList<T>.Find(T) Method. [online] Microsoft Learn.
    //Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1.find?view=net-9.0
    //Date Accessed: 10 September 2025

    //04
    //Generic classes and methods.
    //Adapted from: Microsoft Learn (2025). Generic classes and methods. [online] Microsoft Learn.
    //Available at: https://learn.microsoft.com/en-us/dotnet/api/system.collections.generic.linkedlist-1.find?view=net-9.0
    //Date Accessed: 10 September 2025

    //05
    //How To Create Generics in C#, Including New Features.
    //Adapted from: IAmTimCorey (2024). How To Create Generics in C#, Including New Features. [online] YouTube.
    //Available at: https://www.youtube.com/watch?v=Ld5D6B2Ntjg
    //Date Accessed: 10 September 2025

    public class IssueReportCollection<T>
    {
        private IssueReportNode<T> head;
        private int count;

        public IssueReportCollection()
        {
            head = null;
            count = 0;
        }

        public int Count
        {
            get { return count; }
        }

        // Step 3: Add method
        public void Add(T data)
        {
            IssueReportNode<T> newNode = new IssueReportNode<T>(data);
            
            if (head == null)
            {
                // If list is empty, new node becomes the head
                head = newNode;
            }
            else
            {
                // Traverse to the end of the list
                IssueReportNode<T> current = head;
                while (current.Next != null)
                {
                    current = current.Next;
                }
                // Add new node at the end
                current.Next = newNode;
            }
            count++;
        }

        // Step 4: Display all method
        public void DisplayAll()
        {
            if (head == null)
            {
                Console.WriteLine("List is empty.");
                return;
            }

            IssueReportNode<T> current = head;
            int index = 0;
            
            Console.WriteLine("Displaying all items:");
            while (current != null)
            {
                Console.WriteLine($"[{index}] {current.Data}");
                current = current.Next;
                index++;
            }
            Console.WriteLine($"Total items: {count}");
        }

        // Additional helper method: Get all items as array
        public T[] GetAll()
        {
            if (count == 0)
                return new T[0];

            T[] result = new T[count];
            IssueReportNode<T> current = head;
            int index = 0;

            while (current != null)
            {
                result[index] = current.Data;
                current = current.Next;
                index++;
            }

            return result;
        }

        // Step 5: Advanced Features

        // InsertAt method - Insert at specific index
        public bool InsertAt(int index, T data)
        {
            if (index < 0 || index > count)
                return false;

            if (index == 0)
            {
                // Insert at beginning
                IssueReportNode<T> newNode = new IssueReportNode<T>(data);
                newNode.Next = head;
                head = newNode;
                count++;
                return true;
            }

            // Find the node at index-1
            IssueReportNode<T> current = head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            // Insert new node
            IssueReportNode<T> nodeToInsert = new IssueReportNode<T>(data);
            nodeToInsert.Next = current.Next;
            current.Next = nodeToInsert;
            count++;
            return true;
        }

        // Find method - Find first occurrence of value
        public bool Find(T value)
        {
            if (head == null)
                return false;

            IssueReportNode<T> current = head;
            while (current != null)
            {
                if (current.Data.Equals(value))
                    return true;
                current = current.Next;
            }
            return false;
        }

        // Remove method - Remove first occurrence of value
        public bool Remove(T value)
        {
            if (head == null)
                return false;

            // If head node has the value, remove it
            if (head.Data.Equals(value))
            {
                head = head.Next;
                count--;
                return true;
            }

            // Loop through the list to find the value
            IssueReportNode<T> current = head;
            while (current.Next != null)
            {
                if (current.Next.Data.Equals(value))
                {
                    // Skip over the node to remove it
                    current.Next = current.Next.Next;
                    count--;
                    return true;
                }
                current = current.Next;
            }

            return false; // Value not found
        }

        // Clear method - Remove all items
        public void Clear()
        {
            head = null;
            count = 0;
        }

        // RemoveAt method - Remove item at specific index
        public bool RemoveAt(int index)
        {
            if (index < 0 || index >= count)
                return false;

            if (index == 0)
            {
                // Remove head
                head = head.Next;
                count--;
                return true;
            }

            // Find the node at index-1
            IssueReportNode<T> current = head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            // Remove the node at index
            current.Next = current.Next.Next;
            count--;
            return true;
        }

        // Get method - Get item at specific index
        public T GetAt(int index)
        {
            if (index < 0 || index >= count)
                throw new IndexOutOfRangeException("Index is out of range");

            IssueReportNode<T> current = head;
            for (int i = 0; i < index; i++)
            {
                current = current.Next;
            }

            return current.Data;
        }

        // Contains method - Check if value exists
        public bool Contains(T value)
        {
            return Find(value);
        }

        // IsEmpty method - Check if list is empty
        public bool IsEmpty()
        {
            return count == 0;
        }
    }
}
