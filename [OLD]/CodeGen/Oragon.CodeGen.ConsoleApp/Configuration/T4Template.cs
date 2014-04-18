//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;

//namespace Oragon.CodeGen.ConsoleApp.Configuration
//{
//    public class T4Template
//    {
//        #region Propriedades
		
//        //Injetado pelo container IOC
//        public string TemplatePath { get; set; }
		
//        //Injetado pelo container IOC
//        public string OutputPath { get; set; }

//        //Injetado pelo container IOC
//        public Dictionary<string, object> TemplateParameters { get; set; }

//        //Definido Programatiamente
//        public System.IO.StringWriter Log { get; private set; }

//        #endregion

//        #region Eventos

//        //Definido Programatiamente
//        public event Action<T4Template> GenerationStart;

//        //Definido Programatiamente
//        public event Action<T4Template> CleanUp;

//        //Definido Programatiamente
//        public event Action<T4Template, Exception> GenerationError;

//        //Definido Programatiamente
//        public event Action<T4Template> GenerationSucess;

//        #endregion

//        public T4Template()
//        {

//        }

//        internal void Run()
//        {
//            if (this.Log != null)
//                this.Log.Close();
//            this.Log = new System.IO.StringWriter();


//            if (this.GenerationStart != null)
//                this.GenerationStart(this);
//            bool hasException = false;
//            try
//            {
//                this.RunInternal();
//            }
//            catch (TC.CustomTemplating.TextTransformationException ex)
//            {
//                hasException = true;
//                if (this.GenerationError != null)
//                    this.GenerationError(this, ex);
//                this.Log.WriteLine("Exception------------------------------");
//                this.Log.WriteLine("StackTrace: \r\n {0}", ex.StackTrace);
//                this.Log.WriteLine("Data: \r\n {0}", Newtonsoft.Json.JsonConvert.SerializeObject(ex.Data));
//                this.Log.WriteLine("Exception------------------------------");
//                foreach (System.CodeDom.Compiler.CompilerError compilerError in ex.CompilationErrors)
//                {
//                    string compulerErrorStr = Newtonsoft.Json.JsonConvert.SerializeObject(compilerError);
//                    this.Log.WriteLine(compulerErrorStr);
//                }
//                this.Log.WriteLine("Exception------------------------------");
//                this.Log.WriteLine("Template Class: {0}", ex.TemplateClass);
//                this.Log.WriteLine("Exception------------------------------");
//            }
//            catch (Exception ex)
//            {
//                hasException = true;
//                if (this.GenerationError != null)
//                    this.GenerationError(this, ex);
//                this.Log.WriteLine("Exception------------------------------");
//                this.Log.WriteLine(ex.ToString());
//                this.Log.WriteLine("Exception------------------------------");
//            }

//            if ((hasException == false) && (this.GenerationSucess != null))
//                this.GenerationSucess(this);
//        }

//        private void RunInternal()
//        {
//            System.IO.FileInfo templateFile = new System.IO.FileInfo(this.TemplatePath);
//            System.IO.FileInfo outputFile = new System.IO.FileInfo(this.OutputPath);

//            this.Log.WriteLine("Gerando...");
//            this.Log.WriteLine("\tTemplate Name: {0}", templateFile.Name, templateFile.FullName);
//            this.Log.WriteLine("\tTemplate Path: {0}", templateFile.FullName);
//            this.Log.WriteLine(string.Empty);
//            this.Log.WriteLine("\tOutput Name  : {0}", outputFile.Name);
//            this.Log.WriteLine("\tOutput Path  : {0}", outputFile.FullName);


//            if (outputFile.Directory.Exists == false)
//            {
//                this.Log.WriteLine(string.Empty);
//                this.Log.WriteLine("\tSerá necessário criar diretório");
//                outputFile.Directory.Create();
//                this.Log.WriteLine("\tDiretório criado com sucesso");
//            }

//            this.Log.WriteLine(string.Empty);
//            this.Log.WriteLine("\tObtendo template.");
//            string ttContent = System.IO.File.ReadAllText(this.TemplatePath);
//            this.Log.WriteLine("\tTemplate carregado com sucesso");
//            string output = null;
//            this.Log.WriteLine("\tProcessando argumentos.");
//            TC.CustomTemplating.TemplateArgumentCollection t4Arguments = this.BuildArguments();
//            this.Log.WriteLine("\tArgumentos processados com sucesso");


//            TC.CustomTemplating.DomainTextTransformer domain = new TC.CustomTemplating.DomainTextTransformer();
//            domain.AutoRecycle = false;
//            domain.RecycleThreshold = 40;

//            this.Log.WriteLine(string.Empty);
//            this.Log.WriteLine("\tRealizando parser...");
//            if (t4Arguments == null)
//                output = domain.Transform(ttContent);
//            else
//                output = domain.Transform(ttContent, t4Arguments);
//            this.Log.WriteLine("\tParser realizado com sucesso");

//            this.Log.WriteLine(string.Empty);
//            this.Log.WriteLine("\tEscrevendo no arquivo...");
//            System.IO.File.WriteAllText(this.OutputPath, output);
//            this.Log.WriteLine("\tArquivo gerado: {0}", outputFile.FullName);
//            this.Log.WriteLine("\tProcedimento finalizado com sucesso!");

//            domain.Recycle();
//        }

//        private TC.CustomTemplating.TemplateArgumentCollection BuildArguments()
//        {
//            TC.CustomTemplating.TemplateArgumentCollection returnValue = null;
//            if ((this.TemplateParameters != null) && (this.TemplateParameters.Count > 0))
//            {
//                returnValue = new TC.CustomTemplating.TemplateArgumentCollection();

//                foreach (KeyValuePair<string, object> item in this.TemplateParameters)
//                {
//                    returnValue.Add(new TC.CustomTemplating.TemplateArgument(item.Key, item.Value));
//                }
//            }
//            return returnValue;
//        }

//        internal void RaiseCleanUp()
//        {
//            if (this.CleanUp != null)
//                this.CleanUp(this);
//        }
//    }
//}
