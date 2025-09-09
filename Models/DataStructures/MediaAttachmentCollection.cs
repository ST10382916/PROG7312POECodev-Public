namespace MunicipalServicesMVP.Models.DataStructures
{
    public class MediaAttachmentCollection<T>
    {
        private MediaAttachmentNode<T> head;
        private int count;

        public MediaAttachmentCollection()
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
            MediaAttachmentNode<T> newNode = new MediaAttachmentNode<T>(data);
            
            if (head == null)
            {
                // If list is empty, new node becomes the head
                head = newNode;
            }
            else
            {
                // Traverse to the end of the list
                MediaAttachmentNode<T> current = head;
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
                Console.WriteLine("Media attachment list is empty.");
                return;
            }

            MediaAttachmentNode<T> current = head;
            int index = 0;
            
            Console.WriteLine("Displaying all media attachments:");
            while (current != null)
            {
                Console.WriteLine($"[{index}] {current.Data}");
                current = current.Next;
                index++;
            }
            Console.WriteLine($"Total attachments: {count}");
        }

        // Additional helper method: Get all items as array
        public T[] GetAll()
        {
            if (count == 0)
                return new T[0];

            T[] result = new T[count];
            MediaAttachmentNode<T> current = head;
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
                MediaAttachmentNode<T> newNode = new MediaAttachmentNode<T>(data);
                newNode.Next = head;
                head = newNode;
                count++;
                return true;
            }

            // Find the node at index-1
            MediaAttachmentNode<T> current = head;
            for (int i = 0; i < index - 1; i++)
            {
                current = current.Next;
            }

            // Insert new node
            MediaAttachmentNode<T> nodeToInsert = new MediaAttachmentNode<T>(data);
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

            MediaAttachmentNode<T> current = head;
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
            MediaAttachmentNode<T> current = head;
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
            MediaAttachmentNode<T> current = head;
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

            MediaAttachmentNode<T> current = head;
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

        // Domain-specific methods for Media Attachments

        // AddAttachment method - Add with validation
        public bool AddAttachment(T attachment)
        {
            // Basic validation - assuming T is MediaAttachment
            dynamic media = attachment;
            if (media != null && !string.IsNullOrEmpty(media.FileName))
            {
                Add(attachment);
                return true;
            }
            return false;
        }

        // GetImageFiles method - Get only image attachments
        public T[] GetImageFiles()
        {
            if (count == 0)
                return new T[0];

            // First count image files
            int imageCount = 0;
            MediaAttachmentNode<T> current = head;
            while (current != null)
            {
                dynamic media = current.Data;
                if (media != null && media.IsValidImageType())
                {
                    imageCount++;
                }
                current = current.Next;
            }

            // Create array and populate with image files
            T[] result = new T[imageCount];
            current = head;
            int index = 0;

            while (current != null && index < imageCount)
            {
                dynamic media = current.Data;
                if (media != null && media.IsValidImageType())
                {
                    result[index] = current.Data;
                    index++;
                }
                current = current.Next;
            }

            return result;
        }

        // GetDocumentFiles method - Get only document attachments
        public T[] GetDocumentFiles()
        {
            if (count == 0)
                return new T[0];

            // First count document files
            int docCount = 0;
            MediaAttachmentNode<T> current = head;
            while (current != null)
            {
                dynamic media = current.Data;
                if (media != null && media.IsValidDocumentType())
                {
                    docCount++;
                }
                current = current.Next;
            }

            // Create array and populate with document files
            T[] result = new T[docCount];
            current = head;
            int index = 0;

            while (current != null && index < docCount)
            {
                dynamic media = current.Data;
                if (media != null && media.IsValidDocumentType())
                {
                    result[index] = current.Data;
                    index++;
                }
                current = current.Next;
            }

            return result;
        }

        // CalculateTotalSize method - Get total file size of all attachments
        public long CalculateTotalSize()
        {
            if (head == null)
                return 0;

            long totalSize = 0;
            MediaAttachmentNode<T> current = head;

            while (current != null)
            {
                dynamic media = current.Data;
                if (media != null && media.FileSize > 0)
                {
                    totalSize += media.FileSize;
                }
                current = current.Next;
            }

            return totalSize;
        }

        // GetTotalSizeFormatted method - Get formatted file size string
        public string GetTotalSizeFormatted()
        {
            long totalBytes = CalculateTotalSize();
            
            if (totalBytes == 0)
                return "0 bytes";

            string[] sizes = { "bytes", "KB", "MB", "GB" };
            double size = totalBytes;
            int order = 0;

            while (size >= 1024 && order < sizes.Length - 1)
            {
                order++;
                size = size / 1024;
            }

            return $"{size:0.##} {sizes[order]}";
        }

        // FindByFileName method - Find attachment by filename
        public T FindByFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName))
                return default(T);

            MediaAttachmentNode<T> current = head;
            while (current != null)
            {
                dynamic media = current.Data;
                if (media != null && media.FileName != null && 
                    media.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    return current.Data;
                }
                current = current.Next;
            }
            return default(T);
        }

        // GetFilesByExtension method - Get files by specific extension
        public T[] GetFilesByExtension(string extension)
        {
            if (string.IsNullOrEmpty(extension) || count == 0)
                return new T[0];

            // Ensure extension starts with dot
            if (!extension.StartsWith("."))
                extension = "." + extension;

            // First count matching files
            int matchCount = 0;
            MediaAttachmentNode<T> current = head;
            while (current != null)
            {
                dynamic media = current.Data;
                if (media != null && media.GetFileExtension() != null && 
                    media.GetFileExtension().Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    matchCount++;
                }
                current = current.Next;
            }

            // Create array and populate with matching files
            T[] result = new T[matchCount];
            current = head;
            int index = 0;

            while (current != null && index < matchCount)
            {
                dynamic media = current.Data;
                if (media != null && media.GetFileExtension() != null && 
                    media.GetFileExtension().Equals(extension, StringComparison.OrdinalIgnoreCase))
                {
                    result[index] = current.Data;
                    index++;
                }
                current = current.Next;
            }

            return result;
        }

        // RemoveByFileName method - Remove attachment by filename
        public bool RemoveByFileName(string fileName)
        {
            if (string.IsNullOrEmpty(fileName) || head == null)
                return false;

            // If head node has the filename, remove it
            dynamic headMedia = head.Data;
            if (headMedia != null && headMedia.FileName != null && 
                headMedia.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
            {
                head = head.Next;
                count--;
                return true;
            }

            // Loop through the list to find the filename
            MediaAttachmentNode<T> current = head;
            while (current.Next != null)
            {
                dynamic media = current.Next.Data;
                if (media != null && media.FileName != null && 
                    media.FileName.Equals(fileName, StringComparison.OrdinalIgnoreCase))
                {
                    // Skip over the node to remove it
                    current.Next = current.Next.Next;
                    count--;
                    return true;
                }
                current = current.Next;
            }

            return false; // Filename not found
        }
    }
}

