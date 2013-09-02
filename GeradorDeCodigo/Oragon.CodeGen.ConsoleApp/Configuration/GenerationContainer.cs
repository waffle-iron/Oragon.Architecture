//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Oragon.CodeGen.ConsoleApp.Configuration
//{
//    public class GenerationContainer
//    {
//        public List<T4Template> Templates { get; set; }
//        private Queue<T4Template> TemplateQueue { get; set; }
//        public event EventHandler EndOfProcess;

//        internal void Run()
//        {
//            if ((this.Templates != null) && (this.Templates.Count > 0))
//            {
//                this.TemplateQueue = new Queue<T4Template>(this.Templates);
//                System.ComponentModel.BackgroundWorker backgroundWorker = new System.ComponentModel.BackgroundWorker();
//                backgroundWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(backgroundWorker_DoWork);
//                backgroundWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(backgroundWorker_RunWorkerCompleted);
//                backgroundWorker.RunWorkerAsync(this.TemplateQueue);
//            }
//        }

//        internal void CleanUp()
//        {
//            if ((this.Templates != null) && (this.Templates.Count > 0))
//            {
//                this.Templates.ForEach(it => it.RaiseCleanUp());
//            }
//        }

//        void backgroundWorker_RunWorkerCompleted(object sender, System.ComponentModel.RunWorkerCompletedEventArgs e)
//        {
//            if (this.EndOfProcess != null)
//                this.EndOfProcess(this, EventArgs.Empty);
//        }

//        void backgroundWorker_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
//        {
//            Queue<T4Template> queue = (Queue<T4Template>)e.Argument;
//            while(queue.Count != 0)
//            {
//                queue.Dequeue().Run();
//            }
//        }
//    }
//}
