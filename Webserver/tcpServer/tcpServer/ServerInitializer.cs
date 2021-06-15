using System;
using System.Threading;
using System.Threading.Tasks;
using System.Threading.Tasks.Dataflow;
namespace tcpServer
{
    public class ServerInitializer : DevicePref
    {
        internal void Initializer()
        {
            Header(1);
            Thread.Sleep(1000);
            Console.WriteLine("Initalising...");
            StartWork();
            AsynchronousSocketListener.StartListening();

        }
        ITargetBlock<DateTimeOffset> CreateNeverEndingTask(
        Action<DateTimeOffset> action, CancellationToken cancellationToken)
        {
            // Validate parameters.
            if (action == null) throw new ArgumentNullException("action");

            // Declare the block variable, it needs to be captured.
            ActionBlock<DateTimeOffset> block = null;

            // Create the block, it will call itself, so
            // you need to separate the declaration and
            // the assignment.
            // Async so you can wait easily when the
            // delay comes.
            block = new ActionBlock<DateTimeOffset>(async now => {
                // Perform the action.
                action(now);

                // Wait.
                await Task.Delay(TimeSpan.FromSeconds(10), cancellationToken).
                    // Doing this here because synchronization context more than
                    // likely *doesn't* need to be captured for the continuation
                    // here.  As a matter of fact, that would be downright
                    // dangerous.
                    ConfigureAwait(false);

                // Post the action back to the block.
                block.Post(DateTimeOffset.Now);
            }, new ExecutionDataflowBlockOptions
            {
                CancellationToken = cancellationToken
            });

            // Return the block.
            return block;
        }

        CancellationTokenSource wtoken;
        ActionBlock<DateTimeOffset> task;
        DataHandler dataHandler = new DataHandler();
        void StartWork()
        {
            // Create the token source.
            wtoken = new CancellationTokenSource();

            // Set the task.
            task = (ActionBlock<DateTimeOffset>)CreateNeverEndingTask(now => dataHandler.QueueHandler(), wtoken.Token);

            // Start the task.  Post the time.
            task.Post(DateTimeOffset.Now);
        }

        void StopWork()
        {
            // CancellationTokenSource implements IDisposable.
            using (wtoken)
            {
                // Cancel. This will cancel the task.
                wtoken.Cancel();
            }

            // Set everything to null, since the references
            // are on the class level and keeping them around
            // is holding onto invalid state.
            wtoken = null;
            task = null;
        }
    }
}
