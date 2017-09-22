
namespace Main
{
    public delegate void TaskDelegate();
    public interface IThreadPool
    {
        void EnqueueTask(TaskDelegate task);       
    }
}
