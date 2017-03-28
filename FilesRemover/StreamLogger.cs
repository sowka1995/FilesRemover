using System.IO;

namespace FilesRemover
{
    internal class StreamLogger : ILogger
    {
        StreamWriter streamWriter;
        
        public StreamLogger(string path)
        {
            streamWriter = new StreamWriter(new FileStream(path, FileMode.Create));
        }

        public void Log(string message)
        {
            streamWriter.WriteLine(message);  
        }
    }
}
